using Microsoft.Extensions.Options;
using Structure.Extensions;
using Structure.Threading;
using System;
using System.Threading;

namespace Structure.Auditing
{
    public class EntityAuditProperty
    {
        private readonly AuditOptions options;
        private readonly AsyncLocal<EntityAuditPropertyState> property;

        public bool IsEnabled
        {
            get
            {
                EnsureInitialized();
                return property.Value.IsEnabled;
            }
        }

        public string Name { get; }

        public EntityAuditProperty(string name, IOptions<AuditOptions> options)
        {
            Name = name;
            property = new AsyncLocal<EntityAuditPropertyState>();
            this.options = options.Value;
        }

        public IDisposable Enable()
        {
            if (IsEnabled)
            {
                return NullDisposable.Instance;
            }

            property.Value.IsEnabled = true;

            return new DisposeAction(() => Disable());
        }

        public IDisposable Disable()
        {
            if (!IsEnabled)
            {
                return NullDisposable.Instance;
            }

            property.Value.IsEnabled = false;

            return new DisposeAction(() => Enable());
        }

        private void EnsureInitialized()
        {
            if (property.Value != null)
            {
                return;
            }

            property.Value = options.DefaultStates.GetOrDefault(Name)?.Clone() ?? new EntityAuditPropertyState(true);
        }
    }
}
