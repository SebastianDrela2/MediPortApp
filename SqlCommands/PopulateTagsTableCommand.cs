using MediPortSOAPI.HttpProcessing;
using Microsoft.Data.SqlClient;

namespace MediPortSOAPI.SqlCommands
{
    internal class PopulateTagsTableCommand
    {
        private readonly SqlConnection _connection;      

        public PopulateTagsTableCommand(SqlConnection connection)
        {
            _connection = connection;          
        }

        public void Execute(TagsData tagsData)
        {
            var tableExistsCommand = new TagTableExistsCommand(_connection);
            var tableExists = tableExistsCommand.Execute();

            if (!tableExists)
            {
                var createTableCommand = new CreateTagsTableCommand(_connection);
                createTableCommand.Execute();
            }
            
            foreach (var tag in tagsData.Tags)
            {
                var insertCommand = new InsertIntoTagTableCommand(_connection);
                insertCommand.Execute(tag);               
            }
        }
    }
}
