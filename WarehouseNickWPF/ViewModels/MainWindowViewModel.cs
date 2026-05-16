using System;
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
    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool>? _canExecute;

        public RelayCommand(Action execute, Func<bool>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public event EventHandler? CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public bool CanExecute(object? parameter) => _canExecute == null || _canExecute();
        public void Execute(object? parameter) => _execute();
    }

    public class ProductViewModel : INotifyPropertyChanged
    {
        public Product Product { get; }
        private readonly ICategoryService _categoryService;
        private readonly IManufacturerService _manufacturerService;
        private readonly ISupplierService _supplierService;

        private string _categoryName = string.Empty;
        public string CategoryName
        {
            get => _categoryName;
            private set { _categoryName = value; OnPropertyChanged(); }
        }

        private string _manufacturerName = string.Empty;
        public string ManufacturerName
        {
            get => _manufacturerName;
            private set { _manufacturerName = value; OnPropertyChanged(); }
        }

        private string _supplierName = string.Empty;
        public string SupplierName
        {
            get => _supplierName;
            private set { _supplierName = value; OnPropertyChanged(); }
        }

        public ProductViewModel(Product product, ICategoryService categoryService, IManufacturerService manufacturerService, ISupplierService supplierService)
        {
            Product = product;
            _categoryService = categoryService;
            _manufacturerService = manufacturerService;
            _supplierService = supplierService;
            UpdateNames();
        }

        public void UpdateNames()
        {
            CategoryName = _categoryService.GetById(Product.CategoryId)?.Name ?? string.Empty;
            ManufacturerName = _manufacturerService.GetById(Product.ManufacturerId)?.Name ?? string.Empty;
            SupplierName = _supplierService.GetById(Product.SupplierId)?.Name ?? string.Empty;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private readonly ApplicationContext _context;
        private readonly IOrganizationService _orgService;
        private readonly IWarehouseService _whService;
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IManufacturerService _manufacturerService;
        private readonly ISupplierService _supplierService;
        private readonly IInvoiceService _invoiceService;

        public ObservableCollection<Organization> Organizations { get; set; }
        public ObservableCollection<Warehouse> Warehouses { get; set; }
        public ObservableCollection<ProductViewModel> Products { get; set; }

        private Organization? _selectedOrganization;
        public Organization? SelectedOrganization
        {
            get => _selectedOrganization;
            set
            {
                _selectedOrganization = value;
                LoadWarehouses();
                OnPropertyChanged();
            }
        }

        private Warehouse? _selectedWarehouse;
        public Warehouse? SelectedWarehouse
        {
            get => _selectedWarehouse;
            set
            {
                _selectedWarehouse = value;
                LoadProducts();
                OnPropertyChanged();
            }
        }

        private ProductViewModel? _selectedProduct;
        public ProductViewModel? SelectedProduct
        {
            get => _selectedProduct;
            set { _selectedProduct = value; OnPropertyChanged(); }
        }

        public ICommand AddOrganizationCommand { get; }
        public ICommand EditOrganizationCommand { get; }
        public ICommand DeleteOrganizationCommand { get; }
        public ICommand AddWarehouseCommand { get; }
        public ICommand EditWarehouseCommand { get; }
        public ICommand DeleteWarehouseCommand { get; }
        public ICommand AddProductCommand { get; }
        public ICommand EditProductCommand { get; }
        public ICommand DeleteProductCommand { get; }
        public ICommand ImportProductsCommand { get; }
        public ICommand OpenInvoicesCommand { get; }

        public MainWindowViewModel()
        {
            _context = new ApplicationContext();
            _context.SeedData();

            _orgService = new OrganizationService(_context);
            _whService = new WarehouseService(_context);
            _productService = new ProductService(_context);
            _categoryService = new CategoryService(_context);
            _manufacturerService = new ManufacturerService(_context);
            _supplierService = new SupplierService(_context);
            _invoiceService = new InvoiceService(_context, _productService);

            Organizations = new ObservableCollection<Organization>(_orgService.GetAll());
            Warehouses = new ObservableCollection<Warehouse>();
            Products = new ObservableCollection<ProductViewModel>();

            AddOrganizationCommand = new RelayCommand(AddOrganization);
            EditOrganizationCommand = new RelayCommand(EditOrganization, () => SelectedOrganization != null);
            DeleteOrganizationCommand = new RelayCommand(DeleteOrganization, () => SelectedOrganization != null);
            AddWarehouseCommand = new RelayCommand(AddWarehouse, () => SelectedOrganization != null);
            EditWarehouseCommand = new RelayCommand(EditWarehouse, () => SelectedWarehouse != null);
            DeleteWarehouseCommand = new RelayCommand(DeleteWarehouse, () => SelectedWarehouse != null);
            AddProductCommand = new RelayCommand(AddProduct, () => SelectedWarehouse != null);
            EditProductCommand = new RelayCommand(EditProduct, () => SelectedWarehouse != null && SelectedProduct != null);
            DeleteProductCommand = new RelayCommand(DeleteProduct, () => SelectedWarehouse != null && SelectedProduct != null);
            ImportProductsCommand = new RelayCommand(ImportProducts, () => SelectedWarehouse != null);
            OpenInvoicesCommand = new RelayCommand(OpenInvoices, () => SelectedWarehouse != null);
        }

        private void LoadWarehouses()
        {
            Warehouses.Clear();
            if (SelectedOrganization != null)
            {
                foreach (var wh in _whService.GetByOrganization(SelectedOrganization.Id))
                    Warehouses.Add(wh);
            }
        }

        private void LoadProducts()
        {
            Products.Clear();
            if (SelectedWarehouse != null)
            {
                
                foreach (var p in _productService.GetAll())
                {
                    var vm = new ProductViewModel(p, _categoryService, _manufacturerService, _supplierService);
                    Products.Add(vm);
                }
            }
        }

        private void AddOrganization()
        {
            var dialog = new Dialogs.OrganizationDialog();
            if (dialog.ShowDialog() == true && !string.IsNullOrWhiteSpace(dialog.OrganizationName))
            {
                if (_orgService.ExistsName(dialog.OrganizationName))
                {
                    MessageBox.Show("Организация с таким названием уже существует", "Ошибка");
                    return;
                }
                var newOrg = new Organization { Name = dialog.OrganizationName };
                _orgService.Add(newOrg);
                Organizations.Add(newOrg);
            }
        }

        private void EditOrganization()
        {
            if (SelectedOrganization == null) return;
            var dialog = new Dialogs.OrganizationDialog(SelectedOrganization.Name);
            if (dialog.ShowDialog() == true && !string.IsNullOrWhiteSpace(dialog.OrganizationName))
            {
                if (_orgService.ExistsName(dialog.OrganizationName, SelectedOrganization.Id))
                {
                    MessageBox.Show("Организация с таким названием уже существует", "Ошибка");
                    return;
                }
                SelectedOrganization.Name = dialog.OrganizationName;
                _orgService.Update(SelectedOrganization);
                var index = Organizations.IndexOf(SelectedOrganization);
                Organizations[index] = SelectedOrganization;
            }
        }

        private void DeleteOrganization()
        {
            if (SelectedOrganization == null) return;
            var confirm = new Dialogs.ConfirmDeleteDialog(SelectedOrganization.Name);
            if (confirm.ShowDialog() == true)
            {
                _orgService.Delete(SelectedOrganization.Id);
                Organizations.Remove(SelectedOrganization);
                SelectedOrganization = null;
            }
        }

        private void AddWarehouse()
        {
            if (SelectedOrganization == null) return;
            var dialog = new Dialogs.WarehouseDialog();
            if (dialog.ShowDialog() == true && !string.IsNullOrWhiteSpace(dialog.WarehouseName))
            {
                var newWh = new Warehouse
                {
                    Name = dialog.WarehouseName,
                    OrganizationId = SelectedOrganization.Id
                };
                _whService.Add(newWh);
                Warehouses.Add(newWh);
            }
        }

        private void EditWarehouse()
        {
            if (SelectedWarehouse == null) return;
            var dialog = new Dialogs.WarehouseDialog(SelectedWarehouse.Name);
            if (dialog.ShowDialog() == true && !string.IsNullOrWhiteSpace(dialog.WarehouseName))
            {
                SelectedWarehouse.Name = dialog.WarehouseName;
                _whService.Update(SelectedWarehouse);
                var index = Warehouses.IndexOf(SelectedWarehouse);
                Warehouses[index] = SelectedWarehouse;
            }
        }

        private void DeleteWarehouse()
        {
            if (SelectedWarehouse == null) return;
            if (MessageBox.Show($"Удалить склад \"{SelectedWarehouse.Name}\"?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                _whService.Delete(SelectedWarehouse.Id);
                Warehouses.Remove(SelectedWarehouse);
                SelectedWarehouse = null;
            }
        }

        private void AddProduct()
        {
            if (SelectedWarehouse == null) return;
            MessageBox.Show("Диалог добавления товара ещё не реализован.", "Информация");
        }

        private void EditProduct()
        {
            if (SelectedWarehouse == null || SelectedProduct == null) return;
            MessageBox.Show("Диалог редактирования товара ещё не реализован.", "Информация");
        }

        private void DeleteProduct()
        {
            if (SelectedWarehouse == null || SelectedProduct == null) return;
            if (MessageBox.Show($"Удалить товар \"{SelectedProduct.Product.Name}\"?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                _productService.Delete(SelectedProduct.Product.Id);
                LoadProducts(); 
            }
        }

        private void ImportProducts()
        {
            if (SelectedWarehouse == null) return;
            MessageBox.Show("Функция импорта товаров ещё не реализована.", "Информация");
        }

        private void OpenInvoices()
        {
            if (SelectedWarehouse == null) return;
            MessageBox.Show("Окно накладных ещё не реализовано.", "Информация");
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}