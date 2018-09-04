namespace OrderService
{
    using System;
    using System.Collections.Generic;

    public interface ICustomerOrder
    {
        Boolean ResendEmail(List<int> leadIds);

        Boolean SendCustomerOrders(int dayTotalHours, string emailSubject, int dayTotalMinutes, Boolean isHourBased, string orderFromEmail, string supportEmail);
    }
}