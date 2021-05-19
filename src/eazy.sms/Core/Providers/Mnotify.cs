﻿using System.Threading.Tasks;
using eazy.sms.Common;
using eazy.sms.Core.EfCore;
using eazy.sms.Core.EfCore.Entity;
using eazy.sms.Core.Helper;
using eazy.sms.Core.Providers.MnotifyHelpers.Helpers;
using eazy.sms.Core.Providers.MnotifyHelpers.Models;
using eazy.sms.Model;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;

namespace eazy.sms.Core.Providers
{
    public class Mnotify : INotification
    {
        private readonly IServiceCollection _services;
        private ResponseDto _campaign;

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
                Campaign = title
            };

            var scopeFactory = _services
                .BuildServiceProvider()
                .GetRequiredService<IServiceScopeFactory>();

            var stream = await CreateStream(scopeFactory, data);


            if (!HelperExtention.IsNullAttachment(attachments))
            {
                _campaign = await ApiCallHelper<ResponseDto>.CampaignWithVoice(
                    $"{Constant.MnotifyGatewayJsonEndpoint}/voice/quick?key={ApiKey}",
                    data
                );
                await InsertOrUpdateRecord(scopeFactory, stream);

                return _campaign;
            }

            if (!HelperExtention.IsNullGroupWithMessage(null, null))
            {
                _campaign = await ApiCallHelper<ResponseDto>.CampaignGroup(
                    $"{Constant.MnotifyGatewayJsonEndpoint}/sms/group?key={ApiKey}",
                    data
                );

                await InsertOrUpdateRecord(scopeFactory, stream);

                return _campaign;
            }

            if (!HelperExtention.IsNullAttachmentWithGroup(attachments, null))
            {
                _campaign = await ApiCallHelper<ResponseDto>.CampaignGroupWithVoice(
                    $"{Constant.MnotifyGatewayJsonEndpoint}/voice/group?key={ApiKey}",
                    data
                );

                await InsertOrUpdateRecord(scopeFactory, stream);

                return _campaign;
            }

            _campaign = await ApiCallHelper<ResponseDto>.Campaign(
                $"{Constant.MnotifyGatewayJsonEndpoint}/sms/quick?key={ApiKey}",
                data
            );

            await InsertOrUpdateRecord(scopeFactory, stream);

            return _campaign;
        }


        //=========================================
        //===========  Helper Methods  ============
        //=========================================
        private static async Task<EventMessage> CreateStream(IServiceScopeFactory scopeFactory, DataDto data)
        {
            using var scope = scopeFactory.CreateScope();
            var serviceProvider = scope.ServiceProvider;
            var provider = (IDataProvider) serviceProvider.GetService(typeof(IDataProvider));
            var result = await provider.CreateDataAsync(new EventMessage
            {
                Message = HelperExtention.ToDynamicJson(data),
                ResultMessage = "",
                ResultStatus = "",
                SentStatus = 0
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
                ResultMessage = data.ResultMessage,
                ResultStatus = data.ResultStatus,
                SentStatus = data.SentStatus
            });
            provider.Commit();
        }

        private async Task InsertOrUpdateRecord(IServiceScopeFactory scopeFactory, EventMessage stream)
        {
            if (_campaign.code == ResultHelper.Ok)
            {
                stream.SentStatus = 1;
                stream.ResultStatus = _campaign.code;
                stream.ResultMessage = _campaign.message;
                await UpdateStream(scopeFactory, stream);
            }
            else
            {
                stream.ResultMessage = _campaign.message;
                stream.ResultStatus = _campaign.code;
                stream.SentStatus = 0;
                await UpdateStream(scopeFactory, stream);
            }
        }
    }
}