using FORTUNE_8OS.Entities;
using FORTUNE_8OS.Entities.Enums;
using FORTUNE_8OS.Interfaces;


namespace FORTUNE_8OS.Services
{
    public class ProductService(IProductGateway productGateway)
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
            var product = products.Where(p => p.Name.Equals(productName.Trim(), StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            return product;
        }

        public IEnumerable<Product> GetProducts()
        {
            return _productGateway.GetProducts();
        }

        public string UpdateProductQuantity()
        {
            Console.WriteLine("Type the product name");
            string? productName = Console.ReadLine();

            var product = FindProduct(productName);

            if (product is null)
            {
                return $"Product not found";
            }
            try
            {
                Console.WriteLine("Enter the new quantity to add:");
                int quantity = Convert.ToInt32(Console.ReadLine());
                product.UpdateQuantity(quantity);
                _productGateway.UpdateProduct(product);

                return $"Product {product.Name} quantity was update to: {product.Quantity}";
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

       public string DeleteProduct()
        {
            Console.WriteLine("Type the product name");
            string? productName = Console.ReadLine();

            if (productName is null)
            {
                return "The name of the product was not informed";
            }
            var product = FindProduct(productName);

            if (product is null)
            {
                return $"Product not found";
            }
            _productGateway.DeleteProduct(product);
            return $"Product {productName.Trim()}";
        }
    }
}