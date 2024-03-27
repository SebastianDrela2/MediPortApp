using MediPortApi.HttpProcessing;
using MediPortApi.SqlCommands;
using Microsoft.Data.SqlClient;

namespace Mediport.Api.Tests
{
    [TestFixture]
    internal class SqlCommandsTestFixture
    {
        private const string _testConnectionString = @"Data Source=localhost\MSSQLSERVER01;Initial Catalog=master;Integrated Security=True;Connect Timeout=30;Trust Server Certificate=True;Application Intent=ReadWrite";

        private SqlConnection _connection;

        [SetUp]
        public void SetUp()
        {
            _connection = new SqlConnection(_testConnectionString);
            _connection.Open();

            var command = new SqlCommand
            {
                Connection = _connection,
                CommandText = "DROP TABLE IF EXISTS Tags"
            };

            command.ExecuteNonQuery();
        }

        [TearDown]
        public void TearDown()
        {
            _connection.Close();
            _connection.Dispose();
        }

        [Test]
        public void TagTableExistsCommandChecksIfTagsTableExists()
        {          
            var tagTableExistsCommand = new TagTableExistsCommand(_connection);
            Assert.That(tagTableExistsCommand.Execute(), Is.EqualTo(false));
                    
            var createTagTableCommand = new CreateTagsTableCommand(_connection);
            createTagTableCommand.Execute();
          
            Assert.That(tagTableExistsCommand.Execute(), Is.EqualTo(true));
        }

        [Test]
        public void PopulateTagTableCommandPopulatesTableCorrectly()
        {
            var createTagTableCommand = new CreateTagsTableCommand(_connection);
            createTagTableCommand.Execute();

            var limiter = 100;
            var mockTagData = new MockTagData(limiter).GetMockTagsData();

            var populateTagTableCommand = new PopulateTagsTableCommand(_connection);
            populateTagTableCommand.Execute(mockTagData);

            var command = new SqlCommand()
            {
                Connection = _connection,
                CommandText = "SELECT Count(TagName) FROM Tags;"
            };

            var amountOfRows = command.ExecuteScalar();

            Assert.That(amountOfRows, Is.EqualTo(limiter + 1));
        }     
    }   
}
