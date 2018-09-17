namespace OrderFormService.OtherServices
{
    using System.Collections.Generic;

    using MeetUp.Common;
    using MeetUp.Model;

    public interface ISupplierCustomerNumberRepository : IRepository<SupplierCustomerNumber>
    {
        int MemberSupplierCustomerNumbersCountBySupplierIds(List<int> supplierIds, int companyId);
    }
}