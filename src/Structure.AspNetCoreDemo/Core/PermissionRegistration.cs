using Structure.Localization;
using Structure.Security.Authorization;

namespace Structure.AspNetCoreDemo.Core
{
    public class PermissionRegistration : LocalizableContext, IPermissionRegistration
    {
        public PermissionRegistration(ILocalizer localizer) : base(localizer)
        { }

        public void Register(IPermissionCollection permissions)
        {
            permissions.Add(PermissionNames.User, L("User"))
                .AddChild(PermissionNames.User_GetAll, L("List"))
                .AddChild(PermissionNames.User_Get, L("View"))
                .AddChild(PermissionNames.User_Update, L("Edit"));
        }
    }
}
