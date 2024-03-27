using Serilog;

namespace MediPortApi.SettingsUtils
{
    public class XmlSettingsRetriever
    {
        private readonly string _settingsPath;
        private readonly ILogger _logger;

        public XmlSettingsRetriever(string settingsPath, ILogger logger)
        {
            _settingsPath = settingsPath;
            _logger = logger;
        }

        public Settings GetSettings()
        {
            var xmlSettingsReader = new XmlSettingsReader(_settingsPath, _logger);
            var settings = xmlSettingsReader.GetSettings();         
            return settings!;
        }
    }
}
