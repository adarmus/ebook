using System;
using System.IO;

namespace ebook.core.Mobi.Pdb.Readers.Mobi
{
    public class ExthHeaderReader : BinaryReaderBase
    {
        const int OFFSET_IDENTIFIER = 0;
        const int OFFSET_HEADERLENGTH = 4;
        const int OFFSET_RECORDCOUNT = 8;
        const int INT_NOT_SET = -1;

        bool _verified;
        int _headerLength;
        int _recordCount;

        public ExthHeaderReader(BinaryReader binary)
            : base(binary)
        {
            _verified = false;
            _headerLength = INT_NOT_SET;
            _recordCount = INT_NOT_SET;
        }

        public int? ReadExthIntValue(int exthType)
        {
            VerifyStream();

            int length = FindExthRecordAndSetPosition(exthType);

            if (length == INT_NOT_SET)
                return null;

            return ReadVariableLength(length);
        }

        public string ReadExthStringValue(int exthType)
        {
            VerifyStream();

            int length = FindExthRecordAndSetPosition(exthType);

            if (length == INT_NOT_SET)
                return null;

            return ReadString(length);
        }

        int FindExthRecordAndSetPosition(int exthType)
        {
            int records = ReadInt32(OFFSET_RECORDCOUNT);

            for (int i = 0; i < records; i++)
            {
                int recordType = ReadInt32();
                int recordLength = ReadInt32() - 8;

                if (recordType == exthType)
                {
                    return recordLength;
                }
                else
                {
                    ReadBytes((int)recordLength);
                }
            }

            return INT_NOT_SET;
        }

        int ReadHeaderLength()
        {
            if (_headerLength == INT_NOT_SET)
                _headerLength = ReadInt32(OFFSET_HEADERLENGTH);

            return _headerLength;
        }

        int ReadRecordCount()
        {
            if (_recordCount == INT_NOT_SET)
                _recordCount = ReadInt32(OFFSET_RECORDCOUNT);

            return _recordCount;
        }

        void VerifyStream()
        {
            if (_verified)
                return;

            string identifier = ReadString(OFFSET_IDENTIFIER, 4); // = EXTH
            if (identifier != "EXTH")
                throw new InvalidOperationException("Invalid stream in ExthHeaderReader");

            _verified = true;
        }
    }
}
