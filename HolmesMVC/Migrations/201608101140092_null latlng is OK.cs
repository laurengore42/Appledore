namespace HolmesMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class nulllatlngisOK : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Actors", "Latitude", c => c.Double());
            AlterColumn("dbo.Actors", "Longitude", c => c.Double());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Actors", "Longitude", c => c.Double(nullable: false));
            AlterColumn("dbo.Actors", "Latitude", c => c.Double(nullable: false));
        }
    }
}
