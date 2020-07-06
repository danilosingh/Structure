using NHibernate.Type;

namespace Structure.Nhibernate.Filtering
{
    public class NhFilterDefinitionParameter
    {
        public object Value { get; set; }
        public IType Type { get; set; }

        public NhFilterDefinitionParameter(object value, IType type)
        {
            Value = value;
            Type = type;
        }
    }
}