using InvoiceAppNET5.Data;
using InvoiceAppNET5.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace InvoiceAppNET5.MVVM.View
{
    /// <summary>
    /// Interaction logic for AddInvoicePopup.xaml
    /// </summary>
    public partial class AddInvoicePopup : Window
    {
        private Client _client;
        public AddInvoicePopup(Client client)
        {
            _client = client;
            InitializeComponent();
            AddInvoiceLabel.Content = $"Add invoice for {_client.FullName}";
            InvoiceDatePicker.SelectedDate = DateTime.Today;
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {

            Regex checktime = new Regex(@"([01]?[0-9]|2[0-3]):[0-5][0-9]");
            if (double.TryParse(CostPerHourTextBox.Text, out double _double) && checktime.IsMatch(StartTimeTextBox.Text) && checktime.IsMatch(EndTimeTextBox.Text) && !string.IsNullOrEmpty(DescriptionTextBox.Text))
            {
                Invoice newInvoice = new Invoice(_client.Id, DescriptionTextBox.Text, decimal.Parse(CostPerHourTextBox.Text), (DateTime)InvoiceDatePicker.SelectedDate, StartTimeTextBox.Text, EndTimeTextBox.Text);
                using (InvoiceAppDbModel db = new InvoiceAppDbModel())
                {
                    db.Invoices.Add(newInvoice);
                    db.SaveChanges();
                    Close();
                }
            }
            else
            {
                MessageBox.Show("All fields must be filled and valid", "Invalid input", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void DoubleValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Double.TryParse(e.Text, out double result);
        }
    }
}
