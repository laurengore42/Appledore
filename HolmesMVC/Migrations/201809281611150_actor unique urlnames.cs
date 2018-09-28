namespace HolmesMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class actoruniqueurlnames : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Actors", "UrlName", c => c.String(nullable: false, maxLength: 150));
            CreateIndex("dbo.Actors", "UrlName", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Actors", new[] { "UrlName" });
            AlterColumn("dbo.Actors", "UrlName", c => c.String(maxLength: 150));
        }
    }
}
