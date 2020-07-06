using Structure.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Structure.AspNetCore
{
    public class AspNetCoreOptions
    {
        public MvcRouteOptions Routes { get; set; }        
        public List<IStructureAspNetAddOn> AddOns { get; set; }
        
        public AspNetCoreOptions()
        {
            Routes = new MvcRouteOptions();
            AddOns = new List<IStructureAspNetAddOn>();
        }
    }
}
