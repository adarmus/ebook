using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ebook.core.DataTypes;
using Insight.Database;

namespace ebook.core.Repo.Sql
{
    public interface IBookSqlDal
    {
        [Sql("spBook_INS")]
        Task BookIns(BookInfo book);

        [Sql("spBook_SEL_ALL")]
        Task<IEnumerable<BookInfo>> BookSelAll();

        [Sql("spBook_SEL_BY_ID")]
        Task<BookInfo> BookSelById(Guid id);

        [Sql("spBook_SEL_BY_ISBN")]
        Task<BookInfo> BookSelByIsbn(string isbn);

        [Sql("spBook_SEL_BY_TITLEAUTHOR")]
        Task<BookInfo> BookSelByTitleAuthor(string title, string author);

        [Sql("spFile_INS")]
        Task FileIns(BookFileInfo book);

        [Sql("spFile_SEL_TYPE_BY_BOOKID")]
        Task<IEnumerable<BookFileInfo>> BookFileSelByBookId(Guid bookid);
    }
}
