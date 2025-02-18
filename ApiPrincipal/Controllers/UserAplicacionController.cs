using ApiPrincipal.Controllers;
using ApiPrincipal.Routes;
using Interfaces.Bussines;
using MethodsParameters.Input;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace ApiPrincipal.Controllers
{
    [ApiController]
    [Tags(RoutesPath.AplicationValidateController)]
    [Route(RoutesPath.AplicationValidateController)]
    public class UserAplicacionController : BaseController
    {
        private readonly IIdentityService _identityService;

        public UserAplicacionController(
         IIdentityService identityService,
         ILogService logService,
         IConfiguration config
     ) : base(logService, config)
        {
            _identityService = identityService;
        }

        [HttpPost(RoutesPath.AplicationValidateController_LoginPKI)]
        [TypeFilter(typeof(AllowAnonymousFilter))]
        [ResponseCache(Duration = 15, Location = ResponseCacheLocation.Client)]
        //[PreventSpam(LimitRequests = 15, DelayRequest = 60)]
        public async Task<ActionResult> Login([FromBody] LoginIn input)
        {
            return Ok(await Login(_identityService, input));
        }
    }
}
