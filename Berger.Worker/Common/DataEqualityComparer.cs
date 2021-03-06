using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace Berger.Worker.Common
{
    public class DataEqualityComparer<TEntity>: IDataEqualityComparer<TEntity> where TEntity:class
    {
#pragma warning disable 1998
        public async Task<(List<string>, List<TEntity>)> GetNewDatasetOfApi(Expression<Func<TEntity, string>> predicate,
#pragma warning restore 1998
            List<TEntity> firstList,
            List<TEntity> secondList)
        {
            var newKeyInApiList = firstList.Select(predicate.Compile())
                .Except(secondList.Select(predicate.Compile())).ToList();

            string key = predicate.Body.ToString().Split('.')[1];
            var property = typeof(TEntity).GetProperty(key, BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.Instance);
            var newDataFromApi = firstList.Where(a=> newKeyInApiList.Contains((string)property.GetValue(a))).ToList();
            //newDataFromApi = newDataFromApi.GroupBy(predicate.Compile()).Select(y => y.FirstOrDefault()).ToList();
           
            return (newKeyInApiList, newDataFromApi);
        }



        public async Task<(List<string>, List<TEntity>)> GetDeletedDataOfApi(Expression<Func<TEntity, string>> predicate, List<TEntity> firstList, List<TEntity> secondList)
        {
            var deletedKeysInApiList = secondList.Select(predicate.Compile())
                .Except(firstList.Select(predicate.Compile())).ToList();

            string key = predicate.Body.ToString().Split('.')[1];
            var property = typeof(TEntity).GetProperty(key, BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.Instance);
            var deletedDataInApi = secondList.Where(a => property is { } && deletedKeysInApiList.Contains((string)property.GetValue(a))).ToList();
            //deletedDataInApi = deletedDataInApi.GroupBy(predicate.Compile()).Select(y => y.FirstOrDefault()).ToList();

            return (deletedKeysInApiList, deletedDataInApi);
        }

        public async Task<(List<int>, List<TEntity>)> GetNewDatasetOfApi(Expression<Func<TEntity, int>> predicate, List<TEntity> firstList, List<TEntity> secondList)
        {
            var newKeyInApiList = firstList.Select(predicate.Compile())
                .Except(secondList.Select(predicate.Compile())).ToList();

            string key = predicate.Body.ToString().Split('.')[1];
            var property = typeof(TEntity).GetProperty(key, BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.Instance);
            var newDataFromApi = firstList.Where(a => newKeyInApiList.Contains((int)property.GetValue(a))).ToList();
            //newDataFromApi = newDataFromApi.GroupBy(predicate.Compile()).Select(y => y.FirstOrDefault()).ToList();

            return (newKeyInApiList, newDataFromApi);
        }

        public async Task<(List<int>, List<TEntity>)> GetDeletedDataOfApi(Expression<Func<TEntity, int>> predicate, List<TEntity> firstList, List<TEntity> secondList)
        {
            var deletedKeysInApiList = secondList.Select(predicate.Compile())
                .Except(firstList.Select(predicate.Compile())).ToList();

            string key = predicate.Body.ToString().Split('.')[1];
            var property = typeof(TEntity).GetProperty(key, BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.Instance);
            var deletedDataInApi = secondList.Where(a => deletedKeysInApiList.Contains((int)property.GetValue(a))).ToList();
            //deletedDataInApi = deletedDataInApi.GroupBy(predicate.Compile()).Select(y => y.FirstOrDefault()).ToList();

            return (deletedKeysInApiList, deletedDataInApi);
        }

    }
}
