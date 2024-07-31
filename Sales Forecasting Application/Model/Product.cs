using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Sales_Forecasting_Application.Model
{
    [Keyless]
    public class Product
    {
        public string OrderId { get; set; }
        public string ProductId { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public string ProductName { get; set; }
        public float Sales { get; set; }
        public float Quantity { get; set; }
        public float Discount { get; set; }
        public float Profit { get; set; }

        public Orders Order { get; set; }
        public OrderReturns Returns { get; set; }
    }
}
