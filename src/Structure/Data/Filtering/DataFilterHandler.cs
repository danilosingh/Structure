using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Structure.Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Structure.Data.Filtering
{
    public class DataFilterHandler : IDataFilterHandler
    {
        private readonly ConcurrentDictionary<Type, object> filters;

        private readonly IServiceProvider serviceProvider;
        private readonly DataFilterOptions options;

        public DataFilterHandler(IServiceProvider serviceProvider, IOptions<DataFilterOptions> options)
        {
            this.serviceProvider = serviceProvider;
            this.options = options.Value;
            filters = new ConcurrentDictionary<Type, object>();
        }

        public IDisposable Enable<TFilter>()
            where TFilter : IDataFilter
        {
            return GetFilter<TFilter>().Enable();
        }

        public IDisposable Disable<TFilter>()
            where TFilter : IDataFilter
        {
            return GetFilter<TFilter>().Disable();
        }

        public bool IsEnabled<TFilter>()
            where TFilter : IDataFilter
        {
            return GetFilter<TFilter>().IsEnabled;
        }

        private IDataFilter GetFilter<TFilter>()
            where TFilter : IDataFilter
        {
            return GetFilter(typeof(TFilter));
        }

        private IDataFilter GetFilter(Type filterType)
        {
            return filters.GetOrAdd(filterType,
                () => serviceProvider.GetRequiredService(filterType)) as IDataFilter;
        }

        public IList<IDataFilter> GetEnabledFilters()
        {
            return options.Filters.Keys
                .Select(c => GetFilter(c))
                .Where(c => c.IsEnabled)
                .ToList();
        }

        public IList<IDataFilter> GetFilters()
        {
            return options.Filters.Keys
                .Select(c => GetFilter(c))
                .ToList();
        }
    }
}
