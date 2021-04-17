using eazy.sms.Common;
using eazy.sms.Core;
using eazy.sms.Model;

namespace easy.sms.test.Notifications
{
    public class AccountCreatedNotifiable : Notifiable<string>
    {
        public AccountCreatedNotifiable(string message)
        {
            Message = message;
        }

        private string Message { get; }

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
                .Body(new Body {Content = $"{Message}"})
                .Schedule(false, "")
                .Attach(new Attachment {Path = "", File = "audio.mp3"})
                .Channel(SMSChannel.Mnotify);
        }
    }
}