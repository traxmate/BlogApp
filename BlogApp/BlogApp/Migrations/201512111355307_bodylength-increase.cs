namespace BlogApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class bodylengthincrease : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Blogs", "Body", c => c.String(nullable: false, maxLength: 512));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Blogs", "Body", c => c.String(nullable: false, maxLength: 32));
        }
    }
}
