
namespace Shell.Mobi
{
    public class MobiHeader
    {
        public string Identifier { get; set; }

        public int HeaderLength { get; set; }

        public int MobiType { get; set; }
        
        public int Encoding { get; set; }

        public int FirstImageIndex { get; set; }

        public bool ExthExists{ get; set; }

        public int FullnameOffset { get; set; }

        public int FullnameLength { get; set; }
    }
}
