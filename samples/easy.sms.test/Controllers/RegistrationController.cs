﻿using System.Threading.Tasks;
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

            //call this after creating user to send email
            await _notification.NotifyAsync(new AccountCreation("New User Created. Registration Code is [234223]"));

            return Ok("Message sent to registered users");
        }
    }
}