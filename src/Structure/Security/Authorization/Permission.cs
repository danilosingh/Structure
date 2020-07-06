using Structure.Collections;
using Structure.Localization;
using Structure.MultiTenancy;
using System;
using System.Collections.Generic;

namespace Structure.Security.Authorization
{
    public class Permission : IHierarchical<Permission>, IKey<string>
    {
        private readonly List<Permission> children;

        public Permission Parent { get; private set; }
        public string Name { get; }
        public ILocalizableString DisplayName { get; set; }
        public ILocalizableString Description { get; set; }
        public MultiTenancySides MultiTenancySides { get; set; }

        public IEnumerable<Permission> Children
        {
            get { return children.AsReadOnly(); }
        }
        public string Key { get { return Name; } }

        public Permission(
            string name,
            ILocalizableString displayName = null,
            ILocalizableString description = null,
            MultiTenancySides multiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant)
        {
            Name = name ?? throw new ArgumentNullException("name");
            DisplayName = displayName;
            Description = description;
            MultiTenancySides = multiTenancySides;

            children = new List<Permission>();
        }

        public Permission AddChild(
            string name,
            ILocalizableString displayName = null,
            ILocalizableString description = null,
            MultiTenancySides multiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant)
        {
            var permission = new Permission(name, displayName, description, multiTenancySides) { Parent = this };
            children.Add(permission);
            return permission;
        }

        public void RemoveChildPermission(string name)
        {
            children.RemoveAll(p => p.Name == name);
        }

        public override string ToString()
        {
            return string.Format("[Permission: {0}]", Name);
        }
    }
}
