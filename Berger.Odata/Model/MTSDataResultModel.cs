using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.OData.Edm;

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
    }
}
