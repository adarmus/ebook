using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ebook.Async;
using ebook.core.DataTypes;
using ebook.core.Logic;
using ebook.core.Repo;
using GalaSoft.MvvmLight;
using log4net;
using log4net.Config;

namespace ebook
{
    internal class MainWindowViewModel : ViewModelBase
    {
        readonly ILog _log;

        private ISimpleDataSource _simpleDataSource;

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
                new FileSystemDataSourceInfo {Parameter = @"c:\tmp\"},
                new SqlDataSourceInfo {Parameter = "Server=localhost; Database=ebook; Trusted_Connection=SSPI"}, 
            };

            this.SimpleDataSourceInfoList = new ObservableCollection<ISimpleDataSourceInfo>(dataSources);

            this.SelectedSimpleDataSourceInfo = dataSources[0];

            var sources = new IFullDataSourceInfo[]
            {
                new SqlDataSourceInfo{Parameter = "Server=localhost; Database=ebook; Trusted_Connection=SSPI"}
            };

            this.FullDataSourceInfoList = new ObservableCollection<IFullDataSourceInfo>(sources);

            this.SelectedFullDataSourceInfo = sources[0];

            XmlConfigurator.Configure();

            _log = LogManager.GetLogger("root");
        }

        ConfigProvider GetConfigProvider()
        {
            return new ConfigFile().GetConfigProvider();
        }

        async Task DoView()
        {
            if (this.SelectedSimpleDataSourceInfo == null)
                return;

            _simpleDataSource = this.SelectedSimpleDataSourceInfo.GetSimpleDataSource();

            IEnumerable<BookInfo> books = await _simpleDataSource.GetBooks(this.IncludeMobi, this.IncludeEpub);
            IEnumerable<MatchInfo> matches = books.Select(b => new MatchInfo(b));
            this.BookFileList = new ObservableCollection<MatchInfo>(matches);
        }

        async Task TryDoMatch()
        {
            try
            {
                await DoMatch();
            }
            catch (Exception ex)
            {
                ExceptionHandler.Handle(ex, "comparing");
            }
        }

        async Task DoMatch()
        {
            _log.Debug("Starting compare");

            if (this.SelectedFullDataSourceInfo == null)
                return;

            IFullDataSource repo = this.SelectedFullDataSourceInfo.GetFullDataSource();

            var books = await repo.GetBooks(this.IncludeMobi, this.IncludeEpub);

            var matcher = new BookMatcher(books);

            var matched = matcher.Match(this.BookFileList);

            this.BookFileList = new ObservableCollection<MatchInfo>(matched);
        }
        
        async Task  DoUpload()
        {
            if (this.BookFileList == null)
                return;

            if (this.SelectedFullDataSourceInfo == null)
                return;

            DateTime date;
            if (TryGetDateTimeAdded(out date))
            {
                await UploadBooks(date);
                MessageBox.Show("Done");
            }
            else
            {
                MessageBox.Show("Could not parse date added");
            }
        }

        async Task UploadBooks(DateTime date)
        {
            IEnumerable<BookFilesInfo> contents = await GetBookContentInfosToUpload();

            var repo = new BookRepository(this.SelectedFullDataSourceInfo.GetFullDataSource());

            await repo.SaveBooks(contents, date);
        }

        async Task<IEnumerable<BookFilesInfo>> GetBookContentInfosToUpload()
        {
            IEnumerable<MatchInfo> toUpload = this.BookFileList
                .Where(b => b.IsSelected)
                .Where(b => (b.Status == MatchStatus.NewBook || b.Status == MatchStatus.NewFiles));

            var tasks = toUpload.Select(async (b) => await _simpleDataSource.GetBookContent(b.Book));

            BookFilesInfo[] contents = await Task.WhenAll(tasks);

            // obtain file contents -> BookFileInfo[]

            return contents;
        }

        bool TryGetDateTimeAdded(out DateTime dateAdded)
        {
            if (string.IsNullOrEmpty(DateAddedText))
            {
                dateAdded = DateTime.Now;
                return true;
            }

            DateTime date;
            if (DateTime.TryParse(this.DateAddedText, out date))
            {
                dateAdded = date;
                return true;
            }

            dateAdded = DateTime.MinValue;
            return false;
        }

        void SelectedSimpleDataSourceInfoChanged()
        {
            this.BookFileList = new ObservableCollection<MatchInfo>();
        }

        async Task SelectedBookChanged()
        {
            if (this.SelectedBook == null)
            {
                this.SelectedBookContent = null;
                return;
            }

            this.SelectedBookContent = await _simpleDataSource.GetBookContent(this.SelectedBook.Book);
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

        string _dateAddedText;
        public string DateAddedText
        {
            get { return _dateAddedText; }
            set
            {
                if (value == _dateAddedText)
                    return;

                _dateAddedText = value;
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

                Task.Run(SelectedBookChanged);
            }
        }

        BookFilesInfo _selectedBookContent;

        public BookFilesInfo SelectedBookContent
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

        ObservableCollection<IFullDataSourceInfo> _fullDataSourceInfoList;

        public ObservableCollection<IFullDataSourceInfo> FullDataSourceInfoList
        {
            get { return _fullDataSourceInfoList; }
            set
            {
                if (value == _fullDataSourceInfoList)
                    return;
                _fullDataSourceInfoList = value;
                RaisePropertyChanged();
            }
        }

        IFullDataSourceInfo _selectedFullDataSourceInfo;

        public IFullDataSourceInfo SelectedFullDataSourceInfo
        {
            get { return _selectedFullDataSourceInfo; }
            set
            {
                if (value == _selectedFullDataSourceInfo)
                    return;
                _selectedFullDataSourceInfo = value;
                RaisePropertyChanged();
            }
        }

        ObservableCollection<ISimpleDataSourceInfo> _simpleDataSourceInfoList;

        public ObservableCollection<ISimpleDataSourceInfo> SimpleDataSourceInfoList
        {
            get { return _simpleDataSourceInfoList; }
            set
            {
                if (value == _simpleDataSourceInfoList)
                    return;
                _simpleDataSourceInfoList = value;
                RaisePropertyChanged();
            }
        }

        ISimpleDataSourceInfo _selectedSimpleDataSourceInfo;

        public ISimpleDataSourceInfo SelectedSimpleDataSourceInfo
        {
            get { return _selectedSimpleDataSourceInfo; }
            set
            {
                if (value == _selectedSimpleDataSourceInfo)
                    return;
                _selectedSimpleDataSourceInfo = value;
                RaisePropertyChanged();
                SelectedSimpleDataSourceInfoChanged();
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
        ICommand _viewCommand;

        public ICommand ViewCommand
        {
            get { return _viewCommand ?? (_viewCommand = new AsyncCommand1(this.DoView)); }
        }

        ICommand _matchCommand;

        public ICommand MatchCommand
        {
            get { return _matchCommand ?? (_matchCommand = new AsyncCommand1(TryDoMatch)); }
        }

        ICommand _uploadCommand;

        public ICommand UploadCommand
        {
            get { return _uploadCommand ?? (_uploadCommand = new AsyncCommand1(this.DoUpload)); }
        }
        #endregion
    }
}
