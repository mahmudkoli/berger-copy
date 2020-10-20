using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Berger.Worker.Common
{
    public interface IDataEqualityComparer<TEntity>
    {
        Task<(List<string>, List<TEntity>)> GetNewDatasetOfApi(Expression<Func<TEntity, string>> predicate,
            List<TEntity> firstList, List<TEntity> secondList);

        Task<(List<string>, List<TEntity>)> GetDeletedDataOfApi(Expression<Func<TEntity, string>> predicate,
            List<TEntity> firstList, List<TEntity> secondList);

        Task<(List<int>, List<TEntity>)> GetNewDatasetOfApi(Expression<Func<TEntity, int>> predicate,
            List<TEntity> firstList, List<TEntity> secondList);

        Task<(List<int>, List<TEntity>)> GetDeletedDataOfApi(Expression<Func<TEntity, int>> predicate,
            List<TEntity> firstList, List<TEntity> secondList);
    }
}
