using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ebook.core.DataTypes
{
    public enum MatchStatus
    {
        /// <summary>
        /// The match process has not been run yet.
        /// </summary>
        NotAttempted,
        /// <summary>
        /// This is a new book.
        /// </summary>
        NewBook,
        /// <summary>
        /// This book has been matched but this contains new files.
        /// </summary>
        NewFiles,
        /// <summary>
        /// The book and all its files have been matched.
        /// </summary>
        UpToDate
    }
}
