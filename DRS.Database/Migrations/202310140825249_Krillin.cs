namespace DRS.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Krillin : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "File", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "File");
        }
    }
}
