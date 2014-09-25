using System;
using System.Collections.Generic;
using System.Linq;
using Shell.Pdb;

namespace Shell
{
    public class Aggregator
    {
        public Dictionary<BookInfo, IEnumerable<string>> GetBookList(IEnumerable<MobiFile> mobiFiles)
        {
            var books =
                from m in mobiFiles
                group m by m.Isbn
                into g
                select new {Isbn = g.Key, MobiFiles = g};

            var tuples = books.Select(b =>
            {
                MobiFile first = b.MobiFiles.First();

                var book = new BookInfo
                {
                    Isbn = b.Isbn,
                    Author = first.Author,
                    Description = first.Description,
                    PublishDate = first.PublishDate,
                    Publisher = first.Publisher,
                    Title = first.Title
                };

                var files = b.MobiFiles.Select(f => f.FilePath);

                return new Tuple<BookInfo, IEnumerable<string>>(book, files);
            });

            var dict = new Dictionary<BookInfo, IEnumerable<string>>();
            tuples.ToList().ForEach(t => dict.Add(t.Item1, t.Item2));

            return dict;
        }

        void CompareAllFiles(IEnumerable<MobiFile> mobis)
        {
            MobiFile first = null;

            foreach (var mobi in mobis)
            {
                if (first == null)
                {
                    first = mobi;
                }
                else
                {
                    OutputComparison(first, mobi);
                }
            }
        }

        void OutputComparison(MobiFile first, MobiFile mobi)
        {
            bool titleOk = first.Title == mobi.Title;
            bool authorOk = first.Author == mobi.Author;
            bool publisherOk = first.Publisher == mobi.Publisher;

            if (authorOk && titleOk && publisherOk)
                return;

            Console.WriteLine("  {0}: {1}{2}{3}  {4}{5}{6}",
                first.Isbn,
                titleOk ? " " : "T",
                authorOk ? " " : "A",
                publisherOk ? " " : "P",
                titleOk ? "" : string.Format("({0} -> {1})", first.Title, mobi.Title),
                authorOk ? "" : string.Format("({0} -> {1})", first.Author, mobi.Author),
                publisherOk ? "" : string.Format("({0} -> {1})", first.Publisher, mobi.Publisher)
                );
        }
    }
}
