using MediPortApi.Connections;
using MediPortApi.SettingsUtils;
using Microsoft.Data.SqlClient;

namespace Mediport.Api.Tests
{
    [TestFixture]
    public class ConnectionsTestFixture
    {      
        [Test]
        public void SqlAuthenticationConnectionStringIsCorrectlyCreatedFromSettings()
        {
            var settings = new Settings("192.168.1.1", "database", "username", "pass123", "");

            var builderRetriever = new XmlConnectionStringBuilderRetriever();
            var connectionStringBuilder = builderRetriever.GetSqlConnectionStringBuilder(settings);
        
            Assert.That(connectionStringBuilder.DataSource, Is.EqualTo("192.168.1.1"));
            Assert.That(connectionStringBuilder.InitialCatalog, Is.EqualTo("database"));
            Assert.That(connectionStringBuilder.UserID, Is.EqualTo("username"));
            Assert.That(connectionStringBuilder.Password, Is.EqualTo("pass123"));
            Assert.That(connectionStringBuilder.Authentication, Is.EqualTo(SqlAuthenticationMethod.SqlPassword));
            Assert.That(connectionStringBuilder.IntegratedSecurity, Is.EqualTo(false));
        }

        [Test]
        public void WindowsAuthenticationConnectionStringIsCorrectlyCreatedFromSettings()
        {
            var settings = new Settings("192.168.1.1", "database", "", "", "");

            var builderRetriever = new XmlConnectionStringBuilderRetriever();
            var connectionStringBuilder = builderRetriever.GetSqlConnectionStringBuilder(settings);

            Assert.That(connectionStringBuilder.DataSource, Is.EqualTo("192.168.1.1"));
            Assert.That(connectionStringBuilder.InitialCatalog, Is.EqualTo("database"));
            Assert.That(connectionStringBuilder.UserID, Is.EqualTo(string.Empty));
            Assert.That(connectionStringBuilder.Password, Is.EqualTo(string.Empty));
            Assert.That(connectionStringBuilder.Authentication, Is.EqualTo(SqlAuthenticationMethod.NotSpecified));
            Assert.That(connectionStringBuilder.IntegratedSecurity, Is.EqualTo(true));
        }
    }
}