using System.Xml.Linq;

namespace MediPortSOAPI.SettingsUtils
{
    internal class XmlSettingsSaver
    {
        private readonly string _settingsPath;
        
        public XmlSettingsSaver(string settingsPath)
        {
            _settingsPath = settingsPath;
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

            initialXml.Save(_settingsPath);
        }
    }
}
