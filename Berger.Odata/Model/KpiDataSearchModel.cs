using System;
using System.Collections.Generic;
using System.Text;

namespace Berger.Odata.Model
{
    public class KpiDataSearchModel 
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string Territory { get; set; }
    }

    public class TerritoryTargetAchievementSearchModel : KpiDataSearchModel
    {
    }
    
    public class DealerWiseTargetAchievementSearchModel : KpiDataSearchModel
    {
        public int DealerId { get; set; }
    }



}
