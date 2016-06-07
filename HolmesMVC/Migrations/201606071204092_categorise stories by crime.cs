namespace HolmesMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class categorisestoriesbycrime : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Stories", "VillainType", c => c.Int(nullable: false));
            AddColumn("dbo.Stories", "OutcomeType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Stories", "OutcomeType");
            DropColumn("dbo.Stories", "VillainType");
        }
    }
}
