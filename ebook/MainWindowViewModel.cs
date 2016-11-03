using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using ebook.core;
using ebook.core.ePub;
using ebook.core.Files;
using ebook.core.Mobi;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using log4net;
using log4net.Config;

namespace ebook
{
    internal class MainWindowViewModel : ViewModelBase
    {
        readonly ILog _log;

        public MainWindowViewModel()
        {
            ConfigProvider config = GetConfigProvider();

            this.ImportFolderPath = config.ImportFolderPath;
            this.CompareFolderPath = config.CompareFolderPath;
            this.IncludeEpub = config.IncludeEpub;
            this.IncludeMobi = config.IncludeMobi;

            XmlConfigurator.Configure();

            _log = LogManager.GetLogger("root");
        }

        ConfigProvider GetConfigProvider()
        {
            return new ConfigFile().GetConfigProvider();
        }

        #region Compare
        void TryDoCompare()
        {
            try
            {
                DoCompare();
            }
            catch (Exception ex)
            {
                ExceptionHandler.Handle(ex, "comparing");
            }
        }

        void DoCompare()
        {
            _log.Debug("Starting compare");

            var books = GetBookList(this.CompareFolderPath);

            var compare = new BookListComparison(this.BookFileList, books);

            Dictionary<string, BookComparisonInfo> results = compare.Compare();

            if (_log.IsDebugEnabled)
                _log.DebugFormat("Got {0} results", results.Count);

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
        #endregion

        string C34(string input)
        {
            return string.Format("\"{0}\"", input);
        }

        async void TryDoImport()
        {
            try
            {
                this.IsBusy = true;

                await Task.Run(() => DoImport());
            }
            catch (Exception ex)
            {
                ExceptionHandler.Handle(ex, "importing");
            }
            finally
            {
                this.IsBusy = false;
            }
        }

        void DoImport()
        {
            var dict = GetData();

            this.BookFileList = new ObservableCollection<BookInfo>(dict);
        }

        IEnumerable<BookInfo> GetData()
        {
            IEnumerable<BookInfo> dict = null;

            dict = GetBookList(this.ImportFolderPath);

            return dict;
        }

        IEnumerable<BookInfo> GetBookList(string folderpath)
        {
            var search = new BookFileSearch();

            if (this.IncludeMobi)
            {
                var mobiFiles = new FileFinder(folderpath, "mobi");
                var mobiList = new BookFileList(mobiFiles, new MobiReader());
                search.AddList(mobiList);
            }

            if (this.IncludeEpub)
            {
                var epubFiles = new FileFinder(folderpath, "epub");
                var epubList = new BookFileList(epubFiles, new EpubReader());
                search.AddList(epubList);
            }

            return search.GetBooks();
        }

        void DoSave()
        {
            if (this.BookFileList == null)
                return;

            string path = Path.Combine(this.ImportFolderPath, GetCsvFilename());

            var csv = new CsvWriter(path);
            csv.Write(this.BookFileList);
        }

        string GetCsvFilename()
        {
            string s = string.Format("{0:yyyy-MMM-dd}.csv", DateTime.Today);
            return s;
        }

        #region Properties
        bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                if (value == _isBusy)
                    return;

                _isBusy = value;
                base.RaisePropertyChanged("IsBusy");
            }
        }

        ObservableCollection<BookInfo> _bookFileList;

        public ObservableCollection<BookInfo> BookFileList
        {
            get { return _bookFileList; }
            set
            {
                if (value == _bookFileList)
                    return;
                _bookFileList = value;
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

        bool _includeEpub;
        public bool IncludeEpub
        {
            get { return _includeEpub; }
            set
            {
                if (value == _includeEpub)
                    return;

                _includeEpub = value;
                base.RaisePropertyChanged("IncludeEpub");
            }
        }

        bool _includeMobi;
        public bool IncludeMobi
        {
            get { return _includeMobi; }
            set
            {
                if (value == _includeMobi)
                    return;

                _includeMobi = value;
                base.RaisePropertyChanged("IncludeMobi");
            }
        }
        #endregion

        #region Commands
        ICommand _importCommand;

        public ICommand ImportCommand
        {
            get
            {
                if (_importCommand == null)
                {
                    _importCommand = new RelayCommand(TryDoImport);
                    //_importCommand = AsyncCommand.Create(TryDoImport);
                    //_importCommand = AsyncCommand.Create(async () =>
                    //{
                    //    var dict = await Task.Run(() => GetBookListAsync());
                    //    this.MobiFileList = new ObservableCollection<BookInfo>(dict);
                    //});
                }
                return _importCommand;
            }
        }

        RelayCommand _saveCommand;

        public ICommand SaveCommand
        {
            get { return _saveCommand ?? (_saveCommand = new RelayCommand(this.DoSave)); }
        }

        ICommand _compareCommand;

        public ICommand CompareCommand
        {
            get
            {
                if (_compareCommand == null)
                {
                    _compareCommand = new RelayCommand(TryDoCompare);
                }
                return _compareCommand;
            }
        }
        #endregion
    }
}
