namespace HolmesMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class actorimdbnameindatabase : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Actors", "IMDbName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Actors", "IMDbName");
        }
    }
}
