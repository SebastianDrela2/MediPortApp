using Microsoft.Data.SqlClient;

namespace MediPortApi.SqlCommands
{
    public class DeleteTagsTableCommand
    {
        private readonly SqlConnection _connection;

        public DeleteTagsTableCommand(SqlConnection connection)
        {
            _connection = connection;
        }

        public void Execute(int id = -1)
        {
            var command = new SqlCommand
            {
                CommandText = "DELETE FROM Tags",
                Connection = _connection
            };

            if (id is not -1)
            {
                command.CommandText += $" WHERE TagId = {id}";
            }

            command.ExecuteNonQuery();
        }
    }
}
