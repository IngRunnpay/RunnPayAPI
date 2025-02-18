using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.DataAccess.Utilities
{
    public interface IHelper
    {
        /// <summary>
        /// Ejecuta un store procedure por medio de dapper
        /// </summary>
        /// <typeparam name="T">Objeto que se devuelve</typeparam>
        /// <param name="storeProcedure">Nombre del store procedure</param>
        /// <param name="param">Parametros opcionales si se requiere</param>
        /// <returns></returns>
        //Task<T> ExecuteStoreProcedure<T, U>(string storeProcedure, DynamicParameters param);
        Task<T> ExecuteStoreProcedure<T, U>(string connection, string storeProcedure, DynamicParameters param);

        /// <summary>
        /// Ejecuta un store procedure por medio de dapper
        /// </summary>
        /// <typeparam name="T">Objeto que se devuelve</typeparam>
        /// <param name="storeProcedure">Nombre del store procedure</param>
        /// <param name="param">Parametros opcionales si se requiere</param>
        /// <returns></returns>
        //Task<T> ExecuteStoreProcedureFirstOrDefault<T>(string storeProcedure, DynamicParameters param);
        Task<T> ExecuteStoreProcedureFirstOrDefault<T>(string connection, string storeProcedure, DynamicParameters param);

        //Task<dynamic> ExecuteStoreProcedureGet(string storeProcedure, DynamicParameters param);
        Task<dynamic> ExecuteStoreProcedureGet(string connection, string storeProcedure, DynamicParameters param);
        Task<List<T>> ExecuteStoreProcedureGet<T>(string connection, string storeProcedure, DynamicParameters param);
        Task ExecuteQuery(string connection, string query);
        Task<List<T>> ExecuteQueryResult<T>(string connection, string query);
    }
}
