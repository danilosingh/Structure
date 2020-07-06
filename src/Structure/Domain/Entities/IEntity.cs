using System;

namespace Structure.Domain.Entities
{
    public interface IEntity 
    {
        Guid InstaceId { get; }
        object GetIdentifier();
        bool HasIdentifier();
    }

    public interface IEntity<TId> : IEntity
    {        
        TId Id { get; set; }
    }
}
