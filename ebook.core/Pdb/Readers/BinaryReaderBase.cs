using System;
using System.IO;

namespace Shell.Pdb.Readers
{
    public abstract class BinaryReaderBase
    {
        readonly protected BinaryReader _reader;
        readonly protected long _originalPosition;

        protected BinaryReaderBase(BinaryReader reader)
        {
            _reader = reader;
            _originalPosition = reader.BaseStream.Position;
        }

        protected void SetPositionToOriginal()
        {
            _reader.BaseStream.Position = _originalPosition;
        }

        protected int ReadVariableLength(int bytes)
        {
            // EXTH record values do not have a fixed length, so just read whatever length is provided.

            switch (bytes)
            {
                case 1:
                    return _reader.ReadByte();

                case 2:
                    return ReadInt16();

                case 4:
                    return ReadInt32();

                default:
                    throw new NotSupportedException();
            }
        }


        protected void ReadBytes(int count)
        {
            byte[] bytes = _reader.ReadBytes(count);
        }

        protected string ReadString(int count)
        {
            char[] chars = _reader.ReadChars(count);
            var s = new string(chars);
            return s;
        }

        protected string ReadString(int offset, int count)
        {
            SetPositionToOffset(offset);

            return ReadString(count);
        }

        protected string ReadString(BinaryReader reader, int count)
        {
            char[] chars = reader.ReadChars(count);
            var s = new string(chars);
            return s;
        }

        protected int ReadInt16()
        {
            byte[] b = ReadBytesAndReverse(_reader, 2);

            return BitConverter.ToInt16(b, 0);
        }

        protected int ReadInt16(int offset)
        {
            SetPositionToOffset(offset);

            return ReadInt16();
        }

        protected int ReadInt16(BinaryReader reader)
        {
            byte[] offset = ReadBytesAndReverse(reader, 2);

            return BitConverter.ToInt16(offset, 0);
        }

        protected int ReadInt32()
        {
            byte[] bytes = ReadBytesAndReverse(_reader, 4);

            return BitConverter.ToInt32(bytes, 0);
        }

        protected int ReadInt32(int offset)
        {
            SetPositionToOffset(offset);

            return ReadInt32();
        }

        protected int ReadInt32(BinaryReader reader)
        {
            byte[] bytes = ReadBytesAndReverse(reader, 4);

            return BitConverter.ToInt32(bytes, 0);
        }

        byte[] ReadBytesAndReverse(BinaryReader reader, int count)
        {
            byte[] offset = reader.ReadBytes(count);

            Array.Reverse(offset);

            return offset;
        }

        protected void SetPositionToOffset(int offset)
        {
            _reader.BaseStream.Position = _originalPosition + offset;
        }
    }
}
