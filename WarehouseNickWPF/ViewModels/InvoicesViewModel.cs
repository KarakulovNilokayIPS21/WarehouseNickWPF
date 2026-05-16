using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using WarehouseNickData.Models;
using WarehouseNickData.Services;

namespace WarehouseNickWPF.ViewModels
{
    public class InvoicesViewModel : INotifyPropertyChanged
    {
        private readonly IInvoiceService _invoiceService;
        private readonly int _warehouseId;
        public ObservableCollection<Invoice> Invoices { get; set; }
        private Invoice? _selectedInvoice;
        public Invoice? SelectedInvoice
        {
            get => _selectedInvoice;
            set { _selectedInvoice = value; OnPropertyChanged(nameof(SelectedInvoice)); }
        }

        public ICommand CreateInvoiceCommand { get; }
        public ICommand EditInvoiceCommand { get; }
        public ICommand DeleteInvoiceCommand { get; }
        public ICommand ConfirmInvoiceCommand { get; }

        public InvoicesViewModel(IInvoiceService invoiceService, IProductService productService, int warehouseId)
        {
            _invoiceService = invoiceService;
            _productService = productService;    
            _warehouseId = warehouseId;
            Invoices = new ObservableCollection<Invoice>(_invoiceService.GetAll().Where(i => i.WarehouseId == warehouseId));
            CreateInvoiceCommand = new RelayCommand(CreateInvoice);
            EditInvoiceCommand = new RelayCommand(EditInvoice, () => SelectedInvoice != null);
            DeleteInvoiceCommand = new RelayCommand(DeleteInvoice, () => SelectedInvoice != null);
            ConfirmInvoiceCommand = new RelayCommand(ConfirmInvoice, () => SelectedInvoice != null && SelectedInvoice.Status == InvoiceStatus.Draft);
        }
        private readonly IProductService _productService;

        private void CreateInvoice()
        {
            var products = _productService.GetByWarehouse(_warehouseId).ToList();
            var dialog = new Dialogs.InvoiceDialog(_warehouseId, products);
            if (dialog.ShowDialog() == true)
            {
                var invoice = dialog.GetInvoice();
                invoice.WarehouseId = _warehouseId;
                _invoiceService.Add(invoice);
                Invoices.Add(invoice);
            }
        }


        private void EditInvoice()
        {
            if (SelectedInvoice == null) return;
            var products = _productService.GetByWarehouse(_warehouseId).ToList();
            var dialog = new Dialogs.InvoiceDialog(_warehouseId, products, SelectedInvoice);
            if (dialog.ShowDialog() == true)
            {
                var updated = dialog.GetInvoice();
                updated.WarehouseId = _warehouseId;
                _invoiceService.Update(updated);
                var index = Invoices.IndexOf(SelectedInvoice);
                Invoices[index] = updated;
            }
        }

        private void DeleteInvoice()
        {
            if (SelectedInvoice == null) return;
            _invoiceService.Delete(SelectedInvoice.Id);
            Invoices.Remove(SelectedInvoice);
        }

        private void ConfirmInvoice()
        {
            if (SelectedInvoice == null) return;
            try
            {
                _invoiceService.ConfirmInvoice(SelectedInvoice.Id);
                MessageBox.Show("Накладная подтверждена. Остатки товаров обновлены.", "Успех");
                var idx = Invoices.IndexOf(SelectedInvoice);
                Invoices[idx] = _invoiceService.GetById(SelectedInvoice.Id)!;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}