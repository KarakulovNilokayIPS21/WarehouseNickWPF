using System.Windows;
using WarehouseNickWPF.ViewModels;

namespace WarehouseNickWPF
{
    public partial class OrganizationWindow : Window
    {
        public OrganizationWindow()
        {
            InitializeComponent();
            var vm = new OrganizationWindowViewModel();
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