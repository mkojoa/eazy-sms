using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace eazy.sms.ui.Helpers
{
    public class MiddlewareExtention
    {
        private const string EmbeddedFileNamespace = "eazy.sms.ui.wwwroot.dist";
        private readonly IConfiguration _configuration;
        private readonly StaticFileOptions _options;
        private readonly PathString _matchUrl;
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private readonly IContentTypeProvider _contentTypeProvider;
        private readonly IWebHostEnvironment _hostingEnv;
        private readonly StaticFileMiddleware _staticFileMiddleware;

        /// <summary>
        /// Creates a new instance of the StaticFileMiddleware.
        /// </summary>
        /// <param name="next">The next middleware in the pipeline.</param>
        /// <param name="hostingEnv">The <see cref="IHostingEnvironment"/> used by this middleware.</param>
        /// <param name="options">The configuration options.</param>
        /// <param name="loggerFactory">An <see cref="ILoggerFactory"/> instance used to create loggers.</param>
        public MiddlewareExtention(
            RequestDelegate next, 
            IWebHostEnvironment hostingEnv, 
            IOptions<StaticFileOptions> options, 
            ILoggerFactory loggerFactory,
            IConfiguration configuration
            )
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(next));
            }
            if (next == null)
            {
                throw new ArgumentNullException(nameof(next));
            }

            if (hostingEnv == null)
            {
                throw new ArgumentNullException(nameof(hostingEnv));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (loggerFactory == null)
            {
                throw new ArgumentNullException(nameof(loggerFactory));
            }

            _next = next;
            _options = options.Value;
            _contentTypeProvider = options.Value.ContentTypeProvider ?? new FileExtensionContentTypeProvider();
            _matchUrl = _options.RequestPath;
            _logger = loggerFactory.CreateLogger<StaticFileMiddleware>();
            _staticFileMiddleware = CreateStaticFileMiddleware(next, hostingEnv, loggerFactory);
            _configuration = configuration;
        }

        private StaticFileMiddleware CreateStaticFileMiddleware(RequestDelegate next, IWebHostEnvironment hostingEnv, ILoggerFactory loggerFactory)
        {
            var staticFileOptions = new StaticFileOptions
            {
                RequestPath = $"/{_configuration.GetValue("EazyOptions:SMS:UI:RoutePrefix", "easy-sms")}",
                FileProvider = new EmbeddedFileProvider(typeof(MiddlewareExtention).GetTypeInfo().Assembly,
                    EmbeddedFileNamespace)
            };

            return new StaticFileMiddleware(next, hostingEnv, Options.Create(staticFileOptions), loggerFactory);
        }


        /// <summary>
        /// Processes a request to determine if it matches a known file, and if so, serves it.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext httpContext)
        {
            var httpMethod = httpContext.Request.Method;
            var path = httpContext.Request.Path.Value;

            // If the RoutePrefix is requested (with or without trailing slash), redirect to index URL
            if (httpMethod == "GET" && Regex.IsMatch(path, $"^/{Regex.Escape(_configuration.GetValue("EazyOptions:SMS:UI:RoutePrefix", "easy-sms"))}/api/logs/?$",
                RegexOptions.IgnoreCase))
            { 
            }

            if (httpMethod == "GET" &&
                Regex.IsMatch(path, $"^/?{Regex.Escape(_configuration.GetValue("EazyOptions:SMS:UI:RoutePrefix", "easy-sms"))}/?$", RegexOptions.IgnoreCase))
            {
                var indexUrl = httpContext.Request.GetEncodedUrl().TrimEnd('/') + "/index.html";
                RespondWithRedirect(httpContext.Response, indexUrl);
                return;
            }

            if (httpMethod == "GET" && Regex.IsMatch(path, $"^/{Regex.Escape(_configuration.GetValue("EazyOptions:SMS:UI:RoutePrefix", "easy-sms"))}/?index.html$",
                RegexOptions.IgnoreCase))
            {
                await RespondWithIndexHtml(httpContext.Response);
                return;
            }

            await _staticFileMiddleware.Invoke(httpContext);
        }

        private Task RespondWithIndexHtml(HttpResponse response)
        {
            throw new NotImplementedException();
        }

        private void RespondWithRedirect(HttpResponse response, string indexUrl)
        {
            throw new NotImplementedException();
        }
    }
}
