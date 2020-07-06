using System;
using System.Reflection;

namespace Structure.Runtime
{
    public class FieldMemberInfo : IMemberInfo
    {
        private readonly FieldInfo fieldInfo;

        public MemberInfo MemberInfo
        {
            get { return fieldInfo; }
        }

        public Type MemberType
        {
            get { return fieldInfo.FieldType; }
        }

        public bool HasMember
        {
            get { return fieldInfo != null; }
        }

        public void SetValue(object source, object value)
        {
            fieldInfo.SetValue(source, value);
        }

        public object GetValue(object source)
        {
            return fieldInfo.GetValue(source);
        }

        public FieldMemberInfo(FieldInfo fieldInfo)
        {
            this.fieldInfo = fieldInfo;
        }
    }
}
