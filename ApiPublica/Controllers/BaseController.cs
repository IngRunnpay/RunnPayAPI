using ApiPublica.Routes;
using Entities.Enums;
using Entities.General;
using Interfaces.Bussines;
using MethodsParameters;
using MethodsParameters.Input;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using static ApiPublica.Extensions.ServiceExtension;

namespace ApiPublica.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [EnableCors("AllowOrigin")]
    [TypeFilter(typeof(CustomAuthorizationFilter))]
    public class BaseController : ControllerBase
    {
        protected readonly ILogService _logService;
        protected readonly IConfiguration _config;

        public BaseController(ILogService logService, IConfiguration config)
        {
            _logService = logService;
            _config = config;
        }

        protected void LogInput(object input)
        {
            try
            {
                _logService.Logger(
                        new MethodsParameters.Input.LogIn(
                            "Registro interno json petición.",
                            HttpContext.Request.GetUri().ToString(),
                            JsonConvert.SerializeObject(input ?? new { }),
                            "APIPUBLICO - LOG"
                        )
                        {
                            IdUsuarioAplicacion = IdUsuario
                        }
                     );
            }
            catch { }
        }

        protected async Task LogError(Exception ex)
        {
            try
            {
                await _logService.Logger(
                    new MethodsParameters.Input.LogIn(ex)
                    {
                        IdUsuarioAplicacion = IdUsuario
                    }
                );
            }
            catch { }
        }

        protected string IdUsuarioInternal { get; set; }

        protected string IdUsuario
        {
            get
            {
                try
                {
                    var token = Request.Headers["Authorization"].ToString().Replace("Bearer", "").Trim();
                    var handler = new JwtSecurityTokenHandler();
                    var jwtSecurityToken = handler.ReadJwtToken(token);
                    var id = jwtSecurityToken.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value;
                    return id.ToString();
                }
                catch (Exception ex)
                {
                    _logService.Logger(new MethodsParameters.Input.LogIn(ex));
                    throw new CustomException("Autenticacion no valida.");
                }
            }
        }
        public string IdUsuarioPersona
        {
            get
            {
                try
                {
                    var token = Request.Headers["Authorization"].ToString().Replace("Bearer", "").Trim();
                    var handler = new JwtSecurityTokenHandler();
                    var jwtSecurityToken = handler.ReadJwtToken(token);
                    var id = jwtSecurityToken.Claims.First(claim => claim.Type == ClaimTypes.Sid).Value;
                    return id.ToString();
                }
                catch (Exception ex)
                {
                    _logService.Logger(new MethodsParameters.Input.LogIn(ex));
                    throw new CustomException("Autenticacion no valida.");
                }
            }
        }

        protected string Usuario
        {
            get
            {
                try
                {
                    var token = Request.Headers["Authorization"].ToString().Replace("Bearer", "").Trim();
                    var handler = new JwtSecurityTokenHandler();
                    var jwtSecurityToken = handler.ReadJwtToken(token);
                    var name = jwtSecurityToken.Claims.First(claim => claim.Type == ClaimTypes.Name).Value;
                    return name.ToString();
                }
                catch (Exception ex)
                {
                    _logService.Logger(new MethodsParameters.Input.LogIn(ex));
                    throw new CustomException("Autenticacion no valida.");
                }
            }
        }
        protected enumAccess Access
        {
            get
            {
                try
                {
                    var token = Request.Headers["Authorization"].ToString().Replace("Bearer", "").Trim();
                    var handler = new JwtSecurityTokenHandler();
                    var jwtSecurityToken = handler.ReadJwtToken(token);
                    var accessValue = jwtSecurityToken.Claims.First(claim => claim.Type == ClaimTypes.Role).Value;
                    Enum.TryParse(accessValue, out enumAccess myAcess);
                    return myAcess;
                }
                catch (Exception ex)
                {
                    _logService.Logger(new MethodsParameters.Input.LogIn(ex));
                    throw new CustomException("Autenticacion no valida.");
                }
            }
        }

        protected string IpAddress
        {
            get
            {
                try
                {
                    var ipAddress = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();

                    if (string.IsNullOrEmpty(ipAddress))
                    {
                        ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
                    }

                    return string.IsNullOrEmpty(ipAddress) ? "127.0.0.0" : ipAddress;
                }
                catch (Exception ex)
                {
                    _logService.Logger(new MethodsParameters.Input.LogIn(ex));
                    throw new CustomException("Autenticacion no valida.");
                }
            }
        }


        protected void ValidateAccess(string Method, object input)
        {
            LogInput(input);
            bool withAccess = true;

            if (
                !String.IsNullOrEmpty(Method)
            )
            {
                switch (Access)
                {
                    case enumAccess.ApiPublica:
                        if (
                            Method != RoutesPath.AplicationValidateController

                        )
                        {
                            try
                            {
                                string id = IdUsuario;
                            }
                            catch
                            {
                                withAccess = false;
                            }
                        }
                        break;
                    default:
                           withAccess = false;

                        break;
                }
            }

            if (!withAccess)
            {
                throw new CustomException("No tiene acceso.");
            }
        }

        protected DateTime? ValidateStartDate(string startDate)
        {
            DateTime? returnDate = null;
            if (!string.IsNullOrEmpty(startDate))
            {
                try
                {
                    List<string> spliDate = startDate.Split('-').ToList();
                    returnDate = new DateTime(int.Parse(spliDate[0]), int.Parse(spliDate[1]), int.Parse(spliDate[2]));
                    if (returnDate < DateTime.Today.AddDays(3) || returnDate > DateTime.Today.AddMonths(1))
                    {
                        throw new CustomException("La fecha inicio de vigencia no puede ser inferior a 3 días adicionales a la fecha actual, ni superior a un mes adicional a la fecha actual.");
                    }
                }
                catch (CustomException ex)
                {
                    throw ex;
                }
                catch
                {
                    throw new Exception("La fecha no tiene un formato valido yyyy-MM-dd o la fecha ingresada no es valida.");
                }
            }
            return returnDate;
        }

        protected void ValidateListRange(int Pagina, int Total)
        {
            if (Total < 1 || Total > 100)
            {
                throw new CustomException("Campo no valido [Total] debe tener un rango de 1 a 100");
            }

            if (Pagina <= 0)
            {
                throw new CustomException("Campo no valido [Pagina] debe tener un valor superior a 0");
            }
        }
        protected async Task<BaseResponse> Login(IIdentityService _identityService, LoginIn input)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                LogInput(input);
                if (input == null) throw new CustomException(Utilities.Invalid);

                input.Code = string.IsNullOrEmpty(input.Code) ? "STRING" : input.Code;
                if (input.Code.ToUpper() == "STRING")
                {
                    throw new CustomException("Campo no valido [Codigo]");
                }

                input.Password = string.IsNullOrEmpty(input.Password) ? "STRING" : input.Password;
                if (input.Password.ToUpper() == "STRING")
                {
                    throw new CustomException("Campo no valido [Contrasena]");
                }

                input.User = string.IsNullOrEmpty(input.User) ? "STRING" : input.User;
                if (input.User.ToUpper() == "STRING")
                {
                    throw new CustomException("Campo no valido [Usuario]");
                }
                response = await _identityService.Login(input);

                if (response.Success)
                {
                    IdUsuarioInternal = input.Code;
                }

            }
            catch (CustomException ex)
            {
                await LogError(ex);
                response.CreateError(ex);
            }
            catch (Exception ex)
            {
                await LogError(ex);
                response.CreateError(ex);
            }
            return response;
        }

    }

}
