using System;
using System.IO;

namespace Shell.Pdb.Readers.Pdb
{
    public class PdbRecordEntryReader : BinaryReaderBase
    {
        public int Offset;
        public int UniqueId;
        public int Attributes;

        public PdbRecordEntryReader(BinaryReader reader)
            : base(reader)
        {
        }

        public void Read()
        {
            Offset = ReadInt32(_reader);

            //Offset = reader.ReadInt32();
            //byte attr = reader.ReadByte();
            byte[] attr = _reader.ReadBytes(4);

            Console.WriteLine("PdbRec  offset={0};", Offset);
        }
    }
}
