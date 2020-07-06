using System;
using System.Collections.Generic;

namespace Structure.Data.Filtering
{
    public class DataFilterOptions
    {
        public Dictionary<Type, DataFilterState> Filters { get; }

        public DataFilterOptions()
        {
            Filters = new Dictionary<Type, DataFilterState>();
        }
    }
}
