﻿using MediPortSOAPI.HttpProcessing;
using Microsoft.Data.SqlClient;

namespace MediPortSOAPI.SqlCommands
{
    internal class InsertIntoTagTableCommand
    {
        private readonly SqlConnection _connection;

        public InsertIntoTagTableCommand(SqlConnection connection)
        {
            _connection = connection;
        }

        public void Execute(Tag tag)
        {

            var command = new SqlCommand
            {
                CommandText = $@"
            INSERT INTO Tags (TagName, TagCount, HasSynonyms, IsModeratorOnly, IsRequired)
            SELECT @TagName, @TagCount, @HasSynonyms, @IsModeratorOnly, @IsRequired
            WHERE NOT EXISTS (
                SELECT 1 FROM Tags WHERE TagName = @TagName
            )",
                Connection = _connection
            };

            command.Parameters.AddWithValue("@TagName", tag.Name);
            command.Parameters.AddWithValue("@TagCount", tag.Count);          
            command.Parameters.AddWithValue("@HasSynonyms", tag.HasSynonyms);
            command.Parameters.AddWithValue("@IsModeratorOnly", tag.IsModeratorOnly);
            command.Parameters.AddWithValue("@IsRequired", tag.IsRequired);           

            command.ExecuteNonQuery();        
        }
    }
}