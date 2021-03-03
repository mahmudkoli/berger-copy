using Berger.Common.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BergerMsfaApi.Models.Brand
{
    public class AppBrandSearchModel
    {
        public string MaterialDescription { get; set; }
        public int? PageNo { get; set; }
        public int? PageSize { get; set; }
    }
}
