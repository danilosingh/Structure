using System;
using System.Collections.Generic;

namespace Structure.Data.Filtering
{
    public interface IDataFilterHandler
    {
        IDisposable Enable<TFilter>() where TFilter : IDataFilter;
        IDisposable Disable<TFilter>() where TFilter : IDataFilter;
        bool IsEnabled<TFilter>() where TFilter : IDataFilter;
        IList<IDataFilter> GetEnabledFilters();
        IList<IDataFilter> GetFilters();
    }
}
