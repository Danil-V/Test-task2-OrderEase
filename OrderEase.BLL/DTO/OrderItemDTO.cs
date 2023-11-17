using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderEase.BLL.DTO
{
    public class OrderItemDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public decimal? Quantity { get; set; }
        public string? Unit { get; set; }
        public int OrderId { get; set; }
    }
}
