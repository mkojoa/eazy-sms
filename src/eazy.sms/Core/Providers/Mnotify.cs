using eazy.sms.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eazy.sms.Core.Providers
{
    public class Mnotify : IMessage
    {

        public string ApiKey { get; set; } 
        public string ApiSecret { get; set; }

         
        public Mnotify(string ApiKey, string ApiSecret)
        {
            this.ApiKey = ApiKey;
            this.ApiSecret = ApiSecret;
        }

        public Task SendAsync(string message, string title, Recipient[] recipient, string sender, string scheduleDate, bool isSchedule = false, Attachment attachments = null)
        {
            Dictionary<string, dynamic> data = new Dictionary<string, dynamic>()
            {
                { "message", message}, 
                { "title", title },
                { "recipient", recipient },
                { "sender", sender},
                { "scheduleDate", scheduleDate},
                { "IsSchedule", isSchedule }
            };

            var jsonString = JsonConvert.SerializeObject(data);

            return Task.CompletedTask;
        }
    }
}
