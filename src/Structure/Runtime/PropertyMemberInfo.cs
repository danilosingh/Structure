using System;
using System.Reflection;

namespace Structure.Runtime
{
    public class PropertyMemberInfo : IMemberInfo
    {
        private readonly PropertyInfo propInfo;

        public MemberInfo MemberInfo
        {
            get { return propInfo; }
        }

        public Type MemberType
        {
            get { return propInfo.PropertyType; }
        }

        public bool HasMember
        {
            get { return propInfo != null; }
        }

        public void SetValue(object source, object value)
        {
            propInfo.SetValue(source, value, null);
        }

        public object GetValue(object source)
        {
            return propInfo.GetValue(source, null);
        }

        public PropertyMemberInfo(PropertyInfo propInfo)
        {
            this.propInfo = propInfo;
        }
    }
}
