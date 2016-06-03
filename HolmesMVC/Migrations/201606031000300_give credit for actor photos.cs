namespace HolmesMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class givecreditforactorphotos : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Actors", "PicCredit", c => c.String(maxLength: 1000));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Actors", "PicCredit");
        }
    }
}
