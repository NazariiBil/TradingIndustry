
using System;
using TradingIndustry.DTO; 

namespace TradingIndustry.App.AppMenu
{
    public static class ConsoleHelper
    {
        public static int ReadInt(string prompt)
        {
            int result;
            Console.Write(prompt);
            while (!int.TryParse(Console.ReadLine(), out result) || result <= 0)
            {
                Console.WriteLine("Invalid input. Please enter a positive integer:");
                Console.Write(prompt);
            }
            return result;
        }

        public static string ReadString(string prompt, bool canBeNull = false)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();

            if (canBeNull) return input ?? string.Empty;

            while (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Input cannot be empty. Please enter a value:");
                Console.Write(prompt);
                input = Console.ReadLine();
            }
            return input;
        }

        
        public static T CreateNewEntity<T>() where T : class, new()
        {
            Console.WriteLine($"\n--- Creating new {typeof(T).Name} ---");
            T entity = new T();

        
            var properties = typeof(T).GetProperties();

            foreach (var prop in properties)
            {
                
                if (prop.Name.EndsWith("Id") && (prop.PropertyType == typeof(int) || prop.PropertyType == typeof(long)) && properties[0] == prop)
                {
                    continue;
                }

                
                if (prop.PropertyType == typeof(string) || prop.PropertyType == typeof(int) || prop.PropertyType == typeof(long))
                {
                    bool isNullable = Nullable.GetUnderlyingType(prop.PropertyType) != null || prop.PropertyType == typeof(string);

                    if (prop.PropertyType == typeof(string))
                    {
                        string value = ReadString($"Enter {prop.Name} (string){(isNullable ? " [optional]" : "")}: ", isNullable);
                        if (!string.IsNullOrEmpty(value) || !isNullable)
                        {
                            prop.SetValue(entity, value);
                        }
                    }
                    else if (prop.PropertyType == typeof(int) || prop.PropertyType == typeof(long) || prop.PropertyType == typeof(long?))
                    {
                        if (prop.Name.EndsWith("Id"))
                        {
                            
                            int fkId = ReadInt($"Enter {prop.Name} (Foreign Key ID): ");
                            prop.SetValue(entity, Convert.ChangeType(fkId, prop.PropertyType));
                        }
                        
                    }
                }
            }
            return entity;
        }
    }
}