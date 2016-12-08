namespace BlogApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changedpropertyname : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Nickname", c => c.String(maxLength: 64));
            DropColumn("dbo.AspNetUsers", "Pseudonym");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "Pseudonym", c => c.String(maxLength: 64));
            DropColumn("dbo.AspNetUsers", "Nickname");
        }
    }
}
