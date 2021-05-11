using System;
using System.Threading.Tasks;
using eazy.sms.Common;
using eazy.sms.Core.Exceptions;
using eazy.sms.Core.Helper;
using eazy.sms.Core.Providers.MnotifyHelpers.Models;
using eazy.sms.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace eazy.sms.Core
{
    public class Notifiable<T>
    {
        /// <summary>
        ///     No Render Found Message
        /// </summary>
        private const string NoRenderFoundMessage
            = "Please use one of the available methods for specifying how to render your sms (e.g. Text() or Template())";

        /// <summary>
        ///     Required: This is the sender name.
        ///     The maximum length is 11 alphanumerical characters.
        /// </summary>
        private string _From { get; set; }

        /// <summary>
        ///     Required: The destination mobile numbers.
        /// </summary>
        private string[] _SmsRecipients { get; set; }

        /// <summary>
        ///     The allowed channels field forces a message to only use certain routes.
        /// </summary>
        [JsonProperty(
            DefaultValueHandling = DefaultValueHandling.Ignore,
            PropertyName = "allowedChannels",
            ItemConverterType = typeof(StringEnumConverter)
        )]
        private SmsChannel _SmsChannel { get; set; }

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
        private Content _Content { get; set; }

        /// <summary>
        ///     Optional : FileName.ext && Path to file
        /// </summary>
        private Attachment _Attachment { get; set; }

        /// <summary>
        ///     Optional : The title of the message
        /// </summary>
        /// [JsonProperty("subject")]
        private string _Subject { get; set; }

        /// <summary>
        /// </summary>
        private bool _IsSchedule { get; set; }

        /// <summary>
        /// </summary>
        private string _ScheduleDate { get; set; }

        /// <summary>
        /// </summary>
        /// <param name="from"></param>
        /// <returns></returns>
        protected Notifiable<T> From(string from)
        {
            _From = from;
            return this;
        }

        /// <summary>
        /// </summary>
        /// <param name="recipients"></param>
        /// <returns></returns>
        public Notifiable<T> Recipient(string[] recipients)
        {
            _SmsRecipients = recipients;
            return this;
        }

        /// <summary>
        /// </summary>
        /// <param name="subject"></param>
        /// <returns></returns>
        public Notifiable<T> Subject(string subject)
        {
            _Subject = subject;
            return this;
        }

        /// <summary>
        /// </summary>
        /// <param name="attachment"></param>
        /// <returns></returns>
        public Notifiable<T> Attach(Attachment attachment)
        {
            _Attachment = attachment;
            return this;
        }

        public Notifiable<T> Schedule(bool isSchedule = false, string scheduleDate = null)
        {
            _IsSchedule = isSchedule;
            _ScheduleDate = !_IsSchedule ? "" : scheduleDate;

            return this;
        }

        /// <summary>
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public Notifiable<T> Content(Content content)
        {
            _Content = content ?? throw new ArgumentNullException(nameof(content));
            return this;
        }

        /// <summary>
        ///     Get the notification's delivery channels.
        /// </summary>
        /// <param name="channel"></param>
        /// <returns></returns>
        public Notifiable<T> Channel(SmsChannel channel)
        {
            _SmsChannel = channel;
            return this;
        }

        /// <summary>
        /// </summary>
        /// <param name="templatePath"></param>
        /// <param name="templateModel"></param>
        /// <returns></returns>
        public Notifiable<T> Template(string templatePath, T templateModel)
        {
            _TemplateModel = templateModel;
            _TemplatePath = templatePath;
            return this;
        }

        public Notifiable<T> Template(string templatePath)
        {
            Template(templatePath, default);
            return this;
        }

        protected virtual void Boot()
        {
        }

        internal async Task<ResponseDto> SendAsync(INotification notification)
        {
            Boot();

            //logic here
            var msg = await BuildMsg()
                .ConfigureAwait(false);

            var attach = await BuildAttach()
                .ConfigureAwait(false);

            return await notification.NotifyAsync(
                msg,
                _Subject,
                _SmsRecipients,
                _From,
                _ScheduleDate,
                _IsSchedule,
                attach
            ).ConfigureAwait(true);
        }

        private async Task<Attachment> BuildAttach()
        {
            if (_Attachment != null)
                return await TemplateRenderer.RenderAttachmentToAttachAsync(_Attachment)
                    .ConfigureAwait(false);

            return _Attachment;
        }

        /// <summary>
        ///     Prepare message
        /// </summary>
        /// <returns></returns>
        private async Task<string> BuildMsg()
        {
            if (_Content != null) return _Content._Content;

            if (_Attachment != null) return _Attachment.File;

            if (_TemplatePath != null)
                return await TemplateRenderer.RenderTemplateToStringAsync(_TemplatePath, _TemplateModel)
                    .ConfigureAwait(false);

            throw new NoSmsRendererFound(NoRenderFoundMessage);
        }
    }
}