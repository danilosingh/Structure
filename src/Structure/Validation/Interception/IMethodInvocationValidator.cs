using System.Collections.Generic;
using System.Reflection;

namespace Structure.Validation.Interception
{
    public interface IMethodInvocationValidator
    {
        IList<ValidationError> Validate(MethodInfo method, object[] parameterValues);
    }
}