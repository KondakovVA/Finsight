using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Finsight.Core.Dao
{
    public interface IDao<TData> where TData : EntityData
    {
        Task<TData?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<List<TData>> GetListAsync(Expression<Func<TData, bool>>? condition = null, CancellationToken cancellationToken = default);
        Task<TData?> FirstOrDefaultAsync(Expression<Func<TData, bool>> condition, CancellationToken cancellationToken = default);
        Task CreateAsync(TData entity, CancellationToken cancellationToken = default);
        Task UpdateAsync(TData entity, CancellationToken cancellationToken = default);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
        Task DeleteAsync(Expression<Func<TData, bool>> condition, CancellationToken cancellationToken = default);
        Task<bool> HasAny(CancellationToken cancellationToken = default);
        IJoinedDao<TData, TProperty> Join<TProperty>(Expression<Func<TData, TProperty>> expression) where TProperty : EntityData;
    }
}
