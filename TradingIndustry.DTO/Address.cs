using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingIndustry.DTO
{
    public class Address
    {
        public long AddressId { get; set; } 
        public string Country { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Street { get; set; } = null!;
        public string HouseNumber { get; set; } = null!;
        public string? ApartmentNumber { get; set; } 
        public string? ZipCode { get; set; } 

        public override string ToString()
        {
            return $"{AddressId}: {Country}, {City}, {Street}, {HouseNumber}{(ApartmentNumber != null ? $", Apt. {ApartmentNumber}" : "")}, ZIP: {ZipCode}";
        }
    }
}