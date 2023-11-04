namespace DRS.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Nappa : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Orders", "Received", c => c.String());
            AlterColumn("dbo.Orders", "Attachment", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Orders", "Attachment", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Orders", "Received", c => c.Boolean(nullable: false));
        }
    }
}
