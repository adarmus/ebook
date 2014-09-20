
namespace Shell.Pdb
{
    public class MobiHeader
    {
        public string Identifier { get; set; }

        public int HeaderLength { get; set; }

        public int MobiType { get; set; }
        
        public int Encoding { get; set; }

        public int FirstImageIndex { get; set; }

        public bool ExthExists{ get; set; }
    }
}
