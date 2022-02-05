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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace InvoiceAppNET5.MVVM.View
{
    /// <summary>
    /// Interaction logic for ClientView.xaml
    /// </summary>
    public partial class ClientView : UserControl
    {
        public ClientView()
        {
            InitializeComponent();
            //Set the data grid to show clients from the db
            using (var db = new InvoiceAppDbModel())
            {
                SetGrid(db);
            }
        }

        private void EditDetailsButton_Click(object sender, RoutedEventArgs e)
        {
            if (ClientGrid.SelectedItem is Client)
            {
                Client client = (Client)ClientGrid.SelectedItem;
                EditClientDetailsGroup.Visibility = Visibility.Visible;
                SetClientEditBoxes(client);
            }
            else
            {
                MessageBox.Show("Please select a client to edit.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new InvoiceAppDbModel())
            {
                if (ClientGrid.SelectedItem is Client)
                {
                    Client client = (Client)ClientGrid.SelectedItem;
                    var result = db.Clients.Find(client.Id);
                    result.FirstName = ClientFirstNameTextBox.Text;
                    result.LastName = ClientLastNameTextBox.Text;
                    result.Email = ClientEmailTextBox.Text;
                    db.SaveChanges();
                    EditClientDetailsGroup.Visibility = Visibility.Hidden;
                    SetGrid(db);
                }
            }
        }
        private void SetGrid(InvoiceAppDbModel db)
        {
            var clients = db.Clients.ToList();

            ClientGrid.ItemsSource = clients;
        }
        private void SetClientEditBoxes(Client client)
        {
            ClientFirstNameTextBox.Text = client.FirstName;
            ClientLastNameTextBox.Text = client.LastName;
            ClientEmailTextBox.Text = client.Email;
        }

        private void ClientGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ClientGrid.SelectedItem is Client)
            {
                Client client = (Client)ClientGrid.SelectedItem;
                SetClientEditBoxes(client);
            }
            else
            {
                ClientFirstNameTextBox.Text = "";
                ClientLastNameTextBox.Text = "";
                ClientEmailTextBox.Text = "";
            }
        }

        private void AddClientButton_Click(object sender, RoutedEventArgs e)
        {
            AddClientPopup addClientPopup = new AddClientPopup();
            addClientPopup.ShowDialog();
            using (var db = new InvoiceAppDbModel())
            {
                SetGrid(db);
            }
        }

        private void DeleteClientButton_Click(object sender, RoutedEventArgs e)
        {
            if (ClientGrid.SelectedItems.Count == 1)
            {
                string message = "Are you sure? This will delete all associated Invoices";
                string caption = "Confirmation";
                MessageBoxButton buttons = MessageBoxButton.YesNo;
                MessageBoxImage icon = MessageBoxImage.Question;
                if (MessageBox.Show(message, caption, buttons, icon) == MessageBoxResult.Yes)
                {
                    using (var db = new InvoiceAppDbModel())
                    {
                        Client client = (Client)ClientGrid.SelectedItems[0];
                        var result = db.Clients.Find(client.Id);
                        db.Clients.Remove(result);
                        db.SaveChanges();
                        SetGrid(db);
                    }
                }
                else
                {
                }
            }
            else
            {
                MessageBox.Show("Please select one client to delete.", "Selection Error", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
