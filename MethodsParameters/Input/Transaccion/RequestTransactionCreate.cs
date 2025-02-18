using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MethodsParameters.Input.Transaccion
{
    public class RequestTransactionCreate
    {
        public int IdMedioPago { get; set; }
        public decimal Monto { get; set; }
        public int IdTax { get; set; }        
        public string Referencia { get; set; }
        public string Descripcion { get; set; }
        public DateTime? FechaVencimiento { get; set; }
        public int IdMoneda { get; set; }
        public int TipoDocumento { get; set; }
        public string Documento { get; set; }
        public string Telefono { get; set; }
        public string Correo { get; set; }
        public string UsuNombre { get; set; }
        [JsonIgnore]
        public string? Url { get; set; }
        [JsonIgnore]
        public int? IdUsuario { get; set; }
        [JsonIgnore]
        public decimal? MontoFinal { get; set; }

    }

    public class RequestCreatedIdTransaccion
    {
        public string IdTransaccion { get; set; }
        public string Banco { get; set; }
    }
}
