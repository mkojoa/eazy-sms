using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace eazy.sms.Core.EfCore.Entity
{
    public class EventMessage : BaseEntity
    {
        private string _message;

        public Guid Id { get; set; }
        public string Message { get; set; }
        public string ResultMessage { get; set; }
        public string ResultStatus { get; set; }
        public int SentStatus { get; set; }
    }
}