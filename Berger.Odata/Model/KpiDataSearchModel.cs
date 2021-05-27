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
        public List<string> Zones { get; set; }
    }

    public class TerritoryTargetAchievementSearchModel : KpiDataSearchModel
    {
        
    }
    
    public class DealerWiseTargetAchievementSearchModel : KpiDataSearchModel
    {
        public string CustomerNo { get; set; }
    }

    public class ProductWiseTargetAchievementSearchModel : KpiDataSearchModel
    {
        public int ResultType { get; set; }
    }

}
