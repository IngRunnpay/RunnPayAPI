using Entities.General;
using Interfaces.Bussines;
using Interfaces.DataAccess.Repository;
using MethodsParameters.Input.Transaccion;
using MethodsParameters.Input;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MethodsParameters.Input.Quicklypay;
using System.Text.Json;
using static MethodsParameters.Utilities;
using System.Data;
using MethodsParameters.Output.Quicklypay;
using Entities.Enums;
using System.Net;
using ECD.Utilidades.NuevaApiECD;
using MethodsParameters;
using Newtonsoft.Json.Linq;
using MethodsParameters.Output.Transaccion;
using System.Web;

namespace Bussines
{
    public class QuicklypayServices :IQuicklypayServices
    {
        private readonly IConfiguration _configuration;
        private readonly ILogRepository _logRepository;
        private readonly ITransactionRepository _TransactionRepository;

        public QuicklypayServices(IConfiguration configuration, ILogRepository logRepository, ITransactionRepository transactionRepository)
        {
            _configuration = configuration;
            _logRepository = logRepository;
            _TransactionRepository = transactionRepository;
        }

        public async Task<BaseResponse> QuicklypayStarter(RequestQuicklyStarter ObjRequest, string Endpoint)
        {
            BaseResponse response = new BaseResponse();
            ResponseSpLogPasarelaExterna ResponseSpLog = new ResponseSpLogPasarelaExterna();
            try
            {
                SpLogPasarelaExterna ObjSpRequest = new SpLogPasarelaExterna();

                ObjSpRequest.IdTransaccion = OperacionPasarelasExternas.GetIdTransaccionByReferencia(ObjRequest.referencia);
                ObjSpRequest.Endpoint = Endpoint;
                ObjSpRequest.Request = null;
                ObjSpRequest.Response = JsonSerializer.Serialize(ObjRequest);
                ObjSpRequest.Enviada = false;

                ResponseSpLog = await _logRepository.LoggExternalPasarela(ObjSpRequest);

                if (ResponseSpLog.IdTransaccion > 0)
                {
                    int EstadoAnterior = ResponseSpLog.IdEstadoTransaccion;
                    if (EstadoAnterior == (int)enumEstadoTransaccion.Pendiente)
                    {
                        ActualizarEstadoTransaccion ActualizarEstado = new ActualizarEstadoTransaccion();
                        ActualizarEstado.IdTransaccion = ResponseSpLog.IdTransaccion;
                        switch (ObjRequest.estado)
                        {
                            case "PAGADO":
                                ActualizarEstado.idEstadoTransaccio = (int)enumEstadoTransaccion.Aprobado;
                                break;
                            case "RECHAZADO":
                                ActualizarEstado.idEstadoTransaccio = (int)enumEstadoTransaccion.Rechazado;
                                break;
                        }
                        await _TransactionRepository.UpdateTransaction(ActualizarEstado);
                        response.CreateSuccess("OK", new { });
                    }
                    else
                    {
                        response.CreateError("La transacción ya fue rechazada o aprobada.");
                    }
                }
            }
            catch (CustomException ex)
            {
                response.CreateError(ex);
            }
            catch (Exception ex)
            {
                await _logRepository.Logger(new LogIn(ex));
                response.CreateError(ex);
            }
            return response;
        }
        public async Task<BaseResponse> QuicklypayBank(string Endpoint, string token)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                var resp = QuicklyPayClient.GetBankPSEQuicklyPay();

                SpLogPasarelaExterna ObjSpRequest = new SpLogPasarelaExterna();
                string IdTransaccion = OperacionEncriptacion.DecryptAES(HttpUtility.UrlDecode(token), Utilities.OperacionEncriptacion.Keys.UserAgreementKEY, Utilities.OperacionEncriptacion.Keys.UserAgreementIV);
                ObjSpRequest.IdTransaccion = Convert.ToInt32(IdTransaccion);
                ObjSpRequest.Endpoint = Endpoint;
                ObjSpRequest.Request = JsonSerializer.Serialize(new
                {
                    IdTransaccion = IdTransaccion,
                    Endpoint = Endpoint,
                });
                ObjSpRequest.Response = JsonSerializer.Serialize(resp);
                ObjSpRequest.Enviada = true;
                await _logRepository.LoggExternalPasarela(ObjSpRequest);

                response.CreateSuccess("OK", resp);
            }
            catch (CustomException ex)
            {
                response.CreateError(ex);
            }
            catch (Exception ex)
            {
                await _logRepository.Logger(new LogIn(ex));
                response.CreateError(ex);
            }
            return response;
        }
        public async Task<BaseResponse> QuicklypayCreated(RequestCreatedIdTransaccion ObjRequest, string Endpoint)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                string IdTransaccion = OperacionEncriptacion.DecryptAES(HttpUtility.UrlDecode(ObjRequest.IdTransaccion), Utilities.OperacionEncriptacion.Keys.UserAgreementKEY, Utilities.OperacionEncriptacion.Keys.UserAgreementIV);

                RequestCreatePseQuicklyPay ObjRquestQuickly = new RequestCreatePseQuicklyPay();
                ResponseSp_GetDataTransaccion ResponseQuickly = await _TransactionRepository.DataTransaction(Convert.ToInt32(IdTransaccion));
                string BaseRuta = _configuration.GetSection("RuteResume").Value;
                ObjRquestQuickly.referencia = $"RunnPay|{IdTransaccion}|{ResponseQuickly.Referencia}|{ResponseQuickly.DescripcionCompra}";
                ObjRquestQuickly.vencimiento = (ResponseQuickly.FechaVencimiento == null) ? OperacionPasarelasExternas.FormatearFecha(DateTime.Now.AddYears(1).ToString()) : OperacionPasarelasExternas.FormatearFecha(ResponseQuickly.FechaVencimiento.ToString());
                ObjRquestQuickly.moneda = ResponseQuickly.Moneda;
                ObjRquestQuickly.valor = ResponseQuickly.MontoFinal.ToString("0");
                ObjRquestQuickly.usuTipdoc = ResponseQuickly.Documento;
                ObjRquestQuickly.usuDoc = ResponseQuickly.UsuDocumento;
                ObjRquestQuickly.usuTelefono = ResponseQuickly.UsuTelefono;
                ObjRquestQuickly.usuNombre = ResponseQuickly.UsuNombre;
                ObjRquestQuickly.usuCorreo = ResponseQuickly.UsuCorreo;
                ObjRquestQuickly.tipo = "PAYIN";
                ObjRquestQuickly.metodo = "AMBOS";
                ObjRquestQuickly.link = $"{BaseRuta}{ObjRequest.IdTransaccion}";
                ObjRquestQuickly.banco = ObjRequest.Banco;
                var resp = QuicklyPayClient.CreatePSEQuicklyPay(ObjRquestQuickly);

                SpLogPasarelaExterna ObjSpRequest = new SpLogPasarelaExterna();
                ObjSpRequest.IdTransaccion = Convert.ToInt32(IdTransaccion);
                ObjSpRequest.Endpoint = Endpoint;
                ObjSpRequest.Request = JsonSerializer.Serialize(ObjRequest);
                ObjSpRequest.Response = JsonSerializer.Serialize(resp);
                ObjSpRequest.Enviada = true;
                await _logRepository.LoggExternalPasarela(ObjSpRequest);

                if (resp.code != null)
                {
                    if (resp.code == 200)
                    {
                        response.CreateSuccess("OK", resp.data);
                        _logRepository.LogLinKPSEExternalPasarela(new SpLogLinkPSEPasarelas
                        {
                            IdTransaccion = ObjSpRequest.IdTransaccion,
                            Url = resp.data.checkout
                        });
                    }
                }
                else
                {
                    if (resp.statusCode != null)
                    {
                        if (resp.statusCode == 409)
                        {
                            response.Success = true;
                            response.Message = "Referencia repetida.";
                            response.Code = 409;
                            response.Data = await _logRepository.GetLinkPSEExternalPasarela(ObjSpRequest.IdTransaccion);
                        }
                    }
                    else
                    {
                        response.CreateError((resp.message != null) ? resp.message : null);

                    }
                }
            }
            catch (CustomException ex)
            {
                response.CreateError(ex);
            }
            catch (Exception ex)
            {
                await _logRepository.Logger(new LogIn(ex));
                response.CreateError(ex);
            }
            return response;
        }

        public async Task<BaseResponse> QuicklypayGetDataTransaction(string IdTransaccion)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                string Token = OperacionEncriptacion.DecryptAES(HttpUtility.UrlDecode(IdTransaccion), Utilities.OperacionEncriptacion.Keys.UserAgreementKEY, Utilities.OperacionEncriptacion.Keys.UserAgreementIV);

                ResponseSp_GetDataTransaccion ResponseQuickly = await _TransactionRepository.DataTransaction(Convert.ToInt32(Token));
                response.CreateSuccess("OK", ResponseQuickly);
            }
            catch (CustomException ex)
            {
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
