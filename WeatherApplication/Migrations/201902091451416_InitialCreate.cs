namespace WeatherApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Applications",
                c => new
                    {
                        ApplicationID = c.Int(nullable: false, identity: true),
                        Head = c.String(),
                        Degree = c.Int(nullable: false),
                        Text1 = c.String(),
                        Text2 = c.String(),
                        Text3 = c.String(),
                        Image = c.String(),
                    })
                .PrimaryKey(t => t.ApplicationID);
            
            CreateTable(
                "dbo.Qualities",
                c => new
                    {
                        QualityID = c.Int(nullable: false, identity: true),
                        ApplicationID = c.Int(nullable: false),
                        FeatureID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.QualityID)
                .ForeignKey("dbo.Applications", t => t.ApplicationID, cascadeDelete: true)
                .ForeignKey("dbo.Features", t => t.FeatureID, cascadeDelete: true)
                .Index(t => t.ApplicationID)
                .Index(t => t.FeatureID);
            
            CreateTable(
                "dbo.Features",
                c => new
                    {
                        FeatureID = c.Int(nullable: false, identity: true),
                        Head1 = c.String(),
                        Head2 = c.String(),
                        Icon = c.String(),
                        Head3 = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.FeatureID);
            
            CreateTable(
                "dbo.Descriptions",
                c => new
                    {
                        DescriptionID = c.Int(nullable: false, identity: true),
                        Title1 = c.String(),
                        Title2 = c.String(),
                        Text1 = c.String(),
                        Degree1 = c.Int(nullable: false),
                        Text2 = c.String(),
                        Image1 = c.String(),
                        Image2 = c.String(),
                        Head1 = c.String(),
                        Description1 = c.String(),
                        Humidity = c.Int(nullable: false),
                        Degree2 = c.Int(nullable: false),
                        Text3 = c.String(),
                    })
                .PrimaryKey(t => t.DescriptionID);
            
            CreateTable(
                "dbo.Downloads",
                c => new
                    {
                        DownloadID = c.Int(nullable: false, identity: true),
                        Head = c.String(),
                        Description = c.String(),
                        Image1 = c.String(),
                        Image2 = c.String(),
                        Image3 = c.String(),
                    })
                .PrimaryKey(t => t.DownloadID);
            
            CreateTable(
                "dbo.Reviews",
                c => new
                    {
                        ReviewID = c.Int(nullable: false, identity: true),
                        Title1 = c.String(),
                        Title2 = c.String(),
                        Icon = c.String(),
                        Description = c.String(),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ReviewID);
            
            CreateTable(
                "dbo.Slides",
                c => new
                    {
                        SlideID = c.Int(nullable: false, identity: true),
                        Image1 = c.String(),
                        BackgroundImage1 = c.String(),
                        BackgroundImage2 = c.String(),
                        Background_Image3 = c.String(),
                    })
                .PrimaryKey(t => t.SlideID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Qualities", "FeatureID", "dbo.Features");
            DropForeignKey("dbo.Qualities", "ApplicationID", "dbo.Applications");
            DropIndex("dbo.Qualities", new[] { "FeatureID" });
            DropIndex("dbo.Qualities", new[] { "ApplicationID" });
            DropTable("dbo.Slides");
            DropTable("dbo.Reviews");
            DropTable("dbo.Downloads");
            DropTable("dbo.Descriptions");
            DropTable("dbo.Features");
            DropTable("dbo.Qualities");
            DropTable("dbo.Applications");
        }
    }
}
