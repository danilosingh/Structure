using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Structure.Validation.Interception
{
    public interface IMethodParameterValidator 
    {
        IEnumerable<ValidationError> Validate(object validatingObject);
    }
}
