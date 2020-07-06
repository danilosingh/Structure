using Structure.Domain.Entities.Auditing;

namespace Structure.Nhibernate.Mapping
{
    public class AuditedEntityClassMap<T> : EntityClassMap<T>
        where T : IAudited
    {
        public AuditedEntityClassMap() : base()
        {
            PerformAuditMapping();
        }

        protected virtual void PerformAuditMapping()
        {
            Map(c => c.CreatorId).Indexable();
            Map(c => c.CreationTime).Indexable();
            Map(c => c.LastModifierId).Indexable();
            Map(c => c.LastModificationTime).Indexable();
        }
    }
}
