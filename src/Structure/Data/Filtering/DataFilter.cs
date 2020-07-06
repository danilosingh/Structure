using Microsoft.Extensions.Options;
using Structure.Extensions;
using Structure.Threading;
using System;
using System.Linq.Expressions;
using System.Threading;

namespace Structure.Data.Filtering
{
    public abstract class DataFilter<T> : IDataFilter
    {
        public bool IsEnabled
        {
            get
            {
                EnsureInitialized();
                return filter.Value.IsEnabled;
            }
        }

        public string Name
        {
            get
            {
                return typeof(T).Name;
            }
        }

        private readonly DataFilterOptions options;

        private readonly AsyncLocal<DataFilterState> filter;

        public DataFilter(IOptions<DataFilterOptions> options)
        {
            this.options = options.Value;
            filter = new AsyncLocal<DataFilterState>();
        }

        public IDisposable Enable()
        {
            if (IsEnabled)
            {
                return NullDisposable.Instance;
            }

            filter.Value.IsEnabled = true;

            return new DisposeAction(() => Disable());
        }

        public IDisposable Disable()
        {
            if (!IsEnabled)
            {
                return NullDisposable.Instance;
            }

            filter.Value.IsEnabled = false;

            return new DisposeAction(() => Enable());
        }

        private void EnsureInitialized()
        {
            if (filter.Value != null)
            {
                return;
            }

            var defaultState = options.Filters.GetOrDefault(GetType())?.Clone() ?? new DataFilterState(true);
            filter.Value = new DataFilterState(defaultState.IsEnabled && CanEnable());
        }

        protected virtual bool CanEnable()
        {
            return true;
        }

        protected abstract Expression<Func<T, bool>> CreateExpression();
        
        public Expression ToExpression()
        {
            return CreateExpression().Body;
        }

        public bool IsEnabledForType(Type type)
        {
            return typeof(T).IsAssignableFrom(type);
        }
    }
}
