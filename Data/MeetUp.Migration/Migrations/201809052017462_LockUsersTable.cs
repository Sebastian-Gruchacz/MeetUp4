namespace MeetUp.Migration.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class LockUsersTable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "CreatedBy", c => c.Guid(nullable: false));
            CreateIndex("dbo.AspNetUsers", "CreatedBy");
            AddForeignKey("dbo.AspNetUsers", "CreatedBy", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "CreatedBy", "dbo.AspNetUsers");
            DropIndex("dbo.AspNetUsers", new[] { "CreatedBy" });
            AlterColumn("dbo.AspNetUsers", "CreatedBy", c => c.Guid());
        }
    }
}
