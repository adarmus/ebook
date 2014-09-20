using System;
using System.Collections.Generic;
using System.IO;
using Shell.Pdb.Readers.Pdb;

namespace Shell.Pdb.Readers.Mobi
{
    public class MobiRecordsReader : BinaryReaderBase
    {
        readonly PdbHeaderReader _pdbHeader;
        readonly List<PdbRecordEntryReader> _pdbRecords;

        public MobiRecordsReader(BinaryReader reader, PdbHeaderReader pdbHeader, List<PdbRecordEntryReader> pdbRecords)
            : base(reader)
        {
            _pdbHeader = pdbHeader;
            _pdbRecords = pdbRecords;
        }

        public void Read(BinaryReader reader)
        {
            long recordStart = reader.BaseStream.Position;

            int compression = ReadInt16(reader);
            int unused = ReadInt16(reader);
            int textLength = ReadInt32(reader);
            int recordCount = ReadInt16(reader);
            int recordSize = ReadInt16(reader);         // always 4096
            int encType = ReadInt16(reader);
            int unused2 = ReadInt16(reader);

            Console.WriteLine("PalmDoc compression={0};", compression);

            string identifier = ReadString(reader, 4); // = MOBI
            int headerLength = ReadInt32(reader);
            int mobiType = ReadInt32(reader);           // 2 = Mobipocket Book
            int encoding = ReadInt32(reader);           // 1252 = CP1252 (WinLatin1); 65001 = UTF-8
            int unique = ReadInt32(reader);
            int fileVersion = ReadInt32(reader);
            int ortographicIndex = ReadInt32(reader);
            int inflectionIndex = ReadInt32(reader);
            int indexNames = ReadInt32(reader);
            int indexKeys = ReadInt32(reader);
            int index0 = ReadInt32(reader);
            int index1 = ReadInt32(reader);
            int index2 = ReadInt32(reader);
            int index3 = ReadInt32(reader);
            int index4 = ReadInt32(reader);
            int index5 = ReadInt32(reader);
            int firstNonBookIndex = ReadInt32(reader);
            int fullNameOffset = ReadInt32(reader);
            int fullNameLength = ReadInt32(reader);
            int locale = ReadInt32(reader);             // Low byte is main language 09= English, next byte is dialect, 08 = British, 04 = US. Thus US English is 1033, UK English is 2057.
            int inputLang = ReadInt32(reader);
            int outputLang = ReadInt32(reader);
            int minVersion = ReadInt32(reader);
            int firstImageIndex = ReadInt32(reader);
            int huffmandRecordOffset = ReadInt32(reader);
            int huffmanRecordCount = ReadInt32(reader);
            int huffmanTableOffset = ReadInt32(reader);
            int huffmanTableLength = ReadInt32(reader);
            int exthFlags = ReadInt32(reader);
            reader.ReadBytes(32);
            int drm1 = ReadInt32(reader);
            int drm2 = ReadInt32(reader);
            int drm3 = ReadInt32(reader);
            int drm4 = ReadInt32(reader);
            byte[] toend = reader.ReadBytes(8);

            bool exthExists = (exthFlags & 64) == 64;

            Console.WriteLine("MobiHdr identifier={0}, headerLength={1}, mobiType={2}, encoding={3}, exthExists={4};", identifier, headerLength, mobiType, encoding, exthExists);

            if (exthExists)
            {
                long newpos = recordStart + (long)headerLength + 16;
                reader.BaseStream.Position = newpos;    // 1304; // 1824;
                ReadExth(reader);
            }
        }

        void ReadExth(BinaryReader reader)
        {
            string identifier = ReadString(reader, 4); // = EXTH
            int headerLength = ReadInt32(reader);
            int recordCount = ReadInt32(reader);
        }
    }
}
