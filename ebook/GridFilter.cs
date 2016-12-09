using System;
using System.Text.RegularExpressions;
using ebook.core;
using ebook.core.DataTypes;

namespace ebook
{
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
                    var match = o as MatchInfo;
                    var book = match.Book;

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
                        var match = o as MatchInfo;
                        var book = match.Book;

                        if (book == null || book.Author == null)
                            return false;

                        return StringsMatch(book.Author, AuthorFilter);
                    };
                }
                else
                {
                    return o =>
                    {
                        var match = o as MatchInfo;
                        var book = match.Book;

                        if (book == null || book.Title == null)
                            return false;

                        return StringsMatch(book.Title, TitleFilter);
                    };
                }
            }
        }
    }
}
