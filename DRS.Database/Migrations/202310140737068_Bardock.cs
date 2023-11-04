namespace DRS.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Bardock : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customers", "Erp", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Customers", "Erp");
        }
    }
}
