using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using ebook.core;
using ebook.core.DataTypes;
using ebook.core.Logic;
using ebook.core.Repo;
using ebook.core.Repo.File;
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
        async Task TryDoCompare()
        {
            try
            {
                await DoCompare();
            }
            catch (Exception ex)
            {
                ExceptionHandler.Handle(ex, "comparing");
            }
        }

        async Task DoCompare()
        {
            _log.Debug("Starting compare");

            IBookRepository repo = GetRepoToCompare();
            var books = await repo.GetBooks(this.IncludeMobi, this.IncludeEpub);

            var compare = new BookListComparison(this.BookFileList, books);

            Dictionary<string, BookComparisonInfo> results = compare.Compare();

            if (_log.IsDebugEnabled)
                _log.DebugFormat("Got {0} results", results.Count);

            OutputComparisonResults(results);
        }

        void OutputComparisonResults(Dictionary<string, BookComparisonInfo> results)
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

        async Task TryDoImport()
        {
            try
            {
                this.IsBusy = true;

                await DoImport();
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

        async Task DoImport()
        {
            IBookRepository repo = GetRepoToDisplay();
            var books = await repo.GetBooks(this.IncludeMobi, this.IncludeEpub);
            this.BookFileList = new ObservableCollection<BookInfo>(books);
        }

        IBookRepository GetRepoToDisplay()
        {
            return new FileBasedBookRepository(this.ImportFolderPath);
        }

        IBookRepository GetRepoToCompare()
        {
            return new FileBasedBookRepository(this.CompareFolderPath);
        }

        void DoSave()
        {
            if (this.BookFileList == null)
                return;

            var csv = new CsvWriter(CsvWriter.GetFilePath(this.ImportFolderPath));
            csv.Write(this.BookFileList);
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
                    _importCommand = new AsyncCommand1(TryDoImport);
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
                    _compareCommand = new AsyncCommand1(TryDoCompare);
                }
                return _compareCommand;
            }
        }
        #endregion
    }
}
