using System;
using System.Collections.Generic;
using System.Text;

namespace WarehouseNickData.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public int ManufacturerId { get; set; }
        public int SupplierId { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public int Discount { get; set; }

    }
}