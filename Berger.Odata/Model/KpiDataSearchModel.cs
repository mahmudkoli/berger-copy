using System;
using System.Collections.Generic;
using System.Text;

namespace Berger.Odata.Model
{
    public class KpiDataSearchModel 
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string DepotId { get; set; }
        public List<string> Territories { get; set; }
        public List<string> Zones { get; set; }
    }

    public class TerritoryTargetAchievementSearchModel : KpiDataSearchModel
    {
        
    }
    
    public class DealerWiseTargetAchievementSearchModel : KpiDataSearchModel
    {
        public int DealerId { get; set; }
    }

    public class ProductWiseTargetAchievementSearchModel : KpiDataSearchModel
    {
        public int resutType { get; set; }
    }

}
