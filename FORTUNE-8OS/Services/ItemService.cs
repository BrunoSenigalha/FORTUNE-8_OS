﻿using FORTUNE_8OS.Entities;
using FORTUNE_8OS.Interfaces;
using FORTUNE_8OS.Utilitaries;

namespace FORTUNE_8OS.Services
{
    public class ItemService(IItemGateway itemGateway)
    {
        private readonly IItemGateway _itemGateway = itemGateway;
        private readonly ValidateDecimalImput _validateDecimalInput = new();

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

        public Item CreateNewItemObject()
        {
            Console.WriteLine("Enter a name of the item: ");
            string? name = Console.ReadLine();
            Console.WriteLine("Enter product credits: ");
            decimal credits = _validateDecimalInput.ValidateCredits();

            Item item = new(name, credits);
            return item;
        }

        public IEnumerable<Item> GetItems()
        {
            var items = _itemGateway.GetItemList();
            return items;
        }

        public string UpdateItem()
        {
            Console.WriteLine("Type the name of item you would like to update: ");
            string? itemName = Console.ReadLine();
            var itemFromDatabase = FindItemFromDatabase(itemName);

            if (itemFromDatabase != null)
            {
                Console.WriteLine($"Required Item: {itemFromDatabase.Name} \\ {itemFromDatabase.Credits}");
                Console.WriteLine("Type CONFIRM or DENY");
                string? option = Console.ReadLine();

                if (option is not null && option.ToUpper() == "CONFIRM")
                {
                    Console.WriteLine("Type a new name of item: ");
                    itemName = Console.ReadLine();
                    Console.WriteLine("Type a new credits value: ");
                    decimal credits = _validateDecimalInput.ValidateCredits();

                    Item item = new(itemFromDatabase.Id, itemName, credits);
                    _itemGateway.UpdateItem(item);

                    return $"Item {item.Name} updated successfully";
                }
                return $"You denied the change";
            }
            return "Item not found";
        }

        public string DeleteItem()
        {
            Console.WriteLine("Type the name of item you would like to delete: ");
            string? itemName = Console.ReadLine();
            var itemFromDatabase = FindItemFromDatabase(itemName);

            if (itemFromDatabase != null)
            {
                Console.WriteLine($"Required item to Delete: {itemFromDatabase.Name} \\ {itemFromDatabase.Credits}");
                Console.WriteLine("Type CONFIRM or DENY");
                string? option = Console.ReadLine();

                if (option is not null && option.ToUpper() == "CONFIRM")
                {
                    _itemGateway.DeleteItem(itemFromDatabase);
                    return $"Item {itemFromDatabase.Name} deleted";
                }
                return "You denied the delectation";
            }
            return "Item not found";
        }

        public string ReadItemsFromFile()
        {
            Console.WriteLine("Enter the directory path.");
            string? path = Console.ReadLine();

            if (path != null)
            {
                var listOfItemsFromDatabase = _itemGateway.GetItemList();
                try
                {
                    string[] lines = File.ReadAllLines(path);
                    foreach (string line in lines)
                    {
                        string[] data = line.Split(',');
                        string itemName = data[0];
                        var itemExists = listOfItemsFromDatabase.Where(x => x.Name.Equals(itemName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

                        if (itemExists == null && decimal.TryParse(data[1], out decimal itemCredits))
                        {
                            Item item = new(itemName, itemCredits);
                            _itemGateway.PostItem(item);
                            itemExists = null;
                        }
                    }
                    return "Items were read successfully";
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
            return "Informe the path needed";
        }

        public Item? FindItemFromDatabase(string itemName)
        {
            var items = GetItems();
            var itemFromDatabase = items.Where(p => p.Name.Equals(itemName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

            return itemFromDatabase;
        }

        //public IEnumerable<Item> GetItems()
        //{
        //    return _itemService.GetItemList();
        //}
    }
}