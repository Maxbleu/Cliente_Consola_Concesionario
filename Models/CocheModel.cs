using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp_Concesionario.Models
{
    public class CocheModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Country { get; set; }
        public string CarBrand { get; set; }
        public string CarModel { get; set; }
        public string CarColor { get; set; }
        public int YearOfManufacture { get; set; }
        public string CreditCardType { get; set; }

        public CocheModel(string firstName, string lastName, string country, string carBrand, string carModel, string carColor, int yearOfManufacture, string creditCardType)
        {
            FirstName = firstName;
            LastName = lastName;
            Country = country;
            CarBrand = carBrand;
            CarModel = carModel;
            CarColor = carColor;
            YearOfManufacture = yearOfManufacture;
            CreditCardType = creditCardType;
        }

        public override string ToString()
        {
            return $"{FirstName} {LastName} - {Country}, {CarBrand} {CarModel} ({YearOfManufacture}), {CarColor}, {CreditCardType}";
        }
    }
}
