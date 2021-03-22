using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.OData.Edm;
using Berger.Common.Extensions;

namespace Berger.Odata.Model
{
    public class MTSResultModel
    {
        public string CustomerNo { get; internal set; }
        public string CustomerName { get; internal set; }
        public string MatarialGroupOrBrand { get; set; }
        public decimal TargetVolume { get; internal set; }
        public decimal ActualVolume { get; internal set; }
        public decimal DifferenceVolume { get; internal set; }

        public MTSResultModel()
        {
            CustomConvertExtension.NullToEmptyString(this);
        }
    }

    public class PerformanceResultModel
    {
        public string CustomerNo { get; internal set; }
        public string CustomerName { get; internal set; }
        public string MatarialGroupOrBrand { get; set; }
        public decimal LYSMVolume { get; internal set; }
        public decimal TargetVolume { get; internal set; }
        public decimal ActualVolume { get; internal set; }
        public decimal TargetAchievement { get; internal set; }
        public decimal TillDateGrowth { get; internal set; }

        public PerformanceResultModel()
        {
            CustomConvertExtension.NullToEmptyString(this);
        }
    }

    public class ValueTargetResultModel
    {
        public string CustomerNo { get; internal set; }
        public string CustomerName { get; internal set; }
        //public string MatarialGroupOrBrand { get; set; }
        public decimal TargetValue { get; internal set; }
        public decimal ActualValue { get; internal set; }
        public decimal DifferenceValue { get; internal set; }

        public ValueTargetResultModel()
        {
            CustomConvertExtension.NullToEmptyString(this);
        }
    }

    public class ValueTargetTempResultModel
    {
        public string CustomerNo { get; internal set; }
        public string CustomerName { get; internal set; }
        public string MatarialGroupOrBrand { get; set; }
        public decimal TargetValue { get; internal set; }
        public decimal ActualValue { get; internal set; }
        public decimal DifferenceValue { get; internal set; }

        public ValueTargetTempResultModel()
        {
            CustomConvertExtension.NullToEmptyString(this);
        }
    }
}
