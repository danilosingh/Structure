using Structure.Helpers;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Structure.Runtime
{
    public class MemberInfoFactory
    {
        public static IMemberInfo Create(MemberInfo memberInfo)
        {
            switch (memberInfo)
            {
                case FieldInfo fieldInfo:
                    return new FieldMemberInfo(fieldInfo);
                case PropertyInfo propertyInfo:
                    return new PropertyMemberInfo(propertyInfo);
                default:
                    throw new NotImplementedException();
            }
        }

        public static IMemberInfo Create<T>(Expression<Func<T, object>> member)
        {
            return Create(ExpressionHelper.GetMemberInfo(member));
        }
    }
}
