using System;
using System.Collections.Generic;
using System.Text;

namespace Berger.Odata.Model
{
    public class KpiTargetAchievementResultModel
    {
        public string Territory { get; set; }
        public decimal LiquidTarget { get; set; }
        public decimal LiquidActual { get; set; }
        public decimal LiquidAcv { get; set; }
        public decimal PowderTarget { get; set; }
        public decimal PowderActual { get; set; }
        public decimal PowderAcv { get; set; }
        public decimal ValueTarget { get; set; }
        public decimal ValueActual { get; set; }
        public decimal ValueAcv { get; set; }
    }

    public class TerritoryTargetAchievementResultModel : KpiTargetAchievementResultModel 
    {

    }

    public class AppTargetAchievementResultModel
    {
        public string Category { get; set; }
        public decimal Target { get; set; }
        public decimal Actual { get; set; }
        public decimal Achievement { get; set; }
    }

    public class DealerWiseTargetAchievementResultModel : KpiTargetAchievementResultModel 
    {
        public string CustomerNo { get; set; }
        public string CustomerName { get; set; }
    }

    public class ProductWiseTargetAchievementResultModel 
    {
        public string BrandId { get; set; }
        public string BrandName { get; set; }
        public decimal ProductTarget { get; set; }
        public decimal ProductActual { get; set; }
        public decimal ProductAcv { get; set; }
    }

    public class AppProductWiseTargetAchievementResultModel 
    {
        public string BrandName { get; set; }
        public decimal Target { get; set; }
        public decimal Actual { get; set; }
        public decimal Achievement { get; set; }
    }

}
