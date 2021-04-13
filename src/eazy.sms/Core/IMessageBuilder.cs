using eazy.sms.Common;
using eazy.sms.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

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
        ///     The maximum length is 11 alphanumerical characters or 16 digits. Example: 'CM Telecom'
        /// </summary>
        [JsonProperty("from")]
        public string From { get; set; }

        /// <summary>
        ///     Required: The destination mobile numbers.
        ///     This value should be in international format.
        ///     A single mobile number per request. Example: '+233276002658'
        /// </summary>
        [JsonProperty("to")]
        public Recipient[] Recipients { get; set; }

        /// <summary>
        ///     The allowed channels field forces a message to only use certain routes.
        /// </summary>
        [JsonProperty(
            DefaultValueHandling = DefaultValueHandling.Ignore,
            PropertyName = "allowedChannels",
            ItemConverterType = typeof(StringEnumConverter)
        )]
        public Channel[] AllowedChannels { get; set; }

        /// <summary>
        ///     Template data to pass to the Txt view to render.
        /// </summary>
        public T TemplateModel { get; set; }

        /// <summary>
        ///     The Text template to use to generate the message.
        /// </summary>
        public string TemplatePath { get; set; }

        /// <summary>
        ///     Required: The actual text body of the message.
        /// </summary>
        [JsonProperty("body")]
        public Body Body { get; set; }

    }
}
