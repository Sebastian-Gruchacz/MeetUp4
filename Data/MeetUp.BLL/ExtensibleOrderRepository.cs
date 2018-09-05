// ReSharper disable once CheckNamespace (Incorporate repository as fluent extensions)
namespace MeetUp.Model
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    /// <summary>
    /// This is improved repository pattern to work with UnitOfWork
    /// </summary>
    /// <remarks>Keep queries EF-compatible!</remarks>
    public static class ExtensibleOrderRepository
    {
        public static IQueryable<Customer_Order> FromCustomer(this IQueryable<Customer_Order> orders, Customer customer)
        {
            return orders.Where(o => o.FromCustomerId == customer.Id);
        }

        public static IQueryable<Customer_Order> AreActive(this IQueryable<Customer_Order> orders)
        {
            return orders.Where(o => !o.Deleted);
        }

        public static IQueryable<Customer_Order> WerePlacedBefore(this IQueryable<Customer_Order> orders, DateTime date)
        {
            return orders.Where(o => o.DateCreatedUtc < date);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orders"></param>
        /// <returns></returns>
        /// <remarks>WARN: Not unit-testable!</remarks>
        public static IQueryable<Customer_Order> WithOrderLines(this IQueryable<Customer_Order> orders)
        {
            return orders.Include(o => o.OrderLines);
        }
    }
}
