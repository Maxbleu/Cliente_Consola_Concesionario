using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ConsoleApp_Concesionario.Models;
using Newtonsoft.Json;

namespace ConsoleApp_Concesionario.Services
{
    public class CocheService
    {
        private readonly HttpClient _httpClient;
        
        public CocheService() 
        {
            this._httpClient = new HttpClient
            {
                BaseAddress = new Uri(ConfigurationManager.AppSettings["ApiBaseUrl"])
            };
        }

        public void LoadJwtToken(string token)
        {
            this._httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }

        public async Task<List<CocheModel>> GETCochesAsync()
        {
            HttpResponseMessage response = await this._httpClient.GetAsync(ConfigurationManager.AppSettings["ApiVehiculos"]);
            string jsonContent = await response.Content.ReadAsStringAsync();
            List<CocheModel> coches = JsonConvert.DeserializeObject<IEnumerable<CocheModel>>(jsonContent).ToList();
            return coches;
        }

        public async Task<CocheModel?> GETCocheByIdAsync(int carId)
        {
            CocheModel? coche = null;
            HttpResponseMessage response = await this._httpClient.GetAsync(ConfigurationManager.AppSettings["ApiVehiculos"]+carId);
            if (response.IsSuccessStatusCode)
            {
                string jsonContent = await response.Content.ReadAsStringAsync();
                coche = JsonConvert.DeserializeObject<CocheModel>(jsonContent);
            }
            return coche;
        }

        public async Task<CocheModel> POSTCocheAsync(object nuevoCoche)
        {
            CocheModel coche = null;
            try
            {
                string jsonContentSerialize = JsonConvert.SerializeObject(nuevoCoche);
                HttpContent httpContent = new StringContent(jsonContentSerialize, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await this._httpClient.PostAsync(ConfigurationManager.AppSettings["ApiVehiculos"], httpContent);

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
            catch (JsonSerializationException jsonEx)
            {

                Console.WriteLine($"Error en la serialización/deserialización JSON: {jsonEx.Message}");
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

                string jsonSerializedDatosModificados = JsonConvert.SerializeObject(datosModificados);
                HttpContent httpContent = new StringContent(jsonSerializedDatosModificados);

                HttpResponseMessage response = await this._httpClient.PutAsync(ConfigurationManager.AppSettings["ApiVehiculos"], httpContent);

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
            catch (JsonSerializationException jsonEx)
            {
                Console.WriteLine($"Error en la serialización/deserialización JSON: {jsonEx.Message}");
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
            HttpResponseMessage response = await this._httpClient.DeleteAsync(ConfigurationManager.AppSettings["ApiVehiculos"] + carId);
            if(response.IsSuccessStatusCode) { return; }
        }

    }
}
