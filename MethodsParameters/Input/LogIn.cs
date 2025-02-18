using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MethodsParameters.Input
{
    public class LogIn
    {
        public LogIn(string message1, string message2, string message3, string message4 = "ApiPublico")
        {
            Error = message1;
            if (Error.Length > 1999)
            {
                Error = Error.Substring(0, 1999);
            }
            Detail = message2;
            if (Detail.Length > 1999)
            {
                Detail = Detail.Substring(0, 1999);
            }
            Texto = message3;
                        
            Fuente = message4;
        }

        public LogIn(Exception ex)
        {
            Error = ex.ToString();
            if (Error.Length > 1999)
            {
                Error = Error.Substring(0, 1999);
            }
            Detail = ex.Message.ToString();
            if (Detail.Length > 1999)
            {
                Detail = Detail.Substring(0, 1999);
            }
            Texto = "Error";
            Fuente = "API-PUBLICO";
        }

        public LogIn(int data1, string data2, string data3, string data4, string data5, string data6 = "ApiPublico")
        {
            IdTransaccion = data1;
            IdEstadoAnterior = data2;
            IdEstadoNuevo = data3;
            IdUsuarioAplicacion = data5;
            Fuente = data6;
        }

        public string Texto { get; }
        public string Error { get; }
        public string Detail { get; }
        public string Fuente { get; }

        public int IdTransaccion { get; }
        public string IdEstadoAnterior { get; }
        public string IdEstadoNuevo { get; }
        public string IdUsuarioAplicacion { get; set; }
    }
}
