namespace DRS.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OmegaShenron : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "Confirmation", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "Confirmation");
        }
    }
}
