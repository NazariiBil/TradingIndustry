
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using TradingIndustry.DAL.Concrete;
using TradingIndustry.DAL.Interfaces;
using TradingIndustry.DALEF.Concrete.ctx;
using Microsoft.EntityFrameworkCore;
using BankCardDTO = TradingIndustry.DTO.BankCard;
using BankCardModel = TradingIndustry.DALEF.Models.BankCard;

namespace TradingIndustry.DALEF.Concrete
{
    public class BankCardDALEF : GenericDAL<BankCardDTO>, IBankCardDAL
    {
        public BankCardDALEF(string connStr, IMapper mapper) : base(connStr, mapper) { }

        public override BankCardDTO Create(BankCardDTO entity)
        {
            using var ctx = new TradingIndustryContext(_connStr);
            try
            {
                var model = _mapper.Map<BankCardModel>(entity);
                ctx.BankCards.Add(model);
                ctx.SaveChanges();
                entity.CardId = model.CardId;
                Console.WriteLine($"[CREATE] BankCard created with ID: {entity.CardId}");
                return entity;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Error creating BankCard: {ex.Message}");
                return null;
            }
        }

        public override List<BankCardDTO> GetAll()
        {
            using var ctx = new TradingIndustryContext(_connStr);
            try
            {
                
                var models = ctx.BankCards.Include(bc => bc.User).OrderBy(bc => bc.CardId).ToList();
                Console.WriteLine($"[GET ALL] Retrieved {models.Count} bank cards (with User).");
                return _mapper.Map<List<BankCardDTO>>(models);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Error retrieving BankCards: {ex.Message}");
                return new List<BankCardDTO>();
            }
        }

        public override BankCardDTO GetById(int id)
        {
            long longId = id;
            using var ctx = new TradingIndustryContext(_connStr);
            try
            {
                var model = ctx.BankCards
                    .Include(bc => bc.User)
                    .FirstOrDefault(bc => bc.CardId == longId);

                Console.WriteLine(model != null ? $"[GET BY ID] Retrieved card {longId}." : $"[GET BY ID] Card {longId} not found.");
                return _mapper.Map<BankCardDTO>(model);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Error retrieving BankCard by Id: {ex.Message}");
                return null;
            }
        }

        public override BankCardDTO Update(BankCardDTO entity)
        {
            using var ctx = new TradingIndustryContext(_connStr);
            try
            {
                var existing = ctx.BankCards.Find(entity.CardId);
                if (existing == null) throw new Exception("BankCard not found");

                _mapper.Map(entity, existing);
                ctx.SaveChanges();
                Console.WriteLine($"[UPDATE] BankCard {entity.CardId} updated.");
                return _mapper.Map<BankCardDTO>(existing);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Error updating BankCard: {ex.Message}");
                return null;
            }
        }

        public override bool Delete(int id)
        {
            long longId = id;
            using var ctx = new TradingIndustryContext(_connStr);
            try
            {
                var model = ctx.BankCards.Find(longId);
                if (model == null) return false;

                ctx.BankCards.Remove(model);
                ctx.SaveChanges();
                Console.WriteLine($"[DELETE] BankCard {longId} deleted.");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Error deleting BankCard: {ex.Message}");
                return false;
            }
        }
    }
}