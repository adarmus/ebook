using System.Configuration;

namespace ebook
{
    class ConfigFile
    {
        public ConfigProvider GetConfigProvider()
        {
            return new ConfigProvider
            {
                CompareFolderPath = GetStringSetting("CompareFolderPath"),
                ImportFolderPath = GetStringSetting("ImportFolderPath"),
                SqlConnectionString = GetStringSetting("SqlConnectionString"),
                SqliteFilepath = GetStringSetting("SqliteFilepath"),
                IncludeEpub = GetBoolSetting("IncludeEpub"),
                IncludeMobi = GetBoolSetting("IncludeMobi")
            };
        }

        string GetStringSetting(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        bool GetBoolSetting(string key)
        {
            bool value;
            bool.TryParse(ConfigurationManager.AppSettings[key], out value);
            return value;
        }
    }
}
