using Structure.Nhibernate.Mapping;
using Structure.Tests.Shared.Entities;

namespace Structure.AspNetCoreDemo.Core.Mappings
{
    public class UserMap : AuditedEntityClassMap<User>
    {
        public UserMap() 
        { }

        protected override void PerformMapping()
        {            
            Id(c => c.Id).GeneratedBy.GuidComb();
            
            Map(c => c.Name).Indexable().Length(60);
            Map(c => c.Email).Indexable().Length(60).Unique();
            Map(c => c.NormalizedUserName).Indexable().Length(60).Unique();
            Map(c => c.UserName).Indexable().Length(60).ReadOnly();
            Map(c => c.IsDeleted).Indexable().Default("false");            
            Map(c => c.TenantId).Indexable();
            Map(c => c.PasswordHash).Length(255);
            Map(c => c.PhoneNumber).Length(15);              
        }
    }
}
