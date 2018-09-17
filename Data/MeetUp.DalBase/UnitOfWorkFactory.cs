namespace MeetUp.DalBase
{
    using System.Data.Entity.Infrastructure;

    using MeetUp.Model;

    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        private readonly IDbContextFactory<MeetUpDbContext> _factory;

        public UnitOfWorkFactory(IDbContextFactory<MeetUpDbContext> factory)
        {
            _factory = factory;
        }

        public IReadOnlyUnitOfWork StartReadOnlyUnit()
        {
            return new ReadOnlyUnitOfWork(_factory.Create());
        }

        public IUnitOfWork StartUnit()
        {
            return new UnitOfWork(_factory.Create());
        }
    }
}