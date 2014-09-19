using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shell
{
    public class MobiRecords
    {
        readonly PdbHeader _pdbHeader;
        readonly List<PdbRecordEntry> _pdbRecords;

        public MobiRecords(PdbHeader pdbHeader, List<PdbRecordEntry> pdbRecords)
        {
            _pdbHeader = pdbHeader;
            _pdbRecords = pdbRecords;
        }

        public void Read(BinaryReader reader)
        {
            int compression = ReadInt16(reader);
            int unused = ReadInt16(reader);
            int textLength = ReadInt32(reader);
            int recordCount = ReadInt16(reader);
            int recordSize = ReadInt16(reader);         // always 4096
            int encType = ReadInt16(reader);
            int unused2 = ReadInt16(reader);

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

            bool exthExists = (exthFlags & 64) == 64;

            if (exthExists)
            {
                reader.BaseStream.Position = 1824;
                ReadExth(reader);
            }
        }

        void ReadExth(BinaryReader reader)
        {
            //byte[] offset = reader.ReadBytes(25);

            string identifier = ReadString(reader, 4); // = EXTH
            int headerLength = ReadInt32(reader);
            int recordCount = ReadInt32(reader);
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

        string ReadString(BinaryReader reader, int count)
        {
            char[] chars = reader.ReadChars(count);
            var s = new string(chars);
            return s;
        }
    }
}
