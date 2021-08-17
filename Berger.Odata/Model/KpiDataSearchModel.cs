using Berger.Common.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Berger.Odata.Model
{
    public class KpiDataSearchModel 
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string Depot { get; set; }
        public List<string> SalesGroups { get; set; }
        public List<string> Territories { get; set; }
    }

    public class SalesTargetAchievementSearchModel : KpiDataSearchModel
    {

    }
    
    public class DealerWiseTargetAchievementSearchModel : KpiDataSearchModel
    {
        public string CustomerNo { get; set; }
    }

    public class ProductWiseTargetAchievementSearchModel : KpiDataSearchModel
    {
        public List<string> Brands { get; set; }
        public string Division { get; set; }
        public KpiResultType ResultType { get; set; }
    }

}
