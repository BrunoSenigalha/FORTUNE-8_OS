using FORTUNE_8OS.Entities;
using FORTUNE_8OS.Entities.Enums;
using FORTUNE_8OS.Gateways;
using FORTUNE_8OS.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FORTUNE_8OS.Services
{
    public class ProductServices(IProductGateway productGateway)
    {
        private readonly IProductGateway _productGateway = productGateway;

        public string CreateNewProduct()
        {
            Console.WriteLine("Enter product information:");
            Console.WriteLine("Name:");
            string? productName = Console.ReadLine();

            if (FindProduct(productName) != null)
            {
                return $"The product {productName} does exist";
            }

            Console.WriteLine("Description:");
            string? productDescription = Console.ReadLine();

            try
            {
                Console.WriteLine("Quantity:");
                int quantity = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Price:");
                decimal price = Convert.ToDecimal(Console.ReadLine());
                Console.WriteLine("Please, inform the product Category (Tool/ShipUpgrade/Decor):");
                CategoryEnum category = (CategoryEnum)Enum.Parse(typeof(CategoryEnum), Console.ReadLine(), true);

                Product product = new(productName.Trim(), productDescription, quantity, price, category);
                _productGateway.PostProduct(product);

                return $"The product: {productName} was successfully created";
            }
            catch (FormatException ex)
            {
                return $"Format error: {ex.Message}";
            }
            catch (ArgumentException ex)
            {
                return $"Argument error: {ex.Message}";
            }
        }

        public Product? FindProduct(string productName)
        {
            var products = GetProducts();
            var product = products.Where(p => p.Name.Equals(productName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            return product;
        }

        public IEnumerable<Product> GetProducts()
        {
            return _productGateway.GetProducts();
        }
    }
}