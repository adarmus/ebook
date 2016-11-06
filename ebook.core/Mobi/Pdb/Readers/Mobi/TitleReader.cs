using System.IO;

namespace ebook.core.Mobi.Pdb.Readers.Mobi
{
    public class TitleReader : BinaryReaderBase
    {
        readonly int _titleLength;

        public TitleReader(BinaryReader binary, int titleLength)
            : base(binary)
        {
            _titleLength = titleLength;
        }

        public string ReadTitle()
        {
            return ReadString(_titleLength);
        }
    }
}
