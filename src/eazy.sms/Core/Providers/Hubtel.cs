using eazy.sms.Model;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace eazy.sms.Core.Providers
{
    //public class Hubtel : INotification
    //{
    //    private readonly string _apiKey;
    //    private readonly string _apiSecret;
    //    private readonly IServiceCollection _services;

    //    public Hubtel(string apiKey, string apiSecret, IServiceCollection services)
    //    {
    //        _apiKey = apiKey;
    //        _apiSecret = apiSecret;
    //        _services = services;
    //    }

    //    public Task NotifyAsync<T>(Notifiable<T> notifiable)
    //    {
    //        throw new System.NotImplementedException();
    //    }

    //    public Task NotifyAsync(string message, string title, string[] recipient, string sender, string scheduleDate, bool isSchedule = false, Attachment attachments = null)
    //    {
    //        throw new System.NotImplementedException();
    //    }
    //}
}