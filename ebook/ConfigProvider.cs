
namespace ebook
{
    class ConfigProvider
    {
        public string ImportFolderPath { get; set; }

        public string CompareFolderPath { get; set; }

        public string SqlConnectionString { get; set; }

        public string SqliteFilepath { get; set; }

        public bool IncludeEpub { get; set; }

        public bool IncludeMobi { get; set; }
    }
}
