using Newtonsoft.Json;

namespace eazy.sms.Core.Providers.MnotifyHelpers.Models
{
    public class DataDto
    {
        [JsonProperty("message")] public string Message { get; set; }
        [JsonProperty("recipient")] public string[] Recipient { get; set; }
        [JsonProperty("sender")] public string Sender { get; set; }
        [JsonProperty("schedule_date")] public string ScheduleDate { get; set; }
        [JsonProperty("is_schedule")] public bool IsSchedule { get; set; }
        [JsonProperty("file")] public string File { get; set; }
        [JsonProperty("voice_id")] public string VoiceId { get; set; }
        [JsonProperty("campaign")] public string Campaign { get; set; }
        [JsonProperty("group_id")] public string[] GroupId { get; set; }
        [JsonProperty("message_id")] public string MessageId { get; set; }
    }
}