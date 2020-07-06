using Dapper;
using Structure.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Structure.Dapper
{
    public static class DataContextExtensions
    {
        public static int Execute(this IAdoSupport cnn, string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return cnn.DbConnection.Execute(sql, param, cnn.CurrentTransaction, commandTimeout, commandType);
        }

        public static int Execute(this IAdoSupport ado, CommandDefinition command)
        {
            return ado.DbConnection.Execute(command);
        }
        public static Task<int> ExecuteAsync(this IAdoSupport ado, CommandDefinition command)
        {
            return ado.DbConnection.ExecuteAsync(command);
        }
        public static Task<int> ExecuteAsync(this IAdoSupport ado, string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return ado.DbConnection.ExecuteAsync(sql, param, ado.CurrentTransaction, commandTimeout, commandType);
        }
        public static IDataReader ExecuteReader(this IAdoSupport ado, CommandDefinition command, CommandBehavior commandBehavior)
        {
            return ado.DbConnection.ExecuteReader(command, commandBehavior);
        }

        //public static IDataReader ExecuteReader(this IAdoSupport ado, CommandDefinition command);
        //public static IDataReader ExecuteReader(this IAdoSupport ado, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null);
        //public static Task<IDataReader> ExecuteReaderAsync(this IAdoSupport ado, CommandDefinition command, CommandBehavior commandBehavior);
        //public static Task<DbDataReader> ExecuteReaderAsync(this DbConnection cnn, CommandDefinition command);
        //public static Task<IDataReader> ExecuteReaderAsync(this IAdoSupport ado, CommandDefinition command);
        //public static Task<DbDataReader> ExecuteReaderAsync(this DbConnection cnn, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null);
        //public static Task<IDataReader> ExecuteReaderAsync(this IAdoSupport ado, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null);
        //public static Task<DbDataReader> ExecuteReaderAsync(this DbConnection cnn, CommandDefinition command, CommandBehavior commandBehavior);
        //public static object ExecuteScalar(this IAdoSupport ado, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null);
        //public static T ExecuteScalar<T>(this IAdoSupport ado, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null);
        //public static T ExecuteScalar<T>(this IAdoSupport ado, CommandDefinition command);
        //public static object ExecuteScalar(this IAdoSupport ado, CommandDefinition command);
        //public static Task<object> ExecuteScalarAsync(this IAdoSupport ado, CommandDefinition command);
        //public static Task<T> ExecuteScalarAsync<T>(this IAdoSupport ado, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null);
        //public static Task<object> ExecuteScalarAsync(this IAdoSupport ado, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null);
        //public static Task<T> ExecuteScalarAsync<T>(this IAdoSupport ado, CommandDefinition command);
        //public static IEnumerable<Tuple<string, string, int>> GetCachedSQL(int ignoreHitCountAbove = int.MaxValue);

        public static IEnumerable<object> Query(this IAdoSupport ado, Type type, string sql, object param = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
        {
            return ado.DbConnection.Query(type, sql, param, ado.CurrentTransaction, buffered, commandTimeout, commandType);
        }
        public static IEnumerable<T> Query<T>(this IAdoSupport ado, string sql, object param = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
        {
            return ado.DbConnection.Query<T>(sql, param, ado.CurrentTransaction, buffered, commandTimeout, commandType);
        }
        public static IEnumerable<TReturn> Query<TReturn>(this IAdoSupport ado, string sql, Type[] types, Func<object[], TReturn> map, object param = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
        {
            return ado.DbConnection.Query<TReturn>(sql, types, map, param, ado.CurrentTransaction, buffered, splitOn, commandTimeout, commandType);
        }
        public static IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn>(this IAdoSupport ado, string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn> map, object param = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
        {
            return ado.DbConnection.Query(sql, map, param, ado.CurrentTransaction, buffered, splitOn, commandTimeout, commandType);
        }
        public static IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn>(this IAdoSupport ado, string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn> map, object param = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
        {
            return ado.DbConnection.Query(sql, map, param, ado.CurrentTransaction, buffered, splitOn, commandTimeout, commandType);
        }
        public static IEnumerable<TReturn> Query<TFirst, TSecond, TReturn>(this IAdoSupport ado, string sql, Func<TFirst, TSecond, TReturn> map, object param = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
        {
            return ado.DbConnection.Query(sql, map, param, ado.CurrentTransaction, buffered, splitOn, commandTimeout, commandType);
        }
        public static IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(this IAdoSupport ado, string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> map, object param = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
        {
            return ado.DbConnection.Query(sql, map, param, ado.CurrentTransaction, buffered, splitOn, commandTimeout, commandType);
        }
        public static IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TReturn>(this IAdoSupport ado, string sql, Func<TFirst, TSecond, TThird, TFourth, TReturn> map, object param = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
        {
            return ado.DbConnection.Query(sql, map, param, ado.CurrentTransaction, buffered, splitOn, commandTimeout, commandType);
        }
        public static IEnumerable<dynamic> Query(this IAdoSupport ado, string sql, object param = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
        {
            return ado.DbConnection.Query(sql, param, ado.CurrentTransaction, buffered, commandTimeout, commandType);
        }
        public static IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TReturn>(this IAdoSupport ado, string sql, Func<TFirst, TSecond, TThird, TReturn> map, object param = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
        {
            return ado.DbConnection.Query(sql, map, param, ado.CurrentTransaction, buffered, splitOn, commandTimeout, commandType);
        }
        public static IEnumerable<T> Query<T>(this IAdoSupport ado, CommandDefinition command)
        {
            return ado.DbConnection.Query<T>( command);
        }


        //TODO: Imp all methods

        //public static Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TReturn>(this IAdoSupport ado, CommandDefinition command, Func<TFirst, TSecond, TReturn> map, string splitOn = "Id");
        //public static Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TReturn>(this IAdoSupport ado, string sql, Func<TFirst, TSecond, TReturn> map, object param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null);
        //public static Task<IEnumerable<dynamic>> QueryAsync(this IAdoSupport ado, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null);
        //public static Task<IEnumerable<dynamic>> QueryAsync(this IAdoSupport ado, CommandDefinition command);
        //public static Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TReturn>(this IAdoSupport ado, string sql, Func<TFirst, TSecond, TThird, TReturn> map, object param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null);
        //public static Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TReturn>(this IAdoSupport ado, CommandDefinition command, Func<TFirst, TSecond, TThird, TReturn> map, string splitOn = "Id");
        //public static Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TFourth, TReturn>(this IAdoSupport ado, string sql, Func<TFirst, TSecond, TThird, TFourth, TReturn> map, object param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null);
        //public static Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TFourth, TReturn>(this IAdoSupport ado, CommandDefinition command, Func<TFirst, TSecond, TThird, TFourth, TReturn> map, string splitOn = "Id");
        //public static Task<IEnumerable<object>> QueryAsync(this IAdoSupport ado, Type type, CommandDefinition command);
        //public static Task<IEnumerable<T>> QueryAsync<T>(this IAdoSupport ado, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null);
        //public static Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(this IAdoSupport ado, string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> map, object param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null);
        //public static Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn>(this IAdoSupport ado, string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn> map, object param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null);
        //public static Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn>(this IAdoSupport ado, CommandDefinition command, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn> map, string splitOn = "Id");
        //public static Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn>(this IAdoSupport ado, string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn> map, object param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null);
        //public static Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn>(this IAdoSupport ado, CommandDefinition command, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn> map, string splitOn = "Id");
        //public static Task<IEnumerable<TReturn>> QueryAsync<TReturn>(this IAdoSupport ado, string sql, Type[] types, Func<object[], TReturn> map, object param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null);
        //public static Task<IEnumerable<object>> QueryAsync(this IAdoSupport ado, Type type, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null);
        //public static Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(this IAdoSupport ado, CommandDefinition command, Func<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> map, string splitOn = "Id");
        //public static Task<IEnumerable<T>> QueryAsync<T>(this IAdoSupport ado, CommandDefinition command);
        //public static T QueryFirst<T>(this IAdoSupport ado, CommandDefinition command);
        //public static object QueryFirst(this IAdoSupport ado, Type type, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null);
        //public static T QueryFirst<T>(this IAdoSupport ado, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null);
        //public static dynamic QueryFirst(this IAdoSupport ado, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null);
        //public static Task<dynamic> QueryFirstAsync(this IAdoSupport ado, CommandDefinition command);
        //public static Task<T> QueryFirstAsync<T>(this IAdoSupport ado, CommandDefinition command);
        //public static Task<dynamic> QueryFirstAsync(this IAdoSupport ado, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null);
        //public static Task<object> QueryFirstAsync(this IAdoSupport ado, Type type, CommandDefinition command);
        //public static Task<T> QueryFirstAsync<T>(this IAdoSupport ado, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null);
        //public static Task<object> QueryFirstAsync(this IAdoSupport ado, Type type, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null);
        //public static T QueryFirstOrDefault<T>(this IAdoSupport ado, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null);
        //public static T QueryFirstOrDefault<T>(this IAdoSupport ado, CommandDefinition command);
        //public static dynamic QueryFirstOrDefault(this IAdoSupport ado, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null);
        //public static object QueryFirstOrDefault(this IAdoSupport ado, Type type, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null);
        //public static Task<dynamic> QueryFirstOrDefaultAsync(this IAdoSupport ado, CommandDefinition command);
        //public static Task<dynamic> QueryFirstOrDefaultAsync(this IAdoSupport ado, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null);
        //public static Task<T> QueryFirstOrDefaultAsync<T>(this IAdoSupport ado, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null);
        //public static Task<object> QueryFirstOrDefaultAsync(this IAdoSupport ado, Type type, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null);
        //public static Task<T> QueryFirstOrDefaultAsync<T>(this IAdoSupport ado, CommandDefinition command);
        //public static Task<object> QueryFirstOrDefaultAsync(this IAdoSupport ado, Type type, CommandDefinition command);
        //public static GridReader QueryMultiple(this IAdoSupport ado, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null);
        //public static GridReader QueryMultiple(this IAdoSupport ado, CommandDefinition command);
        //public static Task<GridReader> QueryMultipleAsync(this IAdoSupport ado, CommandDefinition command);
        //public static Task<GridReader> QueryMultipleAsync(this IAdoSupport ado, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null);
        //public static object QuerySingle(this IAdoSupport ado, Type type, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null);
        //public static T QuerySingle<T>(this IAdoSupport ado, CommandDefinition command);
        //public static T QuerySingle<T>(this IAdoSupport ado, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null);
        //public static dynamic QuerySingle(this IAdoSupport ado, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null);
        //public static Task<dynamic> QuerySingleAsync(this IAdoSupport ado, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null);
        //public static Task<object> QuerySingleAsync(this IAdoSupport ado, Type type, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null);
        //public static Task<T> QuerySingleAsync<T>(this IAdoSupport ado, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null);
        //public static Task<object> QuerySingleAsync(this IAdoSupport ado, Type type, CommandDefinition command);
        //public static Task<T> QuerySingleAsync<T>(this IAdoSupport ado, CommandDefinition command);
        //public static Task<dynamic> QuerySingleAsync(this IAdoSupport ado, CommandDefinition command);
        //public static dynamic QuerySingleOrDefault(this IAdoSupport ado, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null);
        //public static T QuerySingleOrDefault<T>(this IAdoSupport ado, CommandDefinition command);
        //public static T QuerySingleOrDefault<T>(this IAdoSupport ado, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null);
        //public static object QuerySingleOrDefault(this IAdoSupport ado, Type type, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null);
        //public static Task<object> QuerySingleOrDefaultAsync(this IAdoSupport ado, Type type, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null);
        //public static Task<object> QuerySingleOrDefaultAsync(this IAdoSupport ado, Type type, CommandDefinition command);
        //public static Task<dynamic> QuerySingleOrDefaultAsync(this IAdoSupport ado, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null);
        //public static Task<T> QuerySingleOrDefaultAsync<T>(this IAdoSupport ado, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null);
        //public static Task<dynamic> QuerySingleOrDefaultAsync(this IAdoSupport ado, CommandDefinition command);
        //public static Task<T> QuerySingleOrDefaultAsync<T>(this IAdoSupport ado, CommandDefinition command);
    }
}
