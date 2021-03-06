﻿namespace MeetUp.DalBase
{
    using System;

    using MeetUp.Model;

    using NLog;

    public class UnitOfWork : ReadOnlyUnitOfWork, IUnitOfWork
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        private bool _saveCalled;
        private bool _rollBackCalled;

        public UnitOfWork(IFullDataContext context) : base(context)
        {
            // TODO: add transaction on InnerContext, pass transaction scope / level in the constructor
            // This would require proper strategies, maybe more methods in Factory for different optimization scenarios
        }

        public virtual void SaveChanges()
        {
            if (_saveCalled || _rollBackCalled)
            {
                throw new InvalidOperationException("This Unit-Of-Work has been already completed.");
            }

            _saveCalled = true;
            InnerContext.SaveChanges(); // TODO: add asynchronous operations as part of the exercise ;-)
        }

        public virtual void RollBack()
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
            // on the other hand - it should be wrapped in IDisposable -> using, so better both.

            bool hasChanges = InnerContext.ChangeTracker.HasChanges();
            if (hasChanges && !(_saveCalled || _rollBackCalled))
            {
                var msg = "Some work has been added to UnitOfWork after it has been completed.";
                Logger.Error(msg); // .WithStackTrace()
                throw new InvalidOperationException(msg);
            }
        }
    }
}