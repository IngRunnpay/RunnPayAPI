using Entities.General;
using MethodsParameters.Input;
using MethodsParameters.Input.Quicklypay;
using MethodsParameters.Input.Transaccion;
using MethodsParameters.Output.Quicklypay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.DataAccess.Repository
{
    public interface ILogRepository
    {
        Task Logger(LogIn input);
        Task<ResponseSpLogPasarelaExterna> LoggExternalPasarela(SpLogPasarelaExterna Request);
        Task<bool> LogLinKPSEExternalPasarela(SpLogLinkPSEPasarelas Request);
        Task<string> GetLinkPSEExternalPasarela(int IdTransaccion);

    }
}
