namespace WebASPPetProj.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSlugUrl : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Categories", "ShortUrl", c => c.String());
            AddColumn("dbo.Posts", "ShortUrl", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Posts", "ShortUrl");
            DropColumn("dbo.Categories", "ShortUrl");
        }
    }
}
