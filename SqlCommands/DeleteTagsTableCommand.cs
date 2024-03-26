using Microsoft.Data.SqlClient;

namespace MediPortSOAPI.SqlCommands
{
    internal class DeleteTagsTableCommand
    {
        private readonly SqlConnection _connection;

        public DeleteTagsTableCommand(SqlConnection connection)
        {
            _connection = connection;
        }

        public void Execute()
        {
            var command = new SqlCommand
            {
                CommandText = "DELETE FROM Tags",
                Connection = _connection
            };

            command.ExecuteNonQuery();
        }
    }
}
