using MediPortApi.HttpProcessing;
using NSubstitute;
using NUnit.Framework.Internal;
using System.Reflection;

namespace Mediport.Api.Tests
{
    [TestFixture]
    internal class HttpProcessingTestFixture
    {            
        [Test]
        public async Task TagsGetFetchedWithLessThan1000ResultsForInvalidApiKey()
        {           
            var logger = Substitute.For<Serilog.ILogger>();
            var stackOverflowService = new StackOverflowService(null, "", 40, logger);
            var tagsData = await stackOverflowService.GetTagsDataAsync();

            Assert.That(tagsData.Tags.Count < 1000);
        }

        [Test]
        public async Task TagsGetFetchedWith1000OrMoreResultsForValidApiKey()
        {
            var apiKey = DecodeEncodedApiKey();
            var logger = Substitute.For<Serilog.ILogger>();

            var stackOverflowService = new StackOverflowService(null, apiKey, 40, logger);
            var tagsData = await stackOverflowService.GetTagsDataAsync();

            Assert.That(tagsData.Tags.Count >= 1000);
        }


        // In actual production this would be replaced with either AES or RSA encryption.
        // Or we would get key from development database.
        private string DecodeEncodedApiKey()
        {
            var encodedKey = ReadResource("MediPort.Api.Tests.encryptedApiKey.txt");
            var base64EncodedBytes = Convert.FromBase64String(encodedKey);

            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);           
        }

        private string ReadResource(string resourceName)
        {
            var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);

            using var reader = new StreamReader(stream!);
            var resource = reader.ReadToEnd();

            return resource;
        }
    }
}
