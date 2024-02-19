using FORTUNE_8OS.Gateways;
using FORTUNE_8OS.Services;

namespace FORTUNE_8OS
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ShipGateway shipGateway = new ShipGateway();
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
            Console.WriteLine("Type \"Help\" for a list of commands.\n");
            string choice;

            do
            {
                choice = Console.ReadLine().ToUpper();
                Console.Clear();

                switch (choice)
                {
                    case "HELP":
                        Console.WriteLine($"{shipService.PrintShipCredits()}\n");
                        //shipService.ShipMoney();
                        Console.WriteLine(">STORE");
                        Console.WriteLine("To see the company store's selection of useful items.\n");

                        Console.WriteLine(">REGISTER PRODUCTS");
                        Console.WriteLine("To register new products on system\n");

                        Console.WriteLine(">READ ITEMS FILE");
                        Console.WriteLine("To read a txt list of items and save into Database\n");

                        Console.WriteLine(">SCRAP ITEMS");
                        Console.WriteLine("To scrap items and receive money\n");

                        Console.WriteLine(">SCRAP HISTORIC");
                        Console.WriteLine("To see the historic of scrapped items\n");

                        Console.WriteLine(">EXIT");
                        Console.WriteLine("To exit the program\n");
                        break;

                    case "STORE":
                        Console.WriteLine("This is a Store");
                       
                        break;

                    case "REGISTER PRODUCTS":
                        
                        break;

                    case "READ ITEMS FILE":
                       
                        break;

                    case "SCRAP ITEMS":
                        
                        break;

                    case "SCRAP HISTORIC":
                       
                        break;
                };
            } while (choice is not "EXIT");
        }
    }
}
