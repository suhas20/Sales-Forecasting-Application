using System.ComponentModel.DataAnnotations;

namespace Sales_Forecasting_Application.Model
{
    public class OrderReturns
    {
        [Key]
        public string OrderId { get; set; }
        public string Comments { get; set; }

    }
}
