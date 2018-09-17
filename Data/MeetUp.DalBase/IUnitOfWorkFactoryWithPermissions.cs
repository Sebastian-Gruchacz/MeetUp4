namespace MeetUp.DalBase
{
    using System;

    /// <summary>
    /// Experimental concept...
    /// </summary>
    /// <remarks>By adding user we can use it to decorate queries with permission checks the same way we can use ITrackeable.
    /// Or no? It depends how complicate it would be to work on Includes...</remarks>
    public interface IUnitOfWorkFactoryWithPermissions
    {
        IReadOnlyUnitOfWork StartReadOnlyUnit(Guid userId);

        IUnitOfWork StartUnit(Guid userId);
    }
}