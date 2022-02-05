namespace InvoiceAppNET5.Migrations
{
    using InvoiceAppNET5.MVVM.Model;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<InvoiceAppNET5.Data.InvoiceAppDbModel>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(InvoiceAppNET5.Data.InvoiceAppDbModel context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
            context.Clients.AddOrUpdate(t => t.FirstName,
                new Client()
                {
                    FirstName = "Pascalle",
                    LastName = "Oliver",
                    Email = "pascalle@gmail.com"
                },
                new Client()
                {
                    FirstName = "Axel",
                    LastName = "Oliver",
                    Email = "axeloliver@gmail.com"
                },
                new Client()
                {
                    FirstName = "Daniel",
                    LastName = "Smith",
                    Email = "danny@gmail.com"
                });
            context.Invoices.AddOrUpdate(t => t.Id,
                new Invoice(1, "CareTaking", 30, DateTime.Today.AddDays(2), "9:00", "15:00") { Id = 1 },
                new Invoice(2, "CareTaking", 35, DateTime.Today, "8:00", "17:00") { Id = 2 },
                new Invoice(2, "CareTaking", 35, DateTime.Today.AddDays(1), "10:00", "15:00") { Id = 3 }
                );
            context.Users.AddOrUpdate(t => t.Id,
                new User()
                {
                    Id = 1,
                    FirstName = "Pascalle",
                    LastName = "Oliver",
                    Bank = "WestPac",
                    AccountNumber = "123456",
                    BSB = "654321",
                    PhoneNumber = "0412 345 678",
                    Email = "test@gmail.com",
                    ABN = "1234 ABCD 5678"
                });
        }
    }
}
