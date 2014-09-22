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
        GridFilter _filter;

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

    class StringMatcher
    {
    }

    internal class GridFilter
    {
        public string AuthorFilter { get; set; }

        public string TitleFilter { get; set; }

        public Predicate<object> GetFilter()
        {
            if (string.IsNullOrWhiteSpace(AuthorFilter) && string.IsNullOrWhiteSpace(TitleFilter))
                return null;

            return BuildFilter();
        }

        public bool StringsMatch(string searchIn, string searchFor)
        {
            if (searchFor == null || searchIn == null)
                return false;

            return Regex.IsMatch(searchIn, string.Format(@"\b{0}", searchFor), RegexOptions.IgnoreCase);
        }

        Predicate<object> BuildFilter()
        {
            bool hasAuthor = !string.IsNullOrWhiteSpace(AuthorFilter);
            bool hastitle = !string.IsNullOrWhiteSpace(TitleFilter);

            if (hasAuthor && hastitle)
            {
                return o =>
                {
                    var book = o as MobiFile;
                    
                    if (book == null || book.Title == null || book.Author == null)
                        return false;

                    return StringsMatch(book.Title, TitleFilter) &&
                           StringsMatch(book.Author, AuthorFilter);
                };
            }
            else
            {
                if (hasAuthor)
                {
                    return o =>
                    {
                        var book = o as MobiFile;

                        if (book == null || book.Author == null)
                            return false;

                        return StringsMatch(book.Author, AuthorFilter);
                    };
                }
                else
                {
                    return o =>
                    {
                        var book = o as MobiFile;

                        if (book == null || book.Title == null)
                            return false;

                        return StringsMatch(book.Title, TitleFilter);
                    };
                }
            }
        }
    }
}
