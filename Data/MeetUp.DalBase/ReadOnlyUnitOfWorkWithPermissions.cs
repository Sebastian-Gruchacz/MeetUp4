namespace MeetUp.DalBase
{
    using System;

    using MeetUp.Model;

    public class ReadOnlyUnitOfWorkWithPermissions : ReadOnlyUnitOfWork
    {
        private readonly Guid _userId;

        public ReadOnlyUnitOfWorkWithPermissions(MeetUpDbContext context, Guid userId) :
            base(new PermissionsWrapper(context, userId))
        {
            _userId = userId;
        }
    }
}