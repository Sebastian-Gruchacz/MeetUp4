namespace MeetUp.Migration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TrackingRedesign : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CustomerLeadStatusDetails", "CreatedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.CustomerLeadStatusDetails", "LastModifiedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.CustomerLeadStatus", "CreatedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.CustomerLeadStatusDetails", "LeadTrackingId", "dbo.CustomerLeadStatus");
            DropForeignKey("dbo.CustomerLeadStatus", "LastModifiedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.Customers", "CreatedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.Customers", "LastModifiedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserCustomerRoles", "CreatedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserCustomerRoles", "LastModifiedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.Departments", "CreatedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.Departments", "LastModifiedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.MailAttachments", "CreatedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.MailAttachments", "LastModifiedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.MailMessages", "CreatedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.MailMessages", "LastModifiedBy", "dbo.AspNetUsers");
            DropIndex("dbo.CustomerLeadStatusDetails", new[] { "LeadTrackingId" });
            DropIndex("dbo.CustomerLeadStatusDetails", new[] { "CreatedBy" });
            DropIndex("dbo.CustomerLeadStatusDetails", new[] { "LastModifiedBy" });
            DropIndex("dbo.CustomerLeadStatus", new[] { "CreatedBy" });
            DropIndex("dbo.CustomerLeadStatus", new[] { "LastModifiedBy" });
            DropIndex("dbo.Customers", new[] { "CreatedBy" });
            DropIndex("dbo.Customers", new[] { "LastModifiedBy" });
            DropIndex("dbo.UserCustomerRoles", new[] { "CreatedBy" });
            DropIndex("dbo.UserCustomerRoles", new[] { "LastModifiedBy" });
            DropIndex("dbo.Departments", new[] { "CreatedBy" });
            DropIndex("dbo.Departments", new[] { "LastModifiedBy" });
            DropIndex("dbo.MailAttachments", new[] { "CreatedBy" });
            DropIndex("dbo.MailAttachments", new[] { "LastModifiedBy" });
            DropIndex("dbo.MailMessages", new[] { "CreatedBy" });
            DropIndex("dbo.MailMessages", new[] { "LastModifiedBy" });
            CreateTable(
                "dbo.CustomerCaseStatusEntries",
                c => new
                    {
                        EntryId = c.Int(nullable: false, identity: true),
                        Status = c.String(),
                        CaseId = c.Int(nullable: false),
                        UserComments = c.String(),
                        Tracking_CreatedBy = c.Guid(nullable: false),
                        Tracking_CreatedDateTimeUtc = c.DateTime(nullable: false),
                        Tracking_LastModifiedBy = c.Guid(),
                        Tracking_ModifiedDateTimeUtc = c.DateTime(),
                    })
                .PrimaryKey(t => t.EntryId)
                .ForeignKey("dbo.CustomerCases", t => t.CaseId)
                .Index(t => t.CaseId);
            
            CreateTable(
                "dbo.CustomerCases",
                c => new
                    {
                        CaseId = c.Int(nullable: false, identity: true),
                        ForSupplierId = c.Int(nullable: false),
                        FromCustomerId = c.Int(nullable: false),
                        FromDepartmentId = c.Int(nullable: false),
                        FromUserId = c.Guid(),
                        OrderId = c.Guid(),
                        Tracking_CreatedBy = c.Guid(nullable: false),
                        Tracking_CreatedDateTimeUtc = c.DateTime(nullable: false),
                        Tracking_LastModifiedBy = c.Guid(),
                        Tracking_ModifiedDateTimeUtc = c.DateTime(),
                    })
                .PrimaryKey(t => t.CaseId);
            
            AddColumn("dbo.AspNetUsers", "Tracking_CreatedBy", c => c.Guid(nullable: false));
            AddColumn("dbo.AspNetUsers", "Tracking_CreatedDateTimeUtc", c => c.DateTime(nullable: false));
            AddColumn("dbo.AspNetUsers", "Tracking_LastModifiedBy", c => c.Guid());
            AddColumn("dbo.AspNetUsers", "Tracking_ModifiedDateTimeUtc", c => c.DateTime());
            AddColumn("dbo.Customers", "Tracking_CreatedBy", c => c.Guid(nullable: false));
            AddColumn("dbo.Customers", "Tracking_CreatedDateTimeUtc", c => c.DateTime(nullable: false));
            AddColumn("dbo.Customers", "Tracking_LastModifiedBy", c => c.Guid());
            AddColumn("dbo.Customers", "Tracking_ModifiedDateTimeUtc", c => c.DateTime());
            AddColumn("dbo.UserCustomerRoles", "Tracking_CreatedBy", c => c.Guid(nullable: false));
            AddColumn("dbo.UserCustomerRoles", "Tracking_CreatedDateTimeUtc", c => c.DateTime(nullable: false));
            AddColumn("dbo.UserCustomerRoles", "Tracking_LastModifiedBy", c => c.Guid());
            AddColumn("dbo.UserCustomerRoles", "Tracking_ModifiedDateTimeUtc", c => c.DateTime());
            AddColumn("dbo.Departments", "Tracking_CreatedBy", c => c.Guid(nullable: false));
            AddColumn("dbo.Departments", "Tracking_CreatedDateTimeUtc", c => c.DateTime(nullable: false));
            AddColumn("dbo.Departments", "Tracking_LastModifiedBy", c => c.Guid());
            AddColumn("dbo.Departments", "Tracking_ModifiedDateTimeUtc", c => c.DateTime());
            AddColumn("dbo.MailAttachments", "Tracking_CreatedBy", c => c.Guid(nullable: false));
            AddColumn("dbo.MailAttachments", "Tracking_CreatedDateTimeUtc", c => c.DateTime(nullable: false));
            AddColumn("dbo.MailAttachments", "Tracking_LastModifiedBy", c => c.Guid());
            AddColumn("dbo.MailAttachments", "Tracking_ModifiedDateTimeUtc", c => c.DateTime());
            AddColumn("dbo.MailMessages", "CauseTrackingId", c => c.Int(nullable: false));
            AddColumn("dbo.MailMessages", "Tracking_CreatedBy", c => c.Guid(nullable: false));
            AddColumn("dbo.MailMessages", "Tracking_CreatedDateTimeUtc", c => c.DateTime(nullable: false));
            AddColumn("dbo.MailMessages", "Tracking_LastModifiedBy", c => c.Guid());
            AddColumn("dbo.MailMessages", "Tracking_ModifiedDateTimeUtc", c => c.DateTime());
            DropColumn("dbo.AspNetUsers", "CreatedDateTimeUtc");
            DropColumn("dbo.AspNetUsers", "CreatedBy");
            DropColumn("dbo.AspNetUsers", "LastModifiedBy");
            DropColumn("dbo.AspNetUsers", "ModifiedDateTimeUtc");
            DropColumn("dbo.Customers", "CreatedBy");
            DropColumn("dbo.Customers", "CreatedDateTimeUtc");
            DropColumn("dbo.Customers", "LastModifiedBy");
            DropColumn("dbo.Customers", "ModifiedDateTimeUtc");
            DropColumn("dbo.UserCustomerRoles", "CreatedBy");
            DropColumn("dbo.UserCustomerRoles", "CreatedDateTimeUtc");
            DropColumn("dbo.UserCustomerRoles", "LastModifiedBy");
            DropColumn("dbo.UserCustomerRoles", "ModifiedDateTimeUtc");
            DropColumn("dbo.Departments", "CreatedBy");
            DropColumn("dbo.Departments", "CreatedDateTimeUtc");
            DropColumn("dbo.Departments", "LastModifiedBy");
            DropColumn("dbo.Departments", "ModifiedDateTimeUtc");
            DropColumn("dbo.MailAttachments", "CreatedBy");
            DropColumn("dbo.MailAttachments", "CreatedDateTimeUtc");
            DropColumn("dbo.MailAttachments", "LastModifiedBy");
            DropColumn("dbo.MailAttachments", "ModifiedDateTimeUtc");
            DropColumn("dbo.MailMessages", "LeadTrackingId");
            DropColumn("dbo.MailMessages", "CreatedBy");
            DropColumn("dbo.MailMessages", "CreatedDateTimeUtc");
            DropColumn("dbo.MailMessages", "LastModifiedBy");
            DropColumn("dbo.MailMessages", "ModifiedDateTimeUtc");
            DropTable("dbo.CustomerLeadStatusDetails");
            DropTable("dbo.CustomerLeadStatus");
        }
        
        public override void Down()
        {
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
                .PrimaryKey(t => t.LeadTrackingId);
            
            CreateTable(
                "dbo.CustomerLeadStatusDetails",
                c => new
                    {
                        TransactionId = c.Int(nullable: false, identity: true),
                        LeadStatus = c.String(),
                        LeadTrackingId = c.Int(nullable: false),
                        UserComments = c.String(),
                        CreatedBy = c.Guid(nullable: false),
                        CreatedDateTimeUtc = c.DateTime(nullable: false),
                        LastModifiedBy = c.Guid(),
                        ModifiedDateTimeUtc = c.DateTime(),
                    })
                .PrimaryKey(t => t.TransactionId);
            
            AddColumn("dbo.MailMessages", "ModifiedDateTimeUtc", c => c.DateTime());
            AddColumn("dbo.MailMessages", "LastModifiedBy", c => c.Guid());
            AddColumn("dbo.MailMessages", "CreatedDateTimeUtc", c => c.DateTime(nullable: false));
            AddColumn("dbo.MailMessages", "CreatedBy", c => c.Guid(nullable: false));
            AddColumn("dbo.MailMessages", "LeadTrackingId", c => c.Int(nullable: false));
            AddColumn("dbo.MailAttachments", "ModifiedDateTimeUtc", c => c.DateTime());
            AddColumn("dbo.MailAttachments", "LastModifiedBy", c => c.Guid());
            AddColumn("dbo.MailAttachments", "CreatedDateTimeUtc", c => c.DateTime(nullable: false));
            AddColumn("dbo.MailAttachments", "CreatedBy", c => c.Guid(nullable: false));
            AddColumn("dbo.Departments", "ModifiedDateTimeUtc", c => c.DateTime());
            AddColumn("dbo.Departments", "LastModifiedBy", c => c.Guid());
            AddColumn("dbo.Departments", "CreatedDateTimeUtc", c => c.DateTime(nullable: false));
            AddColumn("dbo.Departments", "CreatedBy", c => c.Guid(nullable: false));
            AddColumn("dbo.UserCustomerRoles", "ModifiedDateTimeUtc", c => c.DateTime());
            AddColumn("dbo.UserCustomerRoles", "LastModifiedBy", c => c.Guid());
            AddColumn("dbo.UserCustomerRoles", "CreatedDateTimeUtc", c => c.DateTime(nullable: false));
            AddColumn("dbo.UserCustomerRoles", "CreatedBy", c => c.Guid(nullable: false));
            AddColumn("dbo.Customers", "ModifiedDateTimeUtc", c => c.DateTime());
            AddColumn("dbo.Customers", "LastModifiedBy", c => c.Guid());
            AddColumn("dbo.Customers", "CreatedDateTimeUtc", c => c.DateTime(nullable: false));
            AddColumn("dbo.Customers", "CreatedBy", c => c.Guid(nullable: false));
            AddColumn("dbo.AspNetUsers", "ModifiedDateTimeUtc", c => c.DateTime());
            AddColumn("dbo.AspNetUsers", "LastModifiedBy", c => c.Guid());
            AddColumn("dbo.AspNetUsers", "CreatedBy", c => c.Guid(nullable: false));
            AddColumn("dbo.AspNetUsers", "CreatedDateTimeUtc", c => c.DateTime(nullable: false));
            DropForeignKey("dbo.CustomerCaseStatusEntries", "CaseId", "dbo.CustomerCases");
            DropIndex("dbo.CustomerCaseStatusEntries", new[] { "CaseId" });
            DropColumn("dbo.MailMessages", "Tracking_ModifiedDateTimeUtc");
            DropColumn("dbo.MailMessages", "Tracking_LastModifiedBy");
            DropColumn("dbo.MailMessages", "Tracking_CreatedDateTimeUtc");
            DropColumn("dbo.MailMessages", "Tracking_CreatedBy");
            DropColumn("dbo.MailMessages", "CauseTrackingId");
            DropColumn("dbo.MailAttachments", "Tracking_ModifiedDateTimeUtc");
            DropColumn("dbo.MailAttachments", "Tracking_LastModifiedBy");
            DropColumn("dbo.MailAttachments", "Tracking_CreatedDateTimeUtc");
            DropColumn("dbo.MailAttachments", "Tracking_CreatedBy");
            DropColumn("dbo.Departments", "Tracking_ModifiedDateTimeUtc");
            DropColumn("dbo.Departments", "Tracking_LastModifiedBy");
            DropColumn("dbo.Departments", "Tracking_CreatedDateTimeUtc");
            DropColumn("dbo.Departments", "Tracking_CreatedBy");
            DropColumn("dbo.UserCustomerRoles", "Tracking_ModifiedDateTimeUtc");
            DropColumn("dbo.UserCustomerRoles", "Tracking_LastModifiedBy");
            DropColumn("dbo.UserCustomerRoles", "Tracking_CreatedDateTimeUtc");
            DropColumn("dbo.UserCustomerRoles", "Tracking_CreatedBy");
            DropColumn("dbo.Customers", "Tracking_ModifiedDateTimeUtc");
            DropColumn("dbo.Customers", "Tracking_LastModifiedBy");
            DropColumn("dbo.Customers", "Tracking_CreatedDateTimeUtc");
            DropColumn("dbo.Customers", "Tracking_CreatedBy");
            DropColumn("dbo.AspNetUsers", "Tracking_ModifiedDateTimeUtc");
            DropColumn("dbo.AspNetUsers", "Tracking_LastModifiedBy");
            DropColumn("dbo.AspNetUsers", "Tracking_CreatedDateTimeUtc");
            DropColumn("dbo.AspNetUsers", "Tracking_CreatedBy");
            DropTable("dbo.CustomerCases");
            DropTable("dbo.CustomerCaseStatusEntries");
            CreateIndex("dbo.MailMessages", "LastModifiedBy");
            CreateIndex("dbo.MailMessages", "CreatedBy");
            CreateIndex("dbo.MailAttachments", "LastModifiedBy");
            CreateIndex("dbo.MailAttachments", "CreatedBy");
            CreateIndex("dbo.Departments", "LastModifiedBy");
            CreateIndex("dbo.Departments", "CreatedBy");
            CreateIndex("dbo.UserCustomerRoles", "LastModifiedBy");
            CreateIndex("dbo.UserCustomerRoles", "CreatedBy");
            CreateIndex("dbo.Customers", "LastModifiedBy");
            CreateIndex("dbo.Customers", "CreatedBy");
            CreateIndex("dbo.CustomerLeadStatus", "LastModifiedBy");
            CreateIndex("dbo.CustomerLeadStatus", "CreatedBy");
            CreateIndex("dbo.CustomerLeadStatusDetails", "LastModifiedBy");
            CreateIndex("dbo.CustomerLeadStatusDetails", "CreatedBy");
            CreateIndex("dbo.CustomerLeadStatusDetails", "LeadTrackingId");
            AddForeignKey("dbo.MailMessages", "LastModifiedBy", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.MailMessages", "CreatedBy", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.MailAttachments", "LastModifiedBy", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.MailAttachments", "CreatedBy", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Departments", "LastModifiedBy", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Departments", "CreatedBy", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.UserCustomerRoles", "LastModifiedBy", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.UserCustomerRoles", "CreatedBy", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Customers", "LastModifiedBy", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Customers", "CreatedBy", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.CustomerLeadStatus", "LastModifiedBy", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.CustomerLeadStatusDetails", "LeadTrackingId", "dbo.CustomerLeadStatus", "LeadTrackingId");
            AddForeignKey("dbo.CustomerLeadStatus", "CreatedBy", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.CustomerLeadStatusDetails", "LastModifiedBy", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.CustomerLeadStatusDetails", "CreatedBy", "dbo.AspNetUsers", "Id");
        }
    }
}
