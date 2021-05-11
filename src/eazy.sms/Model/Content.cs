namespace eazy.sms.Model
{
    public class Content
    {
        public Content(string content)
        {
            _Content = content;
        }

        /// <summary>
        ///     The actual text body of the message.
        /// </summary>
        public string _Content { get; }
    }
}