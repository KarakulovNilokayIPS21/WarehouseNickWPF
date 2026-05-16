using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Microsoft.Win32;
using WarehouseNickData.Models;

namespace WarehouseNickWPF.Dialogs
{
    public partial class ProductDialog : Window
    {
        public string ProductName => NameTextBox.Text;
        public int SelectedCategoryId => CategoryComboBox.SelectedValue as int? ?? 0;
        public int SelectedManufacturerId => ManufacturerComboBox.SelectedValue as int? ?? 0;
        public int SelectedSupplierId => SupplierComboBox.SelectedValue as int? ?? 0;
        
        public decimal Price => decimal.TryParse(PriceTextBox.Text, out var p) ? p : 0;
        public int Stock => int.TryParse(StockTextBox.Text, out var s) ? s : 0;
        public int Discount => int.TryParse(DiscountTextBox.Text, out var d) ? d : 0;
        public string ImagePath => ImagePathTextBox.Text;

        public ProductDialog(IEnumerable<Category> categories, IEnumerable<Manufacturer> manufacturers, IEnumerable<Supplier> suppliers, Product? product = null)
        {
            InitializeComponent();

            MessageBox.Show($"Категорий: {categories.Count()}, Производителей: {manufacturers.Count()}, Поставщиков: {suppliers.Count()}");

            CategoryComboBox.ItemsSource = categories.ToList();
            ManufacturerComboBox.ItemsSource = manufacturers.ToList();
            SupplierComboBox.ItemsSource = suppliers.ToList();

       
            InitializeComponent();
            CategoryComboBox.ItemsSource = categories.ToList();
            ManufacturerComboBox.ItemsSource = manufacturers.ToList();
            SupplierComboBox.ItemsSource = suppliers.ToList();

            if (product != null)
            {
                NameTextBox.Text = product.Name;
                CategoryComboBox.SelectedValue = product.CategoryId;
                ManufacturerComboBox.SelectedValue = product.ManufacturerId;
                SupplierComboBox.SelectedValue = product.SupplierId;
                PriceTextBox.Text = product.Price.ToString();
                StockTextBox.Text = product.Stock.ToString();
                DiscountTextBox.Text = product.Discount.ToString();
            }
        }

        private void BrowseImage_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog { Filter = "Image files (*.png;*.jpg)|*.png;*.jpg|All files (*.*)|*.*" };
            if (dialog.ShowDialog() == true)
                ImagePathTextBox.Text = dialog.FileName;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ProductName))
            {
                MessageBox.Show("Введите наименование товара.", "Ошибка");
                return;
            }
            if (SelectedCategoryId == 0 || SelectedManufacturerId == 0 || SelectedSupplierId == 0)
            {
                MessageBox.Show("Выберите категорию, производителя и поставщика.", "Ошибка");
                return;
            }
            if (Price <= 0)
            {
                MessageBox.Show("Цена должна быть больше нуля.", "Ошибка");
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
    }
}