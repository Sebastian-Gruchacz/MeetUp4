namespace MeetUp.DalBase
{
    using System;

    using MeetUp.Model;

    public interface IReadOnlyUnitOfWork : IDisposable
    {
        IDataContext Context { get; }
    }
}