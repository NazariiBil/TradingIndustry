using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingIndustry.DTO
{
    public class BankCard
    {
        public long CardId { get; set; } 
        public long UserId { get; set; } 
        public string CardToken { get; set; } = null!;
        public string ExpiryDate { get; set; } = null!; 
        public string CardHolderName { get; set; } = null!;
        public bool IsDefault { get; set; } 

        public override string ToString()
        {
          
            string displayToken = CardToken.Length > 8 ?
                                  $"{CardToken.Substring(0, 4)}...{CardToken.Substring(CardToken.Length - 4)}" :
                                  CardToken;

            return $"{CardId}: User {UserId}, Token: {displayToken}, Exp: {ExpiryDate}, Default: {IsDefault}";
        }
    }
}