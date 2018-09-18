namespace MeetUp.DalBase
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;

    using MeetUp.Model;

    // TODO: Add IReadOnlyContext and wrapper that returns IQueryAble<T> / IDBQuery<T> but first calls to AsNoTracking() ! + version with permissions and without

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


        public virtual DbSet<Customer_Order> CustomerOrders => _context.CustomerOrders; //.Where(o => o.Permissions. ...);
        // TODO: permissions can be also simplified here with Generic extension, then could be partially reusable perhaps for includes?
        // This still does not resolve Lazy Loading permissions, so perhaps should be forbidden when using this pattern.

        public virtual DbSet<OrderLine> CustomerOrderLines => _context.CustomerOrderLines;

        public virtual DbSet<CustomerOrderAttachment> CustomerOrderAttachments => _context.CustomerOrderAttachments;

        public virtual DbSet<Customer> Customers => _context.Customers;

        public virtual DbSet<Supplier> Suppliers => _context.Suppliers;

        public virtual DbSet<Department> Departments => _context.Departments;

        public virtual DbSet<SupplierOrderPolicy> SupplierOrderPolicies => _context.SupplierOrderPolicies;

        public virtual DbSet<AspNetRole> AspNetRoles => _context.AspNetRoles;

        public virtual DbSet<AspNetUser> AspNetUsers => _context.AspNetUsers;

        public virtual DbSet<MailMessage> MailMessages => _context.MailMessages;

        public virtual DbSet<MailAttachment> MailMessageAttachments => _context.MailMessageAttachments;

        public virtual DbSet<UserCustomerRole> UserCustomerRoles => _context.UserCustomerRoles;

        public virtual DbSet<CustomerCase> CustomerCases => _context.CustomerCases;

        public virtual DbSet<CustomerCaseStatusEntry> CustomerCaseHistoryLogs => _context.CustomerCaseHistoryLogs;

        public virtual DbSet<OrderFormHistory> OrderFormHistories => _context.OrderFormHistories;

        public virtual DbSet<SupplierCustomerNumber> SupplierCustomerNumbers => _context.SupplierCustomerNumbers;

        public virtual DbSet<CaseField> CaseFields => _context.CaseFields;

        public virtual DbSet<SupplierCaseField> SupplierCaseFields => _context.SupplierCaseFields;

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