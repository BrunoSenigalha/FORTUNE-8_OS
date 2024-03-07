using FORTUNE_8OS.Entities;
using FORTUNE_8OS.Interfaces;

namespace FORTUNE_8OS.Services
{
    public class PaymentService
    {
        private readonly ShipService _shipService;
        private readonly ProductService _productService;
        public PaymentService(IShipGateway shipGateway, IProductGateway productService)
        {
            _shipService = new ShipService(shipGateway);
            _productService = new ProductService(productService);
        }

        public string ValidatePurchase(Product requiredProduct, int quantity)
        {
            if (quantity < 0)
            {
                return "The quantity cannot be negative";
            }

            var ship = _shipService.GetShip();
            decimal total = requiredProduct.Price * quantity;

            if (ship is not null && total <= ship.Credits && requiredProduct.Quantity >= quantity)
            {
                Console.WriteLine($"You have requested to order a {requiredProduct.Name}\n" +
                $"Total cost of item: {total:F2}.\n");
                Console.WriteLine("Please CONFIRM or DENY.");
                string? option = Console.ReadLine();

                if (option != null && option.Equals("CONFIRM", StringComparison.CurrentCultureIgnoreCase))
                {
                    CompletePurchase(ship, requiredProduct, quantity, total);
                    return $"You ordered {requiredProduct.Name} and your new balance is {ship.Credits}";
                }
                return "You canceled the purchase";
            }
            return quantity > requiredProduct.Quantity ?
                "More quantity then you can buy" : "You could not afford these items!\n" +
                $"Your balance is \\{ship.Credits:F2}. Total cost of these items is \\{total:F2}";
        }

        private void CompletePurchase(Ship ship, Product product, int quantity, decimal total)
        {
            ship.RemoveCredits(total);
            _shipService.UpdateShip(ship);
            product.UpdateQuantity(quantity);
            _productService.UpdateProduct(product);

            Console.WriteLine($"Ordered the {product.Name}! Your new balance is \\{ship.Credits:F0}.");
        }
    }
}
