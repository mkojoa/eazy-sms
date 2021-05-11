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
    public class VoiceCallCreatedNotifiable : Notifiable<string>
    {
        public VoiceCallCreatedNotifiable(){}

        protected override void Boot()
        {
            //logic...
            From("Melteck")
                .Subject("VoiceCall Created")
                .Recipient(new[]
                    {
                        "0276002658",
                        "0553771219"
                    }
                )
                .Schedule(false, "2021-04-08 06:00")
                .Attach(new Attachment {File = "ringtone.mp3"})
                .Channel(SmsChannel.Mnotify);
        }
    }
}
