using System.IO;

namespace ebook.core.Mobi.Pdb.Readers.Pdb
{
    public class PdbRecordEntryReader : BinaryReaderBase
    {
        public PdbRecordEntryReader(BinaryReader reader)
            : base(reader)
        {
        }

        public PdbRecordEntry Read()
        {
            int offset = ReadInt32(_reader);

            int attr = ReadInt32(_reader);

            //Console.WriteLine("PdbRec  offset={0};", offset);

            return new PdbRecordEntry
            {
                Attributes = attr,
                Offset = offset
            };
        }
    }
}
