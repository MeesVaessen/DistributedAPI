using Newtonsoft.Json;

namespace Api.Model
{

    public class Message
    {
        public Message(string messageType, string wsToken, string messageContent)
        {
            Type = messageType;
            WsToken = wsToken;
            Content = messageContent;
        }

        [JsonProperty("Type")]
        public string Type { get; set; }

        [JsonProperty("Content")]
        public string Content { get; set; }

        [JsonProperty("Tried_Passwords")]
        public int TriedPasswords { get; set; }

        [JsonProperty("Elapsed_Time")]
        public double ElapsedTime { get; set; }
        
        [JsonProperty("WsToken")]
        public string WsToken { get; set; }
    }
}