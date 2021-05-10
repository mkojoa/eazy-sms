using easy.sms.test.Models;
using eazy.sms.Common;
using eazy.sms.Core;
using eazy.sms.Model;

namespace easy.sms.test.Notifications
{
    public class AccountCreatedNotifiable : Notifiable<AccountDto>
    {
        public AccountCreatedNotifiable(AccountDto accountDto)
        {
            _AccountDto = accountDto;
        }

        private AccountDto _AccountDto { get; }

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
                //.Content(new Content($"{_AccountDto.Username}, message from big bro"))
                .Schedule(false, "2021-04-08 06:00")
                //.Attach(new Attachment {File = "ringtone.mp3"})
                //.Template("AccountRegistration.txt", _AccountDto)
                .Channel(SMSChannel.Mnotify);
        }
    }
}