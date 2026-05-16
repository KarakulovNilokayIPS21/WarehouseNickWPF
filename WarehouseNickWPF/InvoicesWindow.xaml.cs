using System.Windows;
using WarehouseNickData.Services;
using WarehouseNickWPF.ViewModels;

namespace WarehouseNickWPF
{

    public partial class InvoicesWindow : Window
    {
        public InvoicesWindow(IInvoiceService invoiceService, IProductService productService, int warehouseId)
        {
            InitializeComponent();
            DataContext = new InvoicesViewModel(invoiceService, productService, warehouseId);
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
