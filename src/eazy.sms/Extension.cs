using eazy.sms.Core;
using eazy.sms.Core.EfCore;
using eazy.sms.Core.EfCore.Entity;
using eazy.sms.Core.Providers;
using eazy.sms.ui;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace eazy.sms
{
    public static class Extension
    {
        //static IServiceProvider serviceProvider;
        public static void AddEazySms(this IServiceCollection services, IConfiguration configuration)
        {
            EfCoreIoC(services, configuration);

            INotification mnotify = new Mnotify(
                configuration.GetValue("EazyOptions:SMS:ApiKey", "test"),
                services
            );

            //INotification hubtel = new Hubtel(
            //    configuration.GetValue("EazyOptions:SMS:ApiKey", "test"),
            //    configuration.GetValue("EazyOptions:SMS:ApiKey", "test"),
            //    services
            //);


            services.AddSingleton(mnotify);
            //services.AddSingleton(hubtel);
        }


        public static IApplicationBuilder UseEazySms(this IApplicationBuilder app, IConfiguration configuration)
        {
           return app.UseEazySmsUi(configuration);
        }

        private static void EfCoreIoC(IServiceCollection services, IConfiguration configuration)
        {
            var fullConec = "" +
                            $"Server={configuration.GetValue("EazyOptions:SMS:Database:Instance", "root")};" +
                            $"Database={configuration.GetValue("EazyOptions:SMS:Database:Name", "DefaultSMSCache")};" +
                            $"User ID={configuration.GetValue("EazyOptions:SMS:Database:UserName", "root")};" +
                            $"Password={configuration.GetValue("EazyOptions:SMS:Database:Password", "root")};" +
                            $"Trusted_Connection={configuration.GetValue("EazyOptions:SMS:Database:Trusted_Connection", "False")};" +
                            $"Encrypt={configuration.GetValue("EazyOptions:SMS:Database:Encrypt", "False")};" +
                            $"TrustServerCertificate={configuration.GetValue("EazyOptions:SMS:Database:TrustServerCertificate", "True")}";

            services.AddDbContext<DataContext>(
                options =>
                    options.UseSqlServer(
                        fullConec
                    ));

            services.AddScoped<IDataProvider, DataProvider>();
        }
    }
}