using Newtonsoft.Json;

namespace MediPortSOAPI.HttpProcessing
{
    internal class TagsData
    {
        [JsonProperty("items")]
        public List<Tag> Tags { get; set; } = new List<Tag>();
    }
}
