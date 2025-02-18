using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MethodsParameters.Input.Quicklypay
{
    public class RequestCreatePseQuicklyPay
    {
        public string referencia { get; set; }
        public string vencimiento { get; set; }
        public string moneda { get; set; }
        public string valor { get; set; }
        public string usuDoc { get; set; }
        public string usuTipdoc { get; set; }
        public string usuNombre { get; set; }
        public string usuTelefono { get; set; }
        public string usuCorreo { get; set; }
        public string tipo { get; set; }
        public string metodo { get; set; }
        public string link { get; set; }
        public string banco { get; set; }
    }

}
