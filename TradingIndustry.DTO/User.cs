using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingIndustry.DTO
{
    public class User
    {
        public long UserId { get; set; } 
        public int RoleId { get; set; }
        public long? AddressId { get; set; } 
        public string Login { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string? PasswordResetKey { get; set; } 
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? Gender { get; set; } 
        public DateTime RegistrationDate { get; set; }

        public override string ToString()
        {
            return $"{UserId}: {FirstName} {LastName} ({Login}), Email: {Email}, RoleId: {RoleId}, Reg: {RegistrationDate.ToShortDateString()}";
        }
    }
}