using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using Sales_Forecasting_Application.Model;
using Sales_Forecasting_Application.Model.DataContext;
using Sales_Forecasting_Application.Model.DTO_s;
using System.Text;

namespace Sales_Forecasting_Application.Pages
{
    public class IndexModel : PageModel
    {
        private readonly SalesDBContext _context;

        public IndexModel(SalesDBContext context)
        {
            _context = context;
        }

        public List<OrdersDTO> Sales { get; set; }
        public int selcted_Year { get; set; }
        public decimal TotalSales { get; set; }
        public decimal Increment { get; set; }
        public List<OrdersDTO> Incsales { get; set; }
        [TempData]
        public string TempIncData { get; set; }
        [TempData]
        public string TempIncrement { get; set; }

        public async Task OnPostAsync(int year,decimal increment)
        {
            selcted_Year = year;
            var totalSalesData = await _context.Products
                .Where(p => p.Order.OrderDate.Year == year)
                .GroupBy(p => p.Order.State)
                .Select(g => new
                {
                    State = g.Key,
                    TotalSales = g.Sum(p => p.Sales)
                }).ToListAsync();

            var returnSalesData = await (from p in _context.Products
                                         join r in _context.OrdersReturns on p.OrderId equals r.OrderId
                                         join o in _context.Orders on r.OrderId equals o.OrderId
                                         where o.OrderDate.Year == year
                                         group p by o.State into g
                                         select new
                                         {
                                             State = g.Key,
                                             ReturnSales = g.Sum(p => p.Sales)
                                         }).ToListAsync();

            var salesData = totalSalesData
                .Select(ts => new OrdersDTO
                {
                    State = ts.State,
                    Orders = (decimal)(ts.TotalSales - (returnSalesData.FirstOrDefault(rs => rs.State == ts.State)?.ReturnSales ?? 0))
                }).OrderBy(s => s.State).ToList();

            Sales = salesData;
            TotalSales = salesData.Sum(s => s.Orders);

            if (increment != 0)
            {
                Increment = increment;
                TempIncrement = JsonConvert.SerializeObject(increment);
                Incsales = Sales.Select(s => new OrdersDTO
                {
                    State = s.State,
                    Orders = s.Orders,
                    IncrementedSales = s.Orders * (1 + increment / 100)
                }).ToList();

                TempIncData = JsonConvert.SerializeObject(Incsales);
            }

            
        }

        public IActionResult OnPostDownload()
        {
            Increment = JsonConvert.DeserializeObject<decimal>(TempIncrement);
            Incsales = JsonConvert.DeserializeObject<List<OrdersDTO>>(TempIncData);
            var csv = new StringBuilder();
            csv.AppendLine("State,Percentage Increase,Increased Sales");


            foreach(var sales in Incsales)
            {
                csv.AppendLine($"{sales.State},{Increment},{sales.IncrementedSales}");
            }

            return File(Encoding.UTF8.GetBytes(csv.ToString()), "text/csv", "Forecasted_Sales_Data.csv");
        }
    }
}