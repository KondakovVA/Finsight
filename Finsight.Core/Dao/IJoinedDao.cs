using System;
using System.Linq.Expressions;

namespace Finsight.Core.Dao
{
    public interface IJoinedDao<TData, TProperty> : IDao<TData>
        where TData : EntityData
        where TProperty : EntityData
    {
        IThenJoinedDao<TData, TProperty, TNextProperty> ThenJoin<TNextProperty>(
            Expression<Func<TProperty, TNextProperty>> expression)
            where TNextProperty : EntityData;
    }
}
