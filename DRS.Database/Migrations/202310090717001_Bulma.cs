namespace DRS.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Bulma : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Surname", c => c.String());
            AddColumn("dbo.AspNetUsers", "Branch", c => c.String());
            AddColumn("dbo.AspNetUsers", "Image", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Image");
            DropColumn("dbo.AspNetUsers", "Branch");
            DropColumn("dbo.AspNetUsers", "Surname");
        }
    }
}
