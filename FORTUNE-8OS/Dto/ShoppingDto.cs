using FORTUNE_8OS.Entities;
using FORTUNE_8OS.Entities.Enums;
using FORTUNE_8OS.Interfaces;
using FORTUNE_8OS.Services;
using FORTUNE_8OS.Utilitaries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FORTUNE_8OS.Dto
{
    public class ShoppingDto(ShipService shipService, ShoppingService shoppingService)
    {
        private readonly ShipService _shipService = shipService;
        private readonly ShoppingService _shoppingService = shoppingService;


        public void ShopProducts(IEnumerable<Product> products)
        {
            Console.WriteLine($"{_shipService.PrintShipCredits()}\n");

            Console.WriteLine("Welcome to the company store.\n" +
                "Use words BUY and INFO on any item.\n" +
                "Order tools in bulk by typing a number.");
            Console.WriteLine("------------------------------\n");

            var tools = products.Where(p => p.Category == CategoryEnum.Tool).ToList();
            var shipUpgrade = products.Where(p => p.Category == CategoryEnum.ShipUpgrade).ToList();
            var decor = products.Where(p => p.Category == CategoryEnum.Decor).ToList();
            var productsList = products.ToList();

            //Tools List
            Console.WriteLine("TOOLS:");
            ListOfProducts(tools);
            Console.WriteLine();

            //Ship Upgrades List
            Console.WriteLine("SHIP UPGRADES:");
            ListOfProducts(shipUpgrade);
            Console.WriteLine();

            //Decor List
            Console.WriteLine("The selection of ship decor rotates per-quota. Be\n" +
                "sure to check back next week:");
            Console.WriteLine("------------------------------");
            ListOfProducts(decor);

            Console.WriteLine();
            string? productName = Console.ReadLine();
            var message = _shoppingService.ProcessInformation(productName, productsList);
            Console.WriteLine(message);
        }

        private void ListOfProducts(List<Product> products)
        {
            Dictionary<int, decimal> promotionProducts = _shoppingService.PromotionGenerator(products.Capacity);

            for (int i = 0; i < products.Count; i++)
            {
                if (promotionProducts is not null && promotionProducts.ContainsKey(i))
                {
                    products[i].Price = products[i].Price - (products[i].Price * promotionProducts[i]);
                    Console.WriteLine($"* {products[i].Name} // Price: {products[i].Price:F2} // Quantity: {products[i].Quantity}  ({promotionProducts[i] * 100:F0}% OFF!)");
                }
                else
                    Console.WriteLine($"* {products[i].Name} // Price: {products[i].Price} // Quantity: {products[i].Quantity}");
            }
            Console.WriteLine();
        }
    }
}
