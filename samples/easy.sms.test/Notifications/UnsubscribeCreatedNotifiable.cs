using easy.sms.test.Models;
using eazy.sms.Common;
using eazy.sms.Core;
using eazy.sms.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace easy.sms.test.Notifications
{
    public class UnsubscribeCreatedNotifiable : Notifiable<AccountDto>
    {
        private readonly AccountDto _accountDto;

        public UnsubscribeCreatedNotifiable(AccountDto accountDto)
        {
            _accountDto = accountDto;
        }
        protected override void Boot()
        {
            //logic...
            From("Melteck")
                .Subject("Account Created")
                .Recipient(new[]
                    {
                        "0276002658",
                        "0553771219"
                    }
                )
                .Content(new Content($"{_accountDto.Username}, Reply STOP to unsubscribe or HELP for help. 4 msgs per month, Msg&Data rates may apply."))
                .Schedule(false, "2021-04-08 06:00")
                .Channel(SmsChannel.Mnotify);
        }
    }
}
