using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcessLayer.Entities
{
    public class OrderItem
    {
        public long Id { get; init; }
        public Guid Uid { get; init; }
        public long OrderId { get; set; } 
        public long ProductId { get; set; } // внешняя связь из друго бд будет
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        public Order Order { get; set; }
    }
}
