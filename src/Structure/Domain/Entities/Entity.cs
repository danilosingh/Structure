using Structure.Domain.Helpers;
using Structure.Domain.Notifications;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Structure.Domain.Entities
{
    public abstract class Entity : ICloneable, IEntity
    {
        protected Guid instaceId;

        public virtual Guid InstaceId
        {
            get
            {
                if (instaceId == null || instaceId == Guid.Empty)
                {
                    instaceId = Guid.NewGuid();
                }

                return instaceId;
            }
        }

        public virtual object Clone()
        {
            return EntityClonerHelper.Clone(this);
        }

        public abstract object GetIdentifier();

        public abstract bool HasIdentifier();
    }

    public abstract class Entity<TId> : Entity, IEntity<TId>
    {
        [Key]
        public virtual TId Id { get; set; }

        public override object GetIdentifier()
        {
            return Id;
        }

        public virtual INotificationCollection Notifications
        {
            get { return new NotificationCollection(); }
        }

        public override bool HasIdentifier()
        {
            return !EqualityComparer<TId>.Default.Equals(Id, default);
        }
    }
}
