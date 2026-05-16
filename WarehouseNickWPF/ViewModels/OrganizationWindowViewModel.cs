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
    public class OrganizationWindowViewModel : INotifyPropertyChanged
    {
        private readonly ApplicationContext _context;
        private readonly IOrganizationService _orgService;   

        public ObservableCollection<Organization> Organizations { get; set; }

        private Organization? _selectedOrganization;
        public Organization? SelectedOrganization
        {
            get => _selectedOrganization;
            set { _selectedOrganization = value; OnPropertyChanged(); }
        }

        public ICommand AddOrganizationCommand { get; }
        public ICommand EditOrganizationCommand { get; }
        public ICommand DeleteOrganizationCommand { get; }
        public ICommand OkCommand { get; }

        public event EventHandler? RequestClose;

        public OrganizationWindowViewModel()
        {
            _context = new ApplicationContext();
            _context.SeedData();
            _orgService = new OrganizationService(_context);   

            Organizations = new ObservableCollection<Organization>(_orgService.GetAll());

            AddOrganizationCommand = new RelayCommand(AddOrganization);
            EditOrganizationCommand = new RelayCommand(EditOrganization, () => SelectedOrganization != null);
            DeleteOrganizationCommand = new RelayCommand(DeleteOrganization, () => SelectedOrganization != null);
            OkCommand = new RelayCommand(Ok);
        }

        private void AddOrganization()
        {
            var dialog = new Dialogs.OrganizationDialog();
            if (dialog.ShowDialog() == true && !string.IsNullOrWhiteSpace(dialog.OrganizationName))
            {
                if (_orgService.ExistsName(dialog.OrganizationName))
                {
                    MessageBox.Show("Организация с таким названием уже существует.", "Ошибка");
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
                    MessageBox.Show("Организация с таким названием уже существует.", "Ошибка");
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

        private void Ok()
        {
            if (SelectedOrganization == null)
            {
                MessageBox.Show("Выберите организацию.", "Предупреждение");
                return;
            }
            var warehouseWindow = new WarehouseWindow(SelectedOrganization);
            warehouseWindow.Owner = Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.IsActive);
            warehouseWindow.ShowDialog();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}