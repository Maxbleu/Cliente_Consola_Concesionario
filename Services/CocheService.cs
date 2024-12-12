using System.Configuration;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using ConsoleApp_Concesionario.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace ConsoleApp_Concesionario.Services
{
    public class CocheService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public CocheService(IConfiguration configuration) 
        {
            _configuration = configuration;
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(_configuration["ApiBaseUrl"])
            };
        }

        /// <summary>
        /// Este método se encarga de cargar el token 
        /// JWT en el header de la petición HTTP
        /// </summary>
        /// <param name="token"></param>
        public void LoadJwtToken(string token)
        {
            this._httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
        /// <summary>
        /// Este método se encarga de hacer una petición al servidor
        /// solicitando todos los coches disponibles en la base de datos
        /// </summary>
        /// <returns></returns>
        public async Task<List<CocheModel>> GETCochesAsync()
        {
            List<CocheModel>? listaCoches = null;
            try
            {
                //  Realizamos la petición GET al servidor
                HttpResponseMessage response = await this._httpClient.GetAsync(this._configuration["ApiVehiculos"]);

                if (response.IsSuccessStatusCode)
                {
                    //  Si la petición fue exitosa, entonces procedemos a deserializar el contenido
                    string jsonContent = await response.Content.ReadAsStringAsync();

                    // Deserializamos el contenido JSON a una lista de objetos CocheModel
                    List<CocheModel> coches = JsonConvert.DeserializeObject<IEnumerable<CocheModel>>(jsonContent).ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Se produjo un error inesperado: {ex.Message}");
            }

            // Retornamos la lista de coches
            return listaCoches;
        }
        /// <summary>
        /// Este método se encarga de hacer una petición al servidor de un coche
        /// en específico por su ID 
        /// </summary>
        /// <param name="carId"></param>
        /// <returns></returns>
        public async Task<CocheModel?> GETCocheByIdAsync(int carId)
        {
            CocheModel? coche = null;
            try
            {
                //  Realizamos la petición GET al servidor
                HttpResponseMessage response = await this._httpClient.GetAsync(this._configuration["ApiVehiculos"] + carId);

                //  Si la petición fue exitosa, entonces procedemos a deserializar el contenido
                if (response.IsSuccessStatusCode)
                {
                    //  Leemos el contenido de la respuesta
                    string jsonContent = await response.Content.ReadAsStringAsync();

                    //  Deserializamos el contenido JSON a un objeto CocheModel
                    coche = JsonConvert.DeserializeObject<CocheModel>(jsonContent);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Se produjo un error inesperado: {ex.Message}");
            }
            return coche;
        }
        /// <summary>
        /// Este método se encarga de hacer una petición al servidor para
        /// crear un nuevo coche en la base de datos 
        /// </summary>
        /// <param name="nuevoCoche"></param>
        /// <returns></returns>
        public async Task<CocheModel> POSTCocheAsync(object nuevoCoche)
        {
            CocheModel coche = null;
            try
            {
                //  Serializamos el objeto nuevoCoche a un string JSON
                string jsonContentSerialize = System.Text.Json.JsonSerializer.Serialize(nuevoCoche);

                //  Creamos un objeto HttpContent con el contenido JSON
                HttpContent httpContent = new StringContent(jsonContentSerialize, Encoding.UTF8, "application/json");

                //  Realizamos la petición POST al servidor
                HttpResponseMessage response = await this._httpClient.PostAsync(this._configuration["ApiVehiculos"], httpContent);

                //  Si la petición fue exitosa, entonces procedemos a deserializar el contenido
                if (response.IsSuccessStatusCode)
                {
                    //  Leemos el contenido de la respuesta
                    string jsonContent = await response.Content.ReadAsStringAsync();

                    //  Deserializamos el contenido JSON a un objeto CocheModel
                    coche = JsonConvert.DeserializeObject<CocheModel>(jsonContent);
                }
            }
            catch (HttpRequestException httpEx)
            {
                Console.WriteLine($"Error en la solicitud HTTP: {httpEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Se produjo un error inesperado: {ex.Message}");
            }
            return coche;
        }
        /// <summary>
        /// Este método se encarga de hacer una petición al servidor para
        /// actualizar un coche en la base de datos a partir del id
        /// del coche y las propiedades del coche que se desean modificar
        /// con sus respectivos valores
        /// </summary>
        /// <param name="carId"></param>
        /// <param name="datosModificados"></param>
        /// <returns></returns>
        public async Task<CocheModel> UPDATECocheAsync(int carId, Dictionary<string, JsonElement> datosModificados)
        {
            CocheModel coche = null;
            try
            {

                string jsonSerializedDatosModificados = System.Text.Json.JsonSerializer.Serialize(datosModificados);
                HttpContent httpContent = new StringContent(jsonSerializedDatosModificados, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await this._httpClient.PutAsync(this._configuration["ApiVehiculos"]+carId, httpContent);

                if(response.IsSuccessStatusCode)
                {
                    string jsonContent = await response.Content.ReadAsStringAsync();
                    coche = JsonConvert.DeserializeObject<CocheModel>(jsonContent);
                }

            }
            catch (HttpRequestException httpEx)
            {

                Console.WriteLine($"Error en la solicitud HTTP: {httpEx.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Se produjo un error inesperado: {ex.Message}");
                return null;
            }
            return coche;
        }
        /// <summary>
        /// Este método se encarga de hacer una petición al servidor para
        /// eliminar un coche en la base de datos a partir del id de este
        /// </summary>
        /// <param name="carId"></param>
        /// <returns></returns>
        public async Task DELETECocheAsync(int carId)
        {
            HttpResponseMessage response = await this._httpClient.DeleteAsync(this._configuration["ApiVehiculos"] + carId);
            if(response.IsSuccessStatusCode) { return; }
        }

    }
}
