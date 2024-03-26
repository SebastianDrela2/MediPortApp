using Newtonsoft.Json;

namespace MediPortApi.HttpProcessing
{
    public class Tag
    {
        [JsonProperty("has_synonyms")]
        public bool HasSynonyms { get; set; }

        [JsonProperty("is_moderator_only")]
        public bool IsModeratorOnly { get; set; }

        [JsonProperty("is_required")]
        public bool IsRequired { get; set; }

        [JsonProperty("count")]
        public int Count { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }       
    }
}