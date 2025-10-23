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
    public class BankCardDALEFTests
    {
        private string _testConnectionString;
        private IMapper _mapper;
        private BankCardDALEF _dal;
        private BankCard _tempCard;

        private const int EXISTING_USER_ID = 1;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            _testConnectionString = config.GetConnectionString("TestConnection");

            var configExpression = new MapperConfigurationExpression();
            configExpression.AddProfile<BankCardMap>();
            var mapperConfig = new MapperConfiguration(configExpression, NullLoggerFactory.Instance);
            _mapper = mapperConfig.CreateMapper();

            _dal = new BankCardDALEF(_testConnectionString, _mapper);

            _tempCard = new BankCard
            {
                UserId = EXISTING_USER_ID,
                CardToken = $"tok_test_{Guid.NewGuid().ToString().Substring(0, 8)}",
                ExpiryDate = "12/99",
                CardHolderName = "TEST HOLDER",
                IsDefault = false
            };
            _tempCard = _dal.Create(_tempCard);
            Assert.IsNotNull(_tempCard);
        }

        [Test, Order(1)]
        public void GetAllBankCards_ReturnsCards()
        {
            var list = _dal.GetAll();
            Assert.IsNotNull(list);
            Assert.IsTrue(list.Count > 0);
        }

        [Test, Order(2)]
        public void GetBankCardById_ReturnsCard()
        {
            var card = _dal.GetById((int)_tempCard.CardId);
            Assert.IsNotNull(card);
            Assert.AreEqual(_tempCard.CardId, card.CardId);
        }

        [Test, Order(3)]
        public void UpdateBankCard_WorksCorrectly()
        {
            _tempCard.IsDefault = true;
            var updated = _dal.Update(_tempCard);

            Assert.IsNotNull(updated);
            Assert.IsTrue(updated.IsDefault);
        }

        [OneTimeTearDown]
        public void CleanUp()
        {
            if (_tempCard != null && _tempCard.CardId > 0)
            {
                _dal.Delete((int)_tempCard.CardId);
            }
        }
    }
}