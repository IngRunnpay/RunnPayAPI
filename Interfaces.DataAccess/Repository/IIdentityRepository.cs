using Entities.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.DataAccess.Repository
{
    public interface IIdentityRepository
    {
        Task<BaseResponse> GetUserApp(string user);
    }
}
