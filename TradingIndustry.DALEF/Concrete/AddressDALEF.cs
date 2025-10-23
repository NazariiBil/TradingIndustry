
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using TradingIndustry.DAL.Concrete;
using TradingIndustry.DAL.Interfaces;
using TradingIndustry.DALEF.Concrete.ctx;
using Microsoft.EntityFrameworkCore;
using AddressDTO = TradingIndustry.DTO.Address;
using AddressModel = TradingIndustry.DALEF.Models.Address;

namespace TradingIndustry.DALEF.Concrete
{
    public class AddressDALEF : GenericDAL<AddressDTO>, IAddressDAL
    {
        public AddressDALEF(string connStr, IMapper mapper) : base(connStr, mapper) { }

        public override AddressDTO Create(AddressDTO entity)
        {
            using var ctx = new TradingIndustryContext(_connStr);
            try
            {
                var model = _mapper.Map<AddressModel>(entity);
                ctx.Addresses.Add(model);
                ctx.SaveChanges();
                entity.AddressId = model.AddressId;
                Console.WriteLine($"[CREATE] Address created with ID: {entity.AddressId}");
                return entity;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Error creating Address: {ex.Message}");
                return null;
            }
        }

        public override List<AddressDTO> GetAll()
        {
            using var ctx = new TradingIndustryContext(_connStr);
            try
            {
                var models = ctx.Addresses.OrderBy(a => a.AddressId).ToList();
                Console.WriteLine($"[GET ALL] Retrieved {models.Count} addresses.");
                return _mapper.Map<List<AddressDTO>>(models);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Error retrieving Addresses: {ex.Message}");
                return new List<AddressDTO>();
            }
        }

        public override AddressDTO GetById(int id)
        {
            
            long longId = id;
            using var ctx = new TradingIndustryContext(_connStr);
            try
            {
                var model = ctx.Addresses.Find(longId);
                Console.WriteLine(model != null ? $"[GET BY ID] Retrieved address {longId}." : $"[GET BY ID] Address {longId} not found.");
                return _mapper.Map<AddressDTO>(model);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Error retrieving Address by Id: {ex.Message}");
                return null;
            }
        }

        public override AddressDTO Update(AddressDTO entity)
        {
            using var ctx = new TradingIndustryContext(_connStr);
            try
            {
                var existing = ctx.Addresses.Find(entity.AddressId);
                if (existing == null) throw new Exception("Address not found");

               
                _mapper.Map(entity, existing);

                ctx.SaveChanges();
                Console.WriteLine($"[UPDATE] Address {entity.AddressId} updated.");
                return _mapper.Map<AddressDTO>(existing);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Error updating Address: {ex.Message}");
                return null;
            }
        }

        public override bool Delete(int id)
        {
            long longId = id;
            using var ctx = new TradingIndustryContext(_connStr);
            try
            {
                var model = ctx.Addresses.Find(longId);
                if (model == null) return false;

                ctx.Addresses.Remove(model);
                ctx.SaveChanges();
                Console.WriteLine($"[DELETE] Address {longId} deleted.");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Error deleting Address: {ex.Message}");
                return false;
            }
        }
    }
}