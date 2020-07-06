using NHibernate.Type;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Structure.Nhibernate.Filtering
{
    public class NhFilterDefinition
    {
        public string Condition { get; set; }
        public Dictionary<string, NhFilterDefinitionParameter> Parameters { get; set; } = new Dictionary<string, NhFilterDefinitionParameter>();

        public NhFilterDefinition()
        { }

        public static NhFilterDefinition FromCondition(string condition)
        {
            return new NhFilterDefinition()
            {
                Parameters = new Dictionary<string, NhFilterDefinitionParameter>(),
                Condition = condition
            };
        }

        public static NhFilterDefinition FromParameter(int count, object value, IType type)
        {            
            return new NhFilterDefinition()
            {
                Parameters = { { $":param{count}", new NhFilterDefinitionParameter(value, type) } },
                Condition = $":param{count}"
            };
        }

        public static NhFilterDefinition FromCollection(ref int countStart, IEnumerable values, IType type)
        {
            var parameters = new Dictionary<string, NhFilterDefinitionParameter>();
            var sql = new StringBuilder("(");

            foreach (var value in values)
            {
                parameters.Add($":param{countStart}", new NhFilterDefinitionParameter(values, type));
                sql.Append($":param{countStart},");
                countStart++;
            }

            if (sql.Length == 1)
            {
                sql.Append("null,");
            }

            sql[sql.Length - 1] = ')';

            return new NhFilterDefinition()
            {
                Parameters = parameters,
                Condition = sql.ToString()
            };
        }

        public static NhFilterDefinition FromUnary(string @operator, NhFilterDefinition definition)
        {
            return new NhFilterDefinition()
            {
                Parameters = definition.Parameters,
                Condition = $"({@operator} {definition.Condition})"
            };
        }

        public static NhFilterDefinition Concat(NhFilterDefinition left, string @operator, NhFilterDefinition right)
        {
            return new NhFilterDefinition()
            {
                Parameters = left.Parameters.Union(right.Parameters).ToDictionary(c => c.Key, c => c.Value),
                Condition = $"({left.Condition} {@operator} {right.Condition})"
            };
        }

        public static NhFilterDefinition FromHql(string hql)
        {
            return new NhFilterDefinition()
            {
                Condition = $"{hql}"
            };
        }
    }
}
