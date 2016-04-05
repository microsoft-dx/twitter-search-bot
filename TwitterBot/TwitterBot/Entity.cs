using Newtonsoft.Json;

namespace TwitterBot
{
    public class Entity
    {
        [JsonProperty(PropertyName = "entity")]
        public string EntityName { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "startIndex")]
        public int StartIndex { get; set; }

        [JsonProperty(PropertyName = "endIndex")]
        public int EndIndex { get; set; }

        [JsonProperty(PropertyName = "score")]
        public float Score { get; set; }
    }
}