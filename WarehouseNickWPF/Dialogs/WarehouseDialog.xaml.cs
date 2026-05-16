using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WarehouseNickWPF.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для WarehouseDialog.xaml
    /// </summary>
    public partial class WarehouseDialog : Window
    {
        public string WarehouseName => NameTextBox.Text;

        public WarehouseDialog(string initialName = "")
        {
            InitializeComponent();
            NameTextBox.Text = initialName;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
