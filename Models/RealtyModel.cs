using System;
using System.ComponentModel.DataAnnotations;

namespace Honeymustard
{
    public class RealtyModel
    {
        [Required]
        public string RealtyId { get; set; }

        public string ImageUri { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string Description { get; set; }
        public int SquareMeters { get; set; }
        public int Price { get; set; }
        public int SharedDept { get; set; }
        public int SharedExpenses { get; set; }
        public DateTime Added { get; set; }
    }
}