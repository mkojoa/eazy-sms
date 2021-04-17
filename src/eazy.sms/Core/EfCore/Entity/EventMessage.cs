using System;

namespace eazy.sms.Core.EfCore.Entity
{
    public class EventMessage : BaseEntity
    {
        public Guid Id { get; set; }
        public string Message { get; set; }
        public string Exceptions { get; set; }
        public string ExceptionStatus { get; set; }
        public int Status { get; set; }
    }
}