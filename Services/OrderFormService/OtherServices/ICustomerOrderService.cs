namespace OrderFormService.OtherServices
{
    using System;
    using System.Collections.Generic;

    using MeetUp.Model;

    public interface ICustomerOrderService : IBaseService<Customer_Order>
    {
        Customer_Order GetNewOrders(Guid orderId);

        int GetOrderItemSumForCustomer(List<int> supplierIds, int companyId);
    }
}