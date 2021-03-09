using BergerMsfaApi.Models.Common;
using BergerMsfaApi.Models.DealerSalesCall;
using BergerMsfaApi.Models.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BergerMsfaApi.Services.Report.Interfaces
{
    public interface IPortalReportService
    {
        Task<QueryResultModel<LeadSummaryReportResultModel>> GetLeadSummaryReportAsync(LeadSummaryReportSearchModel query);
        Task<QueryResultModel<LeadGenerationDetailsReportResultModel>> GetLeadGenerationDetailsReportAsync(LeadGenerationDetailsReportSearchModel query);
        Task<QueryResultModel<LeadFollowUpDetailsReportResultModel>> GetLeadFollowUpDetailsReportAsync(LeadFollowUpDetailsReportSearchModel query);

        Task<QueryResultModel<PainterRegistrationReportResultModel>> GetPainterRegistrationReportAsync(PainterRegistrationReportSearchModel query);
        Task<QueryResultModel<DealerOpeningReportResultModel>> GetDealerOpeningReportAsync(DealerOpeningReportSearchModel query);
    }
}
