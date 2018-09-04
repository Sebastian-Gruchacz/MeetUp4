﻿namespace MeetUp.Model
{
    using System.Data.Common;
    using System.Data.Entity;

    using NLog;

    public partial class MeetupDbContext : DbContext
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Do not directly use this constructor - it's for migrations only
        /// </summary>
        public MeetupDbContext() : base("name=MigrationDbContext")
        {
            Logger.Trace("Migration context created.");
        }

        public MeetupDbContext(DbConnection connection) : base(connection, true)
        {
            // Could potentially add more info from connection, but OFC be careful to not expose security-critical data like password...
            Logger.Trace($"Regular context created for {connection.Database}");
        }

        protected override void Dispose(bool disposing)
        {
            Logger.Trace("Context disposed.");

            base.Dispose(disposing);
        }


        public virtual DbSet<Customer_Order> CustomerOrders { get; set; }

        public virtual DbSet<CustomerOrderAttachment> CustomerOrderAttachments { get; set; }

        public virtual DbSet<Customer> Customers { get; set; }

        public virtual DbSet<Supplier> Suppliers { get; set; }

        public virtual DbSet<Department> Departments { get; set; }

        public virtual DbSet<SupplierOrderPolicy> SupplierOrderPolicies { get; set; }



        // ... more entities come here ...
    }
}