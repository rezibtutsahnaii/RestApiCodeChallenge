using Newtonsoft.Json;
using System;

namespace Chat.api.Model
{
    public class Message
    {
        [JsonProperty("created_at")]
        public DateTime CreatedTime { get; set; }
        [JsonProperty("message")]
        public string Text { get; set; }
        [JsonIgnore]
        public Session Creator { get; set; }
    }
}