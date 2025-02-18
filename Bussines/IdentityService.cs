using Entities.Enums;
using Entities.General;
using Entities.Identity;
using Interfaces.Bussines;
using Interfaces.DataAccess.Repository;
using MethodsParameters;
using MethodsParameters.Input;
using MethodsParameters.Output;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bussines
{
    public class IdentityService : IIdentityService
    {
        private readonly IIdentityRepository _identityRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly ILogRepository _logRepository;

        public IdentityService(IConfiguration configuration, IIdentityRepository identityRepository, UserManager<ApplicationUser> userManager, ILogRepository logRepository)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _identityRepository = identityRepository;
            _configuration = configuration;
            _logRepository = logRepository;
        }

        public async Task<BaseResponse> Login(LoginIn input)
        {
            BaseResponse response = new BaseResponse();
            try
            {

                if (string.IsNullOrWhiteSpace(input.Code))
                    throw new CustomException("Codigo invalido.");

                response = await _identityRepository.GetUserApp(input.User);
                if (!response.Success)
                    throw new CustomException("El usuario no existe.");

                if (response.Data == null)
                    throw new CustomException("El usuario no existe.");

                ApplicationUser user = (ApplicationUser)response.Data;

                if (input.Code.ToUpper() != user.Id.ToUpper())
                    throw new CustomException("Codigo invalido.");

                var userHasValidPass = await _userManager.CheckPasswordAsync(user, input.Password);

                if (!userHasValidPass)
                    throw new CustomException("La contraseña es invalida.");
                //int IdApp = 0;
                //switch (Convert.ToInt32(_configuration.GetSection("IdAplicacion").Value))
                //{
                //    case 1:
                //        IdApp = (int)enumAccess.ApiPublica;
                //        break;
                //    case 2:
                //        IdApp = (int)enumAccess.ApiPrincipal;
                //        break;
                //    default:
                //        IdApp = (int)enumAccess.NA;
                //        break;

                //}

                response.CreateSuccess("Ok", new LoginOut()
                {
                    UserEmail = user.NormalizedEmail,
                    UserName = user.NormalizedUserName,
                    Token = await TokenConfig.CreateTokenAsync(user, _configuration, (enumAccess)Convert.ToInt32(_configuration.GetSection("IdAplicacion").Value)),
                    Mensaje = "Exitoso",
                    TimeStamp = DateTime.UtcNow,
                    TrackingIds = user.Id
                }
                );

            }
            catch (CustomException ex)
            {
                await _logRepository.Logger(new LogIn(ex));
                response.CreateError(ex);
            }
            catch (Exception ex)
            {
                await _logRepository.Logger(new LogIn(ex));
                response.CreateError(ex);
            }
            return response;
        }

    }
}
