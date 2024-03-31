using MediPortApi.HttpProcessing;
using Microsoft.Data.SqlClient;

namespace MediPort.Api.SqlCommands
{
    public class SelectTagsFromTableCommand
    {
        private readonly SqlConnection _connection;

        public SelectTagsFromTableCommand(SqlConnection connection)
        {
            _connection = connection;
        }

        public IEnumerable<Tag> Execute(int id = -1)
        {
            IList<Tag> tags = new List<Tag>();

            var command = new SqlCommand
            {
                CommandText = @"SELECT TagName AS [name], 
TagCount AS [count], 
HasSynonyms AS [has_synonyms], 
IsModeratorOnly AS 
[is_moderator_only], 
IsRequired AS [is_required] 
FROM Tags",
                Connection = _connection
            };

            if (id is not -1)
            {
                command.CommandText += $" WHERE TagId = {id}";
            }

            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                var tag = new Tag
                {
                    Name = reader.GetString(reader.GetOrdinal("name")), // assuming name is string
                    Count = reader.GetInt32(reader.GetOrdinal("count")), // assuming count is int
                    HasSynonyms = reader.GetBoolean(reader.GetOrdinal("has_synonyms")),
                    IsModeratorOnly = reader.GetBoolean(reader.GetOrdinal("is_moderator_only")),
                    IsRequired = reader.GetBoolean(reader.GetOrdinal("is_required"))
                };

                tags.Add(tag);
            }

            reader.Close();

            return tags;
        }
    }
}
