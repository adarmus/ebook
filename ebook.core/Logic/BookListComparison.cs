using System.Collections.Generic;
using System.Linq;
using ebook.core.DataTypes;

namespace ebook.core.Logic
{
    /// <summary>
    /// Compares two book lists based on Isbn.
    /// </summary>
    public class BookListComparison
    {
        readonly IEnumerable<BookInfo> _books1;
        readonly IEnumerable<BookInfo> _books2;

        public BookListComparison(IEnumerable<BookInfo> books1, IEnumerable<BookInfo> books2)
        {
            _books1 = books1;
            _books2 = books2;
        }

        public Dictionary<string, BookComparisonInfo> Compare()
        {
            Dictionary<string, BookInfo> dict1 = GetDictionary(_books1);
            Dictionary<string, BookInfo> dict2 = GetDictionary(_books2);

            return DoComparison(dict1, dict2);
        }

        Dictionary<string, BookComparisonInfo> DoComparison(Dictionary<string, BookInfo> dict1, Dictionary<string, BookInfo> dict2)
        {
            var results = new Dictionary<string, BookComparisonInfo>();

            foreach (var isbn in dict1.Keys)
            {
                if (!results.ContainsKey(isbn))
                {
                    if (dict2.ContainsKey(isbn))
                    {
                        results.Add(isbn, new BookComparisonInfo {Isbn = isbn, Book1 = dict1[isbn], Book2 = dict2[isbn]});
                    }
                    else
                    {
                        results.Add(isbn, new BookComparisonInfo {Isbn = isbn, Book1 = dict1[isbn], Book2 = null});
                    }
                }
                else
                {
                    results.Add(isbn, new BookComparisonInfo {Isbn = isbn, Book1 = null, Book2 = dict2[isbn]});
                }
            }
            return results;
        }

        Dictionary<string, BookInfo> GetDictionary(IEnumerable<BookInfo> books)
        {
            var dict = new Dictionary<string, BookInfo>();
            books
                .Where(b => !string.IsNullOrEmpty(b.Isbn))
                .ToList()
                .ForEach(b => dict.Add(b.Isbn, b));

            return dict;
        }
    }
}
