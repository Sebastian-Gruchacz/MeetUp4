namespace MeetUp.DAL
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Validation;
    using System.Linq;
    using System.Linq.Expressions;

    using MeetUp.Common;
    using MeetUp.Model;

    using NLog;

    public abstract class BaseRepository<T> : IRepository<T> where T : class
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        private readonly Lazy<MeetUpDbContext> _context;

        /// <summary>
        /// .ctor
        /// </summary>
        /// <remarks>This is really bad - each repo has separate instance of context, cannot simple join data from different DbSets.</remarks>
        protected BaseRepository()
        {
            _context = new Lazy<MeetUpDbContext>(() => new MeetUpDbContext());
        }

        /// <summary>
        /// Injection .ctor
        /// </summary>
        /// <remarks>This is slightly better, to be used with factory, but this has to be controlled by consumer code</remarks>
        protected BaseRepository(MeetUpDbContext context)
        {
            _context = new Lazy<MeetUpDbContext>(() => context);
        }

        protected MeetUpDbContext GetContext()
        {
            return _context.Value;
        }

        public IQueryable<T> All => _context.Value.Set<T>();

        public IQueryable<T> AllIncluding(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _context.Value.Set<T>();

            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            return query;
        }

        public T Find(object id)
        {
            return _context.Value.Set<T>().Find(id);
        }

        public virtual void Insert(T entity)
        {
            try
            {
                _context.Value.Set<T>().Add(entity);
            }
            catch (DbEntityValidationException e)
            {
                Logger.Fatal(e);

                foreach (var eve in e.EntityValidationErrors)
                {
                    Logger.Fatal($"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation errors:");
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Logger.Fatal($"- Property: \"{ve.PropertyName}\", Error: \"{ve.ErrorMessage}\"");
                    }
                }

                throw;
            }
        }

        public virtual void Attach(T entity)
        {
            _context.Value.Set<T>().Attach(entity);
        }

        public virtual void Update(T entity)
        {
            // Ensure only modified fields are updated.
            try
            {
                _context.Value.Set<T>().Attach(entity);
                var dbEntityEntry = _context.Value.Entry<T>(entity);
                foreach (var property in dbEntityEntry.OriginalValues.PropertyNames)
                {
                    var current = dbEntityEntry.CurrentValues.GetValue<object>(property);
                    if (current != null)
                        dbEntityEntry.Property(property).IsModified = true;
                }

                _context.Value.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                Logger.Fatal(e);

                // TODO: fix evident duplicate...

                foreach (var eve in e.EntityValidationErrors)
                {
                    Logger.Fatal($"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation errors:");
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Logger.Fatal($"- Property: \"{ve.PropertyName}\", Error: \"{ve.ErrorMessage}\"");
                    }
                }

                throw;
            }
        }

        public void Delete(int id)
        {
            var e = _context.Value.Set<T>().Find(id);
            if (e != null)
            {
                _context.Value.Set<T>().Remove(e);
            }
        }

        public void Delete(T obj)
        {
            var e = _context.Value.Set<T>().Find(obj);
            if (e != null)
            {
                _context.Value.Set<T>().Remove(e);
            }
        }

        public void Delete(Guid id)
        {
            var e = _context.Value.Set<T>().Find(id);
            if (e != null)
            {
                _context.Value.Set<T>().Remove(e);
            }
        }

        public void Dispose()
        {
            if (_context.IsValueCreated)
            {
                _context.Value?.Dispose();
            }
        }

        public void SaveChanges()
        {
            try
            {
                _context.Value.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                Logger.Fatal(e);

                // TODO: another duplicate

                foreach (var eve in e.EntityValidationErrors)
                {
                    Logger.Fatal($"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation errors:");
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Logger.Fatal($"- Property: \"{ve.PropertyName}\", Error: \"{ve.ErrorMessage}\"");
                    }
                }

                throw;
            }
        }
    }
}