using Newtonsoft.Json;
using System.IO.Compression;

namespace MediPortSOAPI.HttpProcessing
{
    internal class StackOverflowService
    {
        private const string HttpTagFetchLink = "https://api.stackexchange.com/2.3/tags?order=desc&min=1000&sort=popular&site=stackoverflow";

        private readonly string _apiKey;
        private readonly int _tagLimit;       

        public StackOverflowService(string apiKey, int tagLimit)
        {
            _apiKey = apiKey;
            _tagLimit = tagLimit;
        }

        public async Task<TagsData> GetTagsDataAsync()
        {
            var page = 1;
            var tagsData = new TagsData();

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
            }

            Console.WriteLine($"Finished processing tags.");

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

            return null;
        }
    }
}
