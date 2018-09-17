namespace MeetUp.DalBase
{
    using System;
    using System.Data.Entity.Infrastructure;

    using MeetUp.Model;

    public class UnitOfWorkFactoryWithPermissions : IUnitOfWorkFactoryWithPermissions
    {
        private readonly IDbContextFactory<MeetUpDbContext> _factory;

        public UnitOfWorkFactoryWithPermissions(IDbContextFactory<MeetUpDbContext> factory)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        public IReadOnlyUnitOfWork StartReadOnlyUnit(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                throw new ArgumentOutOfRangeException(nameof(userId), $"{nameof(userId)} cannot be empty.");
            }

            return new ReadOnlyUnitOfWorkWithPermissions(_factory.Create(), userId);
        }

        public IUnitOfWork StartUnit(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                throw new ArgumentOutOfRangeException(nameof(userId), $"{nameof(userId)} cannot be empty.");
            }

            return new UnitOfWorkWithPermissions(_factory.Create(), userId);
        }
    }
}