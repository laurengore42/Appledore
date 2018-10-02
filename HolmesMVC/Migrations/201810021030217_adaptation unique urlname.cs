namespace HolmesMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class adaptationuniqueurlname : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Adaptations", "UrlName", c => c.String(nullable: false, maxLength: 150));
            CreateIndex("dbo.Adaptations", "UrlName", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Adaptations", new[] { "UrlName" });
            AlterColumn("dbo.Adaptations", "UrlName", c => c.String(maxLength: 150));
        }
    }
}
