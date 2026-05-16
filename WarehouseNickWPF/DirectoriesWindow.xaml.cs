using System.Collections.ObjectModel;
using System.Windows;
using WarehouseNickData.Models;
using WarehouseNickData.Services;

namespace WarehouseNickWPF
{
    public partial class DirectoriesWindow : Window
    {
        private readonly ICategoryService _categoryService;
        private readonly IManufacturerService _manufacturerService;
        private readonly ISupplierService _supplierService;

        public ObservableCollection<Category> Categories { get; set; }
        public ObservableCollection<Manufacturer> Manufacturers { get; set; }
        public ObservableCollection<Supplier> Suppliers { get; set; }

        public DirectoriesWindow(ICategoryService categoryService, IManufacturerService manufacturerService, ISupplierService supplierService)
        {
            InitializeComponent();
            _categoryService = categoryService;
            _manufacturerService = manufacturerService;
            _supplierService = supplierService;

            Categories = new ObservableCollection<Category>(_categoryService.GetAll());
            Manufacturers = new ObservableCollection<Manufacturer>(_manufacturerService.GetAll());
            Suppliers = new ObservableCollection<Supplier>(_supplierService.GetAll());

            CategoriesListView.ItemsSource = Categories;
            ManufacturersListView.ItemsSource = Manufacturers;
            SuppliersListView.ItemsSource = Suppliers;
        }

        
        private void AddCategory_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Dialogs.SimpleNameDialog("Введите название категории");
            if (dialog.ShowDialog() == true && !string.IsNullOrWhiteSpace(dialog.EnteredName))
            {
                var newCat = new Category { Name = dialog.EnteredName };
                _categoryService.Add(newCat);
                Categories.Add(newCat);
            }
        }

        private void EditCategory_Click(object sender, RoutedEventArgs e)
        {
            var selected = CategoriesListView.SelectedItem as Category;
            if (selected == null) return;
            var dialog = new Dialogs.SimpleNameDialog("Измените название", selected.Name);
            if (dialog.ShowDialog() == true && !string.IsNullOrWhiteSpace(dialog.EnteredName))
            {
                selected.Name = dialog.EnteredName;
                _categoryService.Update(selected);
                CategoriesListView.Items.Refresh();
            }
        }

        private void DeleteCategory_Click(object sender, RoutedEventArgs e)
        {
            var selected = CategoriesListView.SelectedItem as Category;
            if (selected == null) return;
            if (MessageBox.Show($"Удалить категорию \"{selected.Name}\"?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _categoryService.Delete(selected.Id);
                Categories.Remove(selected);
            }
        }

        
        private void AddManufacturer_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Dialogs.SimpleNameDialog("Введите название производителя");
            if (dialog.ShowDialog() == true && !string.IsNullOrWhiteSpace(dialog.EnteredName))
            {
                var newMan = new Manufacturer { Name = dialog.EnteredName };
                _manufacturerService.Add(newMan);
                Manufacturers.Add(newMan);
            }
        }

        private void EditManufacturer_Click(object sender, RoutedEventArgs e)
        {
            var selected = ManufacturersListView.SelectedItem as Manufacturer;
            if (selected == null) return;
            var dialog = new Dialogs.SimpleNameDialog("Измените название", selected.Name);
            if (dialog.ShowDialog() == true && !string.IsNullOrWhiteSpace(dialog.EnteredName))
            {
                selected.Name = dialog.EnteredName;
                _manufacturerService.Update(selected);
                ManufacturersListView.Items.Refresh();
            }
        }

        private void DeleteManufacturer_Click(object sender, RoutedEventArgs e)
        {
            var selected = ManufacturersListView.SelectedItem as Manufacturer;
            if (selected == null) return;
            if (MessageBox.Show($"Удалить производителя \"{selected.Name}\"?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _manufacturerService.Delete(selected.Id);
                Manufacturers.Remove(selected);
            }
        }

        
        private void AddSupplier_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Dialogs.SimpleNameDialog("Введите название поставщика");
            if (dialog.ShowDialog() == true && !string.IsNullOrWhiteSpace(dialog.EnteredName))
            {
                var newSup = new Supplier { Name = dialog.EnteredName };
                _supplierService.Add(newSup);
                Suppliers.Add(newSup);
            }
        }

        private void EditSupplier_Click(object sender, RoutedEventArgs e)
        {
            var selected = SuppliersListView.SelectedItem as Supplier;
            if (selected == null) return;
            var dialog = new Dialogs.SimpleNameDialog("Измените название", selected.Name);
            if (dialog.ShowDialog() == true && !string.IsNullOrWhiteSpace(dialog.EnteredName))
            {
                selected.Name = dialog.EnteredName;
                _supplierService.Update(selected);
                SuppliersListView.Items.Refresh();
            }
        }

        private void DeleteSupplier_Click(object sender, RoutedEventArgs e)
        {
            var selected = SuppliersListView.SelectedItem as Supplier;
            if (selected == null) return;
            if (MessageBox.Show($"Удалить поставщика \"{selected.Name}\"?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _supplierService.Delete(selected.Id);
                Suppliers.Remove(selected);
            }
        }
    }
}