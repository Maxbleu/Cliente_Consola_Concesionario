using System.Configuration;
using System.Text;
using System.Text.Json;
using ConsoleApp_Concesionario.Utils;

namespace ConsoleApp_Concesionario.Services
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;
        private readonly CocheService _cocheService;

        private readonly string _urlApiAuth = String.Concat(ConfigurationManager.AppSettings["ApiBaseUrl"], ConfigurationManager.AppSettings["ApiAuth"]);
        private string JwtToken { get; set; }

        
        public AuthService(CocheService cocheService) 
        {
            this._httpClient = new HttpClient();
            this._cocheService = cocheService;
        }

        public async Task<bool> EstaLogueadoAsync()
        {
            string username, password = "";
            if (!String.IsNullOrEmpty(this.JwtToken))
            {
                return true;
            }

            ConsoleUIUtils.GetNewDataLogin(out username, out password);
            bool haIniciadoSesion = await this.LoginAsync(username, password);
            return haIniciadoSesion;
        }

        public async Task<bool> LoginAsync(string username, string password)
        {
            var user = new
            {
                UserName = username,
                Password = password
            };

            string objectSerialized = JsonSerializer.Serialize(user);

            HttpContent content = new StringContent(objectSerialized, Encoding.UTF8, "application/json");

            try
            {
                HttpResponseMessage httpResponse = await this._httpClient.PostAsync(this._urlApiAuth, content);

                if (httpResponse.IsSuccessStatusCode)
                {
                    Console.WriteLine("\n¡HAS INICIADO SESION!\n");

                    var result = await httpResponse.Content.ReadAsStringAsync();
                    using (JsonDocument doc = JsonDocument.Parse(result))
                    {
                        JsonElement root = doc.RootElement;

                        if (root.TryGetProperty("token", out JsonElement token))
                        {
                            this.JwtToken = token.Deserialize<string>();
                            this._cocheService.LoadJwtToken(this.JwtToken);
                            return true;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("\nError de autenticación: " + httpResponse.StatusCode);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError de conexión: {ex.Message}");
            }

            return false;
        }
    }
}
