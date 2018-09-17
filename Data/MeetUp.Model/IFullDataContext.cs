namespace MeetUp.Model
{
    using System;
    using System.Data.Entity.Infrastructure;

    public interface IFullDataContext : IDataContext, IDisposable
    {
        int SaveChanges();

        DbChangeTracker ChangeTracker { get; }
    }
}