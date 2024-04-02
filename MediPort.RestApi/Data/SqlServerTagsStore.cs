using MediPort.Api.SqlCommands;
using MediPortApi.HttpProcessing;
using MediPortApi.Logging;
using MediPortApi.SqlCommands;
using MediPortApi.TagProcessing;
using Microsoft.Data.SqlClient;
using Serilog;

namespace MediPort.RestApi.Data
{
    public class SqlServerTagsStore : ITagsStore
    {
        private readonly string _connectionString;
        private readonly Serilog.ILogger _logger;

        public SqlServerTagsStore()
        {
            // lord save me for storing credintials here

            var server = "ms-sql-server";
            var database = "master";
            var port = "1433";
            var user = "SA";
            var password = "Password1234";

            var connectionString = 
                $"Server={server},{port};Initial Catalog={database};User ID={user};Password={password};Trust Server Certificate=True";

            using var connection = new SqlConnection(connectionString);
            connection.Open();

            var tableExistsCommand = new TagTableExistsCommand(connection);
            var tableExists = tableExistsCommand.Execute();

            if (!tableExists)
            {
                var createTableCommand = new CreateTagsTableCommand(connection);
                createTableCommand.Execute();
            }

            _connectionString = connectionString;
            _logger = SerilogFactory.GetLogger();
        }

        public async Task RefreshAllTags(string apiKey)
        {
            await using var connection = new SqlConnection(_connectionString);
            connection.Open();
            
            var stackOverflowService = new StackOverflowService(connection, apiKey, 40, _logger);
            await stackOverflowService.ResetTagsAsync();         
        }

        public async Task<IEnumerable<SimplifiedTag>> GetTagsSorted(int page, SortOption sortOption)
        {
            await using var connection = new SqlConnection(_connectionString);
            connection.Open();

            var selectTagsCommand = new SelectTagsFromTableCommand(connection);

            var tags = await Task.Run(() => selectTagsCommand.Execute(page));
            var simplifiedTagCalculator = new SimplifiedTagCalculator(tags);

            return await Task.Run(() => simplifiedTagCalculator.GetSortedSimplifiedTags(sortOption));

        }
        
        public async Task<SimplifiedTag> GetTag(int id)
        {
            await using var connection = new SqlConnection(_connectionString);
            connection.Open();

            var selectTagsCommand = new SelectTagsFromTableCommand(connection);
            var tags = await Task.Run(() => selectTagsCommand.Execute(id));

            connection.Close();

            var simplifiedTagCalculator = new SimplifiedTagCalculator(tags);

            return await Task.Run(() => simplifiedTagCalculator.GetSimplifiedTags().FirstOrDefault());
        }

        public async Task<SimplifiedTag> CreateTag(Tag tag)
        {
            await using var connection = new SqlConnection(_connectionString);
            connection.Open();

            var insertTagsCommand = new InsertIntoTagTableCommand(connection);

            await Task.Run(() => insertTagsCommand.Execute(tag));

            var simplifiedTagCalculator = new SimplifiedTagCalculator();

            return await Task.Run(() => simplifiedTagCalculator.GetSimplifiedTag(tag));
        }

        public async Task UpdateTag(int id, Tag tag)
        {
            await using var connection = new SqlConnection(_connectionString);
            connection.Open();

            var updateTagCommand = new UpdateTagInTableCommand(connection);
            updateTagCommand.Execute(id, tag);
        }

        public async Task DeleteTag(int id)
        {
            await using var connection = new SqlConnection(_connectionString);
            connection.Open();

            var deleteTagsCommand = new DeleteTagsTableCommand(connection);
            deleteTagsCommand.Execute(id);

        }    
    }
}
