namespace MeetUp.Migration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ResolvewidermodelrequiredfororderHTML : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CaseFields",
                c => new
                    {
                        FieldId = c.Int(nullable: false, identity: true),
                        FieldLabel = c.String(nullable: false, maxLength: 150),
                        FieldValue = c.String(nullable: false, maxLength: 150),
                        FieldDescription = c.String(maxLength: 150),
                        IsActive = c.Boolean(nullable: false),
                        Tracking_CreatedBy = c.Guid(nullable: false),
                        Tracking_CreatedDateTimeUtc = c.DateTime(nullable: false),
                        Tracking_LastModifiedBy = c.Guid(),
                        Tracking_ModifiedDateTimeUtc = c.DateTime(),
                    })
                .PrimaryKey(t => t.FieldId);
            
            CreateTable(
                "dbo.SupplierCaseFields",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CaseFieldId = c.Int(nullable: false),
                        SupplierId = c.Int(nullable: false),
                        ChannelId = c.Int(nullable: false),
                        FieldOrder = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CaseValue = c.String(maxLength: 100),
                        CustomLabel = c.String(maxLength: 100),
                        Tracking_CreatedBy = c.Guid(nullable: false),
                        Tracking_CreatedDateTimeUtc = c.DateTime(nullable: false),
                        Tracking_LastModifiedBy = c.Guid(),
                        Tracking_ModifiedDateTimeUtc = c.DateTime(),
                        CaseField_FieldId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CaseFields", t => t.CaseField_FieldId)
                .ForeignKey("dbo.Suppliers", t => t.SupplierId)
                .Index(t => t.SupplierId)
                .Index(t => t.CaseField_FieldId);
            
            CreateTable(
                "dbo.SupplierCustomerNumbers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CustomerId = c.Int(nullable: false),
                        DepartmentId = c.Int(nullable: false),
                        SupplierId = c.Int(nullable: false),
                        SupplierCustomerId = c.String(maxLength: 200),
                        CreatedUtcDateTime = c.DateTime(),
                        LastUpdatedUtcDateTime = c.DateTime(),
                        ExportedToErp = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.CustomerId)
                .ForeignKey("dbo.Departments", t => t.DepartmentId)
                .ForeignKey("dbo.Suppliers", t => t.SupplierId)
                .Index(t => t.CustomerId)
                .Index(t => t.DepartmentId)
                .Index(t => t.SupplierId);
            
            CreateTable(
                "dbo.OrderFormHistories",
                c => new
                    {
                        FormGuidId = c.Guid(nullable: false),
                        OrderId = c.Guid(nullable: false),
                        Json = c.String(),
                    })
                .PrimaryKey(t => t.FormGuidId);
            
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Suppliers", t => t.SupplierId)
                .Index(t => t.SupplierId);
            
            AddColumn("dbo.AspNetUsers", "PhoneNumber", c => c.String(maxLength: 50));
            AddColumn("dbo.OrderLines", "LineKey", c => c.String(maxLength: 20));
            AddColumn("dbo.OrderLines", "LineValue", c => c.String(maxLength: 100));
            AddColumn("dbo.Customers", "PaymentMethod", c => c.String(maxLength: 20));
            AddColumn("dbo.Customers", "InternalCreditRating", c => c.String(maxLength: 15));
            AddColumn("dbo.Customers", "Industry", c => c.String(maxLength: 50));
            AddColumn("dbo.Customers", "TaxRegistrationNumber", c => c.String(maxLength: 20));
            AddColumn("dbo.Departments", "ExternalId", c => c.String(maxLength: 50));
            AddColumn("dbo.Departments", "Street", c => c.String(maxLength: 100));
            AddColumn("dbo.Departments", "PostalCode", c => c.String(maxLength: 10));
            AddColumn("dbo.Departments", "City", c => c.String(maxLength: 50));
            AddColumn("dbo.Departments", "DepartmentNumber", c => c.String(maxLength: 50));
            AddColumn("dbo.Departments", "Customer_Id", c => c.Int());
            CreateIndex("dbo.Departments", "Customer_Id");
            AddForeignKey("dbo.Departments", "Customer_Id", "dbo.Customers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SupplierCaseLimits", "SupplierId", "dbo.Suppliers");
            DropForeignKey("dbo.SupplierCustomerNumbers", "SupplierId", "dbo.Suppliers");
            DropForeignKey("dbo.SupplierCustomerNumbers", "DepartmentId", "dbo.Departments");
            DropForeignKey("dbo.SupplierCustomerNumbers", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.Departments", "Customer_Id", "dbo.Customers");
            DropForeignKey("dbo.SupplierCaseFields", "SupplierId", "dbo.Suppliers");
            DropForeignKey("dbo.SupplierCaseFields", "CaseField_FieldId", "dbo.CaseFields");
            DropIndex("dbo.SupplierCaseLimits", new[] { "SupplierId" });
            DropIndex("dbo.SupplierCustomerNumbers", new[] { "SupplierId" });
            DropIndex("dbo.SupplierCustomerNumbers", new[] { "DepartmentId" });
            DropIndex("dbo.SupplierCustomerNumbers", new[] { "CustomerId" });
            DropIndex("dbo.Departments", new[] { "Customer_Id" });
            DropIndex("dbo.SupplierCaseFields", new[] { "CaseField_FieldId" });
            DropIndex("dbo.SupplierCaseFields", new[] { "SupplierId" });
            DropColumn("dbo.Departments", "Customer_Id");
            DropColumn("dbo.Departments", "DepartmentNumber");
            DropColumn("dbo.Departments", "City");
            DropColumn("dbo.Departments", "PostalCode");
            DropColumn("dbo.Departments", "Street");
            DropColumn("dbo.Departments", "ExternalId");
            DropColumn("dbo.Customers", "TaxRegistrationNumber");
            DropColumn("dbo.Customers", "Industry");
            DropColumn("dbo.Customers", "InternalCreditRating");
            DropColumn("dbo.Customers", "PaymentMethod");
            DropColumn("dbo.OrderLines", "LineValue");
            DropColumn("dbo.OrderLines", "LineKey");
            DropColumn("dbo.AspNetUsers", "PhoneNumber");
            DropTable("dbo.SupplierCaseLimits");
            DropTable("dbo.OrderFormHistories");
            DropTable("dbo.SupplierCustomerNumbers");
            DropTable("dbo.SupplierCaseFields");
            DropTable("dbo.CaseFields");
        }
    }
}
