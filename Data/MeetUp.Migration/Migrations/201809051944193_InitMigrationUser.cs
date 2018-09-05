namespace MeetUp.Migration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitMigrationUser : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Email = c.String(maxLength: 250),
                        FirstName = c.String(maxLength: 50),
                        LastName = c.String(maxLength: 50),
                        LanguageCode = c.String(),
                        IsSystemUser = c.Boolean(nullable: false),
                        InternalEmail = c.String(maxLength: 250),
                        ModifiedDateTimeUtc = c.DateTime(),
                        CreatedDateTimeUtc = c.DateTime(nullable: false),
                        CreatedBy = c.Guid(),
                        LastModifiedBy = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Email, unique: true, name: "UIX_UserLoginEmail")
                .Index(t => t.InternalEmail, unique: true, name: "UIX_InternaEmail");

            
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.AspNetUsers", "UIX_InternaEmail");
            DropIndex("dbo.AspNetUsers", "UIX_UserLoginEmail");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetRoles");
        }
    }
}
