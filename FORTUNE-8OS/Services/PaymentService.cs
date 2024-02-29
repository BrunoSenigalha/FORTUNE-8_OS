using FORTUNE_8OS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FORTUNE_8OS.Services
{
    public class PaymentService
    {
       public void ValidatePurchase(Product requiredProduct, int quantity, Ship ship)
        {
            bool confirm;
            decimal total = requiredProduct.Price * quantity;

            if (total <= ship.Credits && requiredProduct.Quantity <= quantity)
            {
                Console.Clear();
                Console.WriteLine($"You have requested to order a {requiredProduct.Name}\n" +
                $"Total cost of item: {total:F2}.\n");
                Console.WriteLine("Please CONFIRM or DENY.");
                string option = Console.ReadLine().ToUpper();

                confirm = option == "CONFIRM" ? true : false;
                if (confirm)
                {
                    CompletePurchase(ship, requiredProduct, quantity, total);
                }
            }
            else
            {
                string message = quantity > requiredProduct.Quantity ?
                    "More quantity then you can buy" : "You could not afford these items!\n" +
                    $"Your balance is \\{ship.Credits:F2}. Total cost of these items is \\{total:F2}";

                Console.WriteLine(message);
            }
        }
        public void CompletePurchase(Ship ship, Product product, int quantity, decimal total)
        {
            Console.Clear();
            ship.UpdateMoney(total);
            _shipService.ShipMoneyUpdate(ship);
            product.UpdateQuantity(quantity);
            _productService.UpdateProducts(product);

            Console.WriteLine($"Ordered the {product.Name}! Your new balance is \\{ship.Money:F0}.");
        }
    }
}
