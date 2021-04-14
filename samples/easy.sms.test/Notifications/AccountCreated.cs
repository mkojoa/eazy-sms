using eazy.sms.Common;
using eazy.sms.Core;
using eazy.sms.Model;

namespace easy.sms.test.Notifications
{
    public class AccountCreation : Notifiable<string>
    {
        public AccountCreation(string message)
        {
            Message = message;
        }

        private string Message { get; }

        protected override void Boot()
        {
            var dataPassed = Message;

            From("Melteck")
                .Subject("Account Created")
                .Recipient(new[]
                    {
                        "0276002658",
                        "0553771219"
                    }
                )
                .Body(new Body {Content = "New User Created. Registration Code is [234223]"})
                .Schedule(false, "")
                .Channel(SMSChannel.Mnotify);
        }
    }
}