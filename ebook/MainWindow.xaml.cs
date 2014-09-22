using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Shell.Pdb;

namespace ebook
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly GridFilter _filter;

        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = new MainWindowViewModel();

            _filter = new GridFilter();
        }

        void txtFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textbox = (TextBox)sender;
            if (textbox.Name == "txtTitle")
                _filter.TitleFilter = textbox.Text;
            else if (textbox.Name == "txtAuthor")
                _filter.AuthorFilter = textbox.Text;

            ICollectionView cv = CollectionViewSource.GetDefaultView(bookGrid.ItemsSource);
            cv.Filter = _filter.GetFilter();
        }
    }
}
