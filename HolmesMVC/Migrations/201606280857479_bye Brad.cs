namespace HolmesMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class byeBrad : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Dates", "Keefauver");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Dates", "Keefauver", c => c.DateTime(nullable: false));
        }
    }
}
