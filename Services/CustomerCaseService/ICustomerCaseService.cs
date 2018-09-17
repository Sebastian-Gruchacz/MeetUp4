namespace CustomerCaseService
{
    using MeetUp.Model;

    public interface ICustomerCaseService
    {
        CustomerCaseServiceResponse SaveCustomerCase(CustomerCase @case);
    }
}