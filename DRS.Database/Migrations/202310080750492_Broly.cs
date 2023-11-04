namespace DRS.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Broly : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "Unavailability", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "Unavailability");
        }
    }
}
