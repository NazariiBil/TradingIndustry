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
    public class BankCardDALEFTests : IDisposable
    {
        private readonly BankCardDALEF _dal;
        private readonly BankCard _tempCard;

        private const long EXISTING_USER_ID = 1; 

        
        public BankCardDALEFTests()
        {
            var basePath = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;

            var config = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var configExpression = new MapperConfigurationExpression();
            configExpression.AddProfile<BankCardMap>();
            var mapper = new MapperConfiguration(configExpression, NullLoggerFactory.Instance).CreateMapper();

            _dal = new BankCardDALEF(config.GetConnectionString("TestConnection"), mapper);

            _tempCard = _dal.Create(new BankCard
            {
                UserId = EXISTING_USER_ID,
                CardToken = $"tok_test_{Guid.NewGuid().ToString().Substring(0, 8)}",
                ExpiryDate = "12/99",
                CardHolderName = "TEST HOLDER",
                IsDefault = false
            });
            Xunit.Assert.NotNull(_tempCard);
        }

        public void Dispose()
        {
            if (_tempCard != null && _tempCard.CardId > 0)
            {
                _dal.Delete((int)_tempCard.CardId);
            }
            GC.SuppressFinalize(this);
        }

        [Fact]
        public void A_GetAllBankCards_ShouldReturnCards()
        {
            var list = _dal.GetAll();
            Xunit.Assert.NotNull(list);
            Xunit.Assert.True(list.Count > 0);
        }

        [Fact]
        public void B_UpdateBankCard_ShouldWorkCorrectly()
        {
            _tempCard.IsDefault = true;
            var updated = _dal.Update(_tempCard);

            Xunit.Assert.NotNull(updated);
            Xunit.Assert.True(updated.IsDefault);
        }
    }
}