using ApiPrincipal.Routes;
using Bussines;
using Entities.General;
using Interfaces.Bussines;
using MethodsParameters.Input.Quicklypay;
using MethodsParameters.Input.Transaccion;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace ApiPrincipal.Controllers
{
    [ApiController]
    [Tags(RoutesPath.QuicklypayController)]
    [Route(RoutesPath.QuicklypayController)]
    public class QuicklypayController : BaseController
    {
        private readonly IQuicklypayServices _QuicklypayServices;
        public QuicklypayController(
             ILogService logService,
             IConfiguration config,
             IQuicklypayServices quicklypayServices) : base(logService, config)
        {
            _QuicklypayServices = quicklypayServices;
        }
        
        [HttpPost(RoutesPath.QuicklypayController_QuicklypayStarter)]
        [TypeFilter(typeof(AllowAnonymousFilter))]
        [ResponseCache(Duration = 15, Location = ResponseCacheLocation.Client)]
        public async Task<ActionResult> QuicklypayStarter([FromBody] RequestQuicklyStarter ObjRequest)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                response = await _QuicklypayServices.QuicklypayStarter(ObjRequest, RoutesPath.QuicklypayController_QuicklypayStarter);
            }
            catch (CustomException ex)
            {
                response.CreateError(ex);
            }
            catch (Exception ex)
            {
                await LogError(ex);
                response.CreateError(ex);
            }
            return Ok(response);
        }

        [HttpGet(RoutesPath.QuicklypayController_QuicklypayBank)]
        [TypeFilter(typeof(AllowAnonymousFilter))]
        [ResponseCache(Duration = 15, Location = ResponseCacheLocation.Client)]
        public async Task<ActionResult> QuicklypayBank([FromQuery] string IdTransaccion)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                if (string.IsNullOrEmpty(IdTransaccion))
                {
                    throw new CustomException("Campo no valido [IdTransaccion]");
                }
                ValidateAccess(RoutesPath.QuicklypayController_QuicklypayBank, new { });
                response = await _QuicklypayServices.QuicklypayBank(RoutesPath.QuicklypayController_QuicklypayBank, IdTransaccion);
            }
            catch (CustomException ex)
            {
                response.CreateError(ex);
            }
            catch (Exception ex)
            {
                await LogError(ex);
                response.CreateError(ex);
            }
            return Ok(response);
        }
        [HttpPost(RoutesPath.QuicklypayController_QuicklypayCreated)]
        [TypeFilter(typeof(AllowAnonymousFilter))]
        [ResponseCache(Duration = 15, Location = ResponseCacheLocation.Client)]
        public async Task<ActionResult> QuicklypayCreated([FromBody] RequestCreatedIdTransaccion ObjRequest)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                ValidateAccess(RoutesPath.QuicklypayController_QuicklypayCreated, new { });
                response = await _QuicklypayServices.QuicklypayCreated(ObjRequest, RoutesPath.QuicklypayController_QuicklypayCreated);
            }
            catch (CustomException ex)
            {
                response.CreateError(ex);
            }
            catch (Exception ex)
            {
                await LogError(ex);
                response.CreateError(ex);
            }
            return Ok(response);
        }

        [HttpGet(RoutesPath.QuicklypayController_QuicklypayGetDataTransaction)]
        [TypeFilter(typeof(AllowAnonymousFilter))]
        [ResponseCache(Duration = 15, Location = ResponseCacheLocation.Client)]
        public async Task<ActionResult> QuicklypayGetDataTransaction([FromQuery] string IdTransaccion)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                ValidateAccess(RoutesPath.QuicklypayController_QuicklypayCreated, new { });
                response = await _QuicklypayServices.QuicklypayGetDataTransaction(IdTransaccion);
            }
            catch (CustomException ex)
            {
                response.CreateError(ex);
            }
            catch (Exception ex)
            {
                await LogError(ex);
                response.CreateError(ex);
            }
            return Ok(response);
        }
    }
}
