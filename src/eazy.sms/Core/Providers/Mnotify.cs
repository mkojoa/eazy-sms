using eazy.sms.Common;
using eazy.sms.Core.Helper;
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
        private string ApiKey { get; set; }
        private string ApiSecret { get; set; }

         
        public Mnotify(string apiKey, string apiSecret)
        {
            this.ApiKey = apiKey;
            this.ApiSecret = apiSecret;
        }

        public async Task NotifyAsync<T>(Notifiable<T> notifiable)
        {
             await notifiable.SendAsync(this);
        }

        public async Task NotifyAsync(string message, string title, Recipient[] recipient, string sender,
            string scheduleDate, bool isSchedule = false, Attachment attachments = null)
        {
            var data = new Dictionary<string, dynamic>()
            {
                {"message", message},
                {"title", title},
                {"recipient", recipient},
                {"sender", sender},
                {"scheduleDate", scheduleDate},
                {"IsSchedule", isSchedule}
            };

            await ApiCallHelper<object>.PostRequest(
                $"{Constant.MnotifyGatewayJsonEndpoint}/sms/quick?key={this.ApiKey}", data);
        }
    }
}
