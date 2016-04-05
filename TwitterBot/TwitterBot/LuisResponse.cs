using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TwitterBot
{
    public class LuisResponse
    {
        [JsonProperty(PropertyName = "query")]
        public string Query { get; set; }

        [JsonProperty(PropertyName = "intents")]
        public Intent[] Intents { get; set; }

        [JsonProperty(PropertyName = "entities")]
        public Entity[] Entities { get; set; }
    }
}