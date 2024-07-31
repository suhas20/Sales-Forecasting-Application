using System.ComponentModel.DataAnnotations;

namespace Sales_Forecasting_Application.Model
{
    public class Orders
    {
        [Key]
        public string OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string ShipDate { get; set; }
        public string ShipMode { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string Segment { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public float PostalCode { get; set; }
        public string Region { get; set; }

    }
}
