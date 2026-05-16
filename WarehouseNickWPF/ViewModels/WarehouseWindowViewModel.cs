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
    public class WarehouseWindowViewModel : INotifyPropertyChanged
    {
        private readonly ApplicationContext _context;
        private readonly IWarehouseService _warehouseService;   
        private readonly Organization _organization;

        public string OrganizationName => _organization.Name;
        public ObservableCollection<Warehouse> Warehouses { get; set; }

        private Warehouse? _selectedWarehouse;
        public Warehouse? SelectedWarehouse
        {
            get => _selectedWarehouse;
            set { _selectedWarehouse = value; OnPropertyChanged(); }
        }

        public ICommand AddWarehouseCommand { get; }
        public ICommand EditWarehouseCommand { get; }
        public ICommand DeleteWarehouseCommand { get; }
        public ICommand ImportProductsCommand { get; }
        public ICommand OkCommand { get; }

        public event EventHandler? RequestClose;

        public WarehouseWindowViewModel(Organization organization)
        {
            _organization = organization;
            _context = new ApplicationContext();
            _context.SeedData();   
            _warehouseService = new WarehouseService(_context);

            Warehouses = new ObservableCollection<Warehouse>(_warehouseService.GetByOrganization(_organization.Id));

            AddWarehouseCommand = new RelayCommand(AddWarehouse);
            EditWarehouseCommand = new RelayCommand(EditWarehouse, () => SelectedWarehouse != null);
            DeleteWarehouseCommand = new RelayCommand(DeleteWarehouse, () => SelectedWarehouse != null);
            ImportProductsCommand = new RelayCommand(ImportProducts, () => SelectedWarehouse != null);
            OkCommand = new RelayCommand(Ok);
        }

        private void AddWarehouse()
        {
            var dialog = new Dialogs.WarehouseDialog();
            if (dialog.ShowDialog() == true && !string.IsNullOrWhiteSpace(dialog.WarehouseName))
            {
                var newWh = new Warehouse
                {
                    Name = dialog.WarehouseName,
                    OrganizationId = _organization.Id
                };
                _warehouseService.Add(newWh);
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
                _warehouseService.Update(SelectedWarehouse);
                var index = Warehouses.IndexOf(SelectedWarehouse);
                Warehouses[index] = SelectedWarehouse;
            }
        }

        private void DeleteWarehouse()
        {
            if (SelectedWarehouse == null) return;
            if (MessageBox.Show($"Удалить склад \"{SelectedWarehouse.Name}\"?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _warehouseService.Delete(SelectedWarehouse.Id);
                Warehouses.Remove(SelectedWarehouse);
                SelectedWarehouse = null;
            }
        }

        private void ImportProducts()
        {
            if (SelectedWarehouse == null) return;
            var dialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*",
                Title = "Выберите CSV файл для импорта товаров"
            };
            if (dialog.ShowDialog() == true)
            {
                try
                {
                    // Временно заглушка – импорт реализую позже
                    MessageBox.Show("Импорт товаров будет реализован в следующей версии.", "Информация");
                    // int importedCount = _importService.ImportFromCsv(dialog.FileName, SelectedWarehouse.Id);
                    // MessageBox.Show($"Импортировано {importedCount} товаров.", "Импорт завершён");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка импорта: {ex.Message}", "Ошибка");
                }
            }
        }

        private void Ok()
        {
            if (SelectedWarehouse == null)
            {
                MessageBox.Show("Выберите склад.", "Предупреждение");
                return;
            }
            var productWindow = new ProductWindow(SelectedWarehouse);
            productWindow.Owner = Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.IsActive);
            productWindow.ShowDialog();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}