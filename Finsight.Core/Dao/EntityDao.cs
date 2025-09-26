using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Finsight.Core.Dao
{
    public class EntityDao<TData> : BaseDao, IDao<TData> where TData : EntityData
    {
        protected Func<DbContext, IQueryable<TData>> Query { get;  set; }

        public EntityDao(IDataSource dataSource) : base(dataSource)
        {
            Query = db => db.Set<TData>().AsNoTracking();
        }

        public virtual Task<TData?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return ReadContextAsync(db => Query(db).FirstOrDefaultAsync(s => s.Id.Equals(id), cancellationToken));
        }

        public virtual Task<bool> HasAny(CancellationToken cancellationToken = default)
        {
            return ReadContextAsync(async db =>
            {
                var set = Query(db);
                return await set.AnyAsync(cancellationToken).ConfigureAwait(false);
            });
        }

        public virtual Task<List<TData>> GetListAsync(Expression<Func<TData, bool>>? condition = null, CancellationToken cancellationToken = default)
        {
            return ReadContextAsync(async db =>
            {
                var set = Query(db);
                if (condition != null)
                {
                    set = set.Where(condition);
                }

                return await set.ToListAsync(cancellationToken).ConfigureAwait(false);
            });
        }

        public virtual Task<TData?> FirstOrDefaultAsync(Expression<Func<TData, bool>> condition, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(condition);
            return ReadContextAsync(db => Query(db).FirstOrDefaultAsync(condition, cancellationToken));
        }

        public virtual async Task CreateAsync(TData entity, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(entity);
            await UpdateContextAsync(async db =>
            {
                await db.Set<TData>().AddAsync(entity, cancellationToken).ConfigureAwait(false);
            }, cancellationToken).ConfigureAwait(false);
        }

        public virtual Task UpdateAsync(TData entity, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(entity);
            return UpdateContextAsync(db =>
            {
                db.Set<TData>().Update(entity);
                return Task.CompletedTask;
            }, cancellationToken);
        }

        public virtual async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            await UpdateContextAsync(async db =>
            {
                var set = db.Set<TData>();
                var entity = await set.FindAsync(new object[] { id }, cancellationToken).ConfigureAwait(false);
                if (entity != null)
                {
                    set.Remove(entity);
                }
            }, cancellationToken).ConfigureAwait(false);
        }

        public virtual async Task DeleteAsync(Expression<Func<TData, bool>> condition, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(condition);

            await UpdateContextAsync(async db =>
            {
                var set = db.Set<TData>();
                var entities = await set.Where(condition).ToListAsync(cancellationToken).ConfigureAwait(false);
                if (entities.Count > 0)
                {
                    set.RemoveRange(entities);
                }
            }, cancellationToken).ConfigureAwait(false);
        }

        public IJoinedDao<TData, TProperty> Join<TProperty>(Expression<Func<TData, TProperty>> expression) where TProperty : EntityData
        {
            ArgumentNullException.ThrowIfNull(expression);
            return new JoinedDao<TData, TProperty>(DataSource, Query, expression);
        }
    }
}
