namespace LeadService
{
    using MeetUp.Model;

    public interface ICustomerLeadStatusService
    {
        CustomerLeadServiceResponse SaveCustomerLead(CustomerLeadStatus leadStatus);
    }
}