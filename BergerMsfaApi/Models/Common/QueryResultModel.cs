using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BergerMsfaApi.Models.Common
{
    public class QueryResultModel<T>
    {
        public int Total { get; set; }
        public int TotalFilter { get; set; }
        public IList<T> Items { get; set; }
    }
}