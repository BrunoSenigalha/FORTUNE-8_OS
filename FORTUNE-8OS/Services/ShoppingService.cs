using FORTUNE_8OS.Entities;
using FORTUNE_8OS.Gateways;
using FORTUNE_8OS.Interfaces;

namespace FORTUNE_8OS.Services
{
    public class ShoppingService
    {
        private readonly PaymentService _paymentService;
        private readonly ShipGateway shipGateway = new();
        private readonly ProductGateway productGateway = new();
        public ShoppingService()
        {
            _paymentService = new PaymentService(shipGateway, productGateway);
        }

        public string ProcessInformation(string productName, List<Product> products)
        {
            string[] strings = productName.Split(" ");
            int vetLenght = strings.Length - 1;
            string lastInfo = strings[vetLenght];
            string productNameRequired = "";

            for (int i = 0; i < vetLenght; i++)
            {
                productNameRequired += strings[i] + " ";
            }
            productNameRequired = productNameRequired.Trim();

            var requiredProductFromDatabase = products.Where(p => p.Name.Equals(productNameRequired, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            if (requiredProductFromDatabase != null)
            {
                if (lastInfo.Equals("INFO", StringComparison.CurrentCultureIgnoreCase))
                {
                    return $"{requiredProductFromDatabase.Description}";
                }
                else if (int.TryParse(lastInfo, out int value))
                {
                    int quantity = value;
                    var message = _paymentService.ValidatePurchase(requiredProductFromDatabase, quantity);
                    return message;
                }
                else if (lastInfo.Equals("BUY", StringComparison.CurrentCultureIgnoreCase))
                {
                    Console.WriteLine("Entry the quantity you wish to purchase");
                    int quantity = Convert.ToInt32(Console.ReadLine());
                    return _paymentService.ValidatePurchase(requiredProductFromDatabase, quantity);
                }
            }
            return "Wrong value informed";
        }

        public Dictionary<int, decimal> PromotionGenerator(int vetLenght)
        {
            var random = new Random();
            decimal[] percent = [0.10M, 0.15M, 0.50M, 0.75M, 0.80M];
            Dictionary<int, decimal> indexAndPercent = [];

            // The quantity of products on sale can be from 0 to 3
            int quantityOfProductsOnSale = random.Next(4);
            int[] index = new int[quantityOfProductsOnSale];

            // Checking if there are any products on sale 
            if (quantityOfProductsOnSale > 0)
            {

                for (int i = 0; i < quantityOfProductsOnSale; i++)
                {
                    // Generating the Index and the Percent
                    int randomIndex = random.Next(vetLenght);
                    int randomPercentIndex = random.Next(percent.Length);

                    // This check was created to prevent the same index from being accessed twice or more
                    if (!index.Contains(randomIndex))
                    {
                        index[i] = randomIndex;
                        indexAndPercent.Add(index[i], percent[randomPercentIndex]);
                    }
                }
                return indexAndPercent;
            }
            return null;
        }
    }
}
