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
        public decimal ValueActualIngTk { get; set; }
        public decimal ValueAcv { get; set; }
    }

    public class TerritoryTargetAchievementResultModel : KpiTargetAchievementResultModel {}

    public class DealerWiseTargetAchievementResultModel : KpiTargetAchievementResultModel {}

}
