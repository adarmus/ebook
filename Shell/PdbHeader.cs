using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shell
{
    public class PdbHeader
    {
        public int NumberOfRecords { get; private set; }

        public string Type { get; private set; }

        public string Creator { get; private set; }

        public void Read(BinaryReader reader)
        {
            string identifier = ReadString(reader, 32);
            int attributes = reader.ReadInt16();
            int version = reader.ReadInt16();
            int createDate = reader.ReadInt32();
            int modifiedDate = reader.ReadInt32();
            int backupDate = reader.ReadInt32();
            int modificationNumber = reader.ReadInt32();
            int appInfoId = reader.ReadInt32();
            int sortInfoId = reader.ReadInt32();
            Type = ReadString(reader, 4);
            Creator = ReadString(reader, 4);
            int uniqueIdSeed = reader.ReadInt32();
            int nextRecordListId = reader.ReadInt32();

            byte[] tmp = reader.ReadBytes(2);
            Array.Reverse(tmp);
            NumberOfRecords = BitConverter.ToInt16(tmp, 0);

            //NumberOfRecords = reader.ReadInt16();
        }

        string ReadString(BinaryReader reader, int count)
        {
            char[] chars = reader.ReadChars(count);
            var s = new string(chars);
            return s;
        }
    }
}
