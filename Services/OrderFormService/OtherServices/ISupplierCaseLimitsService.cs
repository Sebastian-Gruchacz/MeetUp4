namespace OrderFormService.OtherServices
{
    using System.Collections.Generic;

    using MeetUp.Model;

    public interface ISupplierCaseLimitsService : IBaseService<SupplierCaseLimits>
    {
        List<SupplierCaseLimits> GetAllSupplierLimitations();
    }
}