using AutoMapper;
using EF = TradingIndustry.DALEF.Models;
using DTO = TradingIndustry.DTO;

namespace TradingIndustry.DALEF.AutoMapper
{
    public class RoleMap : Profile
    {
        public RoleMap()
        {
            CreateMap<EF.Role, DTO.Role>();
            CreateMap<DTO.Role, EF.Role>();
        }
    }
}