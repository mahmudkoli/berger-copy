using System;
using System.Collections.Generic;
using System.Text;
using Berger.Common.Extensions;
using Microsoft.OData.Edm;

namespace Berger.Odata.Model
{
    public class QuarterlyPerformanceDataResultModel
    {
        public IList<MonthlyDataModel> MonthlyTargetData { get; internal set; }
        public IList<MonthlyDataModel> MonthlyActualData { get; internal set; }
        public decimal TotalTarget { get; set; }
        public decimal TotalActual { get; set; }
        public decimal AchivementOrGrowth { get; set; }

        public QuarterlyPerformanceDataResultModel()
        {
            CustomConvertExtension.NullToEmptyString(this);
            this.MonthlyTargetData = new List<MonthlyDataModel>();
            this.MonthlyActualData = new List<MonthlyDataModel>();
        }
    }

    public class MonthlyDataModel
    {
        public string MonthName { get; internal set; }
        public decimal Amount { get; internal set; }

        public MonthlyDataModel()
        {
            CustomConvertExtension.NullToEmptyString(this);
        }
    }
}
