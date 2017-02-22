using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ebook.core.DataTypes;
using ebook.core.Repo;

namespace ebook.core.Logic
{
    public class QueryBookFinder : IBookFinder
    {
        readonly IFullDataSource _originalDataSource;

        public QueryBookFinder(IFullDataSource originalDataSource)
        {
            _originalDataSource = originalDataSource;
        }

        public async Task<FindResultInfo> Find(BookInfo incoming)
        {
            FindResultInfo result = await FindBook(incoming);

            if (result.Status != MatchStatus.UpToDate)
                return result;

            FindResultInfo fileResult = await FindBookFiles(incoming);

            return fileResult;
        }

        async Task<FindResultInfo> FindBookFiles(BookInfo incoming)
        {
            return new FindResultInfo
            {
                Book = incoming,
                Status = MatchStatus.UpToDate
            };
        }

        async Task<FindResultInfo> FindBook(BookInfo incoming)
        {
            if (incoming == null)
                throw new ArgumentException("incoming not set");

            // 1. Find by ISBN
            if (!string.IsNullOrEmpty(incoming.Isbn))
            {
                string isbn = Isbn.Normalise(incoming.Isbn);

                BookInfo book = await _originalDataSource.GetBookByIsbn(isbn);

                if (book != null)
                    return new FindResultInfo { Status = MatchStatus.UpToDate, Book = book };
            }

            // 2. Title + Author
            if (!string.IsNullOrEmpty(incoming.Title) && !string.IsNullOrEmpty(incoming.Author))
            {
                BookInfo book = await _originalDataSource.GetBookByTitleAuthor(incoming.Title, incoming.Author);

                if (book != null)
                    return new FindResultInfo { Status = MatchStatus.UpToDate, Book = book };
            }

            return new FindResultInfo {Book = null, Status = MatchStatus.NewBook};
        }
    }
}
