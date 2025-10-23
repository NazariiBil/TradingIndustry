
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using TradingIndustry.DALEF.AutoMapper;
using TradingIndustry.App.AppMenu;

namespace TradingIndustry.App
{
    internal class Program
    {
        static void Main(string[] args)
        {
            
            var configExpression = new MapperConfigurationExpression();
            configExpression.AddProfile<RoleMap>();
            configExpression.AddProfile<AddressMap>();
            configExpression.AddProfile<UserMap>();
            configExpression.AddProfile<BankCardMap>();

            var loggerFactory = NullLoggerFactory.Instance;
            var mapperConfig = new MapperConfiguration(configExpression, loggerFactory);
            IMapper mapper = mapperConfig.CreateMapper();

           
            var config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            
            string connectionString = config.GetConnectionString("DefaultConnection");

            if (string.IsNullOrEmpty(connectionString) || connectionString.Contains("ВАШ"))
            {
                Console.WriteLine("ERROR: Connection string is not set up correctly in appsettings.json.");
                Console.WriteLine("Please update 'DefaultConnection' before running.");
                return;
            }

            
            new AppMenuService(connectionString, mapper).Show();
        }
    }
}