using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ebook.core.BookFiles;

namespace ebook.core.Repo.File
{
    class AuthorNameCorrectionFilter : IBookFileFilter
    {
        const string DELIM = "=>";

        readonly string _authorNameFile;
        readonly Dictionary<string, string> _names;

        public AuthorNameCorrectionFilter(string authorNameFile)
        {
            _authorNameFile = authorNameFile;
            _names = new Dictionary<string, string>();

            ReadFile();
        }

        public void Accept(BookFile file)
        {
            if (file == null)
                return;

            if (file.Author == null)
                return;

            string author = file.Author;

            if (!_names.ContainsKey(author))
                return;

            file.Author = _names[author];
        }

        void ReadFile()
        {
            if (!System.IO.File.Exists(_authorNameFile))
                return;

            using (var reader = new StreamReader(_authorNameFile))
            {
                string[] delim = new string[] { DELIM };
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Contains(DELIM))
                    {
                        string[] bits = line.Split(delim, StringSplitOptions.None);

                        if (bits.Length == 2)
                        {
                            _names.Add(bits[0].Trim(), bits[1].Trim());
                        }
                    }
                }
            }
        }
    }
}
