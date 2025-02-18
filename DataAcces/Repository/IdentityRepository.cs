using Dapper;
using Entities.Context.RunPayDb;
using Entities.General;
using Entities.Identity;
using Interfaces.DataAccess.Repository;
using Interfaces.DataAccess.Utilities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class IdentityRepository : IIdentityRepository
    {
        private readonly IHelper _helper;
        private readonly IConfiguration _configuration;
        private readonly RunPayDbContext _RunPayDbContext;

        public IdentityRepository(IHelper helper, IConfiguration configuration, RunPayDbContext runPayDbContext)
        {
            _helper = helper;
            _configuration = configuration;
            _RunPayDbContext = runPayDbContext;
        }
        public async Task<BaseResponse> GetUserApp(string user)
        {
            string connectionString = _configuration.GetSection("ConnectionStrings:RunPayDbConnection").Value;
            string storedProcedure = "[dbo].[Seguridad.Sp_GetUserAplication]";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@User", user);

            var result = await _helper.ExecuteStoreProcedureFirstOrDefault<ApplicationUser>(connectionString, storedProcedure, parameters);
            var response = new BaseResponse();
            response.CreateSuccess("Ok", result);
            return response;
        }

    }
}
