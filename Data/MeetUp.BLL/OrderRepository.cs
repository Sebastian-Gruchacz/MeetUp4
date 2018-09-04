// ReSharper disable once CheckNamespace (Incorporate repository as fluent extensions)
namespace MeetUp.Model
{
    using System;
    using System.Linq;

    /// <summary>
    /// This is improved repository pattern to work with UnitOfWork
    /// </summary>
    public static class OrderRepository
    {
        public static IQueryable<Order> ForCustomer(this IQueryable<Order> orders, Customer customer)
        {
            return orders.Where(o => o.CustomerId == customer.Id);
        }

        public static IQueryable<Order> AreActive(this IQueryable<Order> orders)
        {
            return orders.Where(o => !o.Deleted);
        }

        public static IQueryable<Order> WerePlacedBefore(this IQueryable<Order> orders, DateTime date)
        {
            return orders.Where(o => o.Created < date);
        }
    }
}
