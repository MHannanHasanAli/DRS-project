namespace DRS.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tien : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "Attachment", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "Attachment");
        }
    }
}
