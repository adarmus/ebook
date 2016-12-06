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
                new FileSystemDataSourceInfo {Parameter = @"c:\tmp\"},
                new SqlDataSourceInfo {Parameter = "Server=localhost; Database=ebook; Trusted_Connection=SSPI"}, 
            };

            this.SimpleDataSourceInfoList = new ObservableCollection<ISimpleDataSourceInfo>(dataSources);

            XmlConfigurator.Configure();

            _log = LogManager.GetLogger("root");
        }

        ConfigProvider GetConfigProvider()
        {
            return new ConfigFile().GetConfigProvider();
        }

        async Task DoView()
        {
            ISimpleDataSource repo = this.SelectedSimpleDataSourceInfo.GetSimpleDataSource();

            var books = await repo.GetBooks(this.IncludeMobi, this.IncludeEpub);
            var matches = books.Select(b => new MatchInfo(b));
            this.BookFileList = new ObservableCollection<MatchInfo>(matches);
        }

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

        async Task  DoSave()
        {
            if (this.BookFileList == null)
                return;

            var toUpload = this.BookFileList.Where(b => !b.HasMatch);

            IFullDataSource repo = GetRepoToSave();

            var books = new BookRepository(repo);

            await books.SaveBooks(toUpload.Select(b => b.Book));
        }

        ISimpleDataSource GetRepoToSearch()
        {
            return new FileBasedSimpleDataSource(this.ImportFolderPath);
        }

        IFullDataSource GetRepoToSave()
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





        ICommand _importCommand;

        public ICommand ImportCommand
        {
            get { return _importCommand ?? (_importCommand = new AsyncCommand1(TryDoImport)); }
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
