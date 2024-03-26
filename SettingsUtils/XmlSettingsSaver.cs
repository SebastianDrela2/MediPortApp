using Serilog;
using System.Xml.Linq;

namespace MediPortSOAPI.SettingsUtils
{
    internal class XmlSettingsSaver
    {
        private readonly string _settingsPath;
        private readonly ILogger _logger;
        
        public XmlSettingsSaver(string settingsPath, ILogger logger)
        {
            _settingsPath = settingsPath;
            _logger = logger;
        }

        public void SaveSettings()
        {
            var initialXml = new XDocument(
             new XElement("Data",
                 new XElement("ServerName"),
                 new XElement("DatabaseName"),
                 new XElement("Username"),
                 new XElement("Password"),
                 new XElement("StackOverFlowApiKey")                
             )
         );

            foreach (var element in initialXml.Root.Elements())
            {
                Console.Write($"Enter {element.Name}: ");

                var inputValue = Console.ReadLine();
                element.Value = inputValue;
            }

            try
            {
                initialXml.Save(_settingsPath);
            }
            catch(IOException ex)
            {
                _logger.Error($"Failed saving file. {ex.Message}");
            }
        }
    }
}
