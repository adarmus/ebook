using System;
using System.IO;

namespace Shell.Pdb.Readers
{
    public abstract class BinaryReaderBase
    {
        readonly protected BinaryReader _reader;
        readonly long _originalPosition;

        protected BinaryReaderBase(BinaryReader reader)
        {
            _reader = reader;
            _originalPosition = reader.BaseStream.Position;
        }

        protected string ReadString(BinaryReader reader, int count)
        {
            char[] chars = reader.ReadChars(count);
            var s = new string(chars);
            return s;
        }

        protected int ReadInt16(BinaryReader reader)
        {
            byte[] offset = ReadBytesAndReverse(reader, 2);

            return BitConverter.ToInt16(offset, 0);
        }

        protected int ReadInt32(BinaryReader reader)
        {
            byte[] offset = ReadBytesAndReverse(reader, 4);

            return BitConverter.ToInt32(offset, 0);
        }

        byte[] ReadBytesAndReverse(BinaryReader reader, int count)
        {
            byte[] offset = reader.ReadBytes(count);

            Array.Reverse(offset);

            return offset;
        }
    }
}
