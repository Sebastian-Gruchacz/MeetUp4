// ReSharper disable once CheckNamespace (Extend DBSet - join the right name-space.)
namespace System.Data.Entity
{
    using System;

    using MeetUp.Model;

    /// <summary>
    /// This adds extensions to DbSet that realize tracking and Crud operations on repository
    /// </summary>
    public static class CrudExtensions
    {
        public static T Find<T>(this DbSet<T> entitySet, int id) where T : class
        {
            return entitySet.Find(id.ToSingleItemArray());
        }
    
        public static T Find<T>(this DbSet<T> entitySet, Guid id) where T : class
        {
            return entitySet.Find(id.ToSingleItemArray());
        }

        public static void Add<T>(this DbSet<T> entitySet, T entity) 
            where T : class, ITrackeable
        {
            if (entitySet == null)
            {
                throw new ArgumentNullException(nameof(entitySet));
            }
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            if (entity.Tracking == null)
            {
                throw new ArgumentException($"'{nameof(ITrackeable.Tracking)}' property of entity has not been set.", nameof(entity));
            }

            if (entity.Tracking.CreatedBy == Guid.Empty)
            {
                throw new ArgumentException($"'{nameof(EntityTracker.CreatedBy)}' property of entity tracking has not been set.", nameof(entity));
            }

            if (entity.Tracking.CreatedDateTimeUtc == default(DateTime))
            {
                entity.Tracking.CreatedDateTimeUtc  = DateTime.UtcNow;
            }

            // TODO: add other validation checks & logging here?

            entitySet.Add(entity);  // TODO: check will there be infinity loop?
        }

        public static void Update<T>(this DbSet<T> entitySet, T entity) 
            where T : class, ITrackeable
        {
            if (entitySet == null)
            {
                throw new ArgumentNullException(nameof(entitySet));
            }
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            if (entity.Tracking == null)
            {
                throw new ArgumentException($"'{nameof(ITrackeable.Tracking)}' property of entity has not been set.", nameof(entity));
            }

            if (entity.Tracking.CreatedBy == Guid.Empty)
            {
                throw new ArgumentException($"'{nameof(EntityTracker.CreatedBy)}' property of entity tracking has not been set.", nameof(entity));
            }

            if (entity.Tracking.CreatedDateTimeUtc == default(DateTime))
            {
                entity.Tracking.CreatedDateTimeUtc = DateTime.UtcNow; // or throw?
            }

            if (!entity.Tracking.LastModifiedBy.HasValue || entity.Tracking.LastModifiedBy.Value == Guid.Empty)
            {
                throw new ArgumentException($"'{nameof(EntityTracker.LastModifiedBy)}' property of entity tracking has not been set.", nameof(entity));
            }

            if (!entity.Tracking.ModifiedDateTimeUtc.HasValue || entity.Tracking.ModifiedDateTimeUtc == default(DateTime))
            {
                entity.Tracking.ModifiedDateTimeUtc = DateTime.UtcNow;
            }

            // TODO: add other validation checks & logging here?

            entitySet.Attach(entity);
            // TODO: copy attaching code from base repo

        }

        // TODO: do more
    }
}