namespace MeetUp.DalBase
{
    using System;

    using MeetUp.Model;

    using NLog;

    /// <summary>
    /// IImplements read-only unit-of-work pattern.
    /// </summary>
    /// <remarks>This one is pretty straightforward and simple.</remarks>
    public class ReadOnlyUnitOfWork : IReadOnlyUnitOfWork
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        private readonly IFullDataContext _context;

        public ReadOnlyUnitOfWork(IFullDataContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Here access your data sets.
        /// </summary>
        public IDataContext Context => _context;

        protected IFullDataContext InnerContext => _context;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }
    }
}