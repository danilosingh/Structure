using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Structure.Domain.Entities;
using Structure.Domain.Entities.Auditing;
using Structure.MultiTenancy;
using Structure.Session;
using System;
using System.Collections.Concurrent;

namespace Structure.Auditing
{
    public class EntityPropertySetter : IEntityPropertySetter
    {
        protected readonly ICurrentTenant currentTenant;
        protected readonly ICurrentUser currentUser;
        protected readonly ConcurrentDictionary<string, EntityAuditProperty> properties;
        protected readonly IServiceProvider serviceProvider;

        public EntityPropertySetter(IServiceProvider serviceProvider, ICurrentTenant currentTenant, ICurrentUser currentUser)
        {
            this.serviceProvider = serviceProvider;
            this.currentTenant = currentTenant;
            this.currentUser = currentUser;
            this.properties = new ConcurrentDictionary<string, EntityAuditProperty>();
        }

        public virtual void SetCreationProperties(object entity)
        {
            if (IsEnabled(EntityAuditPropertyNames.TenantId))
            {
                SetTenantId(entity);
            }

            if (IsEnabled(EntityAuditPropertyNames.Creation))
            {
                SetCreationTime(entity);
                SetCreatorId(entity);
            }
        }

        public virtual void SetModificationProperties(object entity)
        {
            if (IsEnabled(EntityAuditPropertyNames.Modification))
            {
                SetLastModificationTime(entity);
                SetLastModifierId(entity);
            }
        }

        public virtual void SetDeletionProperties(object entity)
        { }

        public IDisposable Enable(string propertyName)
        {
            return GetOrAdd(propertyName).Enable();
        }

        public IDisposable Disable(string propertyName)
        {
            return GetOrAdd(propertyName).Disable();
        }

        protected void SetTenantId(object entity)
        {
            if (entity is IMultiTenant objectMultiTenant &&
                objectMultiTenant.TenantId == null &&
                currentTenant.Id != null)
            {
                objectMultiTenant.TenantId = currentTenant.Id;
            }
        }

        protected void SetCreationTime(object entity)
        {
            if (!(entity is IHasCreationTime objectWithCreationTime))
            {
                return;
            }

            if (objectWithCreationTime.CreationTime == default)
            {
                objectWithCreationTime.CreationTime = DateTime.Now;
            }
        }

        protected void SetCreatorId(object entity)
        {
            if (!currentUser.Id.HasValue)
            {
                return;
            }

            if (entity is IMayHaveCreator mayHaveCreatorObject)
            {
                if (mayHaveCreatorObject.CreatorId.HasValue && mayHaveCreatorObject.CreatorId.Value != default)
                {
                    return;
                }

                mayHaveCreatorObject.CreatorId = currentUser.Id;
            }
            else if (entity is IMustHaveCreator mustHaveCreatorObject)
            {
                if (mustHaveCreatorObject.CreatorId != default)
                {
                    return;
                }

                mustHaveCreatorObject.CreatorId = currentUser.Id.Value;
            }
        }

        protected void SetLastModificationTime(object entity)
        {
            if (entity is IHasModificationTime objectWithModificationTime)
            {
                objectWithModificationTime.LastModificationTime = DateTime.Now;
            }
        }

        protected void SetLastModifierId(object entity)
        {
            if (!(entity is IModificationAudited modificationAuditedEntity))
            {
                return;
            }

            if (!currentUser.Id.HasValue)
            {
                modificationAuditedEntity.LastModifierId = null;
                return;
            }

            if (modificationAuditedEntity is IMultiTenant multiTenantEntity)
            {
                if (multiTenantEntity.TenantId != currentTenant.Id)
                {
                    modificationAuditedEntity.LastModifierId = null;
                    return;
                }
            }

            modificationAuditedEntity.LastModifierId = currentUser.Id;
        }

        protected bool IsEnabled(string propertyName)
        {
            return GetOrAdd(propertyName).IsEnabled;
        }

        protected EntityAuditProperty GetOrAdd(string propertyName)
        {
            return properties.GetOrAdd(propertyName, (key) =>
                new EntityAuditProperty(key, serviceProvider.GetRequiredService<IOptions<AuditOptions>>()));
        }
    }
}
