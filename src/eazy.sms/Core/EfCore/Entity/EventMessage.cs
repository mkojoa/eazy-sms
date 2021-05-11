using System;

namespace eazy.sms.Core.EfCore.Entity
{
    public class EventMessage : BaseEntity
    {
        public Guid Id { get; set; }
        public string Message { get; set; }
        public string ResultMessage { get; set; }
        public string ResultStatus { get; set; }
        public int SentStatus { get; set; }
    }
}