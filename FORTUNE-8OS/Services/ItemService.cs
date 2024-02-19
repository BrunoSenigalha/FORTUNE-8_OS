using FORTUNE_8OS.Entities;
using FORTUNE_8OS.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FORTUNE_8OS.Services
{
    public class ItemService(IItemGateway itemGateway)
    {
        private readonly IItemGateway _itemGateway = itemGateway;
        public string PostItem(Item item)
        {
            var listOfExistingItems = GetItems();
            var itemForVerify = listOfExistingItems.Where(p => p.Name.Equals(item.Name, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

            if (itemForVerify != null)
            {
                return $"This {item.Name} item already exists";
            }

            _itemGateway.PostItem(item);
            return $"Item {item.Name} created successfully";
        }

        public static Item CreatingNewItemObject()
        {
            Console.WriteLine("Enter with a name of the item: ");
            string? name = Console.ReadLine();

            decimal credits;
            bool validImput = false;

            do
            {
                Console.WriteLine("Enter product credits: ");
                validImput = decimal.TryParse(Console.ReadLine(), out credits);

                if (!validImput)
                {
                    Console.WriteLine("Invalid input. Please, enter a valid decimal value for credits.");
                }
            } while (!validImput);

            Item item = new(name, credits);
            return item;
        }

        public IEnumerable<Item> GetItems()
        {
            var items = _itemGateway.GetItemList();
            return items;
        }


    }
}
