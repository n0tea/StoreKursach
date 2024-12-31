using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcessLayer.Entities
{
    public class Order
    {
        public long Id { get; init; }
        public Guid Uid { get; init; }
        public long UserId { get; set; } // Внешний ID из UserDbContext
        public decimal TotalPrice { get; set; }
        public DateTimeOffset CreationTimestamp { get; set; }

        // Навигационное свойство
        public ICollection<OrderItem> OrderItems { get; set; } = new HashSet<OrderItem>();
    }
}
