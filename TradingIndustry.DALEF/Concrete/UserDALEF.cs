
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using TradingIndustry.DAL.Concrete;
using TradingIndustry.DAL.Interfaces;
using TradingIndustry.DALEF.Concrete.ctx;
using Microsoft.EntityFrameworkCore;
using UserDTO = TradingIndustry.DTO.User;
using UserModel = TradingIndustry.DALEF.Models.User;

namespace TradingIndustry.DALEF.Concrete
{
    public class UserDALEF : GenericDAL<UserDTO>, IUserDAL
    {
        public UserDALEF(string connStr, IMapper mapper) : base(connStr, mapper) { }

        public override UserDTO Create(UserDTO entity)
        {
            using var ctx = new TradingIndustryContext(_connStr);
            try
            {
                var model = _mapper.Map<UserModel>(entity);
                ctx.Users.Add(model);
                ctx.SaveChanges();
                entity.UserId = model.UserId;
                Console.WriteLine($"[CREATE] User created with ID: {entity.UserId}");
                return entity;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Error creating User: {ex.Message}");
                return null;
            }
        }

        public override List<UserDTO> GetAll()
        {
            using var ctx = new TradingIndustryContext(_connStr);
            try
            {
                
                var models = ctx.Users
                    .Include(u => u.Role)
                    .Include(u => u.Address)
                    .OrderBy(u => u.UserId)
                    .ToList();
                Console.WriteLine($"[GET ALL] Retrieved {models.Count} users (with Role/Address).");
                return _mapper.Map<List<UserDTO>>(models);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Error retrieving Users: {ex.Message}");
                return new List<UserDTO>();
            }
        }

        public override UserDTO GetById(int id)
        {
            long longId = id;
            using var ctx = new TradingIndustryContext(_connStr);
            try
            {
                var model = ctx.Users
                    .Include(u => u.Role)
                    .Include(u => u.Address)
                    .FirstOrDefault(u => u.UserId == longId); 

                Console.WriteLine(model != null ? $"[GET BY ID] Retrieved user {longId}." : $"[GET BY ID] User {longId} not found.");
                return _mapper.Map<UserDTO>(model);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Error retrieving User by Id: {ex.Message}");
                return null;
            }
        }

        public override UserDTO Update(UserDTO entity)
        {
            using var ctx = new TradingIndustryContext(_connStr);
            try
            {
                var existing = ctx.Users.Find(entity.UserId);
                if (existing == null) throw new Exception("User not found");

                _mapper.Map(entity, existing);
                ctx.SaveChanges();
                Console.WriteLine($"[UPDATE] User {entity.UserId} updated.");
                return _mapper.Map<UserDTO>(existing);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Error updating User: {ex.Message}");
                return null;
            }
        }

        public override bool Delete(int id)
        {
            long longId = id;
            using var ctx = new TradingIndustryContext(_connStr);
            try
            {
                var model = ctx.Users.Find(longId);
                if (model == null) return false;

                ctx.Users.Remove(model);
                ctx.SaveChanges();
                Console.WriteLine($"[DELETE] User {longId} deleted.");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Error deleting User: {ex.Message}");
                
                return false;
            }
        }
    }
}