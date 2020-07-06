using NHibernate;
using NHibernate.Dialect;
using NHibernate.Dialect.Function;
using System.Data;

namespace Structure.Nhibernate.Dialects
{
    public class SceSqlServer2012Dialect : MsSql2012Dialect
    {
        public SceSqlServer2012Dialect()
        {
            RegisterFunction(
                "AddDays",
                new SQLFunctionTemplate(
                    NHibernateUtil.DateTime,
                    "dateadd(day,?2,?1)"
                    )
                );

            RegisterFunction(
               "AddTimeSpan",
               new SQLFunctionTemplate(
                   NHibernateUtil.DateTime,
                   "dateadd(second, (?2 / 10000000), ?1)"
                   )
               );

            RegisterFunction(
               "SubtractTimeSpan",
               new SQLFunctionTemplate(
                   NHibernateUtil.DateTime,
                   "dateadd(second, (-cast(?2 as bigint) / 10000000), ?1)"
                   )
               );

            RegisterFunction(
                "Birthday",
                new SQLFunctionTemplate(
                    NHibernateUtil.DateTime,
                    @"
                    ((RIGHT('00' + CONVERT(varchar, MONTH(?1)), 2) +
                    RIGHT('00' + CONVERT(varchar, DAY(?1)), 2)) >= 
                    (RIGHT('00' + CONVERT(varchar, MONTH(?2)), 2) +
                     RIGHT('00' + CONVERT(varchar, DAY(?2)), 2)) AND
                    (RIGHT('00' + CONVERT(varchar, MONTH(?1)), 2) +
                    RIGHT('00' + CONVERT(varchar, DAY(?1)), 2)) <= 
                    (RIGHT('00' + CONVERT(varchar, MONTH(?3)), 2) +
                     RIGHT('00' + CONVERT(varchar, DAY(?3)), 2)))"
                    )
                );

            RegisterFunction(
               "TryConvert", new SQLFunctionTemplate(NHibernateUtil.String,
                @"
                   TRY_CONVERT(int, ?1)
                  ")
               );

            RegisterFunction(
              "IsCurrentDate",
              new SQLFunctionTemplate(
                  NHibernateUtil.DateTime,
                  "Convert(date, ?1) = Convert(date, getdate())")
              );

            RegisterFunction(
              "IsLessOrEqualCurrentDateTime",
              new SQLFunctionTemplate(
                  NHibernateUtil.DateTime,
                  "?1 <= getdate()")
              );

            RegisterFunction(
               "Between",
               new SQLFunctionTemplate(
                   NHibernateUtil.DateTime,
                   "?1 between ?2 and ?3"
                   )
               );

            RegisterFunction(
               "Between",
               new SQLFunctionTemplate(
                   NHibernateUtil.Decimal,
                   "?1 between ?2 and ?3"
                   )
               );

            RegisterFunction(
               "Between",
               new SQLFunctionTemplate(
                   NHibernateUtil.Int32,
                   "?1 between ?2 and ?3"
                   )
               );

            RegisterFunction(
               "GetAge",
               new SQLFunctionTemplate(
                   NHibernateUtil.Int32,
                   "convert(int, DATEDIFF(d, ?1, getdate()) / 365.25)"
                   )
               );

            RegisterFunction(
              "DiffTotalHours",
              new SQLFunctionTemplate(
                  NHibernateUtil.TimeSpan,
                  "DATEDIFF(hour, ?1, ?2)"
                  )
              );

            RegisterFunction(
            "IsNull",
            new SQLFunctionTemplate(
                NHibernateUtil.Decimal,
                "isnull(?1, ?2)"
                )
            );

            RegisterFunction(
                "DiffTotalDays",
                new SQLFunctionTemplate(
                    NHibernateUtil.DateTime,
                    "datediff(day,?1,?2)"
                    )
                );

            RegisterColumnType(DbType.Object, "SQL_VARIANT");
        }
    }
}