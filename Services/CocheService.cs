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

        public void LoadJwtToken(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
        public async Task<List<CocheModel>> GETCochesAsync()
        {
            HttpResponseMessage response = await this._httpClient.GetAsync(this._configuration["ApiVehiculos"]);
            string jsonContent = await response.Content.ReadAsStringAsync();
            List<CocheModel> coches =  JsonConvert.DeserializeObject<IEnumerable<CocheModel>>(jsonContent).ToList();
            return coches;
        }
        public async Task<CocheModel?> GETCocheByIdAsync(int carId)
        {
            CocheModel? coche = null;
            try
            {
                this._httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await this._httpClient.GetAsync(this._configuration["ApiVehiculos"] + carId);
                if (response.IsSuccessStatusCode)
                {
                    string jsonContent = await response.Content.ReadAsStringAsync();
                    coche = JsonConvert.DeserializeObject<CocheModel>(jsonContent);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Se produjo un error inesperado: {ex.Message}");
            }
            return coche;
        }
        public async Task<CocheModel> POSTCocheAsync(object nuevoCoche)
        {
            CocheModel coche = null;
            try
            {
                string jsonContentSerialize = System.Text.Json.JsonSerializer.Serialize(nuevoCoche);
                HttpContent httpContent = new StringContent(jsonContentSerialize, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await this._httpClient.PostAsync(this._configuration["ApiVehiculos"], httpContent);

                if (response.IsSuccessStatusCode)
                {
                    string jsonContent = await response.Content.ReadAsStringAsync();
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
        public async Task DELETECocheAsync(int carId)
        {
            HttpResponseMessage response = await this._httpClient.DeleteAsync(this._configuration["ApiVehiculos"] + carId);
            if(response.IsSuccessStatusCode) { return; }
        }

    }
}
