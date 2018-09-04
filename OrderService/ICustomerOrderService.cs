namespace OrderService
{
    using System.Collections.Generic;

    using MeetUp.Model;

    public interface ICustomerOrderService
    {
        List<Customer_Order> GetNewOrders(int supplierId);
        void SaveCustomerOrder(Customer_Order customerOrder);
    }
}