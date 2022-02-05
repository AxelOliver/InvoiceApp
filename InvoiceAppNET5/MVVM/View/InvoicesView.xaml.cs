using InvoiceAppNET5.Data;
using InvoiceAppNET5.MVVM.Model;
using iText.Html2pdf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace InvoiceAppNET5.MVVM.View
{
	/// <summary>
	/// Interaction logic for InvoicesView.xaml
	/// </summary>
	public partial class InvoicesView : UserControl
	{
		InvoiceAppDbModel db;
		Client selectedClient;
		private List<Client> clients;
		private List<Invoice> invoices;
		public InvoicesView()
		{
			InitializeComponent();
			db = new InvoiceAppDbModel();
			clients = db.Clients.Include("Invoices").OrderBy(c => c.LastName).ToList();
			ClientComboBox.ItemsSource = clients;
			ClientComboBox.SelectedIndex = 0;
			selectedClient = (Client)ClientComboBox.SelectedItem;
		}

		private void ClientComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			selectedClient = (Client)ClientComboBox.SelectedItem;
			using (var db = new InvoiceAppDbModel())
			{
				UpdateInvoiceGrid((Client)ClientComboBox.SelectedItem, db);
			}
		}
		private void UpdateInvoiceGrid(Client client, InvoiceAppDbModel db)
		{
			var selectedClient = db.Clients.Include("Invoices").Where(t => t.Id == client.Id).FirstOrDefault();
			invoices = selectedClient.Invoices.ToList();
			InvoicesGrid.ItemsSource = invoices;
		}

		private void AddInvoiceButton_Click(object sender, RoutedEventArgs e)
		{
			if (ClientComboBox.SelectedItem is Client)
			{
				AddInvoicePopup addInvoicePopup = new AddInvoicePopup((Client)ClientComboBox.SelectedItem);
				addInvoicePopup.ShowDialog();
				using (var db = new InvoiceAppDbModel())
				{
					UpdateInvoiceGrid((Client)ClientComboBox.SelectedItem, db);
				}
			}
			else
			{
				MessageBox.Show("Please select a client", "Invalid Selection", MessageBoxButton.OK, MessageBoxImage.Exclamation);
			}
		}

		private void DeleteInvoiceButton_Click(object sender, RoutedEventArgs e)
		{
			if (InvoicesGrid.SelectedItem is Invoice)
			{
				Invoice selectedInvoice = (Invoice)InvoicesGrid.SelectedItem;
				string message = "Are you sure? This will permanently delete the invoice";
				string caption = "Confirmation";
				MessageBoxButton buttons = MessageBoxButton.YesNo;
				MessageBoxImage icon = MessageBoxImage.Question;
				if (MessageBox.Show(message, caption, buttons, icon) == MessageBoxResult.Yes)
				{
					using (var db = new InvoiceAppDbModel())
					{
						var result = db.Invoices.Find(selectedInvoice.Id);
						db.Invoices.Remove(result);
						db.SaveChanges();
						UpdateInvoiceGrid((Client)ClientComboBox.SelectedItem, db);
					}
				}
				else
				{
				}
			}
		}

		private void GeneratePdfButton_Click(object sender, RoutedEventArgs e)
		{
			User user = null;
			using (var db = new InvoiceAppDbModel())
			{
				user = db.Users.FirstOrDefault();
			}
			if (user == null || ClientComboBox.SelectedItem == null || InvoicesGrid.SelectedItems.Count == 0)
			{
				MessageBox.Show("Invalid Selection, make sure invoices are selected", "Selection Error", MessageBoxButton.OK, MessageBoxImage.Error);
			}
			else
			{
				Directory.CreateDirectory(".\\Invoices");
				Directory.CreateDirectory(".\\Invoices\\Sent");
				Directory.CreateDirectory(".\\Invoices\\Sent\\Paid");
				Directory.CreateDirectory(".\\HTML");
				List<Invoice> invoices = InvoicesGrid.SelectedItems.Cast<Invoice>().ToList();
				invoices = invoices.OrderBy(o => o.Date).ToList();
				CreatePdf(invoices, user, (Client)ClientComboBox.SelectedItem);
			}
		}
		private static void CreatePdf(List<Invoice> invoices, User user, Client client)
		{
			int invoiceNumber = Properties.Settings.Default.InvoiceNumber;
			string invoiceNumberString = invoiceNumber.ToString("D4");
			HtmlGenerator(invoices, user, client, invoiceNumberString);
			string saveString = $"{invoiceNumberString}_{client.FirstName}_{client.LastName}.pdf";
			HtmlConverter.ConvertToPdf(
				new FileInfo(@".\HTML\temp.html"),
				new FileInfo($".\\Invoices\\{saveString}")
			);
			//Process.Start($".\\Invoices\\{saveString}");
			Process.Start("explorer.exe", @".\Invoices");
			Properties.Settings.Default.InvoiceNumber = invoiceNumber + 1;
			Properties.Settings.Default.Save();
		}
		private static void HtmlGenerator(List<Invoice> invoices, User user, Client client, string invoiceNumber)
		{
			decimal totalCost = 0;
			string htmlString = @"<!DOCTYPE html>
<html>
<head>
	<meta charset=""utf-8"" />
	<title>Invoice</title>

	<style>
		.invoice-box {
			max-width: 800px;
			margin: auto;
			padding: 30px;
			border: 1px solid #eee;
			box-shadow: 0 0 10px rgba(0, 0, 0, 0.15);
			font-size: 16px;
			line-height: 24px;
			font-family: 'Helvetica Neue', 'Helvetica', Helvetica, Arial, sans-serif;
			color: #555;
		}
		.invoice-box {
		max-width: unset;
		box-shadow: none;
		border: 0px;
		}

			.invoice-box table {
				width: 100%;
				line-height: inherit;
				text-align: left;
			}

				.invoice-box table td {
					padding: 5px;
					vertical-align: top;
				}

				.invoice-box table tr td:nth-child(2) {
					text-align: right;
				}

				.invoice-box table tr.top table td {
					padding-bottom: 20px;
				}

					.invoice-box table tr.top table td.title {
						font-size: 45px;
						line-height: 45px;
						color: #333;
					}

				.invoice-box table tr.information table td {
					padding-bottom: 40px;
				}

				.invoice-box table tr.heading td {
					background: #eee;
					border-bottom: 1px solid #ddd;
					font-weight: bold;
				}

				.invoice-box table tr.details td {
					padding-bottom: 20px;
				}

				.invoice-box table tr.item td {
					border-bottom: 1px solid #eee;
				}

				.invoice-box table tr.item.last td {
					border-bottom: none;
				}

				.invoice-box table tr.total td:nth-child(2) {
					border-top: 2px solid #eee;
					font-weight: bold;
				}

		@media only screen and (max-width: 600px) {
			.invoice-box table tr.top table td {
				width: 100%;
				display: block;
				text-align: center;
			}

			.invoice-box table tr.information table td {
				width: 100%;
				display: block;
				text-align: center;
			}
		}

		/** RTL **/
		.invoice-box.rtl {
			direction: rtl;
			font-family: Tahoma, 'Helvetica Neue', 'Helvetica', Helvetica, Arial, sans-serif;
		}

			.invoice-box.rtl table {
				text-align: right;
			}

				.invoice-box.rtl table tr td:nth-child(2) {
					text-align: left;
				}
	</style>
</head>

<body>
	<div class=""invoice-box"">
<table>
						<tr>
							<td class=""title"">
								<img src=""" + Directory.GetCurrentDirectory() + @"\Images\PascalleOliverLogo.jpg"" style=""width: 100%; max-width: 150px"" />
							</td>

							<td>";
			int daysUntilNextFriday = DayOfWeek.Friday - DateTime.Today.DayOfWeek + 7;

			htmlString += $"Invoice #: {invoiceNumber}<br/>Created: {DateTime.Today:d}<br/>Due: {DateTime.Today.AddDays(daysUntilNextFriday):d}";
			htmlString += @"
							</td>
						</tr>
					</table>

					<table>
						<tr>";
			htmlString += $"<td><b>From</b><br/>{user.FullName}<br/>{user.Email}<br/>Phone: {user.PhoneNumber}<br/>ABN: {user.ABN}</td>";
			htmlString += $"<td><b>To</b><br/>{client.FullName}<br/>{client.Email}<br/></td></tr></table>";
			htmlString += @"
		<table cellpadding=""0"" cellspacing=""0"">

			<tr class=""information"">
				<td colspan=""1""></td></tr>";
			htmlString += @"

			<tr class=""heading"">
				<td>Payment Method</td>

				<td>Bank Transfer</td>
			</tr>

			<tr class=""details"">";
			htmlString += $"<td>Name: {user.FullName}</td><td>BSB: {user.BSB}<br/>Account #: {user.AccountNumber}</td></tr>";
			htmlString += @"

			<tr class=""heading"">
				<td>Date</td>

				<td>Description</td>

				<td>Hourly Rate</td>

				<td>Hours</td>

				<td>Subtotal</td>
			</tr>";
			foreach (var invoice in invoices)
			{
				totalCost += invoice.TotalCost;
				htmlString += $"<tr class=\"item\"><td>{invoice.Date:d} {invoice.StartTime} - {invoice.EndTime}</td><td>{invoice.Description}</td><td>{invoice.HourlyRate}</td><td>{invoice.HoursWorked}</td><td>${invoice.TotalCost}</td></tr>";
			};
			htmlString += @"<tr class=""total"">
							<td></td>
							<td></td>
							<td></td>";
			htmlString += $"<td><b>Total:</b> ${Math.Round(totalCost, 2)}</td>";
			htmlString += @"</tr>
							<tr>
								<td colspan=""4""><br/>Please quote invoice number in bank transfer.<br/><br/><b>-<b/></td></tr>
							</tr>
							<tr>
								<td colspan=""2"">Thanks for your business!</td></tr>
							</tr>
							</table>
						</div>
					</body>
					</html>";
			File.WriteAllText(@".\HTML\temp.html", htmlString);
		}
	}
}
