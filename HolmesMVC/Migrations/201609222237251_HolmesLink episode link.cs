namespace HolmesMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HolmesLinkepisodelink : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HolmesLinks", "Episode", c => c.Int(nullable: false));
            AddColumn("dbo.HolmesLinks", "Episode1_ID", c => c.Int());
            AddForeignKey("dbo.HolmesLinks", "Episode1_ID", "dbo.Episodes", "ID");
            CreateIndex("dbo.HolmesLinks", "Episode1_ID");
        }
        
        public override void Down()
        {
            DropIndex("dbo.HolmesLinks", new[] { "Episode1_ID" });
            DropForeignKey("dbo.HolmesLinks", "Episode1_ID", "dbo.Episodes");
            DropColumn("dbo.HolmesLinks", "Episode1_ID");
            DropColumn("dbo.HolmesLinks", "Episode");
        }
    }
}
