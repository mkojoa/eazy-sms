using Newtonsoft.Json;

namespace eazy.sms.Core.Providers.MnotifyHelpers.Models
{
    public class ResponseDto
    {
        [JsonProperty("code")] public readonly string Code;

        [JsonProperty("message")] public readonly string Message;

        [JsonProperty("status")] public readonly string Status;

        [JsonProperty("summary")] public readonly SummaryDto Summary;

        [JsonConstructor]
        public ResponseDto(
            [JsonProperty("status")] string status,
            [JsonProperty("code")] string code,
            [JsonProperty("message")] string message,
            [JsonProperty("summary")] SummaryDto summary
        )
        {
            Status = status;
            Code = code;
            Message = message;
            Summary = summary;
        }
    }
}