using System;

namespace eazy.sms.Core.EfCore.Entity
{
    public class EventMessage
    {
        public Guid Id { get; set; }
        public string Message { get; set; }
        public string Exceptions { get; set; }
        public int Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}