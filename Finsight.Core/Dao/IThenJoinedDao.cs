using System;
using System.Linq.Expressions;

namespace Finsight.Core.Dao
{
    public interface IThenJoinedDao<TData, TPreviousProperty, TProperty> : IDao<TData>
        where TData : EntityData
        where TPreviousProperty : EntityData
        where TProperty : EntityData
    {
        IThenJoinedDao<TData, TProperty, TNextProperty> ThenJoin<TNextProperty>(
            Expression<Func<TProperty, TNextProperty>> expression)
            where TNextProperty : EntityData;
    }
}
