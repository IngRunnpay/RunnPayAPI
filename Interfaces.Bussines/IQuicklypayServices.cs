using Entities.General;
using MethodsParameters.Input.Quicklypay;
using MethodsParameters.Input.Transaccion;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Bussines
{
    public interface IQuicklypayServices
    {
        Task<BaseResponse> QuicklypayStarter(RequestQuicklyStarter ObjRequest, string Endpoint);
        Task<BaseResponse> QuicklypayBank(string Endpoint, string token);
        Task<BaseResponse> QuicklypayCreated(RequestCreatedIdTransaccion ObjRequest, string Endpoint);
        Task<BaseResponse> QuicklypayGetDataTransaction(string IdTransaccion);
    }
}
