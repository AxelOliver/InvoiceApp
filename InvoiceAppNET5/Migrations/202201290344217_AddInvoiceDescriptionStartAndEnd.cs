namespace InvoiceAppNET5.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddInvoiceDescriptionStartAndEnd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Invoices", "Description", c => c.String());
            AddColumn("dbo.Invoices", "StartTime", c => c.String());
            AddColumn("dbo.Invoices", "EndTime", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Invoices", "EndTime");
            DropColumn("dbo.Invoices", "StartTime");
            DropColumn("dbo.Invoices", "Description");
        }
    }
}
