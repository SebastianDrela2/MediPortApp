using MediPortApi.SqlCommands;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using Serilog;
using System.IO.Compression;

namespace MediPortApi.HttpProcessing
{
    public class StackOverflowService
    {
        private const string HttpTagFetchLink = "https://api.stackexchange.com/2.3/tags?order=desc&min=1000&sort=popular&site=stackoverflow";

        private readonly SqlConnection _connection;
        private readonly string _apiKey;
        private readonly int _tagLimit;
        private readonly ILogger _logger;

        public StackOverflowService(SqlConnection connection, string apiKey, int tagLimit, ILogger logger)
        {
            _connection = connection;
            _apiKey = apiKey;
            _tagLimit = tagLimit;
            _logger = logger;
        }

        public async Task ResetTagsAsync()
        {
            var tagsData = await GetTagsDataAsync();           

            var deleteTableCommand = new DeleteTagsTableCommand(_connection);
            deleteTableCommand.Execute();

            var populateTableCommand = new PopulateTagsTableCommand(_connection);
            populateTableCommand.Execute(tagsData);
        }

        public async Task<TagsData> GetTagsDataAsync()
        {
            var page = 1;
            var tagsData = new TagsData();
            
            while (tagsData.Tags.Count <= _tagLimit)
            {                
                var json = await GetJson(page);

                if (json is not null)
                {
                    var tagsDataReceived = JsonConvert.DeserializeObject<TagsData>(json)!;
                    tagsData.Tags.AddRange(tagsDataReceived.Tags);                                    
                    page++;                   
                }
                else
                {                   
                    break;                    
                }
            }
            
            return tagsData;
        }

        private async Task<string?> GetJson(int page)
        {
            using var httpClient = new HttpClient();

            var currentPageLink = $"{HttpTagFetchLink}&page={page}&key={_apiKey}";

            try
            {
                var response = await httpClient.GetAsync(currentPageLink);

                if (response.IsSuccessStatusCode)
                {
                    using var responseStream = await response.Content.ReadAsStreamAsync();
                    using var decompressedStream = new GZipStream(responseStream, CompressionMode.Decompress);
                    using var streamReader = new StreamReader(decompressedStream);

                    var json = await streamReader.ReadToEndAsync();

                    return json;
                }
            }
            catch(HttpRequestException ex)
            {
                _logger.Error($"Http request failed. {ex.Message}");
                return null;
            }

            _logger.Error($"Page {page} did not get fetched, Most likely provided apikey is invalid or oudated.");
            return null;
        }
    }
}
