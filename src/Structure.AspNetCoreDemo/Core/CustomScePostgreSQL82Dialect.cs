using NHibernate;
using NHibernate.Dialect.Function;

namespace Structure.Nhibernate.Dialects
{
    public class CustomScePostgreSQL82Dialect : ScePostgreSQL82Dialect
    {
        public CustomScePostgreSQL82Dialect()
        {
            RegisterFunction(
                "StartsWithIgnoreCaseAndDiacritics",
                new SQLFunctionTemplate(
                    NHibernateUtil.DateTime,
                    "unaccent(lower(?1)) like  unaccent(lower(?2)) || '%'"
                    )
                );
        }
    }
}