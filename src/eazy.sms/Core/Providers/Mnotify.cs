using System;
using System.Linq;
using System.Threading.Tasks;
using eazy.sms.Common;
using eazy.sms.Core.EfCore;
using eazy.sms.Core.EfCore.Entity;
using eazy.sms.Core.Helper;
using eazy.sms.Model;
using Microsoft.Extensions.DependencyInjection;

namespace eazy.sms.Core.Providers
{
    public class Mnotify : INotification
    {
        private readonly IServiceCollection _services;

        public Mnotify(string apiKey, IServiceCollection services)
        {
            ApiKey = apiKey;
            _services = services;
        }

        private string ApiKey { get; }

        public async Task NotifyAsync<T>(Notifiable<T> notifiable)
        {
            await notifiable.SendAsync(this);
        }

        public async Task NotifyAsync(string message, string title, string[] recipient, string sender,
            string scheduleDate, bool isSchedule = false, Attachment attachments = null)
        {
            var to = string.Join(",", recipient.Select(item => "'" + item + "'"));

            //Byte[] b = System.IO.File.ReadAllBytes(@"E:\\Test.jpg");   // You can use your own method over here.         
            //File(b, "image/jpeg");
            var file = System.IO.File.Open(attachments.File, System.IO.FileMode.Open, System.IO.FileAccess.Read);
           //{
           //    "flac": ["audio/flac"],
           // "m3u": ["audio/mpegurl", "text/plain"],
           // "m3u8": ["audio/mpegurl", "text/plain"],
           // "m4a": ["audio/mp4"],
           // "m4b": ["audio/mp4"],
           // "mp3": ["audio/mpeg"],
           // "ogg": ["audio/ogg"],
           // "opus": ["audio/ogg"],
           // "pls": ["audio/x-scpls", "text/plain"],
           // "wav": ["audio/wav"],
           // "aac": ["audio/aac"]
           //}

           var data = new
            {
                // param
                
                message = $"{message}",
                recipient,
                sender = $"{sender}",

                //optional
                schedule_date = $"{scheduleDate}",
                is_schedule = $"{isSchedule}",

                //for campaign
                file = $"{file}",
                voice_id = "",
                campaign = $"{title}"
            };

            var scopeFactory = _services
                .BuildServiceProvider()
                .GetRequiredService<IServiceScopeFactory>();

            var stream = await CreateStream(scopeFactory, data);

            if (attachments != null)
            {
                // push to gateway
                var gateway = await ApiCallHelper<Response>.PostRequest(
                    $"{Constant.MnotifyGatewayJsonEndpoint}/voice/quick?key={ApiKey}", data
                );
                if (gateway.Code == "2000")
                {
                    stream.Status = 1;
                    stream.ExceptionStatus = gateway.Code;
                    await UpdateStream(scopeFactory, stream);
                }
                else
                {
                    stream.Exceptions = gateway.Message;
                    stream.ExceptionStatus = gateway.Code;
                    stream.Status = 0;
                    await UpdateStream(scopeFactory, stream);
                }
            }
            else
            {
                // push to gateway
                var gateway = await ApiCallHelper<Response>.PostRequest(
                    $"{Constant.MnotifyGatewayJsonEndpoint}/sms/quick?key={ApiKey}", data
                );
                if (gateway.Code == "2000")
                {
                    stream.Status = 1;
                    stream.ExceptionStatus = gateway.Code;
                    await UpdateStream(scopeFactory, stream);
                }
                else
                {
                    stream.Exceptions = gateway.Message;
                    stream.ExceptionStatus = gateway.Code;
                    stream.Status = 0;
                    await UpdateStream(scopeFactory, stream);
                }
            }
        }

        //=========================================
        //===========  Helper Methods  ============
        //=========================================
        private static async Task<EventMessage> CreateStream(IServiceScopeFactory scopeFactory, object data)
        {
            using var scope = scopeFactory.CreateScope();
            var serviceProvider = scope.ServiceProvider;
            var provider = (IDataProvider)serviceProvider.GetService(typeof(IDataProvider));
            var result = await provider.CreateDataAsync(new EventMessage
            {
                Message = data.ToString(),
                Exceptions = "",
                ExceptionStatus = "",
                Status = 0
            });
            provider.Commit();

            return result;
        }

        private static async Task UpdateStream(IServiceScopeFactory scopeFactory, EventMessage data)
        {
            using var scope = scopeFactory.CreateScope();
            var serviceProvider = scope.ServiceProvider;
            var provider = (IDataProvider) serviceProvider.GetService(typeof(IDataProvider));
            await provider.UpdateDataAsync(new EventMessage
            {
                Id = data.Id,
                Message = data.Message,
                Exceptions = data.Exceptions,
                ExceptionStatus = data.ExceptionStatus,
                Status = data.Status
            });
            provider.Commit();
        }
    }
}