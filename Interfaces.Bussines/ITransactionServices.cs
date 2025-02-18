using Entities.General;
using MethodsParameters.Input.Transaccion;
using MethodsParameters.Output.Transaccion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Bussines
{
    public interface ITransactionServices
    {
        Task<BaseResponse> Create(RequestTransactionCreate Request);
        Task<BaseResponse> ListTransaction(SpGetListTransactionByUser Request);
        Task<BaseResponse> HistoriTransaction(SpGetHistoriTransaction Request);

    }
}
