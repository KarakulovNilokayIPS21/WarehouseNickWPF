using System;
using System.Collections.Generic;
using System.Text;

namespace WarehouseNickData.Models
{
    public class InvoiceItem
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal PriceAtTime { get; set; }
    }
}
