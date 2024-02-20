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

        public static Item CreateNewItemObject()
        {
            Console.WriteLine("Enter a name of the item: ");
            string? name = Console.ReadLine();

            decimal credits;
            bool validImput;
            do
            {
                Console.WriteLine("Enter product credits: ");
                validImput = decimal.TryParse(Console.ReadLine(), out credits);

                if (!validImput)
                {
                    throw new InvalidOperationException("Wrong value for credits, please type again.");
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
