using System;
using System.Collections.Generic;
using System.Text;

namespace WarehouseNickData.Models
{
    public class Warehouse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int OrganizationId { get; set; }
    }
}