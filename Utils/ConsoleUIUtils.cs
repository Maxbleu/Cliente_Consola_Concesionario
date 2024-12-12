using System.Reflection;
using System.Text.Json;
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
        /// <summary>
        /// Este método se encarga de mostrar los coches si
        /// no hay disponibles, mostrará un mensaje indicando
        /// </summary>
        /// <param name="coches"></param>
        public static void ShowCoches(List<CocheModel>? coches)
        {
            Console.WriteLine("\n--- COCHES ---");

            //  Si la lista de coches no es nula, entonces mostramos los coches
            if (coches != null)
            {
                //  Mostramos los coches en consola
                string resultado = string.Join(Environment.NewLine, coches);
                Console.WriteLine(resultado+"\n");
            }
            else 
            {
                //  Si no hay coches disponibles, mostramos un mensaje
                Console.WriteLine("\nNo hay coches disponibles\n");
            }
        }
        /// <summary>
        /// Este método se encarga de obtener las credenciales
        /// para iniciar sesión en la app
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public static void GetNewDataLogin(out string username, out string password)
        {
            Console.WriteLine("\n\nINICIAR SESION");

            Console.WriteLine("\nIntroduzca el nombre del usuario");
            username = Console.ReadLine();

            Console.WriteLine("Introduzca la contraseña");
            password = Console.ReadLine();
        }
        /// <summary>
        /// Este método se encarga de obtener un número y si
        /// lo introducido no es un número, se solicitará nuevamente
        /// de manera recursiva
        /// </summary>
        /// <param name="carId"></param>
        /// <param name="mensaje"></param>
        public static void GetDataNumber(out int carId, string? mensaje)
        {
            //  Solicitamos el ID del coche
            if (mensaje != null) Console.WriteLine($"\n{mensaje}");
            string stringCarId = Console.ReadLine();

            //  Verificamos si el ID es un número
            if (!int.TryParse(stringCarId, out carId)) 
            {
                //  Si no es un número, solicitamos nuevamente el ID
                Console.WriteLine("\nPor favor, introduzca un número\n\n");
                GetDataNumber(out carId, null);
            }
            else
            {
                //  Si es un número, lo convertimos a entero
                carId = int.Parse(stringCarId);
            }
        }
        /// <summary>
        /// Este método se encarga de obtener los datos de un nuevo coche
        /// a través de parámetros de salida que recibe el método
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="country"></param>
        /// <param name="carBrand"></param>
        /// <param name="carModel"></param>
        /// <param name="carColor"></param>
        /// <param name="year"></param>
        /// <param name="credirCardType"></param>
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
        /// <summary>
        /// Este método se encarga de obtener los campos que
        /// se desean modificar de un coche y sus respectivos
        /// valores
        /// </summary>
        /// <param name="datosModificados"></param>
        public static void GetDataModificadaCoche(out Dictionary<string, JsonElement> datosModificados)
        {
            datosModificados = new Dictionary<string, JsonElement>();

            //  Obtenemos las propiedades de la clase CocheModel
            Type type = typeof(CocheModel);
            PropertyInfo[] propiedades = type.GetProperties();

            //  Recorremos las propiedades de la clase CocheModel
            for (int i = 0; i<propiedades.Length; i++)
            {
                //  No se solicitará la propiedad Id
                if (i == 0) continue;

                //  Se solicitará al usuario si desea modificar la propiedad
                Console.WriteLine($"¿Desea modificar {propiedades[i].Name}? (s/n)");

                //  Si el usuario desea modificar la propiedad, se solicitará el nuevo valor
                if (Console.ReadLine().ToLower() == "s")
                {
                    Console.WriteLine($"Introduzca el nuevo {propiedades[i].Name}:");

                    //  Si la propiedad es de tipo entero, se solicitará un número
                    if (propiedades[i].PropertyType == typeof(int))
                    {
                        int numberValue = 0;

                        //  Se solicitará el número hasta que el usuario introduzca un número
                        GetDataNumber(out numberValue, null);

                        //  Se añade la propiedad y su valor al diccionario
                        datosModificados.Add(propiedades[i].Name, JsonDocument.Parse(numberValue.ToString()).RootElement);
                    }
                    else if (propiedades[i].PropertyType == typeof(string))
                    {
                        //  Si la propiedad es de tipo string, se solicitará un string
                        string stringValue = Console.ReadLine();

                        //  Se añade la propiedad y su valor al diccionario
                        datosModificados.Add(propiedades[i].Name, JsonDocument.Parse($"\"{stringValue}\"").RootElement);
                    }

                }
            }
        }
    }
}
