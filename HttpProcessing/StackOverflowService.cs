using Newtonsoft.Json;
using Serilog;
using System.IO.Compression;

namespace MediPortSOAPI.HttpProcessing
{
    internal class StackOverflowService
    {
        private const string HttpTagFetchLink = "https://api.stackexchange.com/2.3/tags?order=desc&min=1000&sort=popular&site=stackoverflow";

        private readonly string _apiKey;
        private readonly int _tagLimit;
        private readonly ILogger _logger;

        public StackOverflowService(string apiKey, int tagLimit, ILogger logger)
        {
            _apiKey = apiKey;
            _tagLimit = tagLimit;
            _logger = logger;
        }

        public async Task<TagsData> GetTagsDataAsync()
        {
            var page = 1;
            var tagsData = new TagsData();
            var endMessage = "Finished processing tags.";
            Console.WriteLine($"Processing tags...");

            while (tagsData.Tags.Count <= _tagLimit)
            {                
                var json = await GetJson(page);

                if (json is not null)
                {
                    var tagsDataReceived = JsonConvert.DeserializeObject<TagsData>(json)!;
                    tagsData.Tags.AddRange(tagsDataReceived.Tags);

                    Console.WriteLine($"Processed tag page {page}");                    

                    page++;                   
                }
                else
                {
                    endMessage = "Finished processing tags with errors";
                    break;                    
                }
            }

            Console.WriteLine(endMessage);

            return tagsData;
        }

        private async Task<string?> GetJson(int page)
        {
            using var httpClient = new HttpClient();

            var currentPageLink = $"{HttpTagFetchLink}&page={page}&key={_apiKey}";
            var response = await httpClient.GetAsync(currentPageLink);

            if (response.IsSuccessStatusCode)
            {              
                using var responseStream = await response.Content.ReadAsStreamAsync();
                using var decompressedStream = new GZipStream(responseStream, CompressionMode.Decompress);
                using var streamReader = new StreamReader(decompressedStream);

                var json = await streamReader.ReadToEndAsync();

                return json;
            }

            _logger.Error($"Page {page} did not get fetched, Most likely provided apikey is invalid or oudated.");
            return null;
        }
    }
}
