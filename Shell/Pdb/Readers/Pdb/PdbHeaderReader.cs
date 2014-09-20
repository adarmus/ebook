using System;
using System.IO;

namespace Shell.Pdb.Readers.Pdb
{
    public class PdbHeaderReader : BinaryReaderBase
    {
        public PdbHeaderReader(BinaryReader reader)
            : base(reader)
        {
        }

        public int NumberOfRecords { get; private set; }

        public string Type { get; private set; }

        public string Creator { get; private set; }

        public void Read()
        {
            string name = ReadString(_reader, 32);
            int attributes = _reader.ReadInt16();
            int version = _reader.ReadInt16();
            int createDate = _reader.ReadInt32();
            int modifiedDate = _reader.ReadInt32();
            int backupDate = _reader.ReadInt32();
            int modificationNumber = _reader.ReadInt32();
            int appInfoId = _reader.ReadInt32();
            int sortInfoId = _reader.ReadInt32();
            Type = ReadString(_reader, 4);
            Creator = ReadString(_reader, 4);
            int uniqueIdSeed = _reader.ReadInt32();
            int nextRecordListId = _reader.ReadInt32();
            NumberOfRecords = ReadInt16(_reader);

            Console.WriteLine("PdbHead name={0}; Type={1}; Creator={2}; NumberOfRecords={3}", name, Type, Creator, NumberOfRecords);
        }
    }
}
