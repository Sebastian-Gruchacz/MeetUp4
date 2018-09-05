namespace LeadService
{
    using MeetUp.Model;

    using OrderService;

    public interface ICustomerLeadStatusService
    {
        CustomerLeadServiceResponse SaveCustomerLead(CustomerLeadStatus leadStatus);
    }
}