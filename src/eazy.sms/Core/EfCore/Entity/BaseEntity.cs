using System;

namespace eazy.sms.Core.EfCore.Entity
{
    public class BaseEntity
    {
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}