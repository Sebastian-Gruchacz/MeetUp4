namespace MeetUp.DalBase
{
    public interface IUnitOfWork : IReadOnlyUnitOfWork
    {
        void SaveChanges();

        void RollBack();

    }
}
