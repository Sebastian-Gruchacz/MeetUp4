namespace MeetUp.Model
{
    using System.Data.Common;
    using System.Data.Entity;
    using System.Data.Entity.ModelConfiguration.Conventions;

    using NLog;

    public partial class MeetUpDbContext : DbContext, IFullDataContext
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Do not directly use this constructor - it's for migrations only
        /// </summary>
        public MeetUpDbContext() : base("name=MigrationDbContext")
        {
            Logger.Trace("Migration context created.");
        }

        public MeetUpDbContext(DbConnection connection) : base(connection, true)
        {
            // Could potentially debug some more info from connection, but OFC be careful to not expose security-critical data like password...
            Logger.Trace($"Regular context created for {connection.Database}");
        }

        protected override void Dispose(bool disposing)
        {
            Logger.Trace("Context disposed.");

            base.Dispose(disposing);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
        }

        public virtual DbSet<Customer_Order> CustomerOrders { get; set; }

        public virtual DbSet<OrderLine> CustomerOrderLines { get; set; }

        public virtual DbSet<CustomerOrderAttachment> CustomerOrderAttachments { get; set; }

        public virtual DbSet<Customer> Customers { get; set; }

        public virtual DbSet<Supplier> Suppliers { get; set; }

        public virtual DbSet<Department> Departments { get; set; }

        public virtual DbSet<SupplierOrderPolicy> SupplierOrderPolicies { get; set; }

        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }

        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }

        public virtual DbSet<MailMessage> MailMessages { get; set; }

        public virtual DbSet<MailAttachment> MailMessageAttachments { get; set; }

        public virtual DbSet<UserCustomerRole> UserCustomerRoles { get; set; }

        public virtual DbSet<CustomerCase> CustomerCases { get; set; }

        public virtual DbSet<CustomerCaseStatusEntry> CustomerCaseHistoryLogs { get; set; }

        public virtual DbSet<OrderFormHistory> OrderFormHistories { get; set; }

        public virtual DbSet<SupplierCustomerNumber> SupplierCustomerNumbers { get; set; }

        public virtual DbSet<CaseField> CaseFields { get; set; }

        public virtual DbSet<SupplierCaseField> SupplierCaseFields { get; set; }


        // ... more entities come here ...
    }
}
