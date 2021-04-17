# eazy-sms
This library aims to provide easy way in sending sms 
using mnotify, hubtel sms gateway , etc.. in your .netcore applications.

 ![ ](https://vistr.dev/badge?repo=mkojoa.eazy-sms&color=0058AD)

###### SMS Providers
- [ ] Mnotify
- [ ] Hubtel

###### SMS Channels
- [ ] SMS
- [ ] Mail

 `eazy-sms` Library repository is work in progress and hope to support mobile money, ussd, etc.
 Each notification is represented by a single class that is typically 
stored in the Notifications directory.

###### Using The Notifiable
The `NotifyAsync` method that is provided by this Interface expects to 
receive a notification instance.

###### Specifying Delivery Channels
Every notification class has a `Boot` and Channel method that determines on which channels the notification will be delivered. 
