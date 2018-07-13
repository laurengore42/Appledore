namespace HolmesMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class linksinglestorycharacterstostory : DbMigration
    {
        public override void Up()
        {
            AddForeignKey("dbo.Characters", "Story", "dbo.Stories", "ID");
            CreateIndex("dbo.Characters", "Story");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Characters", new[] { "Story" });
            DropForeignKey("dbo.Characters", "Story", "dbo.Stories");
        }
    }
}
