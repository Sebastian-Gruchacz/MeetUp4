namespace MeetUp.Model
{
    using System.Data.Entity;

    public interface IDataContext
    {
        DbSet<Customer_Order> CustomerOrders { get; set; }
        DbSet<OrderLine> CustomerOrderLines { get; set; }
        DbSet<CustomerOrderAttachment> CustomerOrderAttachments { get; set; }
        DbSet<Customer> Customers { get; set; }
        DbSet<Supplier> Suppliers { get; set; }
        DbSet<Department> Departments { get; set; }
        DbSet<SupplierOrderPolicy> SupplierOrderPolicies { get; set; }
        DbSet<AspNetRole> AspNetRoles { get; set; }
        DbSet<AspNetUser> AspNetUsers { get; set; }
        DbSet<MailMessage> MailMessages { get; set; }
        DbSet<MailAttachment> MailMessageAttachmentss { get; set; }
        DbSet<UserCustomerRole> UserCustomerRoles { get; set; }
        DbSet<CustomerCase> CustomerCases { get; set; }
        DbSet<CustomerCaseStatusEntry> CustomerCaseHistoryLogs { get; set; }
    }
}