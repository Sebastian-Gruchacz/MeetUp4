namespace MeetUp.DAL
{
    using MeetUp.Model;

    /// <summary>
    /// It's just raw repository, better do CRUD with extensions
    /// </summary>
    public class CustomerCaseRepository : BaseRepository<CustomerCase>, ICustomerCaseRepository
    {
    }
}