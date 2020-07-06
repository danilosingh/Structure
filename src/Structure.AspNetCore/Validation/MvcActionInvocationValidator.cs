using Structure.DependencyInjection;
using System;

namespace Structure.AspNetCore.Validation
{
    public class MvcActionInvocationValidator : ActionInvocationValidatorBase
    {
        public MvcActionInvocationValidator(IServiceProvider serviceProvider) : base(serviceProvider)
        { }
    }
}
