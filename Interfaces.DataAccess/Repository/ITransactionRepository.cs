using Entities.General;
using MethodsParameters.Input.Transaccion;
using MethodsParameters.Output.Transaccion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.DataAccess.Repository
{
    public interface ITransactionRepository
    {
        Task<ResponseCreate> Create(RequestTransactionCreate Request);
        Task<BaseResponse> UpdateCreate(RequestUpdateCreate Request);
        Task<BaseResponse> ListTransaction(SpGetListTransactionByUser Request);
        Task<BaseResponse> HistoriTransaction(SpGetHistoriTransaction Request);
        Task<BaseResponse> UpdateTransaction(ActualizarEstadoTransaccion Request);
        Task<ResponseSp_GetDataTransaccion> DataTransaction(int IdTransaccion);

    }
}
