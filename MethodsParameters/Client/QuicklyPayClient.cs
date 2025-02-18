using ECD.Utilidades.Recursos;
using MethodsParameters.Input.Quicklypay;
using MethodsParameters.Output.Quicklypay;
using Microsoft.IdentityModel.Protocols;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ECD.Utilidades.NuevaApiECD
{
    public static class QuicklyPayClient
    {
        //private static readonly string UrlClient = ConfigurationManager.AppSettings["QuicklyPayClient:Url"];
        //private static readonly string TokenClient = ConfigurationManager.AppSettings["QuicklyPayClient:Token"];

        private static readonly string UrlClient = "https://webhook.quicklypay.com.co/apinsert/";
        private static readonly string TokenClient = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6MjEsImlhdCI6MTczODc3MTEyOH0.1RtadBW700_jXSe6o1zuQ6Xg9hkHUhUrc7uXqVFobkw";

        private static DateTime lastDateToken = DateTime.MinValue;


        public static T GetMapObject<T>(object input)
        {
            try
            {
                var resultData = JsonConvert.SerializeObject(input);
                T data = JsonConvert.DeserializeObject<T>(resultData);
                return data;
            }
            catch
            {
                return default(T);
            }
        }

        public static List<ResponseGetBank> GetBankPSEQuicklyPay()
        {
            List<ResponseGetBank> objresponse = new List<ResponseGetBank>();
            try
            {
                ConsumeServices responseExternal = new ConsumeServices();                
                var restBearer = responseExternal.GetAsync<List<ResponseGetBank>>(UrlClient + "transaccion/bank", new { },TokenClient);
                restBearer.Wait();
                objresponse = restBearer.Result;

                return objresponse;

            }
            catch { }
            return objresponse;
        }
        public static ResponseCreatePseQuicklyPay CreatePSEQuicklyPay(RequestCreatePseQuicklyPay ObjRequest)
        {
            ResponseCreatePseQuicklyPay objresponse = new ResponseCreatePseQuicklyPay();
            try
            {
                ConsumeServices responseExternal = new ConsumeServices();
                var restBearer = responseExternal.RestBearer<ResponseCreatePseQuicklyPay>(UrlClient + "transaccion/payin", ObjRequest, TokenClient);
                restBearer.Wait();
                objresponse = restBearer.Result;

                return objresponse;

            }
            catch { }
            return objresponse;
        }

    }
}
