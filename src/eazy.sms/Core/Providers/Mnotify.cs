using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using eazy.sms.Common;
using eazy.sms.Core.EfCore;
using eazy.sms.Core.EfCore.Entity;
using eazy.sms.Core.Helper;
using eazy.sms.Model;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace eazy.sms.Core.Providers
{
    public class Mnotify : INotification
    {
        private readonly IServiceCollection _services;
        //private EventMessage Created { get; set; }

        public Mnotify(string apiKey, string apiSecret, IServiceCollection services)
        {
            ApiKey = apiKey;
            ApiSecret = apiSecret;
            _services = services;
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
            var to = string.Join(",", recipient.Select(item => "'" + item + "'"));

            var data = new
            {
                message = $"{message}",
                recipient = recipient,
                sender = $"{sender}",
                schedule_date = $"{scheduleDate}",
                is_schedule = $"{isSchedule}"
            };

            IServiceScopeFactory scopeFactory = _services
                .BuildServiceProvider()
                .GetRequiredService<IServiceScopeFactory>();

            var stream = await CreateStream(scopeFactory, data);
 
            // push to gateway
            var gateway = await ApiCallHelper<Response>.PostRequest(
                $"{Constant.MnotifyGatewayJsonEndpoint}/sms/quick?key={ApiKey}", data
                );


            await UpdateStream(scopeFactory, stream);
            
        }

        //=========================================
        //===========  Helper Methods  ============
        //=========================================
        public static async Task<EventMessage> CreateStream(IServiceScopeFactory scopeFactory, object data)
        {
            using IServiceScope scope = scopeFactory.CreateScope();
            var serviceProvider = scope.ServiceProvider;
            var provider = (IDataProvider)serviceProvider.GetService(typeof(IDataProvider));
            var result = await provider.CreateDataAsync(new EventMessage
            {
                Message = data.ToString(),
                Exceptions = "",
                Status = 0
            });
            provider.Commit();

            return result;
        }

        public static async Task UpdateStream(IServiceScopeFactory scopeFactory, EventMessage data)
        {
            using var scope = scopeFactory.CreateScope();
            var serviceProvider = scope.ServiceProvider;
            var provider = (IDataProvider)serviceProvider.GetService(typeof(IDataProvider));
            await provider.UpdateDataAsync(new EventMessage
            {
                Message = data.Message,
                Exceptions = data.Exceptions,
                UpdatedAt = DateTime.Now,
                Status = 1
            });
            provider.Commit();
        }

        
    }

    public class User
    { 
        public string success { get; set; }
    }
}