using AutoMapper;
using EF = TradingIndustry.DALEF.Models;
using DTO = TradingIndustry.DTO;

namespace TradingIndustry.DALEF.AutoMapper
{
    public class UserMap : Profile
    {
        public UserMap()
        {
            CreateMap<EF.User, DTO.User>();
            CreateMap<DTO.User, EF.User>();
        }
    }
}