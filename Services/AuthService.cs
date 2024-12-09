using System.Configuration;
using System.Text;
using System.Text.Json;
using ConsoleApp_Concesionario.Models;
using ConsoleApp_Concesionario.Utils;
using Microsoft.Extensions.Configuration;

namespace ConsoleApp_Concesionario.Services
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;
        private readonly CocheService _cocheService;
        private readonly IConfiguration _configuration;

        private readonly string _urlApiAuth;
        private string JwtToken { get; set; }

        public AuthService(CocheService cocheService, IConfiguration configuration)
        {
            _httpClient = new HttpClient();
            _cocheService = cocheService;
            _configuration = configuration;
            _urlApiAuth = $"{_configuration["ApiBaseUrl"]}{_configuration["ApiAuth"]}";
        }

        public async Task<bool> EstaLogueadoAsync()
        {
            string username, password = "";
            if (!String.IsNullOrEmpty(this.JwtToken))
            {
                return true;
            }

            ConsoleUIUtils.GetNewDataLogin(out username, out password);
            UserModel user = new UserModel
            {
                Username = username,
                Password = password
            };
            bool haIniciadoSesion = await this.LoginAsync(user);
            return haIniciadoSesion;
        }
        public async Task<bool> LoginAsync(UserModel user)
        {

            string objectSerialized = JsonSerializer.Serialize(user);

            HttpContent content = new StringContent(objectSerialized, Encoding.UTF8, "application/json");

            try
            {
                HttpResponseMessage httpResponse = await this._httpClient.PostAsync(this._urlApiAuth, content);

                if (httpResponse.IsSuccessStatusCode)
                {
                    this.JwtToken = await httpResponse.Content.ReadAsStringAsync();
                    this._cocheService.LoadJwtToken(this.JwtToken);
                    return true;
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
