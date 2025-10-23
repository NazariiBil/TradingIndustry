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
    public class RoleDALEFTests
    {
        private string _testConnectionString;
        private IMapper _mapper;
        private RoleDALEF _dal;
        private Role _tempRole = new Role { RoleName = "Test_Role_Temp" };

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            _testConnectionString = config.GetConnectionString("TestConnection");

            var configExpression = new MapperConfigurationExpression();
            configExpression.AddProfile<RoleMap>();
            var mapperConfig = new MapperConfiguration(configExpression, NullLoggerFactory.Instance);
            _mapper = mapperConfig.CreateMapper();

            _dal = new RoleDALEF(_testConnectionString, _mapper);

            var existing = _dal.GetAll().FirstOrDefault(r => r.RoleName == _tempRole.RoleName);
            if (existing == null)
            {
                _tempRole = _dal.Create(_tempRole);
            }
            else
            {
                _tempRole = existing;
            }
        }

        [Test, Order(1)]
        public void GetAllRoles_ReturnsRoles()
        {
            var list = _dal.GetAll();
            Assert.IsNotNull(list);
            Assert.IsTrue(list.Count >= 3);
        }

        [Test, Order(2)]
        public void GetRoleById_ReturnsRole()
        {
            var role = _dal.GetById(3);
            Assert.IsNotNull(role);
            Assert.AreEqual(3, role.RoleId);
            Assert.AreEqual("Admin", role.RoleName);
        }

        [Test, Order(3)]
        public void UpdateRole_WorksCorrectly()
        {
            if (_tempRole.RoleId == 0) Assert.Inconclusive("Temp role not created.");

            string originalName = _tempRole.RoleName;
            _tempRole.RoleName = "Role_Updated_Test";
            var updated = _dal.Update(_tempRole);

            Assert.IsNotNull(updated);
            Assert.AreEqual("Role_Updated_Test", updated.RoleName);

            _tempRole.RoleName = originalName;
            _dal.Update(_tempRole);
        }

        [Test, Order(4)]
        public void DeleteRole_WorksCorrectly()
        {
            var roleToDelete = _dal.Create(new Role { RoleName = "Role_to_Delete" });
            Assert.IsNotNull(roleToDelete);

            var deleted = _dal.Delete(roleToDelete.RoleId);
            Assert.IsTrue(deleted);
            Assert.IsNull(_dal.GetById(roleToDelete.RoleId));
        }
    }
}