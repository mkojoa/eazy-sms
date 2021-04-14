using System.Threading.Tasks;
using eazy.sms.Model;

namespace eazy.sms.Core
{
    public interface INotification
    {
        Task NotifyAsync<T>(Notifiable<T> notifiable);

        Task NotifyAsync(
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