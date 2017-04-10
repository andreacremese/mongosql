namespace DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Engineers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Email = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Reports",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        Title = c.String(),
                        Partner_Id = c.Int(),
                        Topic_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Partners", t => t.Partner_Id)
                .ForeignKey("dbo.Topics", t => t.Topic_Id)
                .Index(t => t.Partner_Id)
                .Index(t => t.Topic_Id);
            
            CreateTable(
                "dbo.Partners",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TopicFamilies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Topics",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        TopicFamilyId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TopicFamilies", t => t.TopicFamilyId, cascadeDelete: true)
                .Index(t => t.TopicFamilyId);
            
            CreateTable(
                "dbo.ReportEngineers",
                c => new
                    {
                        Report_Id = c.Int(nullable: false),
                        Engineer_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Report_Id, t.Engineer_Id })
                .ForeignKey("dbo.Reports", t => t.Report_Id, cascadeDelete: true)
                .ForeignKey("dbo.Engineers", t => t.Engineer_Id, cascadeDelete: true)
                .Index(t => t.Report_Id)
                .Index(t => t.Engineer_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Topics", "TopicFamilyId", "dbo.TopicFamilies");
            DropForeignKey("dbo.Reports", "Topic_Id", "dbo.Topics");
            DropForeignKey("dbo.Reports", "Partner_Id", "dbo.Partners");
            DropForeignKey("dbo.ReportEngineers", "Engineer_Id", "dbo.Engineers");
            DropForeignKey("dbo.ReportEngineers", "Report_Id", "dbo.Reports");
            DropIndex("dbo.ReportEngineers", new[] { "Engineer_Id" });
            DropIndex("dbo.ReportEngineers", new[] { "Report_Id" });
            DropIndex("dbo.Topics", new[] { "TopicFamilyId" });
            DropIndex("dbo.Reports", new[] { "Topic_Id" });
            DropIndex("dbo.Reports", new[] { "Partner_Id" });
            DropTable("dbo.ReportEngineers");
            DropTable("dbo.Topics");
            DropTable("dbo.TopicFamilies");
            DropTable("dbo.Partners");
            DropTable("dbo.Reports");
            DropTable("dbo.Engineers");
        }
    }
}
