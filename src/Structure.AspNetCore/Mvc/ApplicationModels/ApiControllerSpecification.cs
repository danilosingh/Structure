using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System;
using System.Reflection;

namespace Structure.AspNetCore.Mvc.ApplicationModels
{
    public class ApiControllerSpecification : IApiControllerSpecification
    {
        readonly Type apiControllerType = typeof(ApiControllerBase).GetTypeInfo();

        public bool IsSatisfiedBy(ControllerModel controller) => apiControllerType.IsAssignableFrom(controller.ControllerType);
}
}
