using AutoMapper;
using Structure.Domain.Entities;
using Structure.Domain.Entities.Auditing;
using System;

namespace Structure.AutoMapper
{
    public class EntityAutoMapperProfile : Profile
    {
        public IMappingExpression<TEntity, TDto> CreateEntityMap<TEntity, TDto>(bool ignoreEntitiesEmpty = true, Action<IMappingExpression<TDto, TEntity>> configureReverseMap = null)
            where TEntity : IEntity
        {
            var entityMap = CreateMap<TEntity, TDto>();
            var reverseEntityMap = ignoreEntitiesEmpty ? entityMap.ReserseMapWithIgnoreEntitiesEmpty() : entityMap.ReverseMap();
            MapAuditMembers(reverseEntityMap);
            configureReverseMap?.Invoke(reverseEntityMap);
            return entityMap;
        }

        private static void MapAuditMembers<TEntity, TDto>(
            IMappingExpression<TDto, TEntity> reverseEntityMap)
        {
            if (typeof(IMultiTenant).IsAssignableFrom(typeof(TEntity)))
            {
                reverseEntityMap.ForPath<TDto, TEntity, Guid?>("TenantId", map => map.Ignore());
            }

            if (typeof(IMayHaveCreator).IsAssignableFrom(typeof(TEntity)))
            {
                reverseEntityMap.ForPath<TDto, TEntity, Guid?>("CreatorId", map => map.Ignore());
            }

            if (typeof(IMustHaveCreator).IsAssignableFrom(typeof(TEntity)))
            {
                reverseEntityMap.ForPath<TDto, TEntity, Guid>("CreatorId", map => map.Ignore());
            }

            if (typeof(IHasCreationTime).IsAssignableFrom(typeof(TEntity)))
            {
                reverseEntityMap.ForPath<TDto, TEntity, DateTime>("CreationTime", map => map.Ignore());
            }

            if (typeof(IModificationAudited).IsAssignableFrom(typeof(TEntity)))
            {
                reverseEntityMap.ForPath<TDto, TEntity, DateTime?>("LastModificationTime", map => map.Ignore());
                reverseEntityMap.ForPath<TDto, TEntity, Guid?>("LastModifierId", map => map.Ignore());
            }
        }
    }
}
