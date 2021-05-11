namespace eazy.sms.Common
{
    /// <summary>
    ///     Get the notification's delivery channels.
    /// </summary>
    public enum SmsChannel
    {
        ///// <summary>
        /////     Messages will be sent using hubtel gateway
        ///// </summary>
        //Hubtel,

        /// <summary>
        ///     Send messages using Mnotify gateway
        /// </summary>
        /// <remarks>
        ///     Note that CM needs to configure this for you to work.
        /// </remarks>
        Mnotify
    }
}