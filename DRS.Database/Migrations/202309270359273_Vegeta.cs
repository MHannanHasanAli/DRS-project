namespace DRS.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Vegeta : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Supplier_Brand", "Default", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Supplier_Brand", "Default", c => c.Boolean(nullable: false));
        }
    }
}
