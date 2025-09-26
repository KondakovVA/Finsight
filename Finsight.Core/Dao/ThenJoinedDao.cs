using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Finsight.Core.Dao
{
    public class ThenJoinedDao<TData, TPreviousProperty, TProperty> : EntityDao<TData>, IThenJoinedDao<TData, TPreviousProperty, TProperty>
        where TData : EntityData
        where TPreviousProperty : EntityData
        where TProperty : EntityData
    {
        private readonly Func<DbContext, IIncludableQueryable<TData, TProperty>> _incQuery;
        public ThenJoinedDao(IDataSource source, Func<DbContext, IIncludableQueryable<TData, TPreviousProperty>> query,
            Expression<Func<TPreviousProperty, TProperty>> expression) : base(source)
        {
            ArgumentNullException.ThrowIfNull(query);
            ArgumentNullException.ThrowIfNull(expression);
            Query = _incQuery = db => query(db).ThenInclude(expression);
        }

        public IThenJoinedDao<TData, TProperty, TNextProperty> ThenJoin<TNextProperty>(Expression<Func<TProperty, TNextProperty>> expression) where TNextProperty : EntityData
        {
            ArgumentNullException.ThrowIfNull(expression);
            return new ThenJoinedDao<TData, TProperty, TNextProperty>(DataSource, _incQuery, expression);
        }
    }
}
