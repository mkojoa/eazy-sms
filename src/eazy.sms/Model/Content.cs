using Newtonsoft.Json;

namespace eazy.sms.Model
{
    public class Content
    {
        /// <summary>
        ///     The actual text body of the message.
        /// </summary>
        public string _Content { get; set; }

        public Content(string content)
        {
            _Content = content;
        }
    }
}