using System.Text.Json;
using ConsoleApp_Concesionario.Managers;
using ConsoleApp_Concesionario.Models;
using ConsoleApp_Concesionario.Services;
using ConsoleApp_Concesionario.Utils;
using Microsoft.Extensions.Configuration;
internal class Program
{

    private static CocheService _cocheServices;
    private static AuthService _authService;
    private static CochesManager _cochesManager;

    private static async Task ObtenerCochesAsync()
    {
        List<CocheModel>? coches = await _cochesManager.ObtenerCochesAsync();
        ConsoleUIUtils.ShowCoches(coches);
    }
    private static async Task ObtenerCochePorIdAsync()
    {
        int car_id;
        ConsoleUIUtils.GetDataNumber(out car_id, "\nIntroduzca el id de un coche");
        CocheModel? coche = await _cochesManager.ObtenerCocheByIdAsync(car_id);
        Console.WriteLine(coche != null ? coche.ToString()+"\n" : "Registro no encontrado\n");
    }
    private static async Task CrearNuevoCocheAsync() 
    {
        if (!await _authService.EstaLogueadoAsync()) return;
        string firstName, lastName, country, carBrand, carModel, carColor, creditCardType;
        int year;
        ConsoleUIUtils.GetNewDataCoche(out firstName, out lastName, out country, out carBrand, out carModel, out carColor, out year, out creditCardType);

        CocheModel coche = await _cochesManager.CrearCocheAsync(firstName, lastName, country, carBrand, carModel, carColor, year, creditCardType);
        Console.WriteLine("\n"+coche.ToString()+"\n");
    }
    private static async Task ActualizarCocheAsync() 
    {
        if (!await _authService.EstaLogueadoAsync()) return;

        int car_id;
        Dictionary<string, JsonElement> datosModificados;
        
        ConsoleUIUtils.GetDataNumber(out car_id, "Introduzca el id de un coche");
        ConsoleUIUtils.GetDataModificadaCoche(out datosModificados);
        
        CocheModel cocheModificado = await _cochesManager.ActualizarCocheAsync(car_id, datosModificados);
        Console.WriteLine(cocheModificado.ToString());
    }
    private static async Task EliminarCocheAsync() 
    {
        if (!await _authService.EstaLogueadoAsync()) return;
        int car_id;
        ConsoleUIUtils.GetDataNumber(out car_id, "Introduzca el id de un coche");
        await _cochesManager.EliminarCocheAsync(car_id);
        Console.WriteLine($"\nEl coche {car_id} ha sido eliminado exitosamente\n");
    }
    private static void Salir()
    {
        Console.WriteLine("\nHas salido del programa");
    }
    private static async Task RunAsync()
    {
        Task tareaSeleccionada = null;
        ConsoleKey input;

        do
        {
            if (tareaSeleccionada != null) await tareaSeleccionada;
            input = ConsoleUIUtils.ShowMenu();

            switch (input)
            {
                case ConsoleKey.A:
                    tareaSeleccionada = ObtenerCochesAsync();
                    break;

                case ConsoleKey.B:
                    tareaSeleccionada = ObtenerCochePorIdAsync();
                    break;

                case ConsoleKey.C:
                    tareaSeleccionada = CrearNuevoCocheAsync();
                    break;

                case ConsoleKey.D:
                    tareaSeleccionada = ActualizarCocheAsync();
                    break;

                case ConsoleKey.E:
                    tareaSeleccionada = EliminarCocheAsync();
                    break;

                case ConsoleKey.F:
                    tareaSeleccionada = Task.Run(() => Salir());
                    break;

                default:
                    Console.WriteLine("\nEsta opción no está disponible\n");
                    break;
            }

        } while (input != ConsoleKey.F);
    }

    private async static Task Main(string[] args)
    {

        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("Config//appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        _cocheServices = new CocheService(configuration);
        _authService = new AuthService(_cocheServices, configuration);
        _cochesManager = new CochesManager(_cocheServices);
        await _cochesManager.CargarCoches();

        await RunAsync();
    }
}