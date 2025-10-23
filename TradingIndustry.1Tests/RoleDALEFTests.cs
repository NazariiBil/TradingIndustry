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
    public class RoleDALEFTests : IDisposable
    {
        private readonly RoleDALEF _dal;
        private readonly Role _tempRole;
        private readonly IMapper _mapper;

       
        public RoleDALEFTests()
        {
            var basePath = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;

            var config = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var configExpression = new MapperConfigurationExpression();
            configExpression.AddProfile<RoleMap>();
            _mapper = new MapperConfiguration(configExpression, NullLoggerFactory.Instance).CreateMapper();

            _dal = new RoleDALEF(config.GetConnectionString("TestConnection"), _mapper);

            _tempRole = _dal.Create(new Role { RoleName = $"Test_Role_{Guid.NewGuid()}" });
            Xunit.Assert.NotNull(_tempRole);
        }

        public void Dispose()
        {
            if (_tempRole != null && _tempRole.RoleId > 0)
            {
                _dal.Delete(_tempRole.RoleId);
            }
            GC.SuppressFinalize(this);
        }

        [Fact]
        public void A_GetAllRoles_ShouldReturnRoles()
        {
            var list = _dal.GetAll();
            Xunit.Assert.NotNull(list);
            Xunit.Assert.True(list.Count > 0);
        }

        [Fact]
        public void C_UpdateRole_ShouldWorkCorrectly()
        {
            _tempRole.RoleName = "Role_Updated_Test_FINAL";
            var updated = _dal.Update(_tempRole);

            Xunit.Assert.NotNull(updated);
            Xunit.Assert.Equal("Role_Updated_Test_FINAL", updated.RoleName);
        }
    }
}