using easy.sms.test.Models;
using eazy.sms.Common;
using eazy.sms.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace easy.sms.test.Notifications
{
    public class NewsLetterCreatedNotifiable : Notifiable<AccountDto>
    {
        private readonly AccountDto _accountDto;

        public NewsLetterCreatedNotifiable(AccountDto accountDto)
        {
            _accountDto = accountDto;
        }

        protected override void Boot()
        {
            //logic...
            From("Melteck")
                .Subject("NewsLetter Subscribed")
                .Recipient(new[]
                    {
                        "0276002658",
                        "0553771219"
                    }
                )
                .Schedule(false, "2021-04-08 06:00")
                .Template("NewsLetter.txt", _accountDto)
                .Channel(SmsChannel.Mnotify);
        }
    }
}
