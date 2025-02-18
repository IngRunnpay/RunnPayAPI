using Interfaces.Bussines;
using Interfaces.DataAccess.Repository;
using MethodsParameters.Input;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bussines
{
    public class LogService : ILogService
    {
        private readonly ILogRepository _logRepository;
        private readonly IConfiguration _configuration;

        public LogService(ILogRepository logRepository, IConfiguration configuration)
        {
            _logRepository = logRepository;
            _configuration = configuration;
        }

        public async Task Logger(LogIn input)
        {
            await _logRepository.Logger(input);
        }
    }
}
