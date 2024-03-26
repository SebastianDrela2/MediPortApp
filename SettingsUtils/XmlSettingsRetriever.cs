using Serilog;

namespace MediPortSOAPI.SettingsUtils
{
    internal class XmlSettingsRetriever
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

            if (settings is null)
            {
                Console.WriteLine($"Provide empty username and password if desired authentication type is a Windows Authentication.");

                var xmlSettingsSaver = new XmlSettingsSaver(_settingsPath , _logger);
                xmlSettingsSaver.SaveSettings();

                settings = xmlSettingsReader.GetSettings();
            }

            return settings!;
        }
    }
}
