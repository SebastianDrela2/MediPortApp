using Microsoft.Data.SqlClient;

namespace MediPortApi.SqlCommands
{
    internal class CreateTagsTableCommand
    {
        private readonly SqlConnection _connection;

        public CreateTagsTableCommand(SqlConnection connection)
        {
            _connection = connection;
        }
        
        public void Execute()
        {
            var command = new SqlCommand
            {
                CommandText = @"CREATE TABLE Tags (
    TagID INT IDENTITY(1,1) PRIMARY KEY,
    TagName VARCHAR(255),
    TagCount INT,
    HasSynonyms BIT,
    IsModeratorOnly BIT,
    IsRequired BIT
    );",
                Connection = _connection
            };

            command.ExecuteNonQuery();
        }       
    }
}
