using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework; // Потрібен для атрибутів [TestFixture], [Test]
using System;
using System.Linq;
using TradingIndustry.DALEF.AutoMapper;
using TradingIndustry.DALEF.Concrete;
using TradingIndustry.DTO;

namespace TradingIndustry.Tests.DALEF
{
    [TestFixture]
    public class UserDALEFTests
    {
        private string _testConnectionString;
        private IMapper _mapper;
        private UserDALEF _dal;
        private User _tempUser;

        private const int EXISTING_ROLE_ID = 2;
        private const int EXISTING_ADDRESS_ID = 1;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            _testConnectionString = config.GetConnectionString("TestConnection");

            var configExpression = new MapperConfigurationExpression();
            configExpression.AddProfile<UserMap>();
            var mapperConfig = new MapperConfiguration(configExpression, NullLoggerFactory.Instance);
            _mapper = mapperConfig.CreateMapper();

            _dal = new UserDALEF(_testConnectionString, _mapper);

            _tempUser = new User
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
            };
            _tempUser = _dal.Create(_tempUser);
            Assert.IsNotNull(_tempUser);
        }

        [Test, Order(1)]
        public void GetAllUsers_ReturnsUsers()
        {
            var list = _dal.GetAll();
            Assert.IsNotNull(list);
            Assert.IsTrue(list.Count >= 10);
        }

        [Test, Order(2)]
        public void GetUserById_ReturnsUser()
        {
            var user = _dal.GetById((int)_tempUser.UserId);
            Assert.IsNotNull(user);
            Assert.AreEqual(_tempUser.UserId, user.UserId);
        }

        [Test, Order(3)]
        public void UpdateUser_WorksCorrectly()
        {
            _tempUser.FirstName = "FirstName_Updated_Test";
            var updated = _dal.Update(_tempUser);

            Assert.IsNotNull(updated);
            Assert.AreEqual("FirstName_Updated_Test", updated.FirstName);
        }

        [OneTimeTearDown]
        public void CleanUp()
        {
            if (_tempUser != null && _tempUser.UserId > 0)
            {
                _dal.Delete((int)_tempUser.UserId);
            }
        }
    }
}