﻿using System.Linq;
using System.Threading.Tasks;
using eazy.sms.Common;
using eazy.sms.Core.EfCore;
using eazy.sms.Core.EfCore.Entity;
using eazy.sms.Core.Helper;
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
            string[] group = new string[] { "1", "2", "3", "4" };
            string messageId = null;

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

            var scopeFactory = _services
                .BuildServiceProvider()
                .GetRequiredService<IServiceScopeFactory>();

            var stream = await CreateStream(scopeFactory, data);


            if (!HelperExtention.IsNullAttachment(attachments))
            {
                campaign = await ApiCallHelper<ResponseDto>.CampaignWithVoice(
                        $"{Constant.MnotifyGatewayJsonEndpoint}/voice/quick?key={ApiKey}",
                        data
                    );
                await InsertOrUpdateRecord(scopeFactory, stream);

                return campaign;
            }
            else if (!HelperExtention.IsNullGroupWithMessage(group, messageId))
            {
                campaign = await ApiCallHelper<ResponseDto>.CampaignGroup(
                        $"{Constant.MnotifyGatewayJsonEndpoint}/sms/group?key={ApiKey}",
                        data
                    );

                await InsertOrUpdateRecord(scopeFactory, stream);

                return campaign;
            }
            else if (!HelperExtention.IsNullAttachmentWithGroup(attachments, group)) 
            {
                campaign = await ApiCallHelper<ResponseDto>.CampaignGroupWithVoice(
                        $"{Constant.MnotifyGatewayJsonEndpoint}/voice/group?key={ApiKey}",
                        data
                    );

                await InsertOrUpdateRecord(scopeFactory, stream);

                return campaign;
            }
            else
            {
                campaign = await ApiCallHelper<ResponseDto>.Campaign(
                    $"{Constant.MnotifyGatewayJsonEndpoint}/sms/quick?key={ApiKey}",
                    data
                );

                await InsertOrUpdateRecord(scopeFactory, stream);

                return campaign;
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

        private async Task InsertOrUpdateRecord(IServiceScopeFactory scopeFactory, EventMessage stream)
        {
            if (campaign.Code == ResultHelper.Ok)
            {
                stream.Status = 1;
                stream.ExceptionStatus = campaign.Code;
                await UpdateStream(scopeFactory, stream);
            }
            else
            {
                stream.Exceptions = campaign.Message;
                stream.ExceptionStatus = campaign.Code;
                stream.Status = 0;
                await UpdateStream(scopeFactory, stream);
            }
        }
    }
}