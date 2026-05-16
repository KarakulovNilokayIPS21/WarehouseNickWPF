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
    /// Логика взаимодействия для ConfirmDeleteDialog.xaml
    /// </summary>
    public partial class ConfirmDeleteDialog : Window
    {
        private readonly string _expectedName;

        public ConfirmDeleteDialog(string organizationName)
        {
            InitializeComponent();
            _expectedName = organizationName;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (NameTextBox.Text == _expectedName)
                DialogResult = true;
            else
                MessageBox.Show("Название не совпадает. Удаление отменено.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
