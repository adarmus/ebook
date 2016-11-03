using System.IO;
using ebook.core.Mobi;

namespace ebook.core.Pdb.Readers.Mobi
{
    public class MobiHeaderReader : BinaryReaderBase
    {
        const int PALM_DOC_HEADER_LENGTH = 16;

        public MobiHeaderReader(BinaryReader reader)
            : base(reader)
        {
        }

        public MobiHeader Read()
        {
            SetPositionToOriginal();

            //long recordStart = _reader.BaseStream.Position;

            int compression = ReadInt16(_reader);
            int unused = ReadInt16(_reader);
            int textLength = ReadInt32(_reader);
            int recordCount = ReadInt16(_reader);
            int recordSize = ReadInt16(_reader);         // always 4096
            int encType = ReadInt16(_reader);
            int unused2 = ReadInt16(_reader);

            //Console.WriteLine("PalmDoc compression={0};", compression);

            string identifier = ReadString(_reader, 4); // = MOBI
            int headerLength = ReadInt32(_reader);
            int mobiType = ReadInt32(_reader);           // 2 = Mobipocket Book
            int encoding = ReadInt32(_reader);           // 1252 = CP1252 (WinLatin1); 65001 = UTF-8
            int unique = ReadInt32(_reader);
            int fileVersion = ReadInt32(_reader);
            int ortographicIndex = ReadInt32(_reader);
            int inflectionIndex = ReadInt32(_reader);
            int indexNames = ReadInt32(_reader);
            int indexKeys = ReadInt32(_reader);
            int index0 = ReadInt32(_reader);
            int index1 = ReadInt32(_reader);
            int index2 = ReadInt32(_reader);
            int index3 = ReadInt32(_reader);
            int index4 = ReadInt32(_reader);
            int index5 = ReadInt32(_reader);
            int firstNonBookIndex = ReadInt32(_reader);
            int fullNameOffset = ReadInt32(_reader);
            int fullNameLength = ReadInt32(_reader);
            int locale = ReadInt32(_reader);             // Low byte is main language 09= English, next byte is dialect, 08 = British, 04 = US. Thus US English is 1033, UK English is 2057.
            int inputLang = ReadInt32(_reader);
            int outputLang = ReadInt32(_reader);
            int minVersion = ReadInt32(_reader);
            int firstImageIndex = ReadInt32(_reader);
            int huffmandRecordOffset = ReadInt32(_reader);
            int huffmanRecordCount = ReadInt32(_reader);
            int huffmanTableOffset = ReadInt32(_reader);
            int huffmanTableLength = ReadInt32(_reader);
            int exthFlags = ReadInt32(_reader);
            _reader.ReadBytes(32);
            int drm1 = ReadInt32(_reader);
            int drm2 = ReadInt32(_reader);
            int drm3 = ReadInt32(_reader);
            int drm4 = ReadInt32(_reader);
            byte[] toend = _reader.ReadBytes(8);

            bool exthExists = (exthFlags & 64) == 64;

            //Console.WriteLine("MobiHdr identifier={0}, headerLength={1}, mobiType={2}, encoding={3}, exthExists={4};", identifier, headerLength, mobiType, encoding, exthExists);

            var header = new MobiHeader
            {
                Encoding = encoding,
                ExthExists = exthExists,
                HeaderLength = headerLength,
                Identifier = identifier,
                MobiType = mobiType,
                FirstImageIndex = firstImageIndex,
                FullnameLength = fullNameLength,
                FullnameOffset = fullNameOffset
            };

            return header;
        }

        public TitleReader GetTitleReader()
        {
            MobiHeader mobi = Read();

            SetPositionToOffset(mobi.FullnameOffset);

            return new TitleReader(_reader, mobi.FullnameLength);
        }


        public ExthHeaderReader GetExthHeaderReader()
        {
            MobiHeader mobi = Read();

            if (!mobi.ExthExists)
                return null;

            long newpos = _originalPosition + (long) mobi.HeaderLength + PALM_DOC_HEADER_LENGTH;
            _reader.BaseStream.Position = newpos;

            return new ExthHeaderReader(_reader);
        }
    }
}
