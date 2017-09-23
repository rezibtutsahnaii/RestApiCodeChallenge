using Newtonsoft.Json;
using System;

namespace Chat.api.Model
{
    public class Session
    {
        [JsonProperty("created_at")]
        public DateTime CreatedTime { get; set; }
        [JsonIgnore]
        public User Creator { get; set; }
        [JsonIgnore]
        public string SecurityToken { get; set; }
    }
}