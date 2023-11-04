namespace DRS.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Gohan : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Order_Item", "AlternativeCode", c => c.String());
            AddColumn("dbo.Order_Item", "Reminder1", c => c.DateTime(nullable: false));
            AddColumn("dbo.Order_Item", "Reminder2", c => c.DateTime(nullable: false));
            AddColumn("dbo.Order_Item", "Reminder3", c => c.DateTime(nullable: false));
            AddColumn("dbo.Order_Item", "DeliveryDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Orders", "Date", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Order_Item", "Attachment", c => c.Boolean(nullable: false));
            DropColumn("dbo.Order_Item", "DateOrder");
            DropColumn("dbo.Order_Item", "DateExpected");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Order_Item", "DateExpected", c => c.String());
            AddColumn("dbo.Order_Item", "DateOrder", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Order_Item", "Attachment", c => c.String());
            DropColumn("dbo.Orders", "Date");
            DropColumn("dbo.Order_Item", "DeliveryDate");
            DropColumn("dbo.Order_Item", "Reminder3");
            DropColumn("dbo.Order_Item", "Reminder2");
            DropColumn("dbo.Order_Item", "Reminder1");
            DropColumn("dbo.Order_Item", "AlternativeCode");
        }
    }
}
