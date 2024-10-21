using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
namespace ivanshoes
{
    class api
    {

        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string NombreCompleto => $"{Nombre} {Apellidos}";

        private readonly string _token = "apis-token-10876.yALxWPL9iXoRctMHIOMkvy5IkVprqb1s";
        private readonly string _baseUrl = "https://api.apis.net.pe/v2/reniec/dni";

        public async Task<api> ObtenerDatosClienteAsync(string dni)
        {
            if (string.IsNullOrEmpty(dni))
            {
                throw new ArgumentException("El DNI no puede estar vacío.");
            }

            string url = $"{_baseUrl}?numero={dni}";

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Referer", "https://apis.net.pe/consulta-dni-api");
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_token}");

                HttpResponseMessage response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string responseData = await response.Content.ReadAsStringAsync();
                    JObject persona = JObject.Parse(responseData);

                    // Crear el objeto Cliente con los datos obtenidos
                    api humano = new api
                    {
                        Nombre = persona["nombres"]?.ToString(),
                        Apellidos = persona["apellidoPaterno"]?.ToString() + " " + persona["apellidoMaterno"]?.ToString(),
                    };

                    return humano;
                }
                else
                {
                    throw new Exception($"Error en la solicitud: {response.StatusCode}");
                }
            }
        }
    }
}
