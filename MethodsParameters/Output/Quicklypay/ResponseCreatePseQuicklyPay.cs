using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MethodsParameters.Output.Quicklypay
{
    public class Data
    {
        public string checkout { get; set; }
    }

    public class ResponseCreatePseQuicklyPay
    {
        public bool? success { get; set; }
        public int? code { get; set; }
        public Data? data { get; set; }
        public int? statusCode { get; set; }
        public string? message { get; set; }

    }
}
