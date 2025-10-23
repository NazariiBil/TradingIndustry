using AutoMapper;
using EF = TradingIndustry.DALEF.Models;
using DTO = TradingIndustry.DTO;

namespace TradingIndustry.DALEF.AutoMapper
{
    public class BankCardMap : Profile
    {
        public BankCardMap()
        {
            CreateMap<EF.BankCard, DTO.BankCard>();
            CreateMap<DTO.BankCard, EF.BankCard>();
        }
    }
}