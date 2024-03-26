using Microsoft.Data.SqlClient;

namespace MediPortSOAPI.SqlCommands
{
    internal class TagTableExistsCommand
    {
        private readonly SqlConnection _connection;

        public TagTableExistsCommand(SqlConnection connection)
        {
            _connection = connection;
        }

        public bool Execute()
        {

            var command = new SqlCommand
            {
                CommandText = @"IF EXISTS (SELECT 1 
           FROM INFORMATION_SCHEMA.TABLES 
           WHERE TABLE_TYPE='BASE TABLE' 
           AND TABLE_NAME='Tags') 
   SELECT 1 AS res ELSE SELECT 0 AS res;",
                Connection = _connection
            };

           var result = (int) command.ExecuteScalar();

            if (result is 1)
            {
                return true;
            }

            return false;
        }
    }
}
