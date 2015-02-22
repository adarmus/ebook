using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

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
