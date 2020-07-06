using System;
using System.Linq.Expressions;

namespace Structure.Data.Filtering
{
    public interface IDataFilter
    {
        string Name { get; }
        IDisposable Enable();
        IDisposable Disable();
        bool IsEnabled { get; }
        bool IsEnabledForType(Type type);
        Expression ToExpression();
    }
}
