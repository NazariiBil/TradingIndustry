
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using TradingIndustry.DAL.Concrete;
using TradingIndustry.DAL.Interfaces;
using TradingIndustry.DALEF.Concrete.ctx;
using Microsoft.EntityFrameworkCore;
using RoleDTO = TradingIndustry.DTO.Role;
using RoleModel = TradingIndustry.DALEF.Models.Role;

namespace TradingIndustry.DALEF.Concrete
{
    public class RoleDALEF : GenericDAL<RoleDTO>, IRoleDAL
    {
        public RoleDALEF(string connStr, IMapper mapper) : base(connStr, mapper) { }

        public override RoleDTO Create(RoleDTO entity)
        {
            using var ctx = new TradingIndustryContext(_connStr);
            try
            {
                var model = _mapper.Map<RoleModel>(entity);
                ctx.Roles.Add(model);
                ctx.SaveChanges();
                entity.RoleId = model.RoleId;
                Console.WriteLine($"[CREATE] Role created with ID: {entity.RoleId}");
                return entity;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Error creating Role: {ex.Message}");
                return null;
            }
        }

        public override List<RoleDTO> GetAll()
        {
            using var ctx = new TradingIndustryContext(_connStr);
            try
            {
                var models = ctx.Roles.OrderBy(r => r.RoleId).ToList();
                Console.WriteLine($"[GET ALL] Retrieved {models.Count} roles.");
                return _mapper.Map<List<RoleDTO>>(models);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Error retrieving Roles: {ex.Message}");
                return new List<RoleDTO>();
            }
        }

        public override RoleDTO GetById(int id)
        {
            using var ctx = new TradingIndustryContext(_connStr);
            try
            {
                var model = ctx.Roles.Find(id);
                Console.WriteLine(model != null ? $"[GET BY ID] Retrieved role {id}." : $"[GET BY ID] Role {id} not found.");
                return _mapper.Map<RoleDTO>(model);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Error retrieving Role by Id: {ex.Message}");
                return null;
            }
        }

        public override RoleDTO Update(RoleDTO entity)
        {
            using var ctx = new TradingIndustryContext(_connStr);
            try
            {
                var existing = ctx.Roles.Find(entity.RoleId);
                if (existing == null) throw new Exception("Role not found");

                _mapper.Map(entity, existing);
                ctx.SaveChanges();
                Console.WriteLine($"[UPDATE] Role {entity.RoleId} updated.");
                return _mapper.Map<RoleDTO>(existing);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Error updating Role: {ex.Message}");
                return null;
            }
        }

        public override bool Delete(int id)
        {
            using var ctx = new TradingIndustryContext(_connStr);
            try
            {
                var model = ctx.Roles.Find(id);
                if (model == null) return false;

                ctx.Roles.Remove(model);
                ctx.SaveChanges();
                Console.WriteLine($"[DELETE] Role {id} deleted.");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Error deleting Role: {ex.Message}");
                
                return false;
            }
        }
    }
}