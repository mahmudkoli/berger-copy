using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.OData.Edm;

namespace Berger.Odata.Model
{
    public class QuarterlyPerformanceSearchModel
    {
        public int FromMonth { get; set; }
        public int FromYear { get; set; }
        public int ToMonth { get; set; }
        public int ToYear { get; set; }
        public string Territory { get; set; }
    }

    public class PortalQuarterlyPerformanceSearchModel
    {
        public int FromMonth { get; set; }
        public int FromYear { get; set; }
        public int ToMonth { get; set; }
        public int ToYear { get; set; }
        public string Depot { get; set; }
        public string SalesOffice { get; set; }
        public string SalesGroup { get; set; }
        public string Territory { get; set; }
        public string Zone { get; set; }
    }
}
