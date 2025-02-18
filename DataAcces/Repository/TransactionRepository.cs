using Dapper;
using Entities.Context.RunPayDb;
using Entities.General;
using Entities.Identity;
using Interfaces.DataAccess.Repository;
using Interfaces.DataAccess.Utilities;
using MethodsParameters.Input.Transaccion;
using MethodsParameters.Output.Transaccion;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly IHelper _helper;
        private readonly IConfiguration _configuration;
        private readonly RunPayDbContext _RunPayDbContext;

        public TransactionRepository(IHelper helper, IConfiguration configuration, RunPayDbContext runPayDbContext)
        {
            _helper = helper;
            _configuration = configuration;
            _RunPayDbContext = runPayDbContext;
        }
        public async Task<ResponseCreate> Create(RequestTransactionCreate Request)
        {
            ResponseCreate ObjResponse = new ResponseCreate();
            string connectionString = _configuration.GetSection("ConnectionStrings:RunPayDbConnection").Value;
            string storedProcedure = "[dbo].[Transaccion.SpCreateTransaccion]";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@IdMedioPago", Request.IdMedioPago);
            parameters.Add("@Monto", Request.Monto);
            parameters.Add("@IdTax", Request.IdTax);
            parameters.Add("@MontoFinal", Request.MontoFinal);
            parameters.Add("@Referencia", Request.Referencia);
            parameters.Add("@Descripcion", Request.Descripcion);
            parameters.Add("@FechaVencimiento", string.IsNullOrEmpty(Request.FechaVencimiento.ToString())? null : Request.FechaVencimiento);
            parameters.Add("@IdMoneda", Request.IdMoneda);
            parameters.Add("@TipoDocumento", Request.TipoDocumento);
            parameters.Add("@Documento", Request.Documento);
            parameters.Add("@Telefono", Request.Telefono);
            parameters.Add("@Correo", Request.Correo);
            parameters.Add("@Url", Request.Url);
            parameters.Add("@IdUsuario", Request.IdUsuario);
            parameters.Add("@UsuNombre", Request.UsuNombre);


            ObjResponse.IdTransaccion = await _helper.ExecuteStoreProcedureFirstOrDefault<int>(connectionString, storedProcedure, parameters);          
            return ObjResponse;
        }

        public async Task<BaseResponse> UpdateCreate(RequestUpdateCreate Request)
        {
            string connectionString = _configuration.GetSection("ConnectionStrings:RunPayDbConnection").Value;
            string storedProcedure = "[dbo].[Transaccion.Sp_UpdateCreateTransaction]";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@IdTransaccion", Request.IdTransaccion);
            parameters.Add("@MontoFinal", Request.MontoFinal);
            parameters.Add("@Url", Request.Url);
            parameters.Add("@IdUsuario", Request.IdUsuario);
            var result = await _helper.ExecuteStoreProcedureFirstOrDefault<object>(connectionString, storedProcedure, parameters);
            var response = new BaseResponse();
            response.CreateSuccess("Ok", result);

            return response;
        }
        public async Task<BaseResponse> ListTransaction(SpGetListTransactionByUser Request)
        {
            string connectionString = _configuration.GetSection("ConnectionStrings:RunPayDbConnection").Value;
            string storedProcedure = "[dbo].[Transaccion.Sp_GetListTransactionByUser]";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@IdUsuario", Request.IdUsuario);

            var result = await _helper.ExecuteStoreProcedureGet<object>(connectionString, storedProcedure, parameters);
            var response = new BaseResponse();
            response.CreateSuccess("Ok", result);

            return response;
        }
        public async Task<BaseResponse> HistoriTransaction(SpGetHistoriTransaction Request)
        {
            string connectionString = _configuration.GetSection("ConnectionStrings:RunPayDbConnection").Value;
            string storedProcedure = "[dbo].[Transaccion.Sp_GetTrazaTransaction]";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@IdUsuario", Request.IdUsuario);
            parameters.Add("@IdTransaccion", Request.IdTransaccion);


            var result = await _helper.ExecuteStoreProcedureGet<object>(connectionString, storedProcedure, parameters);
            var response = new BaseResponse();
            response.CreateSuccess("Ok", result);

            return response;
        }
        public async Task<BaseResponse> UpdateTransaction(ActualizarEstadoTransaccion Request)
        {
            string connectionString = _configuration.GetSection("ConnectionStrings:RunPayDbConnection").Value;
            string storedProcedure = "[dbo].[Transaccion.Sp_UpdateTransaction]";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@IdTransaccion", Request.IdTransaccion);
            parameters.Add("@IdEstado", Request.idEstadoTransaccio);


            var result = await _helper.ExecuteStoreProcedureGet<object>(connectionString, storedProcedure, parameters);
            var response = new BaseResponse();
            response.CreateSuccess("Ok", result);

            return response;
        }
        public async Task<ResponseSp_GetDataTransaccion> DataTransaction(int IdTransaccion)
        {
            string connectionString = _configuration.GetSection("ConnectionStrings:RunPayDbConnection").Value;
            string storedProcedure = "[dbo].[Transaccion.Sp_GetDataTransaccion]";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@IdTransaccion", IdTransaccion);


            var result = await _helper.ExecuteStoreProcedureFirstOrDefault<ResponseSp_GetDataTransaccion>(connectionString, storedProcedure, parameters);
            return result;
        }


    }
}
