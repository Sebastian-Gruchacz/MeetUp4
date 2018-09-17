namespace MeetUp.DalBase
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;

    using MeetUp.Model;

    public class PermissionsWrapper : IFullDataContext
    {
        private readonly MeetUpDbContext _context;
        private readonly Guid _userId;

        public PermissionsWrapper(MeetUpDbContext context, Guid userId)
        {
            _context = context;
            _userId = userId;
        }

        // here one can override DBSet<> properties to always apply permission checks whenever applicable
        // TODO: some dedicated interface and ComplexType Property could be helpful


        public DbSet<Customer_Order> CustomerOrders => _context.CustomerOrders; //.Where(o => o.Permissions. ...);
        // TODO: permissions can be also simplified here with Generic extension, then could be partially reusable perhaps for includes?
        // This still does not resolve Lazy Loading permissions, so perhaps should be forbidden when using this pattern.

        public DbSet<OrderLine> CustomerOrderLines => _context.CustomerOrderLines;

        public DbSet<CustomerOrderAttachment> CustomerOrderAttachments => _context.CustomerOrderAttachments;

        public DbSet<Customer> Customers => _context.Customers;

        public DbSet<Supplier> Suppliers => _context.Suppliers;

        public DbSet<Department> Departments => _context.Departments;

        public DbSet<SupplierOrderPolicy> SupplierOrderPolicies => _context.SupplierOrderPolicies;

        public DbSet<AspNetRole> AspNetRoles => _context.AspNetRoles;

        public DbSet<AspNetUser> AspNetUsers => _context.AspNetUsers;

        public DbSet<MailMessage> MailMessages => _context.MailMessages;

        public DbSet<MailAttachment> MailMessageAttachments => _context.MailMessageAttachments;

        public DbSet<UserCustomerRole> UserCustomerRoles => _context.UserCustomerRoles;

        public DbSet<CustomerCase> CustomerCases => _context.CustomerCases;

        public DbSet<CustomerCaseStatusEntry> CustomerCaseHistoryLogs => _context.CustomerCaseHistoryLogs;

        public DbSet<OrderFormHistory> OrderFormHistories => _context.OrderFormHistories;

        public DbSet<SupplierCustomerNumber> SupplierCustomerNumbers => _context.SupplierCustomerNumbers;

        public DbSet<CaseField> CaseFields => _context.CaseFields;

        public DbSet<SupplierCaseField> SupplierCaseFields => _context.SupplierCaseFields;

        public void Dispose()
        {
            _context.Dispose();
        }

        public int SaveChanges()
        {
            // There is another location where we could create / modify Tracking records instead of in UnitOfWork object
            // Then we could not expose ChangeTracker in IFullDataContext... tempting...
            // TODO: left as an exercise ;-)
            return _context.SaveChanges();
        }

        public DbChangeTracker ChangeTracker => _context.ChangeTracker;
    }
}