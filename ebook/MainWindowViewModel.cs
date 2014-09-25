using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        }

        void DoImport()
        {
            var files = new FileFinder(this.ImportFolderPath);
            var mobilist = new MobiFileList(files);
            IEnumerable<MobiFile> mobis = mobilist.GetMobiFiles();
            this.MobiFileList = new ObservableCollection<MobiFile>(mobis);


            var agg = new Aggregator();
            agg.GetBookList(this.MobiFileList);
        }

        ObservableCollection<MobiFile> _mobiFileList;

        public ObservableCollection<MobiFile> MobiFileList
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

        RelayCommand _importCommand;

        public ICommand ImportCommand
        {
            get { return _importCommand ?? (_importCommand = new RelayCommand(this.DoImport)); }
        }
    }
}
