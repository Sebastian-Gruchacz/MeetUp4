namespace MeetUp.DalBase
{
    public interface IUnitOfWorkFactory
    {
        IReadOnlyUnitOfWork StartReadOnlyUnit();

        IUnitOfWork StartUnit();
    }
}