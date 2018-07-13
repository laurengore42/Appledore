namespace HolmesMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class marksinglestorycharacters : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Characters", "Story", c => c.String(maxLength: 4, fixedLength: true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Characters", "Story");
        }
    }
}
