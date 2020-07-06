using Microsoft.AspNetCore.Authorization;
using Structure.Security.Authorization;
using System;

namespace Structure.AspNetCore.Authorization
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class ProtectedAttribute : AuthorizeAttribute, IAuthorizeInfo
    {
        public string[] Permissions { get; set; }
        public bool RequireAll { get; set; }
        public string Area { get; set; }

        public ProtectedAttribute(params string[] permissions)
        {
            Permissions = permissions;
        }
    }
}
