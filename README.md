# eazy-sms
This library aims to provide easy way in sending sms 
using mnotify, hubtel sms gateway , etc.. in your .netcore applications.

 ![ ](https://vistr.dev/badge?repo=mkojoa.eazy-sms&color=0058AD)

###### SMS Providers
- [X] [Mnotify](https://mnotify.com)
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

> Once you have configured the `AddEazySms()` and `UseEazySms` middleware  in the Program.cs file, 
> you're ready to define the `EazyOptions` in the `app.settings.json`.

###### appsettings
```yaml
   "EazyOptions": {
    "SMS": {
      "Enable": true,
      "Database": {
        "Persist": true,
        "Name": "CacheSMS",
        "Instance": "DESKTOP-6BR1LOC",
        "UserName": "sa",
        "Password": "root",
        "Encrypt": "False",
        "TrustedConnection": "False",
        "TrustServerCertificate": "True"
      },
      "ApiKey": "4n4uhOxPhvTeesTlC3ef7jLCGX5mLEIU1MpiAW8Ss16GtZ",
      "ApiSecret": "",
      "From": "Melteck"
    }
  }
```
- Options
1.  `Enable` -- Enable SMS option. which takes either a true or fale.
2.  `Database` -- Enabled by default. SMS are been stored in a database table before they are sent to the recepient.
3.  `ApiKey` -- unique api key.
4.  `ApiSecret` -- unique api secret. Not required by other providers.
5.  `From` -- Name of the server. visit provider for senderId or Name.


> Each notification is represented by a single class and stored in the Notifications 
> directory.

###### Using The Notifiable
The `NotifyAsync` method that is provided by `INotification` interface expects to 
receive a notification instance.
In other to use the `NotifyAsync` method you need to inject the `INotification` 
interface in your contructor.

```c#
  await _notification.NotifyAsync();
```

###### Specifying Delivery Channels
Every notification class has a `Boot` method for building the notification message and 
also to specify the delivery channel.

