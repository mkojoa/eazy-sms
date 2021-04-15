using System;
using System.Collections.Generic;
using System.Text;

namespace eazy.sms.Core.EfCore.Entity
{
    public class EventMessage : IEntity
    {
        public string Message { get; set; }
        public string Exceptions { get; set; } 
        public bool Status { get; set; } 
    }
}
