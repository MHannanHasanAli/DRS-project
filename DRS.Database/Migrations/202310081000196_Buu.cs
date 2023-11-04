namespace DRS.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Buu : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "AlternativeCode", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "AlternativeCode");
        }
    }
}
