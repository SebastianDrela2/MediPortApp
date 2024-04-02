using Newtonsoft.Json;
using System.Collections.Concurrent;

namespace MediPortApi.HttpProcessing
{
    public class TagsData
    {
        [JsonProperty("items")]
        public ConcurrentBag<Tag> Tags { get; set; } = new ConcurrentBag<Tag>();
    }
}
