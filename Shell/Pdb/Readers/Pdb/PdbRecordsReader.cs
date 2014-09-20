using System;
using System.Collections.Generic;
using System.IO;

namespace Shell.Pdb.Readers.Pdb
{
    public class PdbRecordsReader : BinaryReaderBase
    {
        readonly List<PdbRecordEntryReader> _records;
        readonly PdbHeaderReader _pdbHeader;

        public PdbRecordsReader(BinaryReader reader, PdbHeaderReader pdbHeader)
            : base(reader)
        {
            _pdbHeader = pdbHeader;
            _records = new List<PdbRecordEntryReader>();
        }

        public List<PdbRecordEntryReader> GetRecords()
        {
            return _records; 
        }

        public void Read()
        {
            int lastEntryStart = 0;
            for (int i = 0; i < _pdbHeader.NumberOfRecords; i++)
            {
                var entry = new PdbRecordEntryReader(_reader);
                entry.Read();

                if (entry.Offset < lastEntryStart)
                    throw new ApplicationException("offset < lastEntryStart");

                lastEntryStart = entry.Offset;

                _records.Add(entry);
            }
        }
    }
}
