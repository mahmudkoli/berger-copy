using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Berger.Worker.Common
{
    public class GetGenericNewAndDeletedDataFromTwoList<TEntity> where TEntity:class
    {
        public async Task<(List<int>, List<TEntity>)> GetNewDataFromApiDataList(Expression<Func<TEntity, int>> predicate, List<TEntity> firstList, TEntity[] secondList)
        {
            var newKeyInApiList = firstList.Select(predicate.Compile())
                .Except(secondList.Select(predicate.Compile())).ToList();

            string key = predicate.Body.ToString().Split('.')[1];
            var property = typeof(TEntity).GetProperty(key, BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.Instance);
            var newDataFromApi = firstList.Where(a=> newKeyInApiList.Contains((int)property.GetValue(a))).ToList();
            newDataFromApi = newDataFromApi.GroupBy(predicate.Compile()).Select(y => y.FirstOrDefault()).ToList();
           
            return (newKeyInApiList, newDataFromApi);
        }

        public async Task<(List<int>, List<TEntity>)> GetDeletedData(Expression<Func<TEntity, int>> predicate, List<TEntity> firstList, TEntity[] secondList)
        {
            var deletedKeysInApiList = secondList.Select(predicate.Compile())
                .Except(firstList.Select(predicate.Compile())).ToList();

            string key = predicate.Body.ToString().Split('.')[1];
            var property = typeof(TEntity).GetProperty(key, BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.Instance);
            var deletedDataInApi = firstList.Where(a => deletedKeysInApiList.Contains((int)property.GetValue(a))).ToList();
            deletedDataInApi = deletedDataInApi.GroupBy(predicate.Compile()).Select(y => y.FirstOrDefault()).ToList();

            return (deletedKeysInApiList, deletedDataInApi);
        }
    }
}
