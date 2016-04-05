using Newtonsoft.Json;

namespace TwitterBot
{
    public class Intent
    {
        [JsonProperty(PropertyName = "intent")]
        public string IntentName { get; set; }

        [JsonProperty(PropertyName = "score")]
        public float Score { get; set; }

        [JsonProperty(PropertyName = "actions")]
        public object Actions { get; set; }
    }
}