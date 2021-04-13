using System;
using eazy.sms.Core;
using eazy.sms.Core.Providers;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace eazy.sms
{
    public static class Extension
    {
        public static IServiceCollection AddEazySMS(this IServiceCollection services, IConfiguration configuration)
        {
            IMessage messageBuilder = new Mnotify(
                configuration.GetValue<string>("MelteckXtra:SMS:ApiKey", null),
                configuration.GetValue<string>("MelteckXtra:SMS:ApiSecret", null)
            );

            return services;
        }

        public static IApplicationBuilder UseEazySMS(this IApplicationBuilder app, IConfiguration configuration)
        {
            return app;
        }
    }
} 
