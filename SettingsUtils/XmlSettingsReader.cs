using System.Xml.Linq;

namespace MediPortSOAPI.SettingsUtils
{
    internal class XmlSettingsReader
    {
        private readonly string _settingsPath;

        public XmlSettingsReader(string settingsPath)
        {
            _settingsPath = settingsPath;
        }

        public Settings? GetSettings()
        {           
            if (!File.Exists(_settingsPath))
            {
                return null;
            }

            var xDoc = XDocument.Load(_settingsPath);
            var dataElement = xDoc.Element("Data");

            var serverName = dataElement?.Element("ServerName")?.Value!;
            var databaseName = dataElement?.Element("DatabaseName")?.Value!;
            var userName = dataElement?.Element("Username")?.Value!;
            var password = dataElement?.Element("Password")?.Value!;
            var stackOverflowApiKey = dataElement?.Element("StackOverFlowApiKey")?.Value!;

            var settings = new Settings(serverName, databaseName, userName, password, stackOverflowApiKey);

            return settings;
        }
    }
}
