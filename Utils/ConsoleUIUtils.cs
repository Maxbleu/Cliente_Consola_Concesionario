using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ConsoleApp_Concesionario.Models;

namespace ConsoleApp_Concesionario.Utils
{
    public static class ConsoleUIUtils
    {
        /// <summary>
        /// Este método se encarga de mostrar el menú de la app
        /// y devolver la opción que selecciona el usuario
        /// </summary>
        /// <returns></returns>
        public static ConsoleKey ShowMenu()
        {
            Console.WriteLine("GESTIÓN DE VEHÍCULOS DE UN CONCESIONARIO");
            Console.WriteLine("A. Listar vehículos.");
            Console.WriteLine("B. Obtener vehículo específico.");
            Console.WriteLine("C. Nuevo vehículo.");
            Console.WriteLine("D. Actualizar vehículo.");
            Console.WriteLine("E. Eliminar Vehículo.");
            Console.WriteLine("F. Salir.");
            Console.Write("Elige una opción: \n");

            ConsoleKey input = Console.ReadKey().Key;

            return input;
        }

        public static void ShowCoches(List<CocheModel>? coches)
        {
            Console.WriteLine("\n--- COCHES ---");
            if (coches != null)
            {
                string resultado = string.Join(Environment.NewLine, coches);
                Console.WriteLine(resultado+"\n");
            }
            else 
            {
                Console.WriteLine("\nNo hay coches disponibles\n");
            }
        }

        public static void GetNewDataLogin(out string username, out string password)
        {
            Console.WriteLine("\n\nINICIAR SESION");

            Console.WriteLine("\nIntroduzca el nombre del usuario");
            username = Console.ReadLine();

            Console.WriteLine("Introduzca la contraseña");
            password = Console.ReadLine();
        }

        public static void GetDataNumber(out int carId, string? mensaje)
        {
            if(mensaje != null) Console.WriteLine($"\n{mensaje}");
            string stringCarId = Console.ReadLine();

            if (!int.TryParse(stringCarId, out carId)) 
            { 
                Console.WriteLine("\nPor favor, introduzca un número\n\n");
                GetDataNumber(out carId, null);
            }
            carId = int.Parse(stringCarId);
        }

        public static void GetNewDataCoche(out string firstName, out string lastName, out string country, out string carBrand, out string carModel, out string carColor, out int year, out string credirCardType)
        {
            Console.WriteLine("\nCREAR NUEVO COCHE\n");

            Console.WriteLine("Introduzca el nombre");
            firstName = Console.ReadLine();

            Console.WriteLine("Introduzca el apellido");
            lastName = Console.ReadLine();

            Console.WriteLine("Introduzca el pais");
            country = Console.ReadLine();

            Console.WriteLine("Introduca la marca del coche");
            carBrand = Console.ReadLine();

            Console.WriteLine("Introduzca el modelo del coche");
            carModel = Console.ReadLine();

            Console.WriteLine("Introduzca el color del coche");
            carColor = Console.ReadLine();

            Console.WriteLine("Introduzca el año del coche");
            GetDataNumber(out year, null);

            Console.WriteLine("Introduzca el tipo de tarjeta utilizada");
            credirCardType = Console.ReadLine();
        }
    
        public static void GetDataModificadaCoche(out Dictionary<string, JsonElement> datosModificados)
        {
            datosModificados = new Dictionary<string, JsonElement>();
        }
    }
}
