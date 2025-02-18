using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MethodsParameters.Output.Transaccion
{
    public class ResponseSp_GetDataTransaccion
    {
        public string Moneda { get; set; }
        public decimal MontoFinal {  get; set; }
        public string Referencia {  get; set; }
        public string DescripcionCompra { get; set; }
        public DateTime? FechaVencimiento {  get; set; }
        public string Documento { get; set; }
        public string UsuDocumento {  get; set; }
        public string UsuTelefono { get; set; }
        public string UsuCorreo { get; set; }
        public string UsuNombre { get; set; }

    }
}
