namespace HolmesMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class urlnamesforactors : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Actors", "UrlName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Actors", "UrlName");
        }
    }
}
