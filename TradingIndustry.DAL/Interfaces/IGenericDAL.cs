using System.Collections.Generic;

namespace TradingIndustry.DAL.Interfaces
{
    public interface IGenericDAL<T> where T : class
    {
        T Create(T entity);
        List<T> GetAll();
        T GetById(int id);
        T Update(T entity);
        bool Delete(int id);
    }
}