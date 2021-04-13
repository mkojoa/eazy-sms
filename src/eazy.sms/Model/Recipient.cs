using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace eazy.sms.Model
{
    /// <summary>
    ///     A destination mobile number.
    /// </summary>
    public class Recipient
    {

        /// <summary>
        ///     This value should be in international format.
        ///     A single mobile number per request. Example: '[+233]276002658'
        /// </summary>
        [JsonProperty("number")]
        public string Number { get; set; }
    }
}
