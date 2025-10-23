
using System;
using TradingIndustry.App.Interfaces;
using TradingIndustry.DAL.Interfaces;

namespace TradingIndustry.App.AppMenu
{
    public class Menu<T> where T : class, new()
    {
        private readonly ICommand[] _commands;

        public Menu(IGenericDAL<T> dal)
        {
            _commands = new ICommand[]
            {
                new GetAllCommand<T>(dal),
                new GetByIdCommand<T>(dal),
                new InsertCommand<T>(dal),
                new UpdateCommand<T>(dal),
                new DeleteByIdCommand<T>(dal)
            };
        }

        public void Show()
        {
            char choice = ' ';
            while (choice != 'q' && choice != 'Q')
            {
                Console.WriteLine($"\n--- {typeof(T).Name} CRUD Menu ---");
                for (int i = 0; i < _commands.Length; i++)
                {
                    Console.WriteLine($"{i + 1} - {_commands[i].Description}");
                }
                Console.WriteLine("q - Back to Main Menu");

                string input = Console.ReadLine()?.ToLower() ?? "";
                if (string.IsNullOrEmpty(input)) continue;

                choice = input[0];

                if (int.TryParse(input, out int numChoice) && numChoice >= 1 && numChoice <= _commands.Length)
                {
                    _commands[numChoice - 1].Execute();
                }
                else if (choice != 'q')
                {
                    Console.WriteLine("Invalid choice. Try again.");
                }
            }
            Console.Clear();
        }
    }
}