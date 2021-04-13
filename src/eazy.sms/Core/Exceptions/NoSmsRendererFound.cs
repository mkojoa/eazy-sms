using System;
using System.Runtime.Serialization;

namespace eazy.sms.Core.Exceptions
{
    [Serializable]
    internal class NoSmsRendererFound : Exception
    {
        public NoSmsRendererFound()
        {
        }

        public NoSmsRendererFound(string message) : base(message)
        {
        }

        public NoSmsRendererFound(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NoSmsRendererFound(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}