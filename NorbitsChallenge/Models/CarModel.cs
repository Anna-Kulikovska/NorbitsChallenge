using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NorbitsChallenge.Models
{
    public class CarModel
    {
        public int companyId { get; set; }
        public string licensePlate { get; set; }
        public string model { get; set; }
        public string brand { get; set; }
        public string description { get; set; }
        public int TireCount { get; set; }
    }
}
