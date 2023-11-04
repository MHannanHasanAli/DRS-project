namespace DRS.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Trunks : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Orders", "DeliveryDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Orders", "DeliveryDate", c => c.DateTime(nullable: false));
        }
    }
}
