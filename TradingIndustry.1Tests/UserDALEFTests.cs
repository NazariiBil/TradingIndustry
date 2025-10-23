using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;
using System;
using System.Linq;
using System.Reflection; 
using TradingIndustry.DALEF.AutoMapper;
using TradingIndustry.DALEF.Concrete;
using TradingIndustry.DTO;

namespace TradingIndustry.Tests.DALEF
{
    public class UserDALEFTests : IDisposable
    {
        private readonly UserDALEF _dal;
        private readonly User _tempUser;

        private const int EXISTING_ROLE_ID = 2;
        private const long EXISTING_ADDRESS_ID = 1; 
        private const long EXISTING_USER_ID = 1; 

        public UserDALEFTests()
        {
            var basePath = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;

            var config = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var configExpression = new MapperConfigurationExpression();
            configExpression.AddProfile<UserMap>();
            var mapper = new MapperConfiguration(configExpression, NullLoggerFactory.Instance).CreateMapper();

            _dal = new UserDALEF(config.GetConnectionString("TestConnection"), mapper);

         
            _tempUser = _dal.Create(new User
            {
                RoleId = EXISTING_ROLE_ID,
                AddressId = EXISTING_ADDRESS_ID,
                Login = $"test_login_{Guid.NewGuid().ToString().Substring(0, 8)}",
                Email = $"test_{Guid.NewGuid()}@temp.com",
                PhoneNumber = "+1234567890",
                PasswordHash = "test_hash",
                FirstName = "Test",
                LastName = "User",
                Gender = "Male",
                RegistrationDate = DateTime.Now
            });
            Xunit.Assert.NotNull(_tempUser);
        }

        public void Dispose()
        {
            if (_tempUser != null && _tempUser.UserId > 0)
            {
                _dal.Delete((int)_tempUser.UserId);
            }
            GC.SuppressFinalize(this);
        }

        [Fact]
        public void A_GetAllUsers_ShouldReturnUsers()
        {
            var list = _dal.GetAll();
            Xunit.Assert.NotNull(list);
            Xunit.Assert.True(list.Count >= 10);
        }

        [Fact]
        public void B_UpdateUser_ShouldWorkCorrectly()
        {
            _tempUser.FirstName = "FirstName_Updated_FINAL";
            var updated = _dal.Update(_tempUser);

            Xunit.Assert.NotNull(updated);
            Xunit.Assert.Equal("FirstName_Updated_FINAL", updated.FirstName);
        }
    }
}