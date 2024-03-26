using MediPortSOAPI.Connections;
using MediPortSOAPI.ConsoleActions;
using MediPortSOAPI.HttpProcessing;
using MediPortSOAPI.Logging;
using MediPortSOAPI.SettingsUtils;
using MediPortSOAPI.SqlCommands;

namespace MediPortSOAPI
{
    internal class Program
    {
        private static readonly string _settingsPath = @$"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\MediPortSOAPI\settings.xml";
        internal static async Task Main()
        {
            var parentDirectory = Path.GetDirectoryName(_settingsPath)!;

            if (!Directory.Exists(parentDirectory))
            {
                Directory.CreateDirectory(parentDirectory);               
            }

            var xmlSettingsRetriever = new XmlSettingsRetriever(_settingsPath);
            var settings = xmlSettingsRetriever.GetSettings();

            Console.WriteLine($"Loaded settings from: {_settingsPath} ");
            var logger = SeriloggerFactory.GetLogger();

            var stackOverflowService = new StackOverflowService(settings.StackOverFlowApiKey, 1000, logger);           
            var tagsData = await stackOverflowService.GetTagsDataAsync();
            
            using var connection = SqlConnectionFactory.GetSqlConnection(settings);
            connection.Open();

            var populateTableCommand = new PopulateTagsTableCommand(connection);
            populateTableCommand.Execute(tagsData);
            
            var consoleActionCenter = new ConsoleActionCenter(connection, tagsData, stackOverflowService);
            consoleActionCenter.RenderActionList();

            while (true)
            {             
                Console.Write($"Awaiting input: ");

                var input = int.Parse(Console.ReadLine()!);

                if (input == 5)
                {
                    break;
                }

                Console.Clear();

                await consoleActionCenter.Execute(input);
                consoleActionCenter.RenderActionList();

                Console.WriteLine();
            }

            connection.Close();
        }      
    }
}
