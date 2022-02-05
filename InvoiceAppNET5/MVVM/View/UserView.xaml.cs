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
    /// Interaction logic for UserView.xaml
    /// </summary>
    public partial class UserView : UserControl
    {
        public User _User { get; set; }
        public UserView()
        {
            InitializeComponent();
            using (var db = new InvoiceAppDbModel())
            {
                _User = db.Users.FirstOrDefault();
                if (_User == null)
                {
                    _User = new User { Id = 1, FirstName = "Click edit button to edit" };
                    db.Users.Add(_User);
                    db.SaveChanges();
                    CreateUserButton.Visibility = System.Windows.Visibility.Collapsed;
                    EditUserButton.Visibility = System.Windows.Visibility.Visible;
                    SetDetailsText(db);
                }
                else
                {
                    CreateUserButton.Visibility = System.Windows.Visibility.Collapsed;
                    EditUserButton.Visibility = System.Windows.Visibility.Visible;
                    SetDetailsText(db);
                }
            }

        }
        private void DetailsTextBlock_Initialized(object sender, EventArgs e)
        {

        }

        private void EditUserButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            SetUpdateBoxes();
            EditDetailsGroup.Visibility = System.Windows.Visibility.Visible;
        }

        private void UpdateButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            using (var db = new InvoiceAppDbModel())
            {
                var result = db.Users.FirstOrDefault();
                result.FirstName = FirstNameTextBox.Text;
                result.LastName = LastNameTextBox.Text;
                result.PhoneNumber = PhoneTextBox.Text;
                result.Email = EmailTextBox.Text;
                result.Bank = BankTextBox.Text;
                result.BSB = BSBTextBox.Text;
                result.AccountNumber = AccountTextBox.Text;
                result.ABN = AbnTextBox.Text;
                db.SaveChanges();
                SetDetailsText(db);
            }
            EditDetailsGroup.Visibility = System.Windows.Visibility.Hidden;
        }
        private void SetUpdateBoxes()
        {
            using (var db = new InvoiceAppDbModel())
            {
                _User = db.Users.FirstOrDefault();
                FirstNameTextBox.Text = _User.FirstName;
                LastNameTextBox.Text = _User.LastName;
                PhoneTextBox.Text = _User.PhoneNumber;
                EmailTextBox.Text = _User.Email;
                BankTextBox.Text = _User.Bank;
                BSBTextBox.Text = _User.BSB;
                AccountTextBox.Text = _User.AccountNumber;
                AbnTextBox.Text = _User.ABN;
                SetDetailsText(db);
            }
        }
        private void SetDetailsText(InvoiceAppDbModel db)
        {
            _User = db.Users.FirstOrDefault();
            DetailsTextBlock.Text = $"Name: {_User.FullName}\nBank: {_User.Bank}\nBSB: {_User.BSB}\n" +
                $"Acc#: {_User.AccountNumber}\nPhone: {_User.PhoneNumber}\nEmail: {_User.Email}\nABN: {_User.ABN}";
        }

        private void CreateUserButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            using (var db = new InvoiceAppDbModel())
            {
                _User = new User { FirstName = "Click edit button to edit" };
                db.Users.Add(_User);
                db.SaveChanges();
                SetDetailsText(db);
            }
            CreateUserButton.Visibility = System.Windows.Visibility.Collapsed;
            EditUserButton.Visibility = System.Windows.Visibility.Visible;
        }
    }
}
