using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.OData.Edm;

namespace Berger.Odata.Model
{
    public class QuarterlyPerformanceSearchModel
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string Territory { get; set; }
    }
}
