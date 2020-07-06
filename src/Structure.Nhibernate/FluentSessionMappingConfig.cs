using Structure.Nhibernate.Filtering;
using System.Collections.Generic;

namespace Structure.Nhibernate
{
    public class FluentSessionMappingConfig
    {
        public static FluentSessionMappingConfig Instance = new FluentSessionMappingConfig();

        public IList<NhDataFilter> Filters { get; }
        public bool UseCamelCaseNames { get; set; }

        public FluentSessionMappingConfig()
        {
            Filters = new List<NhDataFilter>();
        }
    }
}
