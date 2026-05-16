using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using WarehouseNickData.Models;

namespace WarehouseNickWPF.Dialogs
{
    public class InvoiceItemEntry : INotifyPropertyChanged
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}