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

        public IEnumerable<Tag> Execute(int page)
        {
            IList<Tag> tags = new List<Tag>();

            var command = new SqlCommand
            {
                CommandText = $@"SELECT *
FROM Tags
ORDER BY TagID
OFFSET ({page} - 1) * 30 ROWS
FETCH NEXT 30 ROWS ONLY;",
                Connection = _connection
            };
            
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                var tag = new Tag
                {
                    Name = reader.GetString(reader.GetOrdinal("TagName")),
                    Count = reader.GetInt32(reader.GetOrdinal("TagCount")),
                    HasSynonyms = reader.GetBoolean(reader.GetOrdinal("HasSynonyms")),
                    IsModeratorOnly = reader.GetBoolean(reader.GetOrdinal("IsModeratorOnly")),
                    IsRequired = reader.GetBoolean(reader.GetOrdinal("IsRequired"))
                };

                tags.Add(tag);
            }

            reader.Close();

            return tags;
        }
    }
}
