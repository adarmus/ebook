using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Shell;
using Shell.Files;
using Shell.Pdb;

namespace ebook
{
    internal class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            this.ImportFolderPath = @"C:\MyDev\eBook\eBooks\2014-09-17";
            this.CompareFolderPath = @"C:\MyDev\eBook\eBooks\2014-09-06";
        }

        void DoCompare()
        {
            var books = GetBookList(this.CompareFolderPath);

            var compare = new BookListComparison(this.MobiFileList, books);

            Dictionary<string, BookComparisonInfo> results = compare.Compare();

            OutputResults(results);
        }

        void OutputResults(Dictionary<string, BookComparisonInfo> results)
        {
            string path = Path.Combine(this.CompareFolderPath, "Compare.csv");
            using (TextWriter writer = new StreamWriter(path))
            {
                foreach (var isbn in results.Keys)
                {
                    BookComparisonInfo comp = results[isbn];

                    string b1 = comp.Book1 == null ? ",," : string.Format("{0},{1},{2}", C34(comp.Book1.Title), C34(comp.Book1.Author), C34(comp.Book1.Files[0]));
                    string b2 = comp.Book2 == null ? ",," : string.Format("{0},{1},{2}", C34(comp.Book2.Title), C34(comp.Book2.Author), C34(comp.Book2.Files[0]));
                    writer.WriteLine("{0},{1},{2}", C34(isbn), b1, b2);
                }
            }
        }

        string C34(string input)
        {
            return string.Format("\"{0}\"", input);
        }

        async Task DoImport()
        {
            //IEnumerable<BookInfo> dict = await Task.Run(() =>
            //{
            //    GetBookListAsync();
            //}); ;
            var dict = await GetData();

            this.MobiFileList = new ObservableCollection<BookInfo>(dict);
        }

        async Task<IEnumerable<BookInfo>> GetData()
        {
            IEnumerable<BookInfo> dict = null;

            await Task.Run(() =>
            {
                dict = GetBookList(this.ImportFolderPath);
            });

            return dict;
        }

        IEnumerable<BookInfo> GetBookList(string folderpath)
        {
            var files = new FileFinder(folderpath);
            var mobilist = new MobiFileList(files);

            IEnumerable<BookInfo> books = null;

            IEnumerable<MobiFile> mobis = mobilist.GetMobiFiles();

            var agg = new Aggregator();
            books = agg.GetBookList(mobis);
            return books;
        }

        ObservableCollection<BookInfo> _mobiFileList;

        public ObservableCollection<BookInfo> MobiFileList
        {
            get { return _mobiFileList; }
            set
            {
                if (value == _mobiFileList)
                    return;
                _mobiFileList = value;
                RaisePropertyChanged();
            }
        }

        string _compareFolderPath;

        public string CompareFolderPath
        {
            get { return _compareFolderPath; }
            set
            {
                if (value == _compareFolderPath)
                    return;
                _compareFolderPath = value;
                RaisePropertyChanged();
            }
        }

        string _importFolderPath;

        public string ImportFolderPath
        {
            get { return _importFolderPath; }
            set
            {
                if (value == _importFolderPath)
                    return;
                _importFolderPath = value;
                RaisePropertyChanged();
            }
        }

        ICommand _importCommand;

        public ICommand ImportCommand
        {
            get
            {
                if (_importCommand == null)
                {
                    _importCommand = AsyncCommand.Create(DoImport);
                    //_importCommand = AsyncCommand.Create(async () =>
                    //{
                    //    var dict = await Task.Run(() => GetBookListAsync());
                    //    this.MobiFileList = new ObservableCollection<BookInfo>(dict);
                    //});
                }
                return _importCommand;
            }
        }

        ICommand _compareCommand;

        public ICommand CompareCommand
        {
            get
            {
                if (_compareCommand == null)
                {
                    _compareCommand = new RelayCommand(DoCompare);
                }
                return _compareCommand;
            }
        }
    }
}
