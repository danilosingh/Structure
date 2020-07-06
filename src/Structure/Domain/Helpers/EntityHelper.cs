using Structure.Domain.Entities;
using Structure.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Structure.Domain.Helpers
{
    public static class EntityHelper
    {        
        public static bool IsNullOrEmpty<T>(IEntity<T> entity)
        {
            return entity == null || EqualityComparer<T>.Default.Equals(entity.Id, default);
        }

        public static bool IsNull<T>(IEntity<T> entity)
        {
            return entity == null;
        }

        public static bool Different<T>(IEntity<T> entityA, IEntity<T> entityB)
        {
            if (EqualityComparer<T>.Default.Equals(entityA.Id, default))
            { 
                return entityA.InstaceId != entityB.InstaceId;
            }

            return !EqualityComparer<T>.Default.Equals(entityA.Id, entityB.Id);
        }

        public static bool Equals(IEntity entityA, IEntity entityB)
        {
            Type typeId = entityA.GetIdentifier() != null ? entityA.GetIdentifier().GetType() : typeof(object);

            if (EqualityComparer<object>.Default.Equals(entityA.GetIdentifier(), typeId.GetDefaultValue()))
            {
                return entityA.InstaceId == entityB.InstaceId;
            }

            return EqualityComparer<object>.Default.Equals(entityA.GetIdentifier(), entityB.GetIdentifier());
        }

        public static bool Equals<T>(IEntity<T> entityA, IEntity<T> entityB)
        {                    
            /* Se o persistent Id  estiver vazio então a comparação é feita com o Guid do objeto */
            if (EqualityComparer<T>.Default.Equals(entityA.Id, default))
            {
                return entityA.InstaceId == entityB.InstaceId;
            }

            return EqualityComparer<T>.Default.Equals(entityA.Id, entityB.Id);
        }

        public static bool IsNullOrEmptyCollection<T>(IEnumerable<T> list)
        {
            return list == null || list.Count() == 0;
        }

        public static bool IsNew<T>(IEntity<T> entity)
        {
            return EqualityComparer<T>.Default.Equals(entity.Id, default);
        }
    }
}
