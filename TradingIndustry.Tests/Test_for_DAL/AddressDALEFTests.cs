// TradingIndustry.Tests/Test_for_DAL/AddressDALEFTests.cs

using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework; // Залишаємо лише для атрибутів
using System;
using System.Linq;
using TradingIndustry.DALEF.AutoMapper;
using TradingIndustry.DALEF.Concrete;
using TradingIndustry.DTO;

namespace TradingIndustry.Tests.DALEF
{
    // Використовуємо повний шлях для NUnit атрибутів
    [NUnit.Framework.TestFixture]
    public class AddressDALEFTests
    {
        private string _testConnectionString;
        private IMapper _mapper;
        private AddressDALEF _dal;
        private TradingIndustry.DTO.Address _tempAddress; // Використовуємо повну назву DTO

        [NUnit.Framework.OneTimeSetUp]
        public void OneTimeSetup()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            _testConnectionString = config.GetConnectionString("TestConnection");

            var configExpression = new MapperConfigurationExpression();
            configExpression.AddProfile<AddressMap>();
            var mapperConfig = new MapperConfiguration(configExpression, NullLoggerFactory.Instance);
            _mapper = mapperConfig.CreateMapper();

            _dal = new AddressDALEF(_testConnectionString, _mapper);

            _tempAddress = _dal.Create(new TradingIndustry.DTO.Address { Country = "Temp", City = "TestCity", Street = "TestStreet", HouseNumber = "1", ZipCode = "000" });

            // Використовуємо повний префікс Assert.
            NUnit.Framework.Assert.IsNotNull(_tempAddress);
        }

        [NUnit.Framework.Test, NUnit.Framework.Order(1)]
        public void GetAllAddresses_ReturnsAddresses()
        {
            var list = _dal.GetAll();
            NUnit.Framework.Assert.IsNotNull(list);
            NUnit.Framework.Assert.IsTrue(list.Count > 0);
        }

        [NUnit.Framework.Test, NUnit.Framework.Order(2)]
        public void GetAddressById_ReturnsAddress()
        {
            var address = _dal.GetById((int)_tempAddress.AddressId);
            NUnit.Framework.Assert.IsNotNull(address);
            NUnit.Framework.Assert.AreEqual("TestCity", address.City);
        }

        [NUnit.Framework.Test, NUnit.Framework.Order(3)]
        public void UpdateAddress_WorksCorrectly()
        {
            _tempAddress.City = "City_Updated_Test";
            var updated = _dal.Update(_tempAddress);

            NUnit.Framework.Assert.IsNotNull(updated);
            NUnit.Framework.Assert.AreEqual("City_Updated_Test", updated.City);
        }

        [NUnit.Framework.OneTimeTearDown]
        public void CleanUp()
        {
            if (_tempAddress != null && _tempAddress.AddressId > 0)
            {
                _dal.Delete((int)_tempAddress.AddressId);
            }
        }
    }
}