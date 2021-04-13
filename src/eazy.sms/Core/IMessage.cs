using eazy.sms.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eazy.sms.Core
{
    public interface IMessage
    {
        Task SendAsync<T>(IMessageBuilder<T> sMsBuilder);

        Task SendAsync(
            string message,
            string title,
            string[] recipient,
            string sender,
            string scheduleDate,
            bool isSchedule = false,
            IEnumerable<Attachment> attachments = null
        );
    }
}
