using System;
using System.Collections.Generic;
using System.Text;

namespace WarehouseNickData.Models
{
    public enum InvoiceType { Incoming, Outgoing }
    public enum InvoiceStatus { Draft, Confirmed }

    public class Invoice
    {
        public int Id { get; set; }
        public string Number { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public InvoiceType Type { get; set; }
        public int WarehouseId { get; set; }
        public InvoiceStatus Status { get; set; }
        public List<InvoiceItem> Items { get; set; } = new();  
    }
}