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
    public class AddressDALEFTests : IDisposable
    {
        private readonly AddressDALEF _dal;
        private readonly Address _tempAddress;
        private readonly IMapper _mapper;

        
        public AddressDALEFTests()
        {
            var basePath = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;

            var config = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var configExpression = new MapperConfigurationExpression();
            configExpression.AddProfile<AddressMap>();
            _mapper = new MapperConfiguration(configExpression, NullLoggerFactory.Instance).CreateMapper();

            _dal = new AddressDALEF(config.GetConnectionString("TestConnection"), _mapper);

            _tempAddress = _dal.Create(new Address { Country = "Temp", City = "TestCity", Street = "TestStreet", HouseNumber = "1", ZipCode = "000" });
            Xunit.Assert.NotNull(_tempAddress);
        }

        public void Dispose()
        {
            if (_tempAddress != null && _tempAddress.AddressId > 0)
            {
                _dal.Delete((int)_tempAddress.AddressId);
            }
            GC.SuppressFinalize(this);
        }

        [Fact]
        public void A_GetAllAddresses_ShouldReturnAddresses()
        {
            var list = _dal.GetAll();
            Xunit.Assert.NotNull(list);
            Xunit.Assert.True(list.Count > 0);
        }

        [Fact]
        public void B_UpdateAddress_ShouldWorkCorrectly()
        {
            _tempAddress.City = "City_Updated_Test_FINAL";
            var updated = _dal.Update(_tempAddress);

            Xunit.Assert.NotNull(updated);
            Xunit.Assert.Equal("City_Updated_Test_FINAL", updated.City);
        }
    }
}