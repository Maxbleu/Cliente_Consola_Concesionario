using System.Text.Json;
using ConsoleApp_Concesionario.Models;
using ConsoleApp_Concesionario.Services;

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

        /// <summary>
        /// Este método se encarga de cargar todos los
        /// coches disponibles de la base de datos
        /// </summary>
        /// <returns></returns>
        public async Task CargarCoches()
        {
            this.Coches = await ObtenerCochesAsync();
        }
        /// <summary>
        /// Este método se encarga de obtener todos los coches
        /// ya registrados en el programa y si no los tiene
        /// los solicita al backend
        /// </summary>
        /// <returns></returns>
        public async Task<List<CocheModel>?> ObtenerCochesAsync()
        {
            if(this.Coches.Count == 0)
            {
                this.Coches = await this._cocheServices.GETCochesAsync();
            }
            return this.Coches;
        }
        /// <summary>
        /// Este método se encarga de obtener un coche específico
        /// por el id del mismo al servidor
        /// </summary>
        /// <param name="carId"></param>
        /// <returns></returns>
        public async Task<CocheModel?> ObtenerCocheByIdAsync(int carId)
        {
            CocheModel? coche = await this._cocheServices.GETCocheByIdAsync(carId);
            return coche;
        }
        /// <summary>
        /// Este método se encarga de crear un coche nuevo en la
        /// base de datos y cuando recibe el coche creado
        /// lo agrega a la lista de coches del programa
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="country"></param>
        /// <param name="carBrand"></param>
        /// <param name="carModel"></param>
        /// <param name="carColor"></param>
        /// <param name="yearOfManufacture"></param>
        /// <param name="creditCardType"></param>
        /// <returns></returns>
        public async Task<CocheModel> CrearCocheAsync(string firstName, string lastName, string country, string carBrand, string carModel, string carColor, int yearOfManufacture, string creditCardType)
        {
            var nuevoCoche = new { FirstName = firstName, LastName = lastName, Country = country, CarBrand = carBrand, CarModel = carModel, CarColor = carColor, YearOfManufacture = yearOfManufacture, CreditCardType = creditCardType };
            CocheModel cocheCreado = await this._cocheServices.POSTCocheAsync(nuevoCoche);
            this.Coches.Add(cocheCreado);
            return cocheCreado;
        }
        /// <summary>
        /// Este método se encarga de actualizar un coche
        /// a partir del id del coche y los datos modificados
        /// cuando el coche es actualizado en el servidor
        /// obtenemos el coche modificado y lo actualizamos
        /// en la lista de coches del programa
        /// </summary>
        /// <param name="carId"></param>
        /// <param name="datosModificados"></param>
        /// <returns></returns>
        public async Task<CocheModel> ActualizarCocheAsync(int carId, Dictionary<string, JsonElement> datosModificados)
        {
            CocheModel cocheModificado = await this._cocheServices.UPDATECocheAsync(carId, datosModificados);
            this.Coches[carId-1] = cocheModificado;
            return cocheModificado;
        }
        /// <summary>
        /// Este método se encarga de eliminar un coche a partir del
        /// id de este tanto en el servidor como en la lista de coches
        /// del programa
        /// </summary>
        /// <param name="carId"></param>
        /// <returns></returns>
        public async Task EliminarCocheAsync(int carId)
        {
            await this._cocheServices.DELETECocheAsync(carId);
            this.Coches.RemoveAt(carId-1);
        }
    }
}
