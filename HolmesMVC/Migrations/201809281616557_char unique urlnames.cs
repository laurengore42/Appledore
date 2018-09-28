namespace HolmesMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class charuniqueurlnames : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Characters", "UrlName", c => c.String(nullable: false, maxLength: 150));
            CreateIndex("dbo.Characters", "UrlName", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Characters", new[] { "UrlName" });
            AlterColumn("dbo.Characters", "UrlName", c => c.String(maxLength: 150));
        }
    }
}
