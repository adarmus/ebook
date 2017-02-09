using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ebook.core.Logic
{
    public class Isbn
    {
        public static string Normalise(string isbn)
        {
            if (string.IsNullOrEmpty(isbn))
                return isbn;

            return isbn
                .Replace("-", string.Empty)
                .Replace(" ", "");
        }
    }
}
