using MediPortApi.Connections;
using MediPortApi.ConsoleActions;
using MediPortApi.HttpProcessing;
using MediPortApi.Logging;
using MediPortApi.SettingsUtils;
using MediPortApi.SqlCommands;
using Microsoft.Data.SqlClient;
using Serilog;

namespace MediPort.Console
{
    internal class Program
    {
        private static readonly string _settingsPath = @$"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\MediPortSOAPI\settings.xml";
        internal static async Task Main()
        {
            CreateSettingsDirectory();

            var logger = SerilogFactory.GetLogger();
            var settings = RetrieveSettings(logger);

            using var connection = SqlConnectionFactory.GetSqlConnection(settings);

            var stackOverflowService = new StackOverflowService(connection, settings.StackOverFlowApiKey, 40, logger);
            var tagsData = await stackOverflowService.GetTagsDataAsync();            

            OpenSqlConnection(connection, logger);
            PopulateTagsTable(connection, tagsData);

            var consoleActionCenter = new ConsoleActionCenter(tagsData, stackOverflowService);
            consoleActionCenter.RenderActionList();

            RenderAndExecuteActions(consoleActionCenter);         
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
                Environment.Exit(0);
            }
        }

        private static void PopulateTagsTable(SqlConnection connection, TagsData tagsData)
        {
            var populateTableCommand = new PopulateTagsTableCommand(connection);
            populateTableCommand.Execute(tagsData);
        }

        private static void RenderAndExecuteActions(ConsoleActionCenter consoleActionCenter)
        {
            while (true)
            {
               System.Console.Write($"Awaiting input: ");

                var input = int.Parse(System.Console.ReadLine()!);

                if (input == 5)
                {
                    break;
                }

                System.Console.Clear();

                consoleActionCenter.Execute(input).Wait();
                consoleActionCenter.RenderActionList();

                System.Console.WriteLine();
            }
        }
    }
}
