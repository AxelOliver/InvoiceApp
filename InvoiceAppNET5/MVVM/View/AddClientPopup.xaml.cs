using InvoiceAppNET5.Data;
using InvoiceAppNET5.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// Interaction logic for AddClientPopup.xaml
    /// </summary>
    public partial class AddClientPopup : Window
    {
        public AddClientPopup()
        {
            InitializeComponent();
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(FirstNameTextBox.Text) || String.IsNullOrWhiteSpace(LastNameTextBox.Text) || String.IsNullOrWhiteSpace(EmailTextBox.Text))
            {
                MessageBox.Show("Please Enter all Details", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                using (var db = new InvoiceAppDbModel())
                {
                    Client client = new Client { FirstName = FirstNameTextBox.Text, LastName = LastNameTextBox.Text, Email = EmailTextBox.Text };
                    db.Clients.Add(client);
                    db.SaveChanges();
                    Close();
                }
            }
        }
    }
}
