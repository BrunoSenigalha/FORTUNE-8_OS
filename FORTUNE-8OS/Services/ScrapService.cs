using FORTUNE_8OS.Entities;
using FORTUNE_8OS.Interfaces;

namespace FORTUNE_8OS.Services
{
    public class ScrapService
    {
        private readonly IScrapGateway _scrapGateway;
        private readonly ItemService _itemService;
        private readonly ShipService _shipService;

        public ScrapService(IScrapGateway scrapGateway, IItemGateway itemGateway, IShipGateway shipGateway)
        {
            _scrapGateway = scrapGateway;
            _itemService = new ItemService(itemGateway);
            _shipService = new ShipService(shipGateway);
        }

        public string ImputScrapInfo()
        {
            Console.WriteLine("Follow the list of Items for Scrap: \n");
            ItemsListForScrap();

            //bool finish = false;
            string option;
            decimal totalPayment = 0;
            do
            {
                Console.WriteLine("Enter the name of the item and the quantity for scrap");
                string scrapItemData = Console.ReadLine();

                (string messageReturn, decimal payment) = PostScrapItems(scrapItemData);
                totalPayment += payment;
                Console.WriteLine(messageReturn);

                Console.WriteLine("Would you like to scrap another item?");
                option = Console.ReadLine().ToUpper();

            } while (option != "NO");

            return $"Total payment for your services: {totalPayment}";
        }

        private void ItemsListForScrap()
        {
            var items = _itemService.GetItems();
            foreach (var item in items)
            {
                Console.WriteLine($"{item.Name} \\ {item.Credits}");
            }
            Console.WriteLine();
        }

        public (string, decimal) PostScrapItems(string scrapItemData)
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
                    Scrap scrap = new(quantity, DateTime.Now, payment, itemForScrap);
                    _scrapGateway.PostScrap(scrap);
                    return ("Successfully scraped!\n", payment);
                }
                return ("Item not found!\n", 0);
            }
            return ("Wrong value informed!\n", 0);
        }

        public decimal ProcessPaymentForScrap(int quantity, decimal credits)
        {
            decimal payment = quantity * credits;
            var ship = _shipService.GetShip();
            if (ship != null)
            {
                ship?.AddCredits(payment);
                _shipService.UpdateShip(ship);
            }
            return payment;
        }

        public void GetScrapHistoric()
        {
            var scraps = _scrapGateway.GetScraps();
            foreach (var scrap in scraps)
            {
                Console.WriteLine($"{scrap.ScrapDate:MM/dd/yyyy HH:mm} -- {scrap.Item.Name} {scrap.Quantity}  \\{scrap.Credits:F2} ");
            }
        }
    }
}
