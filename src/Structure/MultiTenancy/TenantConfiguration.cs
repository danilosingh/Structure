using System;

namespace Structure.MultiTenancy
{
    public class TenantConfiguration
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        //TODO: Add support ConnectionStrings
        //public ConnectionStrings ConnectionStrings { get; set; }

        public TenantConfiguration()
        {

        }

        public TenantConfiguration(Guid id, string name)
        {
            Id = id;
            Name = name;

        }
    }
}
