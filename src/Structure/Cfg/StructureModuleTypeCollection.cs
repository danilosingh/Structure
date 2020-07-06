using System;
using System.Collections.Generic;

namespace Structure.Cfg
{
    public class StructureModuleTypeCollection : List<Type>
    {
        public void Add<TModule>()
        {
            Add(typeof(TModule));
        }
    }
}
