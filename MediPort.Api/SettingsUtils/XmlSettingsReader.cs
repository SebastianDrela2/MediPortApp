using Serilog;
using System.Xml;
using System.Xml.Linq;

namespace MediPortApi.SettingsUtils
{
    internal class XmlSettingsReader
    {
        private readonly string _settingsPath;
        private readonly ILogger _logger;

        public XmlSettingsReader(string settingsPath, ILogger logger)
        {
            _settingsPath = settingsPath;
            _logger = logger;

        }

        public Settings? GetSettings()
        {           
            if (!File.Exists(_settingsPath))
            {
                return null;
            }

            try
            {
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
            catch (XmlException ex)
            {
                _logger.Error($"Invalid xml {ex.Message}");
            }
            catch (NullReferenceException ex)
            {
                _logger.Error($"Null value {ex.Message}");
            }

            return null;
        }
    }
}
