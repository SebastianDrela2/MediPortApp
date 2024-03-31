using MediPortApi.HttpProcessing;
using Microsoft.Data.SqlClient;

namespace MediPort.Api.SqlCommands
{
    public class UpdateTagInTableCommand
    {
        private readonly SqlConnection _connection;

        public UpdateTagInTableCommand(SqlConnection connection)
        {
            _connection = connection;
        }

        public void Execute(int id, Tag tag)
        {
            var command = new SqlCommand
            {
                CommandText = $@"UPDATE Tags
SET TagName = '{tag.Name}',
TagCount = {tag.Count},
IsModeratorOnly = {Convert.ToInt32(tag.IsModeratorOnly)},
IsRequired = {Convert.ToInt32(tag.IsRequired)}
WHERE TagId = '{id}'",

                Connection = _connection
            };

            command.ExecuteScalar();        
        }
    }
}
