using Berger.Common.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BergerMsfaApi.Models.Dealer
{
    public class AppDealerSearchModel
    {
        public string DealerName { get; set; }
        public EnumDealerCategory? DealerCategory { get; set; }
        public int? PageNo { get; set; }
        public int? PageSize { get; set; }
    }
}
