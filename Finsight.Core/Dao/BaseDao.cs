using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Finsight.Core.Dao
{
    public class BaseDao
    {
        protected BaseDao(IDataSource dataSource)
        {
            ArgumentNullException.ThrowIfNull(dataSource);
            DataSource = dataSource;
        }

        protected IDataSource DataSource { get; }

        protected DbContext Context => DataSource.GetContext();

        protected Task<TResult> ReadContextAsync<TResult>(Func<DbContext, Task<TResult>> action)
        {
            ArgumentNullException.ThrowIfNull(action);
            return action(Context);
        }

        protected Task<TResult> ReadContextAsync<TResult>(Func<DbContext, TResult> action)
        {
            ArgumentNullException.ThrowIfNull(action);
            return Task.FromResult(action(Context));
        }

        protected async Task UpdateContextAsync(Func<DbContext, Task> action, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(action);
            await action(Context).ConfigureAwait(false);
            await Context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        protected async Task UpdateContextAsync(Action<DbContext> action, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(action);
            action(Context);
            await Context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }
    }
}
