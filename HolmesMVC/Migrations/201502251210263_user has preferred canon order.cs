namespace HolmesMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class userhaspreferredcanonorder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserProfile", "PreferredCanonOrder", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserProfile", "PreferredCanonOrder");
        }
    }
}
