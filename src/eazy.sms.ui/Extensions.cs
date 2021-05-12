using eazy.sms.ui.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace eazy.sms.ui
{
    public static class Extensions
    {
        public static IApplicationBuilder UseEazySmsUi(this IApplicationBuilder applicationBuilder, IConfiguration configuration)
        {
            if (applicationBuilder == null)
                throw new ArgumentNullException(nameof(applicationBuilder));

            applicationBuilder.UseMiddleware<MiddlewareExtention>(configuration);

            return applicationBuilder;
        }
    }
}
