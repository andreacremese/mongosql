namespace DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_indexes : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Engineers", "Name", c => c.String(maxLength: 40));
            AlterColumn("dbo.Engineers", "Email", c => c.String(maxLength: 40));
            AlterColumn("dbo.Partners", "Name", c => c.String(maxLength: 40));
            CreateIndex("dbo.Engineers", "Name");
            CreateIndex("dbo.Engineers", "Email");
            CreateIndex("dbo.Partners", "Name");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Partners", new[] { "Name" });
            DropIndex("dbo.Engineers", new[] { "Email" });
            DropIndex("dbo.Engineers", new[] { "Name" });
            AlterColumn("dbo.Partners", "Name", c => c.String());
            AlterColumn("dbo.Engineers", "Email", c => c.String());
            AlterColumn("dbo.Engineers", "Name", c => c.String());
        }
    }
}
