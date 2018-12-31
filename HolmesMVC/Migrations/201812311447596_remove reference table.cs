namespace HolmesMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removereferencetable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.References", "Episode", "dbo.Episodes");
            DropForeignKey("dbo.References", "Story", "dbo.Stories");
            DropIndex("dbo.References", new[] { "Story" });
            DropIndex("dbo.References", new[] { "Episode" });
            DropTable("dbo.References");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.References",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        Story = c.String(nullable: false, maxLength: 4, fixedLength: true),
                        Episode = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateIndex("dbo.References", "Episode");
            CreateIndex("dbo.References", "Story");
            AddForeignKey("dbo.References", "Story", "dbo.Stories", "ID", cascadeDelete: true);
            AddForeignKey("dbo.References", "Episode", "dbo.Episodes", "ID", cascadeDelete: true);
        }
    }
}
