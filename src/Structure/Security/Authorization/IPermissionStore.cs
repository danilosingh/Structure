using System.Collections.Generic;

namespace Structure.Security.Authorization
{
    public interface IPermissionStore 
    {
        Permission GetByName(string name);
        Permission GetOrNullByName(string name);
        IEnumerable<Permission> GetAll();
    }
}
