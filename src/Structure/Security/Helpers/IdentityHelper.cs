using System;
using System.Security.Principal;

namespace Structure.Security.Helpers
{
    public static class IdentityHelper
    {
        public static bool WindowsUserIsAdministrator()
        {            
            try
            {
                WindowsPrincipal principal = new WindowsPrincipal(WindowsIdentity.GetCurrent());
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }            
            catch (Exception)
            {
                return false;
            }            
        }
    }
}
