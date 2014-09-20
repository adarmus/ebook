using System;
using System.Collections.Generic;

namespace Shell.Pdb
{
    public class PdbRecords
    {
        readonly List<PdbRecordEntry> _records;

        public PdbRecords(List<PdbRecordEntry> records)
        {
            _records = records;
        }

        public int GetRecordOffset(int index)
        {
            PdbRecordEntry entry = GetRecord(index);

            return entry.Offset;
        }

        public PdbRecordEntry GetRecord(int index)
        {
            if (index > _records.Count)
                throw new ArgumentOutOfRangeException("index", string.Format("Index {0} is greater than record count {1}", index, _records.Count));

            return _records[index];
        }
    }
}
