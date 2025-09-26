using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Finsight.Core.Dao
{
    public class JoinedDao<TData, TProperty> : EntityDao<TData>, IJoinedDao<TData, TProperty>
        where TData : EntityData
        where TProperty : EntityData
    {
        private readonly Func<DbContext, IIncludableQueryable<TData, TProperty>> _incQuery;
        public JoinedDao(IDataSource source, Func<DbContext, IQueryable<TData>> query,
            Expression<Func<TData, TProperty>> expression) : base(source)
        {
            ArgumentNullException.ThrowIfNull(query);
            ArgumentNullException.ThrowIfNull(expression);
            Query = _incQuery = db => query(db).Include(expression);
        }

        public IThenJoinedDao<TData, TProperty, TNextProperty> ThenJoin<TNextProperty>(Expression<Func<TProperty, TNextProperty>> expression) where TNextProperty : EntityData
        {
            ArgumentNullException.ThrowIfNull(expression);
            return new ThenJoinedDao<TData, TProperty, TNextProperty>(DataSource, _incQuery, expression);
        }
    }
}
