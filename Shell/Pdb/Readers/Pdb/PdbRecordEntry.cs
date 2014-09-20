using System;
using System.IO;

namespace Shell.Pdb.Readers.Pdb
{
    public class PdbRecordEntry
    {
        public int Offset;
        public int UniqueId;
        public int Attributes;

        public void Read(BinaryReader reader)
        {
            Offset = ReadInt32(reader);

            //Offset = reader.ReadInt32();
            //byte attr = reader.ReadByte();
            byte[] attr = reader.ReadBytes(4);

            Console.WriteLine("PdbRec  offset={0};", Offset);

        }

        int ReadInt32(BinaryReader reader)
        {
            byte[] offset = reader.ReadBytes(4);

            Array.Reverse(offset);

            return BitConverter.ToInt32(offset, 0);
        }

        int ReadInt16(BinaryReader reader)
        {
            byte[] offset = reader.ReadBytes(2);

            Array.Reverse(offset);

            return BitConverter.ToInt16(offset, 0);
        }
    }
}
