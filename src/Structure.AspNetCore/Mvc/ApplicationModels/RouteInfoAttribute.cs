using System;

namespace Structure.AspNetCore.Mvc.ApplicationModels
{
    public class RouteInfoAttribute : Attribute
    {
        public string ActionName { get; set; }

        public RouteInfoAttribute(string actionName)
        {
            ActionName = actionName;
        }
    }
}
