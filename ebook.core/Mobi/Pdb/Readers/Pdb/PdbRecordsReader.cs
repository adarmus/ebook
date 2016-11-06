using System;
using System.Collections.Generic;
using System.IO;

namespace ebook.core.Mobi.Pdb.Readers.Pdb
{
    public class PdbRecordsReader : BinaryReaderBase
    {
        readonly int _numberOfRecords;

        public PdbRecordsReader(BinaryReader reader, int numberOfRecords)
            : base(reader)
        {
            _numberOfRecords = numberOfRecords;
        }

        public PdbRecords ReadAllRecords()
        {
            SetPositionToOriginal();

            var records = new List<PdbRecordEntry>();

            int lastEntryStart = 0;
            for (int i = 0; i < _numberOfRecords; i++)
            {
                var entry = new PdbRecordEntryReader(_reader);
                PdbRecordEntry ent = entry.Read();

                if (ent.Offset < lastEntryStart)
                    throw new ApplicationException("offset < lastEntryStart");

                lastEntryStart = ent.Offset;

                records.Add(ent);
            }

            return new PdbRecords(records);
        }
    }
}
