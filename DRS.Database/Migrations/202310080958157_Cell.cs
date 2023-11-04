namespace DRS.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Cell : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Order_Item", "Attachment");
            DropColumn("dbo.Order_Item", "AlternativeCode");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Order_Item", "AlternativeCode", c => c.String());
            AddColumn("dbo.Order_Item", "Attachment", c => c.Boolean(nullable: false));
        }
    }
}
