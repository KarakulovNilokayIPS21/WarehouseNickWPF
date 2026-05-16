using System.Windows;

namespace WarehouseNickWPF.Dialogs
{
    public partial class SimpleNameDialog : Window
    {
        public string EnteredName => NameTextBox.Text;

        public SimpleNameDialog(string prompt, string initialValue = "")
        {
            InitializeComponent();
            PromptTextBlock.Text = prompt;
            NameTextBox.Text = initialValue;
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