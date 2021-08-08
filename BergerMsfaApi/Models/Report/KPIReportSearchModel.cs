using Berger.Common.Enumerations;
using BergerMsfaApi.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BergerMsfaApi.Models.Report
{
    public class KPIReportBaseSearchModel
    //: QueryObjectModel
    {
        public string Depot { get; set; }
        public List<string> SalesGroups { get; set; }
        public List<string> Territories { get; set; }
        public List<string> Zones { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }

        public KPIReportBaseSearchModel()
        {
            this.SalesGroups = new List<string>();
            this.Territories = new List<string>();
            this.Zones = new List<string>();
        }
    }

    public class StrikeRateKPIReportSearchModel : KPIReportBaseSearchModel
    {
        public EnumStrikeRateReportType ReportType { get; set; }
    }

    public enum EnumStrikeRateReportType
    {
        All = 1,
        Exclusive = 2,
        NonExclusive = 3
    }

    public class BusinessCallKPIReportSearchModel : KPIReportBaseSearchModel
    {
        public EnumStrikeRateReportType Category { get; set; }
    }

    public class BillingAnalysisKPIReportSearchModel
        //: KPIReportBaseSearchModel
    {
        public string Depot { get; set; }
        public List<string> SalesGroups { get; set; }
        public List<string> Territories { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }

        public BillingAnalysisKPIReportSearchModel()
        {
            this.SalesGroups = new List<string>();
            this.Territories = new List<string>();
        }
    }

    public class CollectionPlanKPIReportSearchModel
    {
        public string Depot { get; set; }
        public string Territory { get; set; }
    }

    public enum EnumReportFor
    {
        App = 1,
        Web = 2
    }
}
