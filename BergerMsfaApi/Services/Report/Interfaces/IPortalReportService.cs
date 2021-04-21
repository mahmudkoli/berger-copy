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



        Task<QueryResultModel<DealerCollectionReportResultModel>> GetDealerCollectionReportAsync(CollectionReportSearchModel query);
        Task<QueryResultModel<SubDealerCollectionReportResultModel>> GetSubDealerCollectionReportAsync(CollectionReportSearchModel query);
        Task<QueryResultModel<CustomerCollectionReportResultModel>> GetCustomerCollectionReportAsync(CollectionReportSearchModel query);
        Task<QueryResultModel<DirectProjectCollectionReportResultModel>> GetDirectProjectCollectionReportAsync(CollectionReportSearchModel query);
        Task<QueryResultModel<PainterCallReportResultModel>> GetPainterCallReportAsync(PainterCallReportSearchModel query);
        Task<QueryResultModel<DealerVisitReportResultModel>> GetDealerVisitReportAsync(DealerVisitReportSearchModel query);
        Task<QueryResultModel<DealerSalesCallReportResultModel>> GetDealerSalesCallReportAsync(DealerSalesCallReportSearchModel query);
        Task<QueryResultModel<SubDealerSalesCallReportResultModel>> GetSubDealerSalesCallReportAsync(SubDealerSalesCallReportSearchModel query);
        Task<QueryResultModel<DealerIssueReportResultModel>> GetDealerIssueReportAsync(DealerIssueReportSearchModel query);
        Task<QueryResultModel<SubDealerIssueReportResultModel>> GetSubDealerIssueReportAsync(SubDealerIssueReportSearchModel query);
        Task<QueryResultModel<OsOver90daysTrendReportResultModel>> GetOsOver90daysTrendReport(OsOver90daysTrendReportSearchModel query);
        Task<QueryResultModel<LogInReportResultModel>> GetLogInReportAsync(LogInReportSearchModel query);
        Task<QueryResultModel<MerchendizingSnapShotReportResultModel>> GetSnapShotReportAsync(MerchendizingSnapShotReportSearchModel query);


        Task<QueryResultModel<TintingMachineReportResultModel>> GetTintingMachineReportAsync(TintingMachineReportSearchModel query);
        Task<QueryResultModel<ActiveSummaryReportResultModel>> GetActiveSummeryReportAsync(ActiveSummeryReportSearchModel query);


    }
}
