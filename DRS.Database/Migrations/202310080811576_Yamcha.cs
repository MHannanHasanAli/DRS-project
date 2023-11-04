namespace DRS.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Yamcha : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "Received", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "Received");
        }
    }
}
