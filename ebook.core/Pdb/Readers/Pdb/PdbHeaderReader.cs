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

        public PdbHeader Read()
        {
            SetPositionToOriginal();

            string name = ReadString(_reader, 32);
            int attributes = _reader.ReadInt16();
            int version = _reader.ReadInt16();
            int createDate = _reader.ReadInt32();
            int modifiedDate = _reader.ReadInt32();
            int backupDate = _reader.ReadInt32();
            int modificationNumber = _reader.ReadInt32();
            int appInfoId = _reader.ReadInt32();
            int sortInfoId = _reader.ReadInt32();
            string type = ReadString(_reader, 4);
            string creator = ReadString(_reader, 4);
            int uniqueIdSeed = _reader.ReadInt32();
            int nextRecordListId = _reader.ReadInt32();
            int numberOfRecords = ReadInt16(_reader);

            //Console.WriteLine("PdbHead name={0}; Type={1}; Creator={2}; NumberOfRecords={3}", name, type, creator, numberOfRecords);

            return new PdbHeader
            {
                Creator = creator,
                NumberOfRecords = numberOfRecords,
                Type = type
            };
        }
    }
}
