using FORTUNE_8OS.Gateways;
using FORTUNE_8OS.Services;

ShipGateway shipGateway = new();
ShipService shipService = new(shipGateway);
shipService.CreateShip();

ItemGateway itemGateway = new();
ItemService itemService = new(itemGateway);
ScrapGateway scrapGateway = new();
ScrapService scrapServices = new(scrapGateway, itemGateway, shipGateway);
ProductGateway productGateway = new();
ProductService productServices = new(productGateway);
string message;

Console.Clear();
Console.ForegroundColor = ConsoleColor.Green;

Console.WriteLine($"{shipService.PrintShipCredits()}\n");

Console.WriteLine("Welcome to the FORTUNE-8 OS");
Console.WriteLine("          Courtesy of the Company");
Console.WriteLine();

DateTime date = DateTime.Now;
Console.WriteLine($"Happy {date.DayOfWeek}");

string choice;

do
{
    Console.WriteLine("Type \"Help\" for a list of commands.\n");

    choice = Console.ReadLine().ToUpper();

    switch (choice)
    {

        case "HELP":
            Console.Clear();
            Console.WriteLine($"{shipService.PrintShipCredits()}\n");

            Console.WriteLine(">STORE");
            Console.WriteLine("To see the company store's selection of useful items.\n");

            Console.WriteLine(">PRODUCT SERVICES");
            Console.WriteLine("To register new products on system\n");

            Console.WriteLine(">ITEM SERVICES");
            Console.WriteLine("To read a txt list of items and save into Database\n");

            Console.WriteLine(">SCRAP ITEMS");
            Console.WriteLine("To scrap items and receive money\n");

            Console.WriteLine(">SCRAP HISTORIC");
            Console.WriteLine("To see the historic of scrapped items\n");

            Console.WriteLine(">EXIT");
            Console.WriteLine("To exit the program\n");
            break;

        case "STORE":
            Console.Clear();
            Console.WriteLine("This is a Store");
            break;

        case "PRODUCT SERVICES":
            Console.Clear();
            ProductServicesMenu();
            break;

        case "ITEM SERVICES":
            Console.Clear();
            ItemServicesMenu();
            break;

        case "SCRAP ITEMS":
            Console.Clear();
            message = scrapServices.ImputScrapInfo();
            Console.WriteLine(message);
            break;

        case "SCRAP HISTORIC":
            Console.Clear();
            scrapServices.GetScrapHistoric();
            break;

        case "EXIT":
            Console.Clear();
            Console.WriteLine("Closing the program...");
            break;

        default:
            Console.Clear();
            Console.WriteLine($"Unknow Command \"{choice}\"");
            break;
    };
} while (choice != "EXIT");

void ProductServicesMenu()
{
    string message;

    Console.WriteLine(">POST NEW PRODUCT");
    Console.WriteLine("To post a new product\n");

    Console.WriteLine(">LIST PRODUCTS");
    Console.WriteLine("To list all products\n");

    Console.WriteLine(">UPDATE PRODUCT QUANTITY");
    Console.WriteLine("To update product quantity available\n");

    Console.WriteLine(">DELETE PRODUCT");
    Console.WriteLine("To delete product");

    Console.WriteLine(">BACK");
    Console.WriteLine("To back to main menu\n");

    string choice;

    choice = Console.ReadLine().ToUpper();
    switch (choice)
    {
        case "POST NEW PRODUCT":
            Console.Clear();
            message = productServices.CreateNewProduct();
            Console.WriteLine(message);
            break;

        case "LIST PRODUCTS":
            Console.Clear();
            var products = productServices.GetProducts();
            foreach (var product in products)
            {
                Console.WriteLine($"{product.Name} -- Quantity:{product.Quantity} -- \\{product.Price} -- {product.Category}");
            }
            Console.WriteLine();
            break;

        case "UPDATE PRODUCT QUANTITY":
            Console.Clear();
            message = productServices.UpdateProductQuantity();
            Console.WriteLine(message);
            break;

        case "DELETE PRODUCT":
            Console.Clear();
            message = productServices.DeleteProduct();
            Console.WriteLine(message);
            break;

        case "BACK":
            Console.Clear();
            Console.WriteLine();
            break;

        default:
            Console.WriteLine($"Unknow Command \"{choice}\"");
            break;
    }
}

void ItemServicesMenu()
{
    string message;

    Console.WriteLine(">POST NEW ITEM");
    Console.WriteLine("To post a new item\n");

    Console.WriteLine(">READ ITEMS FROM FILE");
    Console.WriteLine("To read items from file\n");

    Console.WriteLine(">LIST ITEMS");
    Console.WriteLine("To list all items\n");

    Console.WriteLine(">UPDATE ITEM");
    Console.WriteLine("To update a target item\n");

    Console.WriteLine(">DELETE ITEM");
    Console.WriteLine("To delete a target item \n");

    Console.WriteLine(">BACK");
    Console.WriteLine("To back to main menu\n");


    string? choice = Console.ReadLine().ToUpper();
    switch (choice)
    {
        case "POST NEW ITEM":
            Console.Clear();
            var item = itemService.CreateNewItemObject();
            Console.WriteLine(itemService.PostItem(item));
            break;

        case "READ ITEMS FROM FILE":
            Console.Clear();
            message = itemService.ReadItemsFromFile();
            Console.WriteLine(message);
            break;

        case "LIST ITEMS":
            Console.Clear();
            var items = itemService.GetItems();
            Console.WriteLine();
            foreach (var data in items)
            {
                Console.WriteLine(data.ToString());
            }
            Console.WriteLine();
            break;

        case "UPDATE ITEM":
            Console.Clear();
            message = itemService.UpdateItem();
            Console.WriteLine(message);
            break;

        case "DELETE ITEM":
            Console.Clear();
            message = itemService.DeleteItem();
            Console.WriteLine(message);
            break;

        case "BACK":
            Console.Clear();
            Console.WriteLine();
            break;

        default:
            Console.WriteLine($"Unknow Command \"{choice}\"");
            break;
    }
}