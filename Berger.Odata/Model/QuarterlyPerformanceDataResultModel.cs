using System;
using System.Collections.Generic;
using System.Text;
using Berger.Common.Extensions;
using Microsoft.OData.Edm;

namespace Berger.Odata.Model
{
    public class QuarterlyPerformanceDataResultModel
    {
        public string Territory { get; internal set; } // only used for portal
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

    public class PortalQuarterlyPerformanceDataResultModel
    {
        public string Territory { get; internal set; }
        public string FirstMonthTargetName { get; internal set; }
        public string SecondMonthTargetName { get; internal set; }
        public string ThirdMonthTargetName { get; internal set; }
        public decimal FirstMonthTargetAmount { get; internal set; }
        public decimal SecondMonthTargetAmount { get; internal set; }
        public decimal ThirdMonthTargetAmount { get; internal set; }
        public string FirstMonthActualName { get; internal set; }
        public string SecondMonthActualName { get; internal set; }
        public string ThirdMonthActualName { get; internal set; }
        public decimal FirstMonthActualAmount { get; internal set; }
        public decimal SecondMonthActualAmount { get; internal set; }
        public decimal ThirdMonthActualAmount { get; internal set; }
        public decimal TotalTarget { get; set; }
        public decimal TotalActual { get; set; }
        public decimal AchivementOrGrowth { get; set; }

        public PortalQuarterlyPerformanceDataResultModel()
        {
            CustomConvertExtension.NullToEmptyString(this);
        }
    }

    public class PortalOSOver90DaysTrendResultModel
    {
        public string Month { get; internal set; }
        public decimal OSOver90Days { get; internal set; }
        public decimal Difference { get; internal set; }
        public decimal Sales { get; internal set; }
        public decimal OSPercentageWithSales { get; internal set; }

        public PortalOSOver90DaysTrendResultModel()
        {
            CustomConvertExtension.NullToEmptyString(this);
        }
    }
    public class AppQuarterlyPerformanceDataResultModel
    {
        public string MonthName { get; internal set; }
        public decimal TargetOrLyOrDeco { get; set; }
        public decimal ActualOrCyOrPrem { get; set; }
        public decimal AchivementOrGrowthOrCont { get; set; }
    }
}
