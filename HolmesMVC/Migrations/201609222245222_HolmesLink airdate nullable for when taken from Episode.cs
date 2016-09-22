namespace HolmesMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HolmesLinkairdatenullableforwhentakenfromEpisode : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.HolmesLinks", "Airdate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.HolmesLinks", "Airdate", c => c.DateTime(nullable: false));
        }
    }
}
