using System.Threading.Tasks;
using eazy.sms.Core.Providers.MnotifyHelpers.Models;
using eazy.sms.Model;

namespace eazy.sms.Core
{
    public interface INotification
    {
        Task<ResponseDto> NotifyAsync<T>(Notifiable<T> notifiable);

        Task<ResponseDto> NotifyAsync(
            string message,
            string title,
            string[] recipient,
            string sender,
            string scheduleDate,
            bool isSchedule = false,
            Attachment attachments = null
        );
    }
}