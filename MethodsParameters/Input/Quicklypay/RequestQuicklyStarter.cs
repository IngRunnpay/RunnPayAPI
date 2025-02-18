using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MethodsParameters.Input.Quicklypay
{
    public class RequestQuicklyStarter
    {
        public string? referencia {  get; set; }        
        public string? estado { get; set; }        
        public string? error { get; set; }
    }
}
