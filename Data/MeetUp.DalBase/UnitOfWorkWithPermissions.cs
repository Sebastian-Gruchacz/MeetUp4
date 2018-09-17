namespace MeetUp.DalBase
{
    using System;

    using MeetUp.Model;

    public class UnitOfWorkWithPermissions : UnitOfWork
    {
        private readonly Guid _userId;

        public UnitOfWorkWithPermissions(MeetUpDbContext context) : base(context)
        {
        }

        public UnitOfWorkWithPermissions(MeetUpDbContext context, Guid userId)
            : base(new PermissionsWrapper(context, userId))
        {
            _userId = userId;
        }
    }
}