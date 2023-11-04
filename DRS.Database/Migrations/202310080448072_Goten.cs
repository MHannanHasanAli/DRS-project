namespace DRS.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Goten : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "Reminder1", c => c.DateTime());
            AddColumn("dbo.Orders", "Reminder2", c => c.DateTime());
            AddColumn("dbo.Orders", "Reminder3", c => c.DateTime());
            AddColumn("dbo.Orders", "DeliveryDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.Order_Item", "Reminder1");
            DropColumn("dbo.Order_Item", "Reminder2");
            DropColumn("dbo.Order_Item", "Reminder3");
            DropColumn("dbo.Order_Item", "DeliveryDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Order_Item", "DeliveryDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Order_Item", "Reminder3", c => c.DateTime(nullable: false));
            AddColumn("dbo.Order_Item", "Reminder2", c => c.DateTime(nullable: false));
            AddColumn("dbo.Order_Item", "Reminder1", c => c.DateTime(nullable: false));
            DropColumn("dbo.Orders", "DeliveryDate");
            DropColumn("dbo.Orders", "Reminder3");
            DropColumn("dbo.Orders", "Reminder2");
            DropColumn("dbo.Orders", "Reminder1");
        }
    }
}
