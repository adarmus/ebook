using System;
using System.IO;

namespace Shell.Pdb.Readers.Pdb
{
    public class PdbHeader
    {
        public int NumberOfRecords { get; private set; }

        public string Type { get; private set; }

        public string Creator { get; private set; }

        public void Read(BinaryReader reader)
        {
            string name = ReadString(reader, 32);
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
            NumberOfRecords = ReadInt16(reader);

            Console.WriteLine("PdbHead name={0}; Type={1}; Creator={2}; NumberOfRecords={3}", name, Type, Creator, NumberOfRecords);
        }

        string ReadString(BinaryReader reader, int count)
        {
            char[] chars = reader.ReadChars(count);
            var s = new string(chars);
            return s;
        }

        int ReadInt16(BinaryReader reader)
        {
            byte[] offset = reader.ReadBytes(2);

            Array.Reverse(offset);

            return BitConverter.ToInt16(offset, 0);
        }
    }
}
