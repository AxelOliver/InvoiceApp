using InvoiceAppNET5.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace InvoiceAppNET5.Data
{
    public partial class InvoiceAppDbModel : DbContext
    {
        public InvoiceAppDbModel()
            : base(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + Environment.CurrentDirectory + @"\InvoiceAppDB.mdf;Integrated Security=True")
        {
        }
        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<Invoice> Invoices { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

        }
    }
}
