namespace MeetUp.DalBase
{
    using System;

    using MeetUp.Model;

    using NLog;

    public sealed class UnitOfWork : ReadOnlyUnitOfWork, IUnitOfWork
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        private bool _saveCalled;
        private bool _rollBackCalled;

        public UnitOfWork(MeetUpDbContext context) : base(context)
        {
            // TODO: add transaction on InnerContext, pass transaction scope / level in the constructor
            // This would require proper strategies, maybe more methods in Factory for different optimization scenarios
        }

        public void SaveChanges()
        {
            if (_saveCalled || _rollBackCalled)
            {
                throw new InvalidOperationException("This Unit-Of-Work has been already completed.");
            }

            _saveCalled = true;
            InnerContext.SaveChanges(); // TODO: add asynchronous operations as part of the exercise ;-)
        }

        public void RollBack()
        {
            if (_saveCalled || _rollBackCalled)
            {
                throw new InvalidOperationException("This Unit-Of-Work has been already completed.");
            }

            _rollBackCalled = true;
            // TODO: rollback transaction here...
        }

        protected override void Dispose(bool disposing)
        {
            CheckForUnfinishedWork();

            base.Dispose(disposing);
        }

        private void CheckForUnfinishedWork()
        {
            // throw or log only? Probably better not throw from Dispose, but log only

            bool hasChanges = InnerContext.ChangeTracker.HasChanges();
            if (hasChanges && !(_saveCalled || _rollBackCalled))
            {
                Logger.Error("Some work has been added to UnitOfWork after it has been completed."); // .WithStackTrace()

                // throw new InvalidOperationException();
            }
        }
    }
}