using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MethodsParameters.Input
{
    [DisplayName("Ingreso")]
    public class LoginIn
    {
        [JsonPropertyName("Usuario")]
        [JsonProperty("Usuario")]
        [Required(ErrorMessage = "El campo Usuario es requerido.")]
        public string User { get; set; }

        [JsonPropertyName("Contrasena")]
        [JsonProperty("Contrasena")]
        [Required(ErrorMessage = "El campo Contrasena es requerido.")]
        public string Password { get; set; }

        [JsonPropertyName("Codigo")]
        [JsonProperty("Codigo")]
        [Required(ErrorMessage = "El campo Codigo es requerido.")]
        public string Code { get; set; }
    }
}
