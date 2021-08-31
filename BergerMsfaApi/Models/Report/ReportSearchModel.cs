using Berger.Common.Enumerations;
using BergerMsfaApi.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BergerMsfaApi.Models.Report
{
    public class ReportBaseSearchModel : QueryObjectModel
    {
        public string Depot { get; set; }
        public IList<string> SalesGroups { get; set; }
        public IList<string> Territories { get; set; }
        public IList<string> Zones { get; set; }
        public int? UserId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public DateTime? Date { get; set; }

        public ReportBaseSearchModel()
        {
            this.SalesGroups = new List<string>();
            this.Territories = new List<string>();
            this.Zones = new List<string>();
        }
    }

    public class LeadSummaryReportSearchModel : ReportBaseSearchModel
    {

    }

    public class LeadGenerationDetailsReportSearchModel : ReportBaseSearchModel
    {
        public string ProjectName { get; set; }
        public int? PaintingStageId { get; set; }
    }

    public class LeadFollowUpDetailsReportSearchModel : ReportBaseSearchModel
    {
        public string ProjectName { get; set; }
        public string ProjectCode { get; set; }
        public int? ProjectStatusId { get; set; }
    }

    public class LeadBusinessReportSearchModel : ReportBaseSearchModel
    {
        public string ProjectName { get; set; }
        public string ProjectCode { get; set; }
        public int? ProjectStatusId { get; set; }
    }

    public class PainterRegistrationReportSearchModel : ReportBaseSearchModel
    {
        public int? PainterId { get; set; }
        public string PainterName { get; set; }
        public string PainterMobileNo { get; set; }
        public int? PainterType { get; set; }
    }

    public class InactivePainterReportSearchModel : ReportBaseSearchModel
    {
        public int? PainterId { get; set; }
        public int? PainterType { get; set; }
    }

    public class DealerOpeningReportSearchModel : ReportBaseSearchModel
    {

    }

    public class TintingMachineReportSearchModel : ReportBaseSearchModel
    {
     
    }

    public class ActiveSummeryReportSearchModel : ReportBaseSearchModel
    {
        public string ActivitySummary { get; set; }

    }

    public class CollectionReportSearchModel : ReportBaseSearchModel
    {
        public int? PaymentMethodId { get; set; }
        public int? PaymentFromId { get; set; }
        public int? DealerId { get; set; }
    }

    public class PainterCallReportSearchModel : ReportBaseSearchModel
    {
        public int? PainterId { get; set; }
        public int? PainterType { get; set; }
    }

    public class DealerVisitReportSearchModel : ReportBaseSearchModel
    {
        public int? DealerId { get; set; }
        public int? Month { get; set; }
        public int? Year { get; set; }
    }

    public class DealerSalesCallReportSearchModel : ReportBaseSearchModel
    {
        public int? DealerId { get; set; }
    }

    public class SubDealerSalesCallReportSearchModel : ReportBaseSearchModel
    {
        public int? SubDealerId { get; set; }
    }

    public class DealerIssueReportSearchModel : ReportBaseSearchModel
    {
        public int? DealerId { get; set; }
    }

    public class SubDealerIssueReportSearchModel : ReportBaseSearchModel
    {
        public int? SubDealerId { get; set; }
    }
    public class OsOver90daysTrendReportSearchModel : ReportBaseSearchModel
    {
        public int? DealerId { get; set; }
        public string CreditControlArea { get; set; }
        //public string AccountGroup { get; set; }
        //public string SalesOffice { get; set; }
        public int FromMonth { get; set; }
        public int FromYear { get; set; }
        public int ToMonth { get; set; }
        public int ToYear { get; set; }
    }

    public class MerchendizingSnapShotReportSearchModel : ReportBaseSearchModel
    {
        public int? DealerId { get; set; }
    }

    public class LogInReportSearchModel : ReportBaseSearchModel
    {
        public int? Status { get; set; }
    }

    public class ColorBankTargetSetupSearchModel : ReportBaseSearchModel
    {
        public string Territory { get; set; }
        public int Year { get; set; }
    }

}
