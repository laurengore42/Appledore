namespace HolmesMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class actorbirthplacesync : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Actors", "SyncedBirthplace", c => c.String());
            AddColumn("dbo.Actors", "Latitude", c => c.Double(nullable: false));
            AddColumn("dbo.Actors", "Longitude", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Actors", "Longitude");
            DropColumn("dbo.Actors", "Latitude");
            DropColumn("dbo.Actors", "SyncedBirthplace");
        }
    }
}
