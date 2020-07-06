using System;
using System.Reflection;

namespace Structure.Runtime
{
    public interface IMemberInfo
    {
        Type MemberType { get; }
        MemberInfo MemberInfo { get; }
        bool HasMember { get; }

        void SetValue(object source, object value);
        object GetValue(object source);        
    }
}
