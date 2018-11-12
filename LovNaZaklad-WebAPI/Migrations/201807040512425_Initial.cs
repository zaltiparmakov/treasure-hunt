namespace LovNaZaklad_WebAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Images",
                c => new
                    {
                        ImageID = c.Int(nullable: false, identity: true),
                        Path = c.String(),
                        Features = c.String(),
                    })
                .PrimaryKey(t => t.ImageID);
            
            CreateTable(
                "dbo.Locations",
                c => new
                    {
                        LocationID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Latitude = c.String(nullable: false),
                        Longitude = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.LocationID);
            
            CreateTable(
                "dbo.Questions",
                c => new
                    {
                        QuestionID = c.Int(nullable: false, identity: true),
                        QuestionValue = c.String(nullable: false),
                        Answer = c.String(nullable: false),
                        LocationID = c.Int(nullable: false),
                        NextLocationID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.QuestionID)
                .ForeignKey("dbo.Locations", t => t.LocationID)
                .ForeignKey("dbo.Locations", t => t.NextLocationID)
                .Index(t => t.LocationID)
                .Index(t => t.NextLocationID);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        RoleID = c.Int(nullable: false, identity: true),
                        RoleName = c.String(nullable: false),
                        ScopeID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.RoleID)
                .ForeignKey("dbo.Scopes", t => t.ScopeID, cascadeDelete: true)
                .Index(t => t.ScopeID);
            
            CreateTable(
                "dbo.Scopes",
                c => new
                    {
                        ScopeID = c.Int(nullable: false, identity: true),
                        ScopeValue = c.String(),
                    })
                .PrimaryKey(t => t.ScopeID);
            
            CreateTable(
                "dbo.Treasures",
                c => new
                    {
                        TreasureID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        LocationID = c.Int(nullable: false),
                        Image_ImageID = c.Int(),
                    })
                .PrimaryKey(t => t.TreasureID)
                .ForeignKey("dbo.Images", t => t.Image_ImageID)
                .ForeignKey("dbo.Locations", t => t.LocationID, cascadeDelete: true)
                .Index(t => t.LocationID)
                .Index(t => t.Image_ImageID);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserID = c.Int(nullable: false, identity: true),
                        Email = c.String(nullable: false),
                        Username = c.String(nullable: false),
                        Password = c.String(nullable: false),
                        FirstName = c.String(maxLength: 30),
                        LastName = c.String(maxLength: 50),
                        DateOfBirth = c.DateTime(nullable: false),
                        Points = c.Int(nullable: false),
                        RoleID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UserID)
                .ForeignKey("dbo.Roles", t => t.RoleID, cascadeDelete: true)
                .Index(t => t.RoleID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Users", "RoleID", "dbo.Roles");
            DropForeignKey("dbo.Treasures", "LocationID", "dbo.Locations");
            DropForeignKey("dbo.Treasures", "Image_ImageID", "dbo.Images");
            DropForeignKey("dbo.Roles", "ScopeID", "dbo.Scopes");
            DropForeignKey("dbo.Questions", "NextLocationID", "dbo.Locations");
            DropForeignKey("dbo.Questions", "LocationID", "dbo.Locations");
            DropIndex("dbo.Users", new[] { "RoleID" });
            DropIndex("dbo.Treasures", new[] { "Image_ImageID" });
            DropIndex("dbo.Treasures", new[] { "LocationID" });
            DropIndex("dbo.Roles", new[] { "ScopeID" });
            DropIndex("dbo.Questions", new[] { "NextLocationID" });
            DropIndex("dbo.Questions", new[] { "LocationID" });
            DropTable("dbo.Users");
            DropTable("dbo.Treasures");
            DropTable("dbo.Scopes");
            DropTable("dbo.Roles");
            DropTable("dbo.Questions");
            DropTable("dbo.Locations");
            DropTable("dbo.Images");
        }
    }
}
