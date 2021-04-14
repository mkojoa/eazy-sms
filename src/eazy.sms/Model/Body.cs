using Newtonsoft.Json;

namespace eazy.sms.Model
{
    public class Body
    {
        /// <summary>
        ///     The actual text body of the message.
        /// </summary>
        [JsonProperty("content")]
        public string Content { get; set; }
    }
}