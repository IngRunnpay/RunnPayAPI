using Azure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MethodsParameters.Input.Quicklypay
{
    public class SpLogPasarelaExterna
    {
        public int IdTransaccion { get; set; }
        public string Endpoint { get; set; }
        public string Request { get; set; }
        public string? Response { get; set; }
        public bool Enviada { get; set; }
    }
}
