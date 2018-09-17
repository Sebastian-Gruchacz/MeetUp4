namespace OrderService
{
    using System.Collections.Generic;

    using MeetUp.Model;

    public interface ISupplierService
    {
        List<Supplier> GetSuppliersForSendingOrder();
    }
}