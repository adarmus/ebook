using System;
using System.IO;
using Shell.Pdb.Readers.Mobi;
using Shell.Pdb.Readers.Pdb;

namespace Shell.Pdb
{
    public class PdbFileReader
    {
        /// <summary>
        /// Magic number taken from pdbfmt.cpp for start of record Info List
        /// </summary>
        const int OFFSET_PDB_RECORD_LIST = 78;

        readonly string _filepath;

        BookFile _mobiFile;

        public PdbFileReader(string filepath)
        {
            _filepath = filepath;
            _mobiFile = null;
        }

        // http://wiki.mobileread.com/wiki/PDB

        public BookFile ReadMobiFile()
        {
            try
            {
                ReadExth(ReadMobiFile);
                return _mobiFile;
            }
            catch (Exception ex)
            {
                throw new ApplicationException(string.Format("Failed to read file {0}", _filepath), ex);
            }
        }

        void ReadMobiFile(MobiHeaderReader mobiReader, PdbRecords pdbRecords)
        {
            string title = mobiReader.GetTitleReader().ReadTitle();

            ExthHeaderReader exthReader = mobiReader.GetExthHeaderReader();

            if (exthReader == null)
            {
                _mobiFile = MakeMobiFileNoExth(title);
            }
            else
            {
                _mobiFile = MakeMobiFromExth(exthReader, title);
            }
        }

        BookFile MakeMobiFromExth(ExthHeaderReader exthReader, string title)
        {
            string author = exthReader.ReadExthStringValue(100);
            string publisher = exthReader.ReadExthStringValue(101);
            string description = exthReader.ReadExthStringValue(103);
            string isbn = exthReader.ReadExthStringValue(104);

            string publishDate = exthReader.ReadExthStringValue(106);

            int? coverOffset = exthReader.ReadExthIntValue(201);
            int? thumbOffset = exthReader.ReadExthIntValue(202);

            return new BookFile
            {
                Author = author,
                Description = description,
                Isbn = isbn,
                PublishDate = publishDate,
                Publisher = publisher,
                Title = title
            };
        }

        BookFile MakeMobiFileNoExth(string title)
        {
            return new BookFile
            {
                Title = title
            };
        }

        void ReadExth(Action<MobiHeaderReader, PdbRecords> exthHandler)
        {
            if (!File.Exists(_filepath))
                throw new FileNotFoundException(string.Format("Cannot find file '{0}'", _filepath));

            using (var file = new FileStream(_filepath, FileMode.Open))
            {
                using (var binary = new BinaryReader(file))
                {
                    PdbRecords pdbRecords = ObtainPdbRecords(binary);

                    binary.BaseStream.Position = pdbRecords.GetRecordOffset(0);

                    var mobiReader = new MobiHeaderReader(binary);

                    exthHandler(mobiReader, pdbRecords);
                }
            }
        }

        PdbRecords ObtainPdbRecords(BinaryReader binary)
        {
            // PDB header
            PdbHeader pdbHeader = new PdbHeaderReader(binary)
                .Read();

            if (pdbHeader.NumberOfRecords == 0)
                throw new ApplicationException("Zero PDB records");

            if (pdbHeader.Creator != "MOBI")
                throw new ApplicationException("Creator not MOBI");

            if (pdbHeader.Type != "BOOK")
                throw new ApplicationException("Type not BOOK");

            binary.BaseStream.Position = OFFSET_PDB_RECORD_LIST;

            // PDB records
            PdbRecords pdbRecords = new PdbRecordsReader(binary, pdbHeader.NumberOfRecords)
                .ReadAllRecords();

            return pdbRecords;
        }
    }
}
