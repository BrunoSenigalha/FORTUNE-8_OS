using FORTUNE_8OS.Gateways;
using FORTUNE_8OS.Services;
using System.Runtime.CompilerServices;

ShipGateway shipGateway = new();
ShipService shipService = new(shipGateway);
shipService.CreateShip();

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
            break;

        case "SCRAP HISTORIC":
            Console.Clear();
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
    Console.WriteLine(">POST NEW PRODUCT");
    Console.WriteLine("To post a new product\n");

    Console.WriteLine(">LIST PRODUCTS");
    Console.WriteLine("To list all products\n");

    Console.WriteLine(">UPDATE PRODUCT");
    Console.WriteLine("To update product name or credits\n");

    Console.WriteLine(">BACK");
    Console.WriteLine("To back to main menu\n");

    string choice;
    do
    {
        choice = Console.ReadLine().ToUpper();
        switch (choice)
        {
            case "POST NEW PRODUCT":
                break;

            case "LIST PRODUCTS":
                break;

            case "UPDATE PRODUCT":
                break;

            case "BACK":
                Console.Clear();
                break;

            default:
                Console.WriteLine($"Unknow Command \"{choice}\"");
                break;
        }
    } while (choice != "BACK");
}

void ItemServicesMenu()
{
    Console.WriteLine(">POST NEW ITEM");
    Console.WriteLine("To post a new item\n");

    Console.WriteLine(">LIST ITEMS");
    Console.WriteLine("To list all items\n");

    Console.WriteLine(">UPDATE ITEM");
    Console.WriteLine("To update item name or credits\n");

    Console.WriteLine(">BACK");
    Console.WriteLine("To back to main menu\n");

    string choice;
    do
    {
        choice = Console.ReadLine().ToUpper();
        switch (choice)
        {
            case "POST NEW ITEM":
                Console.Clear();

                break;

            case "LIST ITEMS":
                break;

            case "UPDATE ITEM":
                break;

            case "BACK":
                Console.Clear();
                break;

            default:
                Console.WriteLine($"Unknow Command \"{choice}\"");
                break;
        }
    } while (choice != "BACK");
}
