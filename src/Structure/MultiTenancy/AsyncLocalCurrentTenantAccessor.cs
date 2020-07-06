using System.Threading;

namespace Structure.MultiTenancy
{
    public class AsyncLocalCurrentTenantAccessor : ICurrentTenantAccessor
    {
        public TenantInfo Current
        {
            get => currentScope.Value;
            set => currentScope.Value = value;
        }

        private readonly AsyncLocal<TenantInfo> currentScope;

        public AsyncLocalCurrentTenantAccessor()
        {
            currentScope = new AsyncLocal<TenantInfo>();
        }
    }
}