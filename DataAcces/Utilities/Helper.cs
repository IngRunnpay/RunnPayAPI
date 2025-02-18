using Dapper;
using Interfaces.DataAccess.Utilities;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Utilities
{
    public class Helper : IHelper
    {
        public Helper()
        {
        }

        public async Task ExecuteQuery(string connection, string query)
        {
            using (var conn = new SqlConnection(connection))
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                await conn.ExecuteAsync(query);
            }
        }

        public async Task<List<T>> ExecuteQueryResult<T>(string connection, string query)
        {
            using (var conn = new SqlConnection(connection))
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                var affectedRows = await conn.QueryAsync<T>(
                    sql: query,
                    commandType: CommandType.Text);

                return (List<T>)affectedRows;
            }
        }

        public async Task<T> ExecuteStoreProcedure<T, U>(string connection, string storeProcedure, DynamicParameters param)
        {
            using (var conn = new SqlConnection(connection))
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                var affectedRows = await conn.QueryAsync<U>(
                    sql: storeProcedure,
                    param: param,
                    commandType: CommandType.StoredProcedure);

                return (T)affectedRows;
            }
        }

        public async Task<T> ExecuteStoreProcedureFirstOrDefault<T>(string connection, string storeProcedure, DynamicParameters param)
        {

            using (var conn = new SqlConnection(connection))
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                var affectedRows = await conn.QueryFirstOrDefaultAsync<T>(
                    sql: storeProcedure,
                    param: param,
                    commandType: CommandType.StoredProcedure);

                return affectedRows;
            }
        }

        public async Task<dynamic> ExecuteStoreProcedureGet(string connection, string storeProcedure, DynamicParameters param)
        {

            using (var conn = new SqlConnection(connection))
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                return await conn.QueryAsync<dynamic>(
                     sql: storeProcedure,
                     param: param,
                     commandType: CommandType.StoredProcedure);

            }
        }
        public async Task<List<T>> ExecuteStoreProcedureGet<T>(string connection, string storeProcedure, DynamicParameters param)
        {
            using (var conn = new SqlConnection(connection))
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                var result = await conn.QueryAsync<T>(
                    sql: storeProcedure,
                    param: param,
                    commandType: CommandType.StoredProcedure);

                return result.ToList();

            }
        }

    }
}
