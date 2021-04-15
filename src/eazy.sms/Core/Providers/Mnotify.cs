using System;
using System.Linq;
using System.Threading.Tasks;
using eazy.sms.Common;
using eazy.sms.Core.EfCore;
using eazy.sms.Core.EfCore.Entity;
using eazy.sms.Core.Helper;
using eazy.sms.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace eazy.sms.Core.Providers
{
    public class Mnotify : INotification
    {

        public Mnotify(string apiKey, string apiSecret, IServiceCollection services)
        {
            ApiKey = apiKey;
            ApiSecret = apiSecret;
            _services = services;
        }

        private string ApiKey { get; }
        private string ApiSecret { get; }
        private readonly IServiceCollection _services; 

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

            var scopeFactory = _services
                    .BuildServiceProvider()
                    .GetRequiredService<IServiceScopeFactory>();

            using (var scope = scopeFactory.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                var provider = (IDataProvider)serviceProvider.GetService(typeof(IDataProvider));
                await provider.CreateDataAsync(new EventMessage
                {
                    Message = data,
                    Exceptions = "",
                    Status = false
                });
            }



            var gateway =  await ApiCallHelper<object>.PostRequest(
                $"{Constant.MnotifyGatewayJsonEndpoint}/sms/quick?key={ApiKey}", data);


            using (var scope = scopeFactory.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                var provider = (IDataProvider)serviceProvider.GetService(typeof(IDataProvider));
                await provider.UpdateDataAsync(new EventMessage
                {
                    Status = false
                });
            }
        }
    }
}