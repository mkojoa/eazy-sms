using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace eazy.sms.Model
{
    public class Response
    {
        [JsonConstructor]
        public Response( 
            [JsonProperty("status")] string status,
            [JsonProperty("code")] string code,
            [JsonProperty("message")] string message,
            [JsonProperty("summary")] Summary summary
        )
        {
            this.Status = status;
            this.Code = code;
            this.Message = message;
            this.Summary = summary;
        }

        [JsonProperty("status")]
        public readonly string Status;

        [JsonProperty("code")]
        public readonly string Code;

        [JsonProperty("message")]
        public readonly string Message;

        [JsonProperty("summary")]
        public readonly Summary Summary;
    }

    public class Summary
    {
        [JsonConstructor]
        public Summary(
            [JsonProperty("_id")] string id,
            [JsonProperty("type")] string type,
            [JsonProperty("total_sent")] int totalSent,
            [JsonProperty("contacts")] int contacts,
            [JsonProperty("total_rejected")] int totalRejected,
            [JsonProperty("numbers_sent")] List<string> numbersSent,
            [JsonProperty("credit_used")] int creditUsed,
            [JsonProperty("credit_left")] int creditLeft
        )
        {
            this.Id = id;
            this.Type = type;
            this.TotalSent = totalSent;
            this.Contacts = contacts;
            this.TotalRejected = totalRejected;
            this.NumbersSent = numbersSent;
            this.CreditUsed = creditUsed;
            this.CreditLeft = creditLeft;
        }

        [JsonProperty("_id")]
        public readonly string Id;

        [JsonProperty("type")]
        public readonly string Type;

        [JsonProperty("total_sent")]
        public readonly int TotalSent;

        [JsonProperty("contacts")]
        public readonly int Contacts;

        [JsonProperty("total_rejected")]
        public readonly int TotalRejected;

        [JsonProperty("numbers_sent")]
        public readonly List<string> NumbersSent;

        [JsonProperty("credit_used")]
        public readonly int CreditUsed;

        [JsonProperty("credit_left")]
        public readonly int CreditLeft;
    }
}
