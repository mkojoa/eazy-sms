using System.Collections.Generic;
using Newtonsoft.Json;

namespace eazy.sms.Core.Providers.MnotifyHelpers.Models
{
    public class SummaryDto
    {
        public string _id;
        public string voice_id;
        public string type;
        public int total_sent;
        public int contacts;
        public int total_rejected;
        public List<string> numbers_sent;
        public int credit_used;
    }
}