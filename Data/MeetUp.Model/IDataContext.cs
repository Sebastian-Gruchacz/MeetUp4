namespace MeetUp.Model
{
    using System.Data.Entity;

    public interface IDataContext
    {
        DbSet<Customer_Order> CustomerOrders { get; }
        DbSet<OrderLine> CustomerOrderLines { get; }
        DbSet<CustomerOrderAttachment> CustomerOrderAttachments { get; }
        DbSet<Customer> Customers { get; }
        DbSet<Supplier> Suppliers { get; }
        DbSet<Department> Departments { get; }
        DbSet<SupplierOrderPolicy> SupplierOrderPolicies { get; }
        DbSet<AspNetRole> AspNetRoles { get; }
        DbSet<AspNetUser> AspNetUsers { get; }
        DbSet<MailMessage> MailMessages { get; }
        DbSet<MailAttachment> MailMessageAttachments { get; }
        DbSet<UserCustomerRole> UserCustomerRoles { get; }
        DbSet<CustomerCase> CustomerCases { get; }
        DbSet<CustomerCaseStatusEntry> CustomerCaseHistoryLogs { get; }
        DbSet<OrderFormHistory> OrderFormHistories { get; }
        DbSet<SupplierCustomerNumber> SupplierCustomerNumbers { get; }
        DbSet<CaseField> CaseFields { get; }
        DbSet<SupplierCaseField> SupplierCaseFields { get; }
    }
}