using System.Linq;
using System.Threading.Tasks;
using eazy.sms.Common;
using eazy.sms.Core.EfCore;
using eazy.sms.Core.EfCore.Entity;
using eazy.sms.Core.Helper;
using eazy.sms.Model;

namespace eazy.sms.Core.Providers
{
    public class Mnotify : INotification
    {
        public Mnotify(string apiKey, string apiSecret, IDataProvider dataProvider = null)
        {
            ApiKey = apiKey;
            ApiSecret = apiSecret;
            DataProvider = dataProvider;
        }

        private string ApiKey { get; }
        private string ApiSecret { get; }
        private IDataProvider DataProvider { get; }

        public async Task NotifyAsync<T>(Notifiable<T> notifiable)
        {
            await notifiable.SendAsync(this);
        }

        public async Task NotifyAsync(string message, string title, string[] recipient, string sender,
            string scheduleDate, bool isSchedule = false, Attachment attachments = null)
        {

            var to = string.Join(",", recipient.Select(item => "'" + item + "'"));

            var data = $"{"{"}" +
                       $"'message':'{message}', " +
                       $"'title':'{title}'," +
                       $"'recipient':'[{to}]'," +
                       $"'sender':'{sender}'," +
                       $"'scheduleDate':'{scheduleDate}'," +
                       $"'IsSchedule':'{isSchedule}'" +
                       $"{"}"}";

            // push to database for resend ability
            await DataProvider.CreateDataAsync(new EventMessage
            {
                Message = data,
                Exceptions = "",
                Status = false
            });

            var gateway =  await ApiCallHelper<object>.PostRequest(
                $"{Constant.MnotifyGatewayJsonEndpoint}/sms/quick?key={ApiKey}", data);

            // update with message sent status;
            await DataProvider.UpdateDataAsync(new EventMessage
            {
                Status = true
            });
        }
    }
}