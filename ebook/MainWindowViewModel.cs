using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ebook.Async;
using ebook.core;
using ebook.core.DataTypes;
using ebook.core.Logic;
using ebook.core.Repo;
using ebook.core.Repo.File;
using ebook.core.Repo.Sql;
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

            var dataSources = new ISimpleDataSourceInfo[]
            {
                new FileSystemDataSourceInfo {Parameter = config.ImportFolderPath},
                new FileSystemDataSourceInfo {Parameter = @"c:\tmp\"}
            };

            this.DataSourceInfoList = new ObservableCollection<ISimpleDataSourceInfo>(dataSources);

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

            ISimpleDataSource repo = GetRepoToCompare();
            var books = await repo.GetBooks(this.IncludeMobi, this.IncludeEpub);

            var matcher = new BookMatcher(books);

            var matched = matcher.Match(this.BookFileList);

            this.BookFileList = new ObservableCollection<MatchInfo>(matched);
        }

        async Task DoCompareOrig()
        {
            _log.Debug("Starting compare");

            ISimpleDataSource repo = GetRepoToCompare();
            var books = await repo.GetBooks(this.IncludeMobi, this.IncludeEpub);

            var compare = new BookListComparison(this.BookFileList.Select(m => m.Book), books);

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

                    //string b1 = comp.Book1 == null ? ",," : string.Format("{0},{1},{2}", C34(comp.Book1.Title), C34(comp.Book1.Author), C34(comp.Book1.Files[0]));
                    //string b2 = comp.Book2 == null ? ",," : string.Format("{0},{1},{2}", C34(comp.Book2.Title), C34(comp.Book2.Author), C34(comp.Book2.Files[0]));

                    string b1 = comp.Book1 == null ? ",," : string.Format("{0},{1}", C34(comp.Book1.Title), C34(comp.Book1.Author));
                    string b2 = comp.Book2 == null ? ",," : string.Format("{0},{1}", C34(comp.Book2.Title), C34(comp.Book2.Author));
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
            ISimpleDataSource repo = GetRepoToSearch();
            var books = await repo.GetBooks(this.IncludeMobi, this.IncludeEpub);
            var matches = books.Select(b => new MatchInfo(b));
            this.BookFileList = new ObservableCollection<MatchInfo>(matches);
        }

        async Task DoView()
        {
            ISimpleDataSource repo = GetRepoToCompare();
            var books = await repo.GetBooks(this.IncludeMobi, this.IncludeEpub);
            var matches = books.Select(b => new MatchInfo(b));
            this.BookFileList = new ObservableCollection<MatchInfo>(matches);
        }

        async Task  DoSave()
        {
            if (this.BookFileList == null)
                return;

            var toUpload = this.BookFileList.Where(b => !b.HasMatch);

            IBookDataSource repo = GetRepoToSave();

            var books = new BookRepository(repo);

            await books.SaveBooks(toUpload.Select(b => b.Book));
        }

        ISimpleDataSource GetRepoToSearch()
        {
            return new FileBasedSimpleDataSource(this.ImportFolderPath);
        }

        IBookDataSource GetRepoToSave()
        {
            return new SqlDataSource("Server=localhost; Database=ebook; Trusted_Connection=SSPI");
        }

        ISimpleDataSource GetRepoToCompare()
        {
            return new SqlDataSource("Server=localhost; Database=ebook; Trusted_Connection=SSPI");
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
                base.RaisePropertyChanged();
            }
        }

        MatchInfo _selectedBook;

        public MatchInfo SelectedBook
        {
            get { return _selectedBook; }
            set
            {
                if (value == _selectedBook)
                    return;
                _selectedBook = value;
                RaisePropertyChanged();
            }
        }

        BookContentInfo _selectedBookContent;

        public BookContentInfo SelectedBookContent
        {
            get { return _selectedBookContent; }
            set
            {
                if (value == _selectedBookContent)
                    return;
                _selectedBookContent = value;
                RaisePropertyChanged();
            }
        }

        ObservableCollection<ISimpleDataSourceInfo> _dataSourceInfoList;

        public ObservableCollection<ISimpleDataSourceInfo> DataSourceInfoList
        {
            get { return _dataSourceInfoList; }
            set
            {
                if (value == _dataSourceInfoList)
                    return;
                _dataSourceInfoList = value;
                RaisePropertyChanged();
            }
        }

        ISimpleDataSourceInfo _selectedDataSourceInfo;

        public ISimpleDataSourceInfo SelectedDataSourceInfo
        {
            get { return _selectedDataSourceInfo; }
            set
            {
                if (value == _selectedDataSourceInfo)
                    return;
                _selectedDataSourceInfo = value;
                RaisePropertyChanged();
            }
        }

        ObservableCollection<MatchInfo> _bookFileList;

        public ObservableCollection<MatchInfo> BookFileList
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
                base.RaisePropertyChanged();
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
                base.RaisePropertyChanged();
            }
        }
        #endregion

        #region Commands
        ICommand _importCommand;

        public ICommand ImportCommand
        {
            get { return _importCommand ?? (_importCommand = new AsyncCommand1(TryDoImport)); }
        }

        ICommand _viewCommand;

        public ICommand ViewCommand
        {
            get { return _viewCommand ?? (_viewCommand = new AsyncCommand1(this.DoView)); }
        }

        ICommand _saveCommand;

        public ICommand SaveCommand
        {
            get { return _saveCommand ?? (_saveCommand = new AsyncCommand1(this.DoSave)); }
        }

        ICommand _compareCommand;

        public ICommand CompareCommand
        {
            get { return _compareCommand ?? (_compareCommand = new AsyncCommand1(TryDoCompare)); }
        }
        #endregion
    }
}
