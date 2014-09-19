using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shell
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

            return BitConverter.ToInt32(offset, 0);
        }
    }
}
