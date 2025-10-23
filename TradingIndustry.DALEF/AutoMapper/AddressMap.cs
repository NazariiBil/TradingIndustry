using AutoMapper;
using EF = TradingIndustry.DALEF.Models;
using DTO = TradingIndustry.DTO;

namespace TradingIndustry.DALEF.AutoMapper
{
    public class AddressMap : Profile
    {
        public AddressMap()
        {
            CreateMap<EF.Address, DTO.Address>();
            CreateMap<DTO.Address, EF.Address>();
        }
    }
}