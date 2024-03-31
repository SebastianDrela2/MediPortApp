using MediPort.Api.SqlCommands;
using MediPortApi.HttpProcessing;
using MediPortApi.SqlCommands;
using MediPortApi.TagProcessing;
using Microsoft.Data.SqlClient;

namespace MediPort.RestApi.Data
{
    public class SqlServerTagsStore : ITagsStore
    {
        private readonly string _connectionString;

        public SqlServerTagsStore(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("MasterDb");

            if (connectionString == null)
            {
                throw new ArgumentNullException(nameof(connectionString));
            }
          
            _connectionString = connectionString;
        }
        
        public async Task<IEnumerable<SimplifiedTag>> GetAllTags()
        {
            await using var connection = new SqlConnection(_connectionString);
            connection.Open();

            var selectTagsCommand = new SelectTagsFromTableCommand(connection);
            
            var tags = await Task.Run(() => selectTagsCommand.Execute());

            connection.Close();

            var simplifiedTagCalculator = new SimplifiedTagCalculator(tags);

            return await Task.Run(simplifiedTagCalculator.GetSimplifiedTags);
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
