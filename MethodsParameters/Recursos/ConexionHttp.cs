using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;

namespace ECD.Utilidades.Recursos
{
    public class ConexionHTTP
    {
        public HttpClient ObtenerClient(string urlbase, bool mediaTypeBson = false)
        {
            string urlServicio = !string.IsNullOrEmpty(urlbase) ? urlbase : string.Empty;

            if (urlServicio != string.Empty)
            {
                string mediaType = !mediaTypeBson ? "application/json" : "application/bson";

                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(urlServicio);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue(mediaType)
                );

                return client;
            }
            else
            {
                throw new Exception("No se encuentra configurada la URL del Servicio Firma de Correo en el archivo App.config en el AppSettings: urlServicioFirmaCorreo");
            }
        }

    }
}
