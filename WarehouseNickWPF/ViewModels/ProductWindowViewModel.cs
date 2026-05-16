using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using WarehouseNickData.Context;
using WarehouseNickData.Models;
using WarehouseNickData.Services;

namespace WarehouseNickWPF.ViewModels
{

    public class ProductWindowViewModel : INotifyPropertyChanged
    {
        private readonly ApplicationContext _context;
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IManufacturerService _manufacturerService;
        private readonly ISupplierService _supplierService;
        private readonly IInvoiceService _invoiceService;
        private readonly Warehouse _warehouse;
        


        public string WarehouseName => _warehouse.Name;
        public ObservableCollection<ProductViewModel> Products { get; set; }

        private ProductViewModel? _selectedProduct;
        public ProductViewModel? SelectedProduct
        {
            get => _selectedProduct;
            set { _selectedProduct = value; OnPropertyChanged(); }
        }

        public ICommand AddProductCommand { get; }
        public ICommand EditProductCommand { get; }
        public ICommand DeleteProductCommand { get; }
        public ICommand ImportProductsCommand { get; }
        public ICommand OpenInvoicesCommand { get; }
        public ICommand OpenDirectoriesCommand { get; }

        public ProductWindowViewModel(Warehouse warehouse)
        {
            _warehouse = warehouse;
            _context = new ApplicationContext();
            _context.SeedData(); 

            _productService = new ProductService(_context);
            _categoryService = new CategoryService(_context);
            _manufacturerService = new ManufacturerService(_context);
            _supplierService = new SupplierService(_context);
            _invoiceService = new InvoiceService(_context, _productService);

            Products = new ObservableCollection<ProductViewModel>();
            LoadProducts();

            AddProductCommand = new RelayCommand(AddProduct);
            EditProductCommand = new RelayCommand(EditProduct, () => SelectedProduct != null);
            DeleteProductCommand = new RelayCommand(DeleteProduct, () => SelectedProduct != null);
            ImportProductsCommand = new RelayCommand(ImportProducts);
            OpenInvoicesCommand = new RelayCommand(OpenInvoices);
            OpenDirectoriesCommand = new RelayCommand(OpenDirectories);
        }

        private void LoadProducts()
        {
            Products.Clear();
            
            foreach (var p in _productService.GetAll())
            {
                var vm = new ProductViewModel(p, _categoryService, _manufacturerService, _supplierService);
                Products.Add(vm);
            }
        }

        private void AddProduct()
        {
            var dialog = new Dialogs.ProductDialog(
                _categoryService.GetAll(),
                _manufacturerService.GetAll(),
                _supplierService.GetAll());
            if (dialog.ShowDialog() == true)
            {
                var newProduct = new Product
                {
                    Name = dialog.ProductName,
                    CategoryId = dialog.SelectedCategoryId,
                    ManufacturerId = dialog.SelectedManufacturerId,
                    SupplierId = dialog.SelectedSupplierId,
                    Price = dialog.Price,
                    Stock = dialog.Stock,
                    Discount = dialog.Discount,
                    
                };
                _productService.Add(newProduct, _warehouse.Id);
                LoadProducts();
            }
        }

        private void EditProduct()
        {
            if (SelectedProduct == null) return;
            var prod = SelectedProduct.Product;
            var dialog = new Dialogs.ProductDialog(
                _categoryService.GetAll(),
                _manufacturerService.GetAll(),
                _supplierService.GetAll(),
                prod);
            if (dialog.ShowDialog() == true)
            {
                prod.Name = dialog.ProductName;
                prod.CategoryId = dialog.SelectedCategoryId;
                prod.ManufacturerId = dialog.SelectedManufacturerId;
                prod.SupplierId = dialog.SelectedSupplierId;
                prod.Price = dialog.Price;
                prod.Stock = dialog.Stock;
                prod.Discount = dialog.Discount;
                _productService.Update(prod);
                LoadProducts();
            }
        }

        private void DeleteProduct()
        {
            if (SelectedProduct == null) return;
            if (MessageBox.Show($"Удалить товар \"{SelectedProduct.Product.Name}\"?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _productService.Delete(SelectedProduct.Product.Id);
                LoadProducts();
            }
        }

        private void ImportProducts()
        {
            var dialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*",
                Title = "Выберите CSV файл для импорта товаров"
            };
            if (dialog.ShowDialog() == true)
            {
                try
                {
                    
                    MessageBox.Show("Импорт пока не реализован", "Информация");
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}");
                }
            }
        }

        private void OpenInvoices()
        {
            var invoicesWindow = new InvoicesWindow(_invoiceService, _productService, _warehouse.Id);
            invoicesWindow.Owner = Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.IsActive);
            invoicesWindow.ShowDialog();
            LoadProducts();
        }

        private void OpenDirectories()
        {
            
            var dirWindow = new DirectoriesWindow(_categoryService, _manufacturerService, _supplierService);
            dirWindow.Owner = Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.IsActive);
            dirWindow.ShowDialog();
            LoadProducts(); 
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}