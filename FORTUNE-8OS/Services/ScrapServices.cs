using FORTUNE_8OS.Entities;
using FORTUNE_8OS.Gateways;
using FORTUNE_8OS.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FORTUNE_8OS.Services
{
    public class ScrapServices
    {
        private readonly IScrapGateway _scrapGateway;
        private readonly ItemGateway _itemGateway;
        private readonly ItemService _itemService;
        private readonly ShipGateway _shipGateway;
        private readonly ShipService _shipService;
        public ScrapServices(IScrapGateway scrapGateway)
        {
            _scrapGateway = scrapGateway;
            _itemService = new ItemService(_itemGateway);
            _shipService = new ShipService(_shipGateway);
        }

        public string ImputScrapInfo()
        {
            Console.WriteLine("Follow the list of Items for Scrap: \n");
            ItemsListForScrap();

            string? option;
            decimal totalPayment = 0;
            do
            {
                Console.WriteLine("Enter the name of the item and the quantity for scrap");
                string? scrapItemData = Console.ReadLine();

                (string messageReturn, decimal payment) = PostScrapItems(scrapItemData);
                totalPayment += payment;
                Console.WriteLine(messageReturn);

                Console.WriteLine("Would you like to scrap another item?");
                option = Console.ReadLine();

            } while (option != "NO");

            return $"Total payment for your services: {totalPayment}";
        }

        private void ItemsListForScrap()
        {
            var items = _itemService.GetItems();
            foreach (var item in items)
            {
                Console.WriteLine($"{item.Name} \\ {item.Id}");
            }
            Console.WriteLine();
        }

        private (string, decimal) PostScrapItems(string scrapItemData)
        {
            string[] strings = scrapItemData.Split(" ");
            int vetLenght = strings.Length - 1;

            if (int.TryParse(strings[vetLenght], out int quantity))
            {
                string itemName = "";
                for (int i = 0; i < vetLenght; i++)
                {
                    itemName += strings[i] + " ";
                }

                var itemsFromDatabase = _itemService.GetItems();
                var itemForScrap = itemsFromDatabase.Where(p => p.Name.Equals(itemName.TrimEnd(), StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                if (itemForScrap != null)
                {
                    decimal payment = ProcessPaymentForScrap(quantity, itemForScrap.Credits);
                    Scrap scrap = new(itemForScrap.Id, quantity, DateTime.Now, payment, itemForScrap);
                    _scrapGateway.PostScrap(scrap);
                    return ("Successfully scraped", payment);
                }
                return ("Item not found", 0);
            }
            return ("Wrong value informed", 0);
        }

        private decimal ProcessPaymentForScrap(int quantity, decimal credits)
        {
            decimal payment = quantity * credits;
            var ship = _shipService.GetShip();
            ship.AddCredits(payment);

            return payment;
        }
    }
}
