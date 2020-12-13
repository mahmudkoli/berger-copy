using System;
using System.Collections.Generic;
using System.Linq;


namespace BergerMsfaApi.Extensions
{
    public static class LinqExtension
    {
        public static List<TEntity> Search<TEntity>(this List<TEntity> sources, string search)
        {
            var tempSource = sources.Select((s, i) => new { key = i, Value = s });
            var stringList = tempSource.ToDictionary(f => f.key, f => f.Value.RowToString());
            var filterkeys = (from r in stringList where r.Value.Contains(search) select r.Key).ToList();
            return tempSource.Where(f => filterkeys.Contains(f.key)).Select(s => s.Value).ToList();

        }
    }
}
