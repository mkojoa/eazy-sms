using System.Threading.Tasks;
using easy.sms.test.Models;
using easy.sms.test.Notifications;
using eazy.sms.Core;
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

        [HttpGet("Register")]
        public async Task<IActionResult> Register()
        {
            //Create new user
            var accountUser = new AccountDto {Username = "Michael Ameyaw"};

            //call this after creating user to send email
            var sms = await _notification.NotifyAsync(new AccountCreatedNotifiable(accountUser));

            return Ok(sms);
        }
    }
}