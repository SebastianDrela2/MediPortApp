using MediPortApi.SettingsUtils;
using NSubstitute;

namespace Mediport.Api.Tests
{
    [TestFixture]
    internal class SettingUtilsTestFixture
    {
        [Test]
        public void XmlSettingsRetrieverReturnsSettings()
        {
            var logger = Substitute.For<Serilog.ILogger>();
            var tempFilePath = Path.Combine(Path.GetTempPath(), "tempData.xml");
            var xmlSettingsSaver = new XmlSettingsSaver(tempFilePath, logger);

            var settings = new Settings("serverName", "databaseName", "userName", "password123", "stackKey");
            xmlSettingsSaver.SaveSettings(settings);

            try
            {               
                var xmlSettingsRetriever = new XmlSettingsRetriever(tempFilePath, logger);
                var recievedSettings = xmlSettingsRetriever.GetSettings();

                Assert.That(recievedSettings.ServerName, Is.EqualTo("serverName"));
                Assert.That(recievedSettings.DatabaseName, Is.EqualTo("databaseName"));
                Assert.That(recievedSettings.UserName, Is.EqualTo("userName"));
                Assert.That(recievedSettings.Password, Is.EqualTo("password123"));
                Assert.That(recievedSettings.StackOverFlowApiKey, Is.EqualTo("stackKey"));
            }
            finally
            {
                File.Delete(tempFilePath);
            }
        }

        [Test]
        public void SettingsAreCorrectlyMapped()
        {
            var settings = new Settings("serverName", "databaseName", "userName", "password123", "stackKey");

            Assert.That(settings.ServerName, Is.EqualTo("serverName"));
            Assert.That(settings.DatabaseName, Is.EqualTo("databaseName"));
            Assert.That(settings.UserName, Is.EqualTo("userName"));
            Assert.That(settings.Password, Is.EqualTo("password123"));
            Assert.That(settings.StackOverFlowApiKey, Is.EqualTo("stackKey"));
        }
    }
}
