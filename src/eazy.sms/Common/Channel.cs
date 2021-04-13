namespace eazy.sms.Common
{
    /// <summary>
    ///
    /// </summary>
    public enum Channel
    {
        /// <summary>
        ///     Messages will be sent using hubtel gatway
        /// </summary>
        Hubtel,

        /// <summary>
        ///     Send messages using Mnotify gateway
        /// </summary>
        /// <remarks>
        ///     Note that CM needs to configure this for you to work.
        /// </remarks>
        Mnotify, 
    }
}
