using System.Threading.Tasks;
using easy.sms.test.Models;
using easy.sms.test.Notifications;
using eazy.sms.Core;
using eazy.sms.Core.Providers.MnotifyHelpers.Models;
using Microsoft.AspNetCore.Mvc;

namespace easy.sms.test.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegistrationController : ControllerBase
    {
        private readonly INotification _notification;

        public RegistrationController(INotification notification)
        {
            _notification = notification;
        }

        [HttpPost("AccountRegistration")]
        public async Task<IActionResult> AccountRegistration()
        {
            //Create new user
            var accountUser = new AccountDto { Username = "Michael Ameyaw" };

            //call this after creating user to send email
            var sms = await _notification.NotifyAsync(new AccountCreatedNotifiable(accountUser));

            return Ok(sms);
        }


        [HttpPost("SubscribeToNewsLetter")]
        public async Task<IActionResult> SubscribeToNewsLetter()
        {
            //Get user from somewhere
            var accountUser = new AccountDto { Username = "Michael" };

            //call this after creating user to send email
            var sms = await _notification.NotifyAsync(new NewsLetterCreatedNotifiable(accountUser));

            return Ok(sms);
        }


        [HttpPost("SubscribeWithVoiceCall")]
        public async Task<IActionResult> SubscribeWithVoiceCall()
        {

            //call this after creating user to send email
            var sms = await _notification.NotifyAsync(new VoiceCallCreatedNotifiable());

            return Ok(sms);
        }


        [HttpPost("UnSubscribeToNewsLetter")]
        public async Task<IActionResult> UnSubscribeToNewsLetter()
        {
            //Get user from somewhere
            var accountUser = new AccountDto { Username = "Michael" };


            //call this after creating user to send email
            var sms = await _notification.NotifyAsync(new UnsubscribeCreatedNotifiable(accountUser));

            return Ok(sms);
        }

    }
}