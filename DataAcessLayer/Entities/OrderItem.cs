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
        public Guid Uid { get; init; } //= Guid.NewGuid();
        public long OrderId { get; set; } // Связь с Order
        public long ProductId { get; set; } // Связь с Product
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
