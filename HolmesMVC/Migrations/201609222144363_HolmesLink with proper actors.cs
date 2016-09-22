namespace HolmesMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HolmesLinkwithproperactors : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.HolmesLinkAppearances", "Actor1_ID", "dbo.Actors");
            DropIndex("dbo.HolmesLinkAppearances", new[] { "Actor1_ID" });
            CreateTable(
                "dbo.HolmesLinkActors",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Actor = c.Int(nullable: false),
                        Name = c.String(),
                        Actor1_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Actors", t => t.Actor1_ID)
                .Index(t => t.Actor1_ID);
            
            AddColumn("dbo.HolmesLinkAppearances", "HolmesLinkActor", c => c.Int(nullable: false));
            AddColumn("dbo.HolmesLinkAppearances", "HolmesLinkActor1_ID", c => c.Int());
            AddForeignKey("dbo.HolmesLinkAppearances", "HolmesLinkActor1_ID", "dbo.HolmesLinkActors", "ID");
            CreateIndex("dbo.HolmesLinkAppearances", "HolmesLinkActor1_ID");
            DropColumn("dbo.HolmesLinkAppearances", "Actor");
            DropColumn("dbo.HolmesLinkAppearances", "Actor1_ID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.HolmesLinkAppearances", "Actor1_ID", c => c.Int());
            AddColumn("dbo.HolmesLinkAppearances", "Actor", c => c.Int(nullable: false));
            DropIndex("dbo.HolmesLinkAppearances", new[] { "HolmesLinkActor1_ID" });
            DropIndex("dbo.HolmesLinkActors", new[] { "Actor1_ID" });
            DropForeignKey("dbo.HolmesLinkAppearances", "HolmesLinkActor1_ID", "dbo.HolmesLinkActors");
            DropForeignKey("dbo.HolmesLinkActors", "Actor1_ID", "dbo.Actors");
            DropColumn("dbo.HolmesLinkAppearances", "HolmesLinkActor1_ID");
            DropColumn("dbo.HolmesLinkAppearances", "HolmesLinkActor");
            DropTable("dbo.HolmesLinkActors");
            CreateIndex("dbo.HolmesLinkAppearances", "Actor1_ID");
            AddForeignKey("dbo.HolmesLinkAppearances", "Actor1_ID", "dbo.Actors", "ID");
        }
    }
}
