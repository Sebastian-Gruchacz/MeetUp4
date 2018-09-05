namespace LeadService
{
    using OrderService;

    public interface ICustomerLeadStatusService
    {
        CustomerLeadServiceResponse SaveCustomerLead(CustomerLeadStatus leadStatus);
    }
}