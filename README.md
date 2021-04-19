# eazy-sms
This library aims to provide easy way in sending sms 
using mnotify, hubtel sms gateway , etc.. in your .netcore applications.

 ![ ](https://vistr.dev/badge?repo=mkojoa.eazy-sms&color=0058AD)

###### SMS Providers
- [X] Mnotify
- [ ] Hubtel
- [ ] Dashboard - Resend failed SMS Message.

###### SMS Channels
- [X] SMS
- [ ] Mail


> `eazy-sms` repository is work in progress and hope to support mobile money, ussd, etc.


#### Getting Started
`AddEazySms(Configuration)` which accepts IConfiguration object  must be injected in `ConfigureServices` method in the `Startup` class.

> ConfigureServices
   ```c#
    //ConfigureServices Method.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddEazySms(Configuration);
    }

    // Configure Method.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseEazySms(Configuration);
    }
   ```

> Each notification is represented by a single class and stored in the Notifications 
> directory.

###### Using The Notifiable
The `NotifyAsync` method that is provided by `INotification` interface expects to 
receive a notification instance.
In other to use the `NotifyAsync` method you need to inject the `INotification` 
interface in your contructor.

```c#
  await _notification.NotifyAsync(new AccountCreatedNotifiable(accountUser));
```

###### Specifying Delivery Channels
Every notification class has a `Boot` method for building the notification message and 
also to specify the delivery channel.

