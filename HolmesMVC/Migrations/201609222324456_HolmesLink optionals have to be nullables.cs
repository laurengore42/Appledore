namespace HolmesMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HolmesLinkoptionalshavetobenullables : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.HolmesLinkAppearances", "HolmesLinkActor1_ID", "dbo.HolmesLinkActors");
            DropForeignKey("dbo.HolmesLinkAppearances", "HolmesLink1_ID", "dbo.HolmesLinks");
            DropIndex("dbo.HolmesLinkAppearances", new[] { "HolmesLinkActor1_ID" });
            DropIndex("dbo.HolmesLinkAppearances", new[] { "HolmesLink1_ID" });
            RenameColumn(table: "dbo.HolmesLinks", name: "Episode1_ID", newName: "Episode");
            RenameColumn(table: "dbo.HolmesLinkAppearances", name: "HolmesLink1_ID", newName: "HolmesLink");
            RenameColumn(table: "dbo.HolmesLinkActors", name: "Actor1_ID", newName: "Actor");
            AlterColumn("dbo.HolmesLinks", "Episode", c => c.Int());
            AlterColumn("dbo.HolmesLinkActors", "Actor", c => c.Int());
            AddForeignKey("dbo.HolmesLinkAppearances", "HolmesLinkActor", "dbo.HolmesLinkActors", "ID", cascadeDelete: true);
            AddForeignKey("dbo.HolmesLinkAppearances", "HolmesLink", "dbo.HolmesLinks", "ID", cascadeDelete: true);
            CreateIndex("dbo.HolmesLinkAppearances", "HolmesLinkActor");
            CreateIndex("dbo.HolmesLinkAppearances", "HolmesLink");
            DropColumn("dbo.HolmesLinkAppearances", "HolmesLinkActor1_ID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.HolmesLinkAppearances", "HolmesLinkActor1_ID", c => c.Int());
            DropIndex("dbo.HolmesLinkAppearances", new[] { "HolmesLink" });
            DropIndex("dbo.HolmesLinkAppearances", new[] { "HolmesLinkActor" });
            DropForeignKey("dbo.HolmesLinkAppearances", "HolmesLink", "dbo.HolmesLinks");
            DropForeignKey("dbo.HolmesLinkAppearances", "HolmesLinkActor", "dbo.HolmesLinkActors");
            AlterColumn("dbo.HolmesLinkActors", "Actor", c => c.Int(nullable: false));
            AlterColumn("dbo.HolmesLinks", "Episode", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.HolmesLinkActors", name: "Actor", newName: "Actor1_ID");
            RenameColumn(table: "dbo.HolmesLinkAppearances", name: "HolmesLink", newName: "HolmesLink1_ID");
            RenameColumn(table: "dbo.HolmesLinks", name: "Episode", newName: "Episode1_ID");
            CreateIndex("dbo.HolmesLinkAppearances", "HolmesLink1_ID");
            CreateIndex("dbo.HolmesLinkAppearances", "HolmesLinkActor1_ID");
            AddForeignKey("dbo.HolmesLinkAppearances", "HolmesLink1_ID", "dbo.HolmesLinks", "ID");
            AddForeignKey("dbo.HolmesLinkAppearances", "HolmesLinkActor1_ID", "dbo.HolmesLinkActors", "ID");
        }
    }
}
