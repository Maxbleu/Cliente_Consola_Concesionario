using System;
using System.Text.Json;
using ConsoleApp_Concesionario.Models;
using ConsoleApp_Concesionario.Services;
using Newtonsoft.Json;

namespace ConsoleApp_Concesionario.Managers
{
    public class CochesManager
    {
        private readonly CocheService _cocheServices;
        public List<CocheModel> Coches;
        public CochesManager(CocheService cocheServices) 
        {
            this._cocheServices = cocheServices;
            this.Coches = new List<CocheModel>();
        }

        public async Task CargarCoches()
        {
            this.Coches = await ObtenerCochesAsync();
        }
        public async Task<List<CocheModel>?> ObtenerCochesAsync()
        {
            if(this.Coches.Count == 0)
            {
                this.Coches = await this._cocheServices.GETCochesAsync();
            }
            return this.Coches;
        }
        public async Task<CocheModel?> ObtenerCocheByIdAsync(int carId)
        {
            CocheModel? coche = await this._cocheServices.GETCocheByIdAsync(carId);
            return coche;
        }
        public async Task<CocheModel> CrearCocheAsync(string firstName, string lastName, string country, string carBrand, string carModel, string carColor, int yearOfManufacture, string creditCardType)
        {
            var nuevoCoche = new { FirstName = firstName, LastName = lastName, Country = country, CarBrand = carBrand, CarModel = carModel, CarColor = carColor, YearOfManufacture = yearOfManufacture, CreditCardType = creditCardType };
            CocheModel cocheCreado = await this._cocheServices.POSTCocheAsync(nuevoCoche);
            this.Coches.Add(cocheCreado);
            return cocheCreado;
        }
        public async Task<CocheModel> ActualizarCocheAsync(int carId, Dictionary<string, JsonElement> datosModificados)
        {
            CocheModel cocheModificado = await this._cocheServices.UPDATECocheAsync(carId, datosModificados);
            this.Coches[carId-1] = cocheModificado;
            return cocheModificado;
        }
        public async Task EliminarCocheAsync(int carId)
        {
            await this._cocheServices.DELETECocheAsync(carId);
            this.Coches.RemoveAt(carId-1);
        }
    }
}
