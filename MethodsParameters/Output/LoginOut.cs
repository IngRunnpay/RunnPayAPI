using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MethodsParameters.Output
{
    public class LoginOut
    {
        [JsonPropertyName("Tiempo")]
        public DateTime TimeStamp { get; set; }
        [JsonPropertyName("Id")]
        public string TrackingIds { get; set; }
        [JsonPropertyName("Mensaje")]
        public string Mensaje { get; set; }
        [JsonPropertyName("Correo")]
        public string UserEmail { get; set; }
        [JsonPropertyName("Usuario")]
        public string UserName { get; set; }
        [JsonPropertyName("Token")]
        public string Token { get; set; }
    }
}
