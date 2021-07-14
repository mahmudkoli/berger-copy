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
        Task<IList<OutstandingSummaryReportResultModel>> GetOutstandingSummaryReport(OutstandingSummaryReportSearchModel model);
        Task<IList<OSOver90DaysTrendReportResultModel>> GetOSOver90DaysTrendReport(OSOver90DaysTrendSearchModel model);
        Task<IList<PaymentFollowUpResultModel>> GetPaymentFollowUp(PaymentFollowUpSearchModel model);
        Task<IList<FinancialDataModel>> GetOsOver90DaysTrend(string dealerId, DateTime fromDate, DateTime toDate, string creditControlArea = "");
        Task<IList<FinancialDataModel>> GetCustomerSlippageAmount(IList<string> dealerIds, DateTime endDate);
        Task<(bool HasOS, bool HasSlippage)> CheckCustomerOSSlippage(string dealerId);
    }
}
