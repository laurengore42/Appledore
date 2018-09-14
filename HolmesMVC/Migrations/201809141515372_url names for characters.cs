namespace HolmesMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class urlnamesforcharacters : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Characters", "UrlName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Characters", "UrlName");
        }
    }
}
