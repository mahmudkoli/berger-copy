using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.OData.Edm;

namespace Berger.Odata.Model
{
    public class CollectionHistorySearchModel
    {
        public string CustomerNo { get; set; }
        public string Division { get; set; }
    }

    public class OutstandingDetailsSearchModel
    {
        public string CustomerNo { get; set; }
        public string Division { get; set; }
        public EnumOutstandingDetailsDaysCount Days { get; set; }
    }

    public class OutstandingSummarySearchModel
    {
        public string CustomerNo { get; set; }
    }

    public enum EnumOutstandingDetailsDaysCount
    {
        _All_Days = 1,
        _0_To_30_Days = 2,
        _31_To_60_Days = 3,
        _61_To_90_Days = 4,
        _GT_90_Days = 5,
    }
}
