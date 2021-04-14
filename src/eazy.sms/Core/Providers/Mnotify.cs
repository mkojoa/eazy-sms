using System.Threading.Tasks;
using eazy.sms.Common;
using eazy.sms.Core.Helper;
using eazy.sms.Model;

namespace eazy.sms.Core.Providers
{
    public class Mnotify : INotification
    {
        public Mnotify(string apiKey, string apiSecret)
        {
            ApiKey = apiKey;
            ApiSecret = apiSecret;
        }

        private string ApiKey { get; }
        private string ApiSecret { get; }

        public async Task NotifyAsync<T>(Notifiable<T> notifiable)
        {
            await notifiable.SendAsync(this);
        }

        public async Task NotifyAsync(string message, string title, string[] recipient, string sender,
            string scheduleDate, bool isSchedule = false, Attachment attachments = null)
        {
            //var data = new Dictionary<string, dynamic>()
            //{
            //    {"message", message},
            //    {"title", title},
            //    {"recipient", recipient},
            //    {"sender", sender},
            //    {"scheduleDate", scheduleDate},
            //    {"IsSchedule", isSchedule}
            //};

            var data = $"{"{"}" +
                       $"'message':'{message}', " +
                       $"'title':'{title}'," +
                       $"'recipient':'{recipient}'," +
                       $"'sender':'{sender}'," +
                       $"'scheduleDate':'{scheduleDate}'," +
                       $"'IsSchedule':'{isSchedule}'," +
                       $"{"}"}"; // this is your json input


            await ApiCallHelper<object>.PostRequest(
                $"{Constant.MnotifyGatewayJsonEndpoint}/sms/quick?key={ApiKey}", data);
        }
    }
}