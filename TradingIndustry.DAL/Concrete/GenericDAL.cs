﻿using AutoMapper;
using System.Collections.Generic;
using TradingIndustry.DAL.Interfaces;

namespace TradingIndustry.DAL.Concrete
{
    public abstract class GenericDAL<T> : IGenericDAL<T> where T : class
    {
        protected readonly IMapper _mapper;
        protected readonly string _connStr;

        protected GenericDAL(string connStr, IMapper mapper)
        {
            _connStr = connStr;
            _mapper = mapper;
        }

        public abstract T Create(T entity);
        public abstract List<T> GetAll();
        public abstract T GetById(int id);
        public abstract T Update(T entity);
        public abstract bool Delete(int id);
    }
}