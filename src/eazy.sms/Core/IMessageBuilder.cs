using eazy.sms.Common;
using eazy.sms.Core.Exceptions;
using eazy.sms.Core.Helper;
using eazy.sms.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eazy.sms.Core
{
    public class IMessageBuilder<T>
    {
        /// <summary>
        /// No Render Found Message
        /// </summary>
        private const string NoRenderFoundMessage
            = "Please use one of the available methods for specifying how to render your sms (e.g. Text() or Template())";

        /// <summary>
        ///     Required: This is the sender name.
        ///     The maximum length is 11 alphanumerical characters.
        /// </summary>
        [JsonProperty("from")]
        private string _From { get; set; }

        /// <summary>
        ///     Required: The destination mobile numbers.
        /// </summary>
        [JsonProperty("to")]
        private Recipient[] _Recipients { get; set; }

        /// <summary>
        ///     The allowed channels field forces a message to only use certain routes.
        /// </summary>
        [JsonProperty(
            DefaultValueHandling = DefaultValueHandling.Ignore,
            PropertyName = "allowedChannels",
            ItemConverterType = typeof(StringEnumConverter)
        )]
        private Channel[] AllowedChannels { get; set; }

        /// <summary>
        ///     Template data to pass to the Txt view to render.
        /// </summary>
        private T _TemplateModel { get; set; }

        /// <summary>
        ///     The Text template to use to generate the message.
        /// </summary>
        private string _TemplatePath { get; set; }

        /// <summary>
        ///     Required: The actual text body of the message.
        /// </summary>
        [JsonProperty("body")]
        private Body _Body { get; set; }

        /// <summary>
        /// Optional : FileName.ext && Path to file
        /// </summary>
        [JsonProperty("attachment")]
        private Attachment _Attachment { get; set; }

        /// <summary>
        /// Optional : The title of the message
        /// </summary>
        /// [JsonProperty("subject")]
        private string _Subject { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool _IsSchedule { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string _ScheduleDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="from"></param>
        /// <returns></returns>
        public IMessageBuilder<T> From(string from)
        {
            _From = from;
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="recipients"></param>
        /// <returns></returns>
        public IMessageBuilder<T> Recipient(Recipient[] recipients)
        {
            _Recipients = recipients;
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="subject"></param>
        /// <returns></returns>
        public IMessageBuilder<T> Subject(string subject)
        {
            _Subject = subject;
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attachment"></param>
        /// <returns></returns>
        public IMessageBuilder<T> Attach(Attachment attachment)
        {
            _Attachment = attachment;
            return this;
        }

        public IMessageBuilder<T> Schedule(bool isSchedule = false, string scheduleDate = null)
        {
            _IsSchedule = isSchedule;
            _ScheduleDate = scheduleDate;
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        public IMessageBuilder<T> Body(Body body)
        {
            _Body = body;
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="templatePath"></param>
        /// <param name="templateModel"></param>
        /// <returns></returns>
        public IMessageBuilder<T> Template(string templatePath, T templateModel)
        {
            _TemplateModel = templateModel;
            _TemplatePath = templatePath;
            return this;
        }

        protected virtual void Build() { }

        internal async Task SendAsync(TemplateRenderer renderer, IMessage message)
        {
            Build();

            //logic here
        }

        private async Task<string> BuildMessage(TemplateRenderer renderer)
        {
            if (_Body != null) return _Body.Content;

            if (_TemplatePath != null)
                return await TemplateRenderer.RenderTemplateToStringAsync(_TemplatePath, _TemplateModel)
                    .ConfigureAwait(false);

            throw new NoSmsRendererFound(NoRenderFoundMessage);
        }
    }
}
