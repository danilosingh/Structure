using Structure.Data.Filtering;
using System;

namespace Structure.Nhibernate.Filtering
{
    public class NhDataFilter
    {
        private readonly IDataFilter dataFilter;

        public string Name 
        {
            get { return dataFilter.Name; }
        }

        public Type Type
        {
            get { return dataFilter.GetType(); }
        }

        public NhFilterDefinition FilterDefinition { get; }

        public NhDataFilter(IDataFilter dataFilter, NhFilterDefinition filterDefinition)
        {
            this.dataFilter = dataFilter;
            FilterDefinition = filterDefinition;
        }

        public bool IsEnabledForType(Type type)
        {
            return dataFilter.IsEnabledForType(type);
        }

        public bool Is(Type type)
        {
            return dataFilter.GetType() == type;
        }
    }
}
