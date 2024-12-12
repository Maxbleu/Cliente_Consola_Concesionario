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

    /// <summary>
    /// Obtiene todos los coches de la base de datos y 
    /// los muestra por consola
    /// </summary>
    /// <returns></returns>
    private static async Task ObtenerCochesAsync()
    {
        //  Solicitamos los coches a la base de datos
        List<CocheModel>? coches = await _cochesManager.ObtenerCochesAsync();

        //  Mostramos los coches por consola    
        ConsoleUIUtils.ShowCoches(coches);
    }
    /// <summary>
    /// Obtiene un coche por su id a partir de la base de datos 
    /// y lo muestra por consola
    /// </summary>
    /// <returns></returns>
    private static async Task ObtenerCochePorIdAsync()
    {
        int car_id;

        //  Obtenemos el id del coche
        ConsoleUIUtils.GetDataNumber(out car_id, "\nIntroduzca el id de un coche");

        //  Solicitados el coche por su id a la base de datos
        CocheModel? coche = await _cochesManager.ObtenerCocheByIdAsync(car_id);

        //  Mostramos el coche por consola
        Console.WriteLine(coche != null ? coche.ToString()+"\n" : "Registro no encontrado\n");
    }
    /// <summary>
    /// Crea un nuevo coche en la base de datos y el resultado
    /// de la creación lo muestra por pantalla
    /// </summary>
    /// <returns></returns>
    private static async Task CrearNuevoCocheAsync() 
    {
        //  Verificamos si el usuario está logueado
        if (!await _authService.EstaLogueadoAsync()) return;
        
        string firstName, lastName, country, carBrand, carModel, carColor, creditCardType;
        int year;

        //  Obtenemos los datos del nuevo coche
        ConsoleUIUtils.GetNewDataCoche(out firstName, out lastName, out country, out carBrand, out carModel, out carColor, out year, out creditCardType);

        //  Creamos el coche en la base de datos
        CocheModel coche = await _cochesManager.CrearCocheAsync(firstName, lastName, country, carBrand, carModel, carColor, year, creditCardType);

        //  Mostramos el coche creado por consola
        Console.WriteLine("\n"+coche.ToString()+"\n");
    }
    /// <summary>
    /// Actualiza un coche en la base de datos y el resultado de 
    /// la operación lo muestra por consola
    /// </summary>
    /// <returns></returns>
    private static async Task ActualizarCocheAsync() 
    {
        //  Verificamos si el usuario está logueado
        if (!await _authService.EstaLogueadoAsync()) return;

        int car_id;
        Dictionary<string, JsonElement> datosModificados;

        //  Obtenemos el id del coche a modificar
        ConsoleUIUtils.GetDataNumber(out car_id, "Introduzca el id de un coche");

        //  Obtenemos los datos modificados del coche
        ConsoleUIUtils.GetDataModificadaCoche(out datosModificados);

        //  Actualizamos el coche en la base de datos
        CocheModel cocheModificado = await _cochesManager.ActualizarCocheAsync(car_id, datosModificados);

        //  Mostramos el coche modificado por consola
        Console.WriteLine(cocheModificado.ToString());
    }
    /// <summary>
    /// Elimina un coche de la base de datos y el resultado de la
    /// operación lo muestra por consola
    /// </summary>
    /// <returns></returns>
    private static async Task EliminarCocheAsync() 
    {
        //  Verificamos si el usuario está logueado
        if (!await _authService.EstaLogueadoAsync()) return;
        int car_id;

        //  Obtenemos el id del coche a eliminar
        ConsoleUIUtils.GetDataNumber(out car_id, "Introduzca el id de un coche");

        //  Eliminamos el coche de la base de datos
        await _cochesManager.EliminarCocheAsync(car_id);

        //  Mostramos el resultado de la operación por consola
        Console.WriteLine($"\nEl coche {car_id} ha sido eliminado exitosamente\n");
    }
    /// <summary>
    /// Muestra un mensaje de salida por consola
    /// </summary>
    private static void Salir()
    {
        Console.WriteLine("\nHas salido del programa");
    }
    /// <summary>
    /// Ejecuta el programa principal de la aplicación
    /// </summary>
    /// <returns></returns>
    private static async Task RunAsync()
    {
        Task tareaSeleccionada = null;
        ConsoleKey input;

        //  Mientras que el usuario no decida salir
        //  (PULSAR F) el bucle continuará ejecutándose

        do
        {
            //  Comprobamos si hay una tarea en ejecución
            //  y si la hay esperamos a que termine
            if (tareaSeleccionada != null) await tareaSeleccionada;

            //  Mostramos el menú principal de la aplicación
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