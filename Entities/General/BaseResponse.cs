using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Entities.General
{
    public class BaseResponse
    {
        [JsonPropertyName("Mensaje")]
        public string Message { get; set; } = "OK";
        [JsonPropertyName("Respuesta")]
        public bool Success { get; set; } = true;
        [JsonPropertyName("Codigo")]
        public int Code { get; set; } = 200;
        [JsonPropertyName("Data")]
        public object? Data { get; set; }

        public BaseResponse(string message)
        {
            Code = 500;
            Message = message;
            Data = null;
            Success = false;
        }

        public BaseResponse()
        {
            Code = 500;
            Message = "";
            Data = null;
            Success = false;
        }

        public void CreateSuccess(string message, object data)
        {
            Code = 200;
            Data = data;
            Success = true;
            Message = message;
        }
        public void CreateError(CustomException ex)
        {
            Code = 404;
            Data = null;
            Success = false;
            Message = ex.Message;
        }
        public void CreateError(string message, int code = 404)
        {
            Code = code;
            Data = null;
            Success = false;
            Message = message;
        }
        public void CreateError(Exception ex)
        {
            Code = 500;
            Data = null;
            Success = false;
            Message = ex.ToString();
        }
    }

    public class BaseResponseContent
    {
        [JsonPropertyName("Respuesta")]
        public bool Status { get; set; } = false;
        [JsonPropertyName("Mensaje")]
        public string Message { get; set; } = "";
        [JsonPropertyName("Data")]
        public object? Data { get; set; }
        [JsonPropertyName("Codigo")]
        public string StatusCode { get; set; } = "";
    }
}
