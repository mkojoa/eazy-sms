using System;

namespace eazy.sms.Core.EfCore.Entity
{
    public abstract class IEntity
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}