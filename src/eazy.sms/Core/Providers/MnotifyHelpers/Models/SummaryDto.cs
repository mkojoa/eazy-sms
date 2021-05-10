using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace eazy.sms.Core.Providers.MnotifyHelpers.Models
{
    public class SummaryDto
    {
        [JsonProperty("contacts")] public readonly int Contacts;

        [JsonProperty("credit_left")] public readonly int CreditLeft;

        [JsonProperty("credit_used")] public readonly int CreditUsed;

        [JsonProperty("_id")] public readonly string Id;

        [JsonProperty("numbers_sent")] public readonly List<string> NumbersSent;

        [JsonProperty("total_rejected")] public readonly int TotalRejected;

        [JsonProperty("total_sent")] public readonly int TotalSent;

        [JsonProperty("type")] public readonly string Type;

        [JsonConstructor]
        public SummaryDto( 
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
            Id = id;
            Type = type;
            TotalSent = totalSent;
            Contacts = contacts;
            TotalRejected = totalRejected;
            NumbersSent = numbersSent;
            CreditUsed = creditUsed;
            CreditLeft = creditLeft;
        }
    }
}
