using System;
using System.Collections.Generic;
using System.Text;

namespace WarehouseNickData.Models
{
    public class WarehouseProduct
    {
        public int Id { get; set; }
        public int WarehouseId { get; set; }
        public int ProductId { get; set; }
        public int Stock { get; set; }        
        public decimal Price { get; set; }    
        public int Discount { get; set; }     
    }
}
