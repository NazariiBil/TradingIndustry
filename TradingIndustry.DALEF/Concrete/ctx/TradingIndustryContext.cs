
using Microsoft.EntityFrameworkCore;
using TradingIndustry.DALEF.Data;

namespace TradingIndustry.DALEF.Concrete.ctx
{
    
    public class TradingIndustryContext : TradIndustryCtx
    {
        private readonly string _connStr;

        
        public TradingIndustryContext(string connStr) : base()
        {
            _connStr = connStr;
        }

        
        public TradingIndustryContext() : base() { }

        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
            if (!optionsBuilder.IsConfigured && !string.IsNullOrEmpty(_connStr))
            {
                optionsBuilder.UseSqlServer(_connStr);
            }

            
            base.OnConfiguring(optionsBuilder);
        }
    }
}