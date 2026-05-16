using System.Windows;
using WarehouseNickData.Models;
using WarehouseNickWPF.ViewModels;

namespace WarehouseNickWPF
{
    public partial class ProductWindow : Window
    {
        public ProductWindow(Warehouse warehouse)
        {
            InitializeComponent();
            DataContext = new ProductWindowViewModel(warehouse);
        }
    }
}