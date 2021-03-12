using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Berger.Common.Extensions;
using Berger.Common.HttpClient;
using Berger.Common.JSONParser;
using Berger.Odata.Common;
using Berger.Odata.Extensions;
using Berger.Odata.Model;
using Microsoft.Extensions.Options;

namespace Berger.Odata.Services
{
    public class BalanceDataService : IBalanceDataService
    {
        private readonly IODataService _odataService;

        public BalanceDataService(
            IODataService odataService
            )
        {
            _odataService = odataService;
        }

        public async Task<IList<BalanceConfirmationSummaryResultModel>> GetBalanceConfirmationSummary(BalanceConfirmationSummarySearchModel model)
        {
            var currentDate = CustomConvertExtension.ObjectToDateTime(model.PostingDate);
            var fromDate = currentDate.AddDays(-90).DateTimeFormat();
            var toDate = currentDate.DateTimeFormat();

            var selectQueryBuilder = new SelectQueryOptionBuilder();
            selectQueryBuilder.AddProperty(BalanceColDef.LineText)
                                .AddProperty(BalanceColDef.CustomerNo)
                                .AddProperty(BalanceColDef.CustomerName)
                                .AddProperty(BalanceColDef.CreditControlArea)
                                .AddProperty(BalanceColDef.PostingDate)
                                .AddProperty(BalanceColDef.Amount);

            var data = (await _odataService.GetBalanceDataByCustomerAndCreditControlArea(selectQueryBuilder, model.CustomerNo, fromDate, toDate, model.CreditControlArea, model.FiscalYear)).ToList();

            var groupData = data.GroupBy(x => x.PostingDate);

            var result = groupData.Select(x =>
                                new BalanceConfirmationSummaryResultModel()
                                {
                                    Date = CustomConvertExtension.ObjectToDateString(x.Key),
                                    OpeningBalance = (x.Where(w => w.LineText == ConstantsValue.BalanceLineTextOpening).Sum(s => CustomConvertExtension.ObjectToDecimal(s.Amount))),
                                    InvoiceBalance = (x.Where(w => w.LineText == ConstantsValue.BalanceLineTextTransaction && CustomConvertExtension.ObjectToDecimal(w.Amount) >= 0).Sum(s => CustomConvertExtension.ObjectToDecimal(s.Amount))),
                                    PaymentBalance = ((x.Where(w => w.LineText == ConstantsValue.BalanceLineTextTransaction && CustomConvertExtension.ObjectToDecimal(w.Amount) < 0).Sum(s => CustomConvertExtension.ObjectToDecimal(s.Amount))) * -1),
                                    ClosingBalance = (x.Where(w => w.LineText == ConstantsValue.BalanceLineTextClosing).Sum(s => CustomConvertExtension.ObjectToDecimal(s.Amount)))
                                }).ToList();

            return result;
        }

        public async Task<IList<ChequeBounceResultModel>> GetChequeBounce(ChequeBounceSearchModel model)
        {
            var currentDate = new DateTime(model.Year, model.Month, 1);
            var fromDate = currentDate.AddMonths(-1).DateTimeFormat();
            var toDate = currentDate.GetCYLD().DateTimeFormat();

            var selectQueryBuilder = new SelectQueryOptionBuilder();
            selectQueryBuilder.AddProperty(BalanceColDef.CustomerNo)
                                .AddProperty(BalanceColDef.CustomerName)
                                .AddProperty(BalanceColDef.BankNo)
                                .AddProperty(BalanceColDef.PostingDate)
                                .AddProperty(BalanceColDef.Amount)
                                .AddProperty(BalanceColDef.CreditControlArea)
                                .AddProperty(BalanceColDef.ChequeNo)
                                .AddProperty(BalanceColDef.ChequeBounceStatus);

            var data = (await _odataService.GetBalanceDataByCustomerAndCreditControlArea(selectQueryBuilder, model.CustomerNo, fromDate, toDate)).ToList();

            var result = data.Select(x =>
                                new ChequeBounceResultModel()
                                {
                                    ReversalDate = CustomConvertExtension.ObjectToDateString(x.PostingDate),
                                    CustomerNo = x.CustomerNo,
                                    CustomerName = x.CustomerName,
                                    Division = x.CreditControlArea,
                                    ReversalAmount = CustomConvertExtension.ObjectToDecimal(x.Amount),
                                    ChequeNo = x.ChequeNo,
                                    Bank = x.BankNo,
                                    Reason = x.ChequeBounceStatus
                                }).ToList();

            return result;
        }

        public async Task<ChequeSummaryResultModel> GetChequeSummary(ChequeSummarySearchModel model)
        {
            var currentDate = new DateTime(model.Year, model.Month, 1);

            var cmfd = currentDate.DateTimeFormat();
            var cfyfd = currentDate.GetCFYFD().DateTimeFormat();
            var toDate = currentDate.GetCYLD().DateTimeFormat();

            var dataCm = new List<BalanceDataModel>();
            var dataCy = new List<BalanceDataModel>();

            var selectQueryBuilder = new SelectQueryOptionBuilder();
            selectQueryBuilder.AddProperty(BalanceColDef.CustomerNo)
                                .AddProperty(BalanceColDef.CustomerName)
                                .AddProperty(BalanceColDef.PostingDate)
                                .AddProperty(BalanceColDef.Amount)
                                .AddProperty(BalanceColDef.ChequeNo)
                                .AddProperty(BalanceColDef.ChequeBounceStatus);

            dataCm = (await _odataService.GetBalanceDataByCustomerAndCreditControlArea(selectQueryBuilder, model.CustomerNo, cmfd, toDate)).ToList();

            dataCy = (await _odataService.GetBalanceDataByCustomerAndCreditControlArea(selectQueryBuilder, model.CustomerNo, cfyfd, toDate)).ToList();

            var result = new ChequeSummaryResultModel();

            result.CustomerNo = dataCy.FirstOrDefault()?.CustomerNo ?? string.Empty;
            result.CustomerName = dataCy.FirstOrDefault()?.CustomerName ?? string.Empty;

            #region Cheque Details
            result.ChequeDetails = new List<ChequeSummaryChequeDetailsModel>();

            var totalChqRec = new ChequeSummaryChequeDetailsModel();
            totalChqRec.ChequeDetails = "Total Chq Rec";
            totalChqRec.MTDNoOfCheque = dataCm.Count();
            totalChqRec.YTDNoOfCheque = dataCy.Count();
            totalChqRec.MTDTotalChequeValue = dataCm.Sum(s => CustomConvertExtension.ObjectToDecimal(s.Amount));
            totalChqRec.YTDTotalChequeValue = dataCy.Sum(s => CustomConvertExtension.ObjectToDecimal(s.Amount));

            var totalChqBncd = new ChequeSummaryChequeDetailsModel();
            totalChqBncd.ChequeDetails = "Total Chq Bncd";
            totalChqBncd.MTDNoOfCheque = dataCm.Where(c => !string.IsNullOrEmpty(c.ChequeBounceStatus)).Count();
            totalChqBncd.YTDNoOfCheque = dataCy.Where(c => !string.IsNullOrEmpty(c.ChequeBounceStatus)).Count();
            totalChqBncd.MTDTotalChequeValue = dataCm.Where(c => !string.IsNullOrEmpty(c.ChequeBounceStatus)).Sum(s => CustomConvertExtension.ObjectToDecimal(s.Amount));
            totalChqBncd.YTDTotalChequeValue = dataCy.Where(c => !string.IsNullOrEmpty(c.ChequeBounceStatus)).Sum(s => CustomConvertExtension.ObjectToDecimal(s.Amount));

            var bncdPercent = new ChequeSummaryChequeDetailsModel();
            bncdPercent.ChequeDetails = "Bncd Percent";
            bncdPercent.MTDNoOfCheque = (totalChqBncd.MTDNoOfCheque * 100) / (totalChqRec.MTDNoOfCheque == 0 ? 1 : totalChqRec.MTDNoOfCheque);
            bncdPercent.YTDNoOfCheque = (totalChqBncd.YTDNoOfCheque * 100) / (totalChqRec.YTDNoOfCheque == 0 ? 1 : totalChqRec.YTDNoOfCheque);
            bncdPercent.MTDTotalChequeValue = (totalChqBncd.MTDTotalChequeValue * 100) / (totalChqRec.MTDTotalChequeValue == 0 ? 1 : totalChqRec.MTDTotalChequeValue);
            bncdPercent.YTDTotalChequeValue = (totalChqBncd.YTDTotalChequeValue * 100) / (totalChqRec.YTDTotalChequeValue == 0 ? 1 : totalChqRec.YTDTotalChequeValue);

            result.ChequeDetails.Add(totalChqRec);
            result.ChequeDetails.Add(totalChqBncd);
            result.ChequeDetails.Add(bncdPercent);
            #endregion

            #region Cheque Bounce Details
            result.ChequeBounceDetails = dataCm.Where(c => !string.IsNullOrEmpty(c.ChequeBounceStatus))
                                                            .Select(s => new ChequeSummaryChequeBounceDetailsModel() 
                                                            {
                                                                DealerCodeName = $"{s.CustomerName} {s.CustomerNo}",
                                                                Date = CustomConvertExtension.ObjectToDateString(s.PostingDate),
                                                                ChequeNo = s.ChequeNo,
                                                                ChequeAmount = s.Amount
                                                            }).ToList();
            #endregion

            return result;
        }
    }
}
