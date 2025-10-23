
using System;
using System.Data;
using System.Net;
using AutoMapper;
using TradingIndustry.DALEF.Concrete;
using TradingIndustry.DTO;

namespace TradingIndustry.App.AppMenu
{
    internal class AppMenuService
    {
        private readonly string _connectionString;
        private readonly IMapper _mapper;

        public AppMenuService(string connectionString, IMapper mapper)
        {
            _connectionString = connectionString;
            _mapper = mapper;
        }

        public void Show()
        {
            Console.WriteLine("Welcome to TradingIndustry DAL CRUD Demo!\n");

            char choice = ' ';
            while (choice != 'q' && choice != 'Q')
            {
                Console.WriteLine("Select an entity menu for CRUD operations:");
                Console.WriteLine("1 - Role Menu (Roles)");
                Console.WriteLine("2 - User Menu (Users)");
                Console.WriteLine("3 - Address Menu (Addresses)");
                Console.WriteLine("4 - BankCard Menu (BankCards)");
                Console.WriteLine("q - Quit Application\n");

                string input = Console.ReadLine()?.ToLower() ?? "";
                if (string.IsNullOrEmpty(input)) continue;

                choice = input[0];

                switch (choice)
                {
                    case '1':
                        new Menu<Role>(new RoleDALEF(_connectionString, _mapper)).Show();
                        break;
                    case '2':
                        new Menu<User>(new UserDALEF(_connectionString, _mapper)).Show();
                        break;
                    case '3':
                        new Menu<Address>(new AddressDALEF(_connectionString, _mapper)).Show();
                        break;
                    case '4':
                        new Menu<BankCard>(new BankCardDALEF(_connectionString, _mapper)).Show();
                        break;
                    case 'q':
                        Console.WriteLine("Exiting application...");
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Try again.");
                        break;
                }
            }
        }
    }
}