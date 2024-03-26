namespace MediPortSOAPI.SettingsUtils
{
    internal class XmlSettingsRetriever
    {
        private string _settingsPath;

        public XmlSettingsRetriever(string settingsPath)
        {
            _settingsPath = settingsPath;
        }

        public Settings GetSettings()
        {
            var xmlSettingsReader = new XmlSettingsReader(_settingsPath);
            var settings = xmlSettingsReader.GetSettings();

            if (settings is null)
            {
                Console.WriteLine($"Provide empty username and password if desired authentication type is a Windows Authentication.");

                var xmlSettingsSaver = new XmlSettingsSaver(_settingsPath);
                xmlSettingsSaver.SaveSettings();

                settings = xmlSettingsReader.GetSettings();
            }

            return settings;
        }
    }
}
