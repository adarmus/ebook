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

        MobiFile _mobiFile;

        public PdbFileReader(string filepath)
        {
            _filepath = filepath;
            _mobiFile = null;
        }

        // http://wiki.mobileread.com/wiki/PDB

        public MobiFile ReadMobiFile()
        {
            ReadExth(ReadMobiFile);

            return _mobiFile;
        }

        void ReadMobiFile(ExthHeaderReader exthReader)
        {
            string author = exthReader.ReadExthStringValue(100);
            string publisher = exthReader.ReadExthStringValue(101);
            string description = exthReader.ReadExthStringValue(103);
            string isbn = exthReader.ReadExthStringValue(104);

            string publishDate = exthReader.ReadExthStringValue(106);

            int? coverOffset = exthReader.ReadExthIntValue(201);
            int? thumbOffset = exthReader.ReadExthIntValue(202);

            _mobiFile = new MobiFile
            {

            };
        }

        void ReadExth(Action<ExthHeaderReader> exthHandler)
        {
            if (!File.Exists(_filepath))
                throw new FileNotFoundException(string.Format("Cannot find file '{0}'", _filepath));

            using (var file = new FileStream(_filepath, FileMode.Open))
            {
                using (var binary = new BinaryReader(file))
                {
                    ExthHeaderReader exthReader = ObtainExthHeaderReader(binary);

                    exthHandler(exthReader);
                }
            }
        }

        ExthHeaderReader ObtainExthHeaderReader(BinaryReader binary)
        {
            // PDB header
            PdbHeader pdbHeader = new PdbHeaderReader(binary)
                .Read();

            if (pdbHeader.NumberOfRecords == 0)
                throw new ApplicationException("Zero PDB records");

            binary.BaseStream.Position = OFFSET_PDB_RECORD_LIST;

            // PDB records
            PdbRecords pdbRecords = new PdbRecordsReader(binary, pdbHeader.NumberOfRecords)
                .ReadAllRecords();

            binary.BaseStream.Position = pdbRecords.GetRecordOffset(0);

            // Main content
            MobiHeaderReader mobiReader = GetRecordReader(binary, pdbHeader, pdbRecords);

            ExthHeaderReader exthReader = mobiReader.GetExthHeaderReader();

            return exthReader;
        }

        MobiHeaderReader GetRecordReader(BinaryReader binary, PdbHeader pdbHeader, PdbRecords pdbRecords)
        {
            if (pdbHeader.Creator != "MOBI")
                throw new ApplicationException("Creator not MOBI");

            if (pdbHeader.Type != "BOOK")
                throw new ApplicationException("Type not BOOK");

            return new MobiHeaderReader(binary, pdbHeader, pdbRecords);
        }
    }
}
