using Entities.General;
using MethodsParameters.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Bussines
{
    public interface IIdentityService
    {
        Task<BaseResponse> Login(LoginIn input);
    }
}
