using Berger.Odata.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Berger.Odata.Services
{
    public interface IFinancialDataService
    {
        Task<IList<OutstandingDetailsResultModel>> GetOutstandingDetails(OutstandingDetailsSearchModel model);
        Task<IList<OutstandingSummaryResultModel>> GetOutstandingSummary(OutstandingSummarySearchModel model);
        Task<IList<ReportOutstandingSummaryResultModel>> GetReportOutstandingSummary(IList<int> dealerIds);
        Task<IList<ReportOSOver90DaysResultModel>> GetReportOSOver90Days(OSOver90DaysSearchModel model, IList<int> dealerIds);
        Task<IList<ReportPaymentFollowUpResultModel>> GetReportPaymentFollowUp(PaymentFollowUpSearchModel model, IList<int> dealerIds);
        Task<IList<FinancialDataModel>> GetOsOver90DaysTrend(IList<int> dealerIds, DateTime fromDate, DateTime toDate);
        Task<IList<FinancialDataModel>> GetOsOver90DaysTrend(int dealerId, DateTime fromDate, DateTime toDate);
    }
}
