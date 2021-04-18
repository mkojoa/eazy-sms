using eazy.sms.Core;
using eazy.sms.Core.EfCore;
using eazy.sms.Core.EfCore.Entity;
using eazy.sms.Core.Providers;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace eazy.sms
{
    public static class Extension
    {
        //static IServiceProvider serviceProvider;
        public static IServiceCollection AddEazySms(this IServiceCollection services, IConfiguration configuration)
        {
            EfCoreIoC(services, configuration);

            INotification mnotify = new Mnotify(
                configuration.GetValue("EazyConfig:SMS:ApiKey", "test"),
                services
            );

            //INotification hubtel = new Hubtel(
            //    configuration.GetValue("EazyConfig:SMS:ApiKey", "test"),
            //    configuration.GetValue("EazyConfig:SMS:ApiKey", "test"),
            //    services
            //);


            services.AddSingleton(mnotify);
            //services.AddSingleton(hubtel);

            return services;
        }


        public static IApplicationBuilder UseEazySms(this IApplicationBuilder app, IConfiguration configuration)
        {
            configuration.GetSection("");

            return app;
        }

        private static void EfCoreIoC(IServiceCollection services, IConfiguration configuration)
        {
            var fullConec = "" +
                            $"Server={configuration.GetValue("EazyConfig:SMS:Database:Instance", "root")};" +
                            $"Database={configuration.GetValue("EazyConfig:SMS:Database:Name", "DefaultSMSCache")};" +
                            $"User ID={configuration.GetValue("EazyConfig:SMS:Database:UserName", "root")};" +
                            $"Password={configuration.GetValue("EazyConfig:SMS:Database:Password", "root")};" +
                            $"Trusted_Connection={configuration.GetValue("EazyConfig:SMS:Database:Trusted_Connection", "False")};" +
                            $"Encrypt={configuration.GetValue("EazyConfig:SMS:Database:Encrypt", "False")};" +
                            $"TrustServerCertificate={configuration.GetValue("EazyConfig:SMS:Database:TrustServerCertificate", "True")}";

            services.AddDbContext<DataContext>(
                options =>
                    options.UseSqlServer(
                        fullConec
                    ));

            services.AddScoped<IDataProvider, DataProvider>();
        }
    }
}