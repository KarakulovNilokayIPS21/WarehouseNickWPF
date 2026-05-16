using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WarehouseNickData.Models;

namespace WarehouseNickWPF.Dialogs
{
    public partial class InvoiceDialog : Window, INotifyPropertyChanged
    {
        public string Number => NumberTextBox.Text;
        public DateTime Date => DatePicker.SelectedDate ?? DateTime.Now;
        public InvoiceType Type => ((ComboBoxItem)TypeComboBox.SelectedItem)?.Tag.ToString() == "Incoming" ? InvoiceType.Incoming : InvoiceType.Outgoing;

        public ObservableCollection<InvoiceItemEntry> Items { get; set; }
        public List<Product> AvailableProducts { get; set; }

        public ObservableCollection<InvoiceItemEntry> ItemsSource => Items;

        public InvoiceDialog(int warehouseId, List<Product> availableProducts, Invoice? invoice = null)
        {
            InitializeComponent();
            DataContext = this;
            AvailableProducts = availableProducts;
            Items = new ObservableCollection<InvoiceItemEntry>();

            if (invoice != null)
            {
                NumberTextBox.Text = invoice.Number;
                DatePicker.SelectedDate = invoice.Date;
                TypeComboBox.SelectedIndex = invoice.Type == InvoiceType.Incoming ? 0 : 1;
                foreach (var item in invoice.Items)
                {
                    var product = AvailableProducts.FirstOrDefault(p => p.Id == item.ProductId);
                    if (product != null)
                    {
                        Items.Add(new InvoiceItemEntry
                        {
                            Product = product,
                            Quantity = item.Quantity,
                            Price = item.PriceAtTime
                        });
                    }
                }
            }

            ItemsDataGrid.ItemsSource = Items;
        }

        public Invoice GetInvoice()
        {
            var invoice = new Invoice
            {
                Number = Number,
                Date = Date,
                Type = Type,
                WarehouseId = 0,
                Status = InvoiceStatus.Draft,
                Items = Items
                    .Where(item => item.Product != null)
                    .Select(item => new InvoiceItem
                    {
                        ProductId = item.Product.Id,
                        Quantity = item.Quantity,
                        PriceAtTime = item.Price
                    }).ToList()
            };
            return invoice;
        }


        private void RemoveItem_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var item = btn?.Tag as InvoiceItemEntry;
            if (item != null) Items.Remove(item);
        }


        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Number))
            {
                MessageBox.Show("Введите номер накладной.", "Ошибка");
                return;
            }
            if (Items.Count == 0)
            {
                MessageBox.Show("Добавьте хотя бы одну позицию товара.", "Ошибка");
                return;
            }
            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}