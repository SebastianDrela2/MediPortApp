using Serilog;
using System.Xml.Linq;

namespace MediPortApi.SettingsUtils
{
    public class XmlSettingsSaver
    {
        private readonly string _settingsPath;
        private readonly ILogger _logger;
        
        public XmlSettingsSaver(string settingsPath, ILogger logger)
        {
            _settingsPath = settingsPath;
            _logger = logger;
        }

        public void SaveSettings(Settings settings)
        {
            var initialXml = new XDocument(
            new XElement("Data",
                new XElement("ServerName", settings.ServerName),
                new XElement("DatabaseName", settings.DatabaseName),
                new XElement("Username", settings.UserName),
                new XElement("Password", settings.Password),
                new XElement("StackOverFlowApiKey", settings.StackOverFlowApiKey)
               )
            );

            try
            {
                initialXml.Save(_settingsPath);
            }
            catch (IOException ex)
            {
                _logger.Error($"Failed saving file. {ex.Message}");
            }
        }
    }
}
