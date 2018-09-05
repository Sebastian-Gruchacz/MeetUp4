namespace MeetUp.Migration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModelUpdate : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUsers", "CreatedBy", "dbo.AspNetUsers");
            DropIndex("dbo.AspNetUsers", "UIX_UserLoginEmail");
            DropIndex("dbo.AspNetUsers", new[] { "CreatedBy" });
            CreateTable(
                "dbo.CustomerLeadStatusDetails",
                c => new
                    {
                        StatusId = c.Int(nullable: false, identity: true),
                        LeadStatus = c.String(),
                        UserComments = c.String(),
                        TransactionId = c.Int(nullable: false),
                        CreatedBy = c.Guid(nullable: false),
                        CreatedDateTimeUtc = c.DateTime(nullable: false),
                        LastModifiedBy = c.Guid(),
                        ModifiedDateTimeUtc = c.DateTime(),
                    })
                .PrimaryKey(t => t.StatusId)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedBy, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.LastModifiedBy)
                .ForeignKey("dbo.CustomerLeadStatus", t => t.TransactionId, cascadeDelete: true)
                .Index(t => t.TransactionId)
                .Index(t => t.CreatedBy)
                .Index(t => t.LastModifiedBy);
            
            CreateTable(
                "dbo.CustomerLeadStatus",
                c => new
                    {
                        LeadTrackingId = c.Int(nullable: false, identity: true),
                        ForSupplierId = c.Int(nullable: false),
                        FromCustomerId = c.Int(nullable: false),
                        FromDepartmentId = c.Int(nullable: false),
                        FromUserId = c.Guid(),
                        OrderId = c.Guid(),
                        CreatedBy = c.Guid(nullable: false),
                        CreatedDateTimeUtc = c.DateTime(nullable: false),
                        LastModifiedBy = c.Guid(),
                        ModifiedDateTimeUtc = c.DateTime(),
                    })
                .PrimaryKey(t => t.LeadTrackingId)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedBy, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.LastModifiedBy)
                .Index(t => t.CreatedBy)
                .Index(t => t.LastModifiedBy);
            
            CreateTable(
                "dbo.CustomerOrderAttachments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FileName = c.String(maxLength: 250),
                        FilePath = c.String(maxLength: 250),
                        MimeType = c.String(maxLength: 50),
                        Customer_Order_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customer_Order", t => t.Customer_Order_Id)
                .Index(t => t.Customer_Order_Id);
            
            CreateTable(
                "dbo.OrderLines",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OrderId = c.Int(nullable: false),
                        Customer_Order_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customer_Order", t => t.Customer_Order_Id)
                .Index(t => t.Customer_Order_Id);
            
            CreateTable(
                "dbo.Customer_Order",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ForSupplierId = c.Int(nullable: false),
                        FromCustomerId = c.Int(nullable: false),
                        FromDepartmentId = c.Int(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                        OrderEmail = c.String(maxLength: 150),
                        CreatedByUserId = c.Guid(nullable: false),
                        Status = c.String(nullable: false, maxLength: 10),
                        IsJson = c.Boolean(),
                        DateCreatedUtc = c.DateTime(nullable: false),
                        Customer_Id = c.Int(),
                        Department_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.Customer_Id)
                .ForeignKey("dbo.Departments", t => t.Department_Id)
                .Index(t => t.Customer_Id)
                .Index(t => t.Department_Id);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CustomerName = c.String(nullable: false, maxLength: 150),
                        ChannelId = c.Int(nullable: false),
                        NoOfEmployee = c.Int(nullable: false),
                        InternalEmail = c.String(maxLength: 250),
                        CreatedBy = c.Guid(nullable: false),
                        CreatedDateTimeUtc = c.DateTime(nullable: false),
                        LastModifiedBy = c.Guid(),
                        ModifiedDateTimeUtc = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedBy, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.LastModifiedBy)
                .Index(t => t.CustomerName, unique: true, name: "UIX_CustomerName")
                .Index(t => t.InternalEmail, unique: true, name: "UIX_InternaEmail")
                .Index(t => t.CreatedBy)
                .Index(t => t.LastModifiedBy);
            
            CreateTable(
                "dbo.UserCustomerRoles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AspNetUserId = c.Guid(nullable: false),
                        AspNetRoleId = c.Int(nullable: false),
                        CreatedBy = c.Guid(nullable: false),
                        CreatedDateTimeUtc = c.DateTime(nullable: false),
                        LastModifiedBy = c.Guid(),
                        ModifiedDateTimeUtc = c.DateTime(),
                        Customer_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetRoles", t => t.AspNetRoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.AspNetUserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedBy, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.LastModifiedBy)
                .ForeignKey("dbo.Customers", t => t.Customer_Id)
                .Index(t => t.AspNetUserId)
                .Index(t => t.AspNetRoleId)
                .Index(t => t.CreatedBy)
                .Index(t => t.LastModifiedBy)
                .Index(t => t.Customer_Id);
            
            CreateTable(
                "dbo.Departments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 150),
                        InternalEmail = c.String(maxLength: 250),
                        CreatedBy = c.Guid(nullable: false),
                        CreatedDateTimeUtc = c.DateTime(nullable: false),
                        LastModifiedBy = c.Guid(),
                        ModifiedDateTimeUtc = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedBy, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.LastModifiedBy)
                .Index(t => t.CreatedBy)
                .Index(t => t.LastModifiedBy);
            
            CreateTable(
                "dbo.MailAttachments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MessageId = c.Guid(nullable: false),
                        FileName = c.String(maxLength: 250),
                        FileUrl = c.String(maxLength: 250),
                        FilePath = c.String(maxLength: 250),
                        MimeType = c.String(maxLength: 50),
                        CreatedBy = c.Guid(nullable: false),
                        CreatedDateTimeUtc = c.DateTime(nullable: false),
                        LastModifiedBy = c.Guid(),
                        ModifiedDateTimeUtc = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedBy, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.LastModifiedBy)
                .ForeignKey("dbo.MailMessages", t => t.MessageId, cascadeDelete: true)
                .Index(t => t.MessageId)
                .Index(t => t.CreatedBy)
                .Index(t => t.LastModifiedBy);
            
            CreateTable(
                "dbo.MailMessages",
                c => new
                    {
                        MessageId = c.Guid(nullable: false),
                        Body = c.String(),
                        FromAddress = c.String(maxLength: 250),
                        DisplayName = c.String(maxLength: 250),
                        ToAddress = c.String(maxLength: 250),
                        Subject = c.String(maxLength: 250),
                        Kind = c.Int(nullable: false),
                        Type = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        CustomerId = c.Int(),
                        DepartmentId = c.Int(),
                        LeadTrackingId = c.Int(nullable: false),
                        UserId = c.Guid(),
                        HideFromUser = c.Boolean(nullable: false),
                        SentUtcDateTime = c.DateTime(nullable: false),
                        ParentMessageId = c.Guid(),
                        CreatedBy = c.Guid(nullable: false),
                        CreatedDateTimeUtc = c.DateTime(nullable: false),
                        LastModifiedBy = c.Guid(),
                        ModifiedDateTimeUtc = c.DateTime(),
                        AspNetUser_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.MessageId)
                .ForeignKey("dbo.AspNetUsers", t => t.AspNetUser_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedBy, cascadeDelete: true)
                .ForeignKey("dbo.Customers", t => t.CustomerId)
                .ForeignKey("dbo.AspNetUsers", t => t.LastModifiedBy)
                .Index(t => t.CustomerId)
                .Index(t => t.CreatedBy)
                .Index(t => t.LastModifiedBy)
                .Index(t => t.AspNetUser_Id);
            
            CreateTable(
                "dbo.SupplierOrderPolicies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SupplierId = c.Int(nullable: false),
                        SendOrderViaEmail = c.Boolean(nullable: false),
                        OrderEmailAddress = c.String(maxLength: 250),
                        MinEmployeeLimitForOrder = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Suppliers", t => t.SupplierId, cascadeDelete: true)
                .Index(t => t.SupplierId);
            
            CreateTable(
                "dbo.Suppliers",
                c => new
                    {
                        SupplierId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        InternalSupportEmailForSpecialSituations = c.String(maxLength: 250),
                    })
                .PrimaryKey(t => t.SupplierId)
                .Index(t => t.Name, unique: true, name: "UIX_SupplierName")
                .Index(t => t.InternalSupportEmailForSpecialSituations, unique: true, name: "UIX_InternaEmail");
            
            AlterColumn("dbo.AspNetRoles", "Name", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.AspNetUsers", "Email", c => c.String(nullable: false, maxLength: 250));
            AlterColumn("dbo.AspNetUsers", "FirstName", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.AspNetUsers", "LastName", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.AspNetUsers", "LanguageCode", c => c.String(nullable: false, maxLength: 5));
            CreateIndex("dbo.AspNetUsers", "Email", unique: true, name: "UIX_UserLoginEmail");
            DropColumn("dbo.AspNetUsers", "ModifiedDateTimeUtc");
            DropColumn("dbo.AspNetUsers", "CreatedDateTimeUtc");
            DropColumn("dbo.AspNetUsers", "CreatedBy");
            DropColumn("dbo.AspNetUsers", "LastModifiedBy");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "LastModifiedBy", c => c.Guid());
            AddColumn("dbo.AspNetUsers", "CreatedBy", c => c.Guid(nullable: false));
            AddColumn("dbo.AspNetUsers", "CreatedDateTimeUtc", c => c.DateTime(nullable: false));
            AddColumn("dbo.AspNetUsers", "ModifiedDateTimeUtc", c => c.DateTime());
            DropForeignKey("dbo.SupplierOrderPolicies", "SupplierId", "dbo.Suppliers");
            DropForeignKey("dbo.MailMessages", "LastModifiedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.MailMessages", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.MailMessages", "CreatedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.MailAttachments", "MessageId", "dbo.MailMessages");
            DropForeignKey("dbo.MailMessages", "AspNetUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.MailAttachments", "LastModifiedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.MailAttachments", "CreatedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.OrderLines", "Customer_Order_Id", "dbo.Customer_Order");
            DropForeignKey("dbo.Customer_Order", "Department_Id", "dbo.Departments");
            DropForeignKey("dbo.Departments", "LastModifiedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.Departments", "CreatedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.CustomerOrderAttachments", "Customer_Order_Id", "dbo.Customer_Order");
            DropForeignKey("dbo.Customer_Order", "Customer_Id", "dbo.Customers");
            DropForeignKey("dbo.UserCustomerRoles", "Customer_Id", "dbo.Customers");
            DropForeignKey("dbo.UserCustomerRoles", "LastModifiedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserCustomerRoles", "CreatedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserCustomerRoles", "AspNetUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserCustomerRoles", "AspNetRoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Customers", "LastModifiedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.Customers", "CreatedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.CustomerLeadStatus", "LastModifiedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.CustomerLeadStatusDetails", "TransactionId", "dbo.CustomerLeadStatus");
            DropForeignKey("dbo.CustomerLeadStatus", "CreatedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.CustomerLeadStatusDetails", "LastModifiedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.CustomerLeadStatusDetails", "CreatedBy", "dbo.AspNetUsers");
            DropIndex("dbo.Suppliers", "UIX_InternaEmail");
            DropIndex("dbo.Suppliers", "UIX_SupplierName");
            DropIndex("dbo.SupplierOrderPolicies", new[] { "SupplierId" });
            DropIndex("dbo.MailMessages", new[] { "AspNetUser_Id" });
            DropIndex("dbo.MailMessages", new[] { "LastModifiedBy" });
            DropIndex("dbo.MailMessages", new[] { "CreatedBy" });
            DropIndex("dbo.MailMessages", new[] { "CustomerId" });
            DropIndex("dbo.MailAttachments", new[] { "LastModifiedBy" });
            DropIndex("dbo.MailAttachments", new[] { "CreatedBy" });
            DropIndex("dbo.MailAttachments", new[] { "MessageId" });
            DropIndex("dbo.Departments", new[] { "LastModifiedBy" });
            DropIndex("dbo.Departments", new[] { "CreatedBy" });
            DropIndex("dbo.UserCustomerRoles", new[] { "Customer_Id" });
            DropIndex("dbo.UserCustomerRoles", new[] { "LastModifiedBy" });
            DropIndex("dbo.UserCustomerRoles", new[] { "CreatedBy" });
            DropIndex("dbo.UserCustomerRoles", new[] { "AspNetRoleId" });
            DropIndex("dbo.UserCustomerRoles", new[] { "AspNetUserId" });
            DropIndex("dbo.Customers", new[] { "LastModifiedBy" });
            DropIndex("dbo.Customers", new[] { "CreatedBy" });
            DropIndex("dbo.Customers", "UIX_InternaEmail");
            DropIndex("dbo.Customers", "UIX_CustomerName");
            DropIndex("dbo.Customer_Order", new[] { "Department_Id" });
            DropIndex("dbo.Customer_Order", new[] { "Customer_Id" });
            DropIndex("dbo.OrderLines", new[] { "Customer_Order_Id" });
            DropIndex("dbo.CustomerOrderAttachments", new[] { "Customer_Order_Id" });
            DropIndex("dbo.CustomerLeadStatus", new[] { "LastModifiedBy" });
            DropIndex("dbo.CustomerLeadStatus", new[] { "CreatedBy" });
            DropIndex("dbo.CustomerLeadStatusDetails", new[] { "LastModifiedBy" });
            DropIndex("dbo.CustomerLeadStatusDetails", new[] { "CreatedBy" });
            DropIndex("dbo.CustomerLeadStatusDetails", new[] { "TransactionId" });
            DropIndex("dbo.AspNetUsers", "UIX_UserLoginEmail");
            AlterColumn("dbo.AspNetUsers", "LanguageCode", c => c.String());
            AlterColumn("dbo.AspNetUsers", "LastName", c => c.String(maxLength: 50));
            AlterColumn("dbo.AspNetUsers", "FirstName", c => c.String(maxLength: 50));
            AlterColumn("dbo.AspNetUsers", "Email", c => c.String(maxLength: 250));
            AlterColumn("dbo.AspNetRoles", "Name", c => c.String(maxLength: 50));
            DropTable("dbo.Suppliers");
            DropTable("dbo.SupplierOrderPolicies");
            DropTable("dbo.MailMessages");
            DropTable("dbo.MailAttachments");
            DropTable("dbo.Departments");
            DropTable("dbo.UserCustomerRoles");
            DropTable("dbo.Customers");
            DropTable("dbo.Customer_Order");
            DropTable("dbo.OrderLines");
            DropTable("dbo.CustomerOrderAttachments");
            DropTable("dbo.CustomerLeadStatus");
            DropTable("dbo.CustomerLeadStatusDetails");
            CreateIndex("dbo.AspNetUsers", "CreatedBy");
            CreateIndex("dbo.AspNetUsers", "Email", unique: true, name: "UIX_UserLoginEmail");
            AddForeignKey("dbo.AspNetUsers", "CreatedBy", "dbo.AspNetUsers", "Id");
        }
    }
}
