namespace MeetUp.Common
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    public interface IRepository<T> : IDisposable
    {
        IQueryable<T> All { get; }

        IQueryable<T> AllIncluding(params Expression<Func<T, object>>[] includeProperties);

        T Find(object id);

        void Insert(T entity);

        void Update(T entity);

        void Delete(int id);

        void Delete(T obj);

        void Delete(Guid id);

        void Attach(T entity);

        void SaveChanges(); // This is so weak... Need Unit-of-Work
    }
}