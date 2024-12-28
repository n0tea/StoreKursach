using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcessLayer.Entities
{
    public class Product
    {
        public long Id { get; init; }
        public Guid Uid { get; init; } //= Guid.NewGuid();
        public string Name { get; set; }
        public string? Description { get; set; }
        public int Quantity { get; set; }
        public string? Size { get; set; }
    }
}
