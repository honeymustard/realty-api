using System;

namespace Honeymustard
{
    public class RealtyModel
    {
        public string ID { get; set; }
        public string ImageUri { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public int SquareMeters { get; set; }
        public int Price { get; set; }
        public int SharedDept { get; set; }
        public int SharedExpenses { get; set; }
        public DateTime Added { get; set; }
    }
}