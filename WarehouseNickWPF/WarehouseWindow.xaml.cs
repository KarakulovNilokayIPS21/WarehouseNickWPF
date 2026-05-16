using System.Windows;
using WarehouseNickData.Models;
using WarehouseNickWPF.ViewModels;

namespace WarehouseNickWPF
{
    public partial class WarehouseWindow : Window
    {
        public WarehouseWindow(Organization organization)
        {
            InitializeComponent();
            var vm = new WarehouseWindowViewModel(organization);
            vm.RequestClose += (s, e) => DialogResult = true;
            DataContext = vm;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}