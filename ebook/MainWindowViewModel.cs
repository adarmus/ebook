using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace ebook
{
    internal class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
        }

        int myVar;

        public int MyProperty
        {
            get { return myVar; }
            set
            {
                myVar = value;
            }
        }

    }
}
