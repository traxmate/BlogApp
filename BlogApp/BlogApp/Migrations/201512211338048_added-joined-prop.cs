namespace BlogApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedjoinedprop : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Joined", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Joined");
        }
    }
}
