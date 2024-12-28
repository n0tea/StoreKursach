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
        public Guid Uid { get; init; } //= Guid.NewGuid();
        public long UserId { get; set; } // Связь с User
        public decimal TotalPrice { get; set; }
        public DateTimeOffset CreationTimestamp { get; set; }
    }
}
