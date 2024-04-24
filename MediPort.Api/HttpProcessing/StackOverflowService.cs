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
        private readonly int _pages;
        private readonly ILogger _logger;

        public StackOverflowService(SqlConnection connection, string apiKey, int pages, ILogger logger)
        {
            _connection = connection;
            _apiKey = apiKey;
            _pages = pages;
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
            var maxConcurrentTasks = 20;

            var tasks = new List<Task>();
            var semaphore = new SemaphoreSlim(maxConcurrentTasks);

            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;

            var tagsData = new TagsData();
            try
            {
                while (page <= _pages)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    tasks.Add(AppendTags(semaphore, tagsData, cancellationTokenSource, page));
                    page++;
                }

                await Task.WhenAll(tasks);
            }
            catch(OperationCanceledException)
            {
                // return current
            }

            return tagsData;
        }

        private async Task AppendTags(SemaphoreSlim semaphore, TagsData tagsData, CancellationTokenSource cancellationTokenSource, int page)
        {
            var json = await GetTagsJson(semaphore, page);

            if (json is null)
            {
                cancellationTokenSource.Cancel();
                return;
            }

            var tagsDataReceived = JsonConvert.DeserializeObject<TagsData>(json)!;

            foreach(var tag in tagsDataReceived.Tags)
            {
                tagsData.Tags.Add(tag);
            }                      
        }

        private async Task<string?> GetTagsJson(SemaphoreSlim semaphore, int page)
        {
            await semaphore.WaitAsync();

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
            finally
            {
                semaphore.Release();
            }

            _logger.Error($"Page {page} did not get fetched, Most likely provided apikey is invalid or oudated.");
            return null;
        }
    }
}
