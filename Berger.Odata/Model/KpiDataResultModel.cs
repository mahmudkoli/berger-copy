using System;
using System.Collections.Generic;
using System.Text;

namespace Berger.Odata.Model
{
    public class KpiTargetAchievementResultModel
    {
        public decimal LiquidTargetInGallons { get; set; }
        public decimal LiquidActualInGallons { get; set; }
        public decimal LiquidAcv { get; set; }
        public decimal PowderTargetInKg { get; set; }
        public decimal PowderActualInKg { get; set; }
        public decimal PowderAcv { get; set; }
        public decimal ValueTargetInTk { get; set; }
        public decimal ValueActualInTk { get; set; }
        public decimal ValueAcv { get; set; }
    }

    public class TerritoryTargetAchievementResultModel : KpiTargetAchievementResultModel 
    {
        public string Territory { get; set; }
    }

    public class DealerWiseTargetAchievementResultModel : KpiTargetAchievementResultModel {}

    public class ProductWiseTargetAchievementResultModel 
    {
        public string BrandId { get; set; }
        public string BrandName { get; set; }
        public decimal ProductTarget { get; set; }
        public decimal ProductActual { get; set; }
        public decimal ProductAcv { get; set; }
    }

}
