using MediPortSOAPI.Connections;
using MediPortSOAPI.ConsoleActions;
using MediPortSOAPI.HttpProcessing;
using MediPortSOAPI.Logging;
using MediPortSOAPI.SettingsUtils;
using MediPortSOAPI.SqlCommands;
using Microsoft.Data.SqlClient;
using Serilog;

namespace MediPortSOAPI
{
    internal class Program
    {
        private static readonly string _settingsPath = @$"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\MediPortSOAPI\settings.xml";
        internal static async Task Main()
        {
            CreateSettingsDirectory();

            var logger = SeriloggerFactory.GetLogger();
            var settings = RetrieveSettings(logger);

            Console.WriteLine($"Loaded settings from: {_settingsPath}");        
            var stackOverflowService = new StackOverflowService(settings.StackOverFlowApiKey, 1000, logger);
            var tagsData = await stackOverflowService.GetTagsDataAsync();
            using var connection = SqlConnectionFactory.GetSqlConnection(settings);

            OpenSqlConnection(connection, logger);
            PopulateTagsTable(connection, tagsData);

            var consoleActionCenter = new ConsoleActionCenter(connection, tagsData, stackOverflowService);
            consoleActionCenter.RenderActionList();

            RenderAndExecuteActions(consoleActionCenter, logger);

            connection.Close();
        }

        private static void CreateSettingsDirectory()
        {
            var parentDirectory = Path.GetDirectoryName(_settingsPath)!;

            if (!Directory.Exists(parentDirectory))
            {
                Directory.CreateDirectory(parentDirectory);
            }         
        }

        private static Settings RetrieveSettings(ILogger logger)
        {
            var xmlSettingsRetriever = new XmlSettingsRetriever(_settingsPath, logger);
            return xmlSettingsRetriever.GetSettings();
        }
        
        private static void OpenSqlConnection(SqlConnection connection, ILogger logger)
        {
            try
            {
                connection.Open();
            }
            catch (SqlException ex)
            {
                logger.Error($"Opening connection failed. {ex.Message}");
                Console.WriteLine("Opening connection failed.");
                Environment.Exit(0);
            }
        }

        private static void PopulateTagsTable(SqlConnection connection, TagsData tagsData)
        {
            var populateTableCommand = new PopulateTagsTableCommand(connection);
            populateTableCommand.Execute(tagsData);
        }

        private static void RenderAndExecuteActions(ConsoleActionCenter consoleActionCenter, ILogger logger)
        {
            while (true)
            {
                Console.Write($"Awaiting input: ");

                var input = int.Parse(Console.ReadLine()!);

                if (input == 5)
                {
                    break;
                }

                Console.Clear();

                consoleActionCenter.Execute(input).Wait();
                consoleActionCenter.RenderActionList();

                Console.WriteLine();
            }
        }
    }
}
