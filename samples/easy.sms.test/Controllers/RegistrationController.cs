using System.Threading.Tasks;
using easy.sms.test.Models;
using easy.sms.test.Notifications;
using eazy.sms.Core;
using eazy.sms.Core.Helper;
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

        /// <summary>
        /// Send Message without using notifiables
        /// </summary>
        /// <returns></returns>
        [HttpPost("RawTest")]
        public async Task<IActionResult> RawTest()
        {
            //Create new user
            var accountUser = new AccountDto { Username = "Michael Ameyaw" };

            //call this after creating user to send email
            var sms = await _notification.NotifyAsync(
                $"Hello {accountUser.Username}, This is direct test from RawTest method. thanks",
                "Raw SMS Testing",
                new[] { "0553771219" },
                "Melteck",
                "2021-04-08 06:00",
                false,
                new eazy.sms.Model.Attachment
                {
                    File = "" // full path 
                });

            return Ok(sms);
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

        [Produces("application/json", Type = typeof(ResponseDto))]
        [HttpPost("SubscribeToNewsLetter")]
        public async Task<ResponseDto> SubscribeToNewsLetter()
        {
            //Get user from somewhere
            var accountUser = new AccountDto { Username = "Michael" };

            //call this after creating user to send email
            var sms = await _notification.NotifyAsync(new NewsLetterCreatedNotifiable(accountUser));

            return sms;
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