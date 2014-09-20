using System;
using System.Collections.Generic;
using System.IO;

namespace Shell.Pdb.Readers.Pdb
{
    public class PdbRecords
    {
        readonly List<PdbRecordEntry> _records;
        readonly PdbHeader _pdbHeader;

        public PdbRecords(PdbHeader pdbHeader)
        {
            _pdbHeader = pdbHeader;
            _records = new List<PdbRecordEntry>();
        }

        public List<PdbRecordEntry> GetRecords()
        {
            return _records; 
        }

        public void Read(BinaryReader reader)
        {
            int lastEntryStart = 0;
            for (int i = 0; i < _pdbHeader.NumberOfRecords; i++)
            {
                var entry = new PdbRecordEntry();
                entry.Read(reader);

                if (entry.Offset < lastEntryStart)
                    throw new ApplicationException("offset < lastEntryStart");

                lastEntryStart = entry.Offset;

                _records.Add(entry);
            }
        }
    }
}
