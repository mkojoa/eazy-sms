using eazy.sms.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eazy.sms.Core
{
    public interface IMessage
    {
        Task NotifyAsync<T>(Notifiable<T> notifiable);
        Task NotifyAsync(
            string message,
            string title,
            Recipient[] recipient,
            string sender,
            string scheduleDate,
            bool isSchedule = false,
            Attachment attachments = null
        );
    }
}
