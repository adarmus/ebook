using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shell
{
    public class PdbFileReader
    {
        readonly string _filepath;

        public PdbFileReader(string filepath)
        {
            _filepath = filepath;
        }

        public MobiFile Read()
        {
            if (!File.Exists(_filepath))
                throw new FileNotFoundException(string.Format("Cannot find file '{0}'", _filepath));

            using (var file = new FileStream(_filepath, FileMode.Open))
            {
                using (var reader = new BinaryReader(file))
                {
                    // PDB header
                    var header = new PdbHeader();
                    header.Read(reader);

                    if (header.NumberOfRecords == 0)
                        throw new ApplicationException("Zero records");

                    reader.BaseStream.Position = 78; // Magic number taken from pdbfmt.cpp

                    // PDB records
                    var pdbRecords = new PdbRecords(header);
                    pdbRecords.Read(reader);
                    List<PdbRecordEntry> records = pdbRecords.GetRecords();

                    reader.BaseStream.Position = records[0].Offset;

                    // Main content
                    var mobi = GetRecordReader(header, records);
                    mobi.Read(reader);
                }
            }

            return new MobiFile();
        }

        MobiRecords GetRecordReader(PdbHeader pdbHeader, List<PdbRecordEntry> pdbRecords)
        {
            if (pdbHeader.Creator != "MOBI")
                throw new ApplicationException("Not MOBI");

            if (pdbHeader.Type != "BOOK")
                throw new ApplicationException("Not BOOK");

            var mobi = new MobiRecords(pdbHeader, pdbRecords);
            return mobi;
        }
    }
}
