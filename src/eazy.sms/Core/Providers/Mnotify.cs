using System.Linq;
using System.Threading.Tasks;
using eazy.sms.Common;
using eazy.sms.Core.EfCore;
using eazy.sms.Core.EfCore.Entity;
using eazy.sms.Core.Providers.MnotifyHelpers.Helpers;
using eazy.sms.Core.Providers.MnotifyHelpers.Models;
using eazy.sms.Model;
using Microsoft.Extensions.DependencyInjection;

namespace eazy.sms.Core.Providers
{
    public class Mnotify : INotification
    {
        private readonly IServiceCollection _services;
        private ResponseDto campaign;

        public Mnotify(string apiKey, IServiceCollection services)
        {
            ApiKey = apiKey;
            _services = services;
        }

        private string ApiKey { get; }

        public async Task<ResponseDto> NotifyAsync<T>(Notifiable<T> notifiable)
        {
            return await notifiable.SendAsync(this);
        }

        public async Task<ResponseDto> NotifyAsync(string message, string title, string[] recipient, string sender,
            string scheduleDate, bool isSchedule = false, Attachment attachments = null)
        {
            //init data object
            var data = new DataDto
            {
                Message = message,
                Recipient = recipient,
                Sender = sender,
                ScheduleDate = scheduleDate,
                IsSchedule = isSchedule,
                File = attachments?.File,
                Campaign = title,
            };


            if (attachments != null)
            {
                campaign = await ApiCallHelper<ResponseDto>.CampaignWithVoice(
                        $"{Constant.MnotifyGatewayJsonEndpoint}/voice/quick?key={ApiKey}",
                        data
                    );
            }
            else
            {
                campaign = await ApiCallHelper<ResponseDto>.Campaign(
                    $"{Constant.MnotifyGatewayJsonEndpoint}/sms/quick?key={ApiKey}",
                    data
                );
            }

              

            var scopeFactory = _services
                .BuildServiceProvider()
                .GetRequiredService<IServiceScopeFactory>();

            var stream = await CreateStream(scopeFactory, data);

            return campaign;
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
            var provider = (IDataProvider)serviceProvider.GetService(typeof(IDataProvider));
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

//if (attachments != null)
//{
//    // push to gateway
//    var gateway = await ApiCallHelper<Response>.PostRequest(
//        $"{Constant.MnotifyGatewayJsonEndpoint}/voice/quick?key={ApiKey}", data
//    );
//    if (gateway.Code == "2000")
//    {
//        stream.Status = 1;
//        stream.ExceptionStatus = gateway.Code;
//        await UpdateStream(scopeFactory, stream);
//    }
//    else
//    {
//        stream.Exceptions = gateway.Message;
//        stream.ExceptionStatus = gateway.Code;
//        stream.Status = 0;
//        await UpdateStream(scopeFactory, stream);
//    }
//}
//else
//{
//    // push to gateway
//    var gateway = await ApiCallHelper<ResponseDto>.PostRequest(
//        $"{Constant.MnotifyGatewayJsonEndpoint}/sms/quick?key={ApiKey}", data
//    );
//    if (gateway.Code == "2000")
//    {
//        stream.Status = 1;
//        stream.ExceptionStatus = gateway.Code;
//        await UpdateStream(scopeFactory, stream);
//    }
//    else
//    {
//        stream.Exceptions = gateway.Message;
//        stream.ExceptionStatus = gateway.Code;
//        stream.Status = 0;
//        await UpdateStream(scopeFactory, stream);
//    }
//}