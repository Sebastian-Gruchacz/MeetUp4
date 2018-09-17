namespace MeetUp.Migration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Simplifymodel : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SupplierCaseLimits", "SupplierId", "dbo.Suppliers");
            DropIndex("dbo.SupplierCaseLimits", new[] { "SupplierId" });
            DropTable("dbo.SupplierCaseLimits");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.SupplierCaseLimits",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SupplierId = c.Int(nullable: false),
                        CreditRating = c.String(maxLength: 10),
                        PaymentMethod = c.String(maxLength: 20),
                        MaxCaseLimit = c.Int(),
                        LimitEnabled = c.Boolean(nullable: false),
                        Tracking_CreatedBy = c.Guid(nullable: false),
                        Tracking_CreatedDateTimeUtc = c.DateTime(nullable: false),
                        Tracking_LastModifiedBy = c.Guid(),
                        Tracking_ModifiedDateTimeUtc = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.SupplierCaseLimits", "SupplierId");
            AddForeignKey("dbo.SupplierCaseLimits", "SupplierId", "dbo.Suppliers", "SupplierId");
        }
    }
}
