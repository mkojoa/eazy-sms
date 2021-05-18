using eazy.sms.Core.EfCore;
using eazy.sms.Core.Helper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
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
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private readonly StaticFileMiddleware _staticFileMiddleware;
        private readonly JsonSerializerSettings _jsonSerializerOptions;

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
            CheckContructorParam(next, hostingEnv, options, loggerFactory, configuration);

            _next = next;
            _options = options.Value;
            _configuration = configuration;
            _logger = loggerFactory.CreateLogger<StaticFileMiddleware>();
            _staticFileMiddleware = CreateStaticFileMiddleware(next, hostingEnv, loggerFactory, configuration);
            _jsonSerializerOptions = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.None
            };
        }

        private static void CheckContructorParam(RequestDelegate next, IWebHostEnvironment hostingEnv, IOptions<StaticFileOptions> options, ILoggerFactory loggerFactory, IConfiguration configuration)
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
        }

        private StaticFileMiddleware CreateStaticFileMiddleware(RequestDelegate next, IWebHostEnvironment hostingEnv, ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            var RequestPath = $"/{configuration.GetSection("EazyOptions:SMS:UI:RoutePrefix").Value}";
            var staticFileOptions = new StaticFileOptions
            {
                RequestPath = RequestPath,
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
            if (httpMethod == "GET" && Regex.IsMatch(path, $"^/{Regex.Escape(_configuration.GetSection("EazyOptions:SMS:UI:RoutePrefix").Value)}/api/sms/?$",
                RegexOptions.IgnoreCase))
            {
                //get api data
                try
                {
                    httpContext.Response.ContentType = "application/json;charset=utf-8";

                    var result = await FetchSMSDataAsync(httpContext);
                    httpContext.Response.StatusCode = (int)HttpStatusCode.OK;
                    await httpContext.Response.WriteAsync(result);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                    httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                    var errorMessage = httpContext.Request.IsLocal()
                        ? JsonConvert.SerializeObject(new { errorMessage = ex.Message })
                        : JsonConvert.SerializeObject(new { errorMessage = "Internal server error" });

                    await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(new { errorMessage }));
                }

                return;
            }

            if (httpMethod == "GET" &&
                Regex.IsMatch(path, $"^/?{Regex.Escape(_configuration.GetSection("EazyOptions:SMS:UI:RoutePrefix").Value)}/?$", RegexOptions.IgnoreCase))
            {
                // load html pages based on routePrefix
                var indexUrl = httpContext.Request.GetEncodedUrl().TrimEnd('/') + "/index.html";
                RespondWithRedirect(httpContext.Response, indexUrl);
                return;
            }

            if (httpMethod == "GET" && Regex.IsMatch(path, $"^/{Regex.Escape(_configuration.GetSection("EazyOptions:SMS:UI:RoutePrefix").Value)}/?index.html$",
                RegexOptions.IgnoreCase))
            {
                await RespondWithIndexHtml(httpContext.Response);
                return;
            }

            await _staticFileMiddleware.Invoke(httpContext);
        }

        private async Task<string> FetchSMSDataAsync(HttpContext httpContext)
        {
            var provider = httpContext.RequestServices.GetService<IDataProvider>();
            var dataResult = await provider.FetchDataAsync(2); // two for all record
            var result = JsonConvert.SerializeObject(dataResult, _jsonSerializerOptions);
            return result;
        }

        private async Task RespondWithIndexHtml(HttpResponse response)
        {
            var RoutePrefix = _configuration.GetSection("EazyOptions:SMS:UI:RoutePrefix").Value;
            var AuthType = "";

            response.StatusCode = 200;
            response.ContentType = "text/html;charset=utf-8";


            await using var stream = IndexStream();
            var htmlBuilder = new StringBuilder(await new StreamReader(stream).ReadToEndAsync());
            htmlBuilder.Replace("%(Configs)", JsonConvert.SerializeObject(
                new { RoutePrefix, AuthType }, _jsonSerializerOptions));

            await response.WriteAsync(htmlBuilder.ToString(), Encoding.UTF8);
        }

        private void RespondWithRedirect(HttpResponse response, string indexUrl)
        {
            response.StatusCode = 301;
            response.Headers["Location"] = indexUrl;
        }

        private Func<Stream> IndexStream { get; } =
            () => Assembly.GetExecutingAssembly().GetManifestResourceStream("eazy.sms.ui.wwwroot.index.html");

    }
}
