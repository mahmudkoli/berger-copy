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
        private readonly IODataCommonService _odataCommonService;

        public BalanceDataService(
            IODataService odataService,
            IODataCommonService odataCommonService
            )
        {
            _odataService = odataService;
            _odataCommonService = odataCommonService;
        }

        public async Task<IList<CollectionHistoryResultModel>> GetCollectionHistory(CollectionHistorySearchModel model)
        {
            var currentDate = DateTime.Now;
            var fromDate = currentDate.AddMonths(-1).GetCYFD().DateTimeFormat();
            var toDate = currentDate.AddMonths(-1).GetCYLD().DateTimeFormat();

            var selectQueryBuilder = new SelectQueryOptionBuilder();
            selectQueryBuilder.AddProperty(CollectionColDef.DocNumber)
                                .AddProperty(CollectionColDef.CustomerNo)
                                .AddProperty(CollectionColDef.CustomerName)
                                .AddProperty(CollectionColDef.ChequeNo)
                                .AddProperty(CollectionColDef.BankName)
                                .AddProperty(CollectionColDef.CreditControlArea)
                                .AddProperty(CollectionColDef.PostingDate)
                                .AddProperty(CollectionColDef.Amount);

            var data = (await _odataService.GetCollectionDataByCustomerAndCreditControlArea(selectQueryBuilder, model.CustomerNo, startPostingDate: fromDate, endPostingDate: toDate, creditControlArea: model.CreditControlArea)).ToList();

            var result = data.Select(x =>
                                new CollectionHistoryResultModel()
                                {
                                    DocumentNo = x.DocNumber,
                                    CustomerNo = x.CustomerNo,
                                    CustomerName = x.CustomerName,
                                    InstrumentNo = x.ChequeNo,
                                    BankName = x.BankName,
                                    CreditControlArea = x.CreditControlArea,
                                    PostingDate = x.PostingDate,
                                    Amount = CustomConvertExtension.ObjectToDecimal(x.Amount)
                                }).ToList();

            #region Credit Control Area 
            var creditControlAreas = await _odataCommonService.GetAllCreditControlAreasAsync();

            foreach (var item in result)
            {
                item.CreditControlAreaName = creditControlAreas.FirstOrDefault(f => f.CreditControlAreaId.ToString() == item.CreditControlArea)?.Description ?? string.Empty;
            }
            #endregion

            return result;
        }

        public async Task<IList<BalanceConfirmationSummaryResultModel>> GetBalanceConfirmationSummary(BalanceConfirmationSummarySearchModel model)
        {
            var currentDate = new DateTime(model.Year, model.Month, 1);
            var fromDate = currentDate.GetCYFD();
            var toDate = currentDate.GetCYLD();
            var fromDateStr = fromDate.DateTimeFormat();
            var toDateStr = toDate.DateTimeFormat();

            var selectBalanceQueryBuilder = new SelectQueryOptionBuilder();
            selectBalanceQueryBuilder.AddProperty(BalanceColDef.LineText)
                                .AddProperty(BalanceColDef.CustomerNo)
                                .AddProperty(BalanceColDef.CustomerName)
                                .AddProperty(BalanceColDef.CreditControlArea)
                                .AddProperty(BalanceColDef.PostingDateDoc)
                                .AddProperty(BalanceColDef.Amount);

            var selectCollectionQueryBuilder = new SelectQueryOptionBuilder();
            selectCollectionQueryBuilder.AddProperty(CollectionColDef.CollectionType)
                                .AddProperty(CollectionColDef.CustomerNo)
                                .AddProperty(CollectionColDef.CustomerName)
                                .AddProperty(CollectionColDef.CreditControlArea)
                                .AddProperty(CollectionColDef.PostingDate)
                                .AddProperty(CollectionColDef.Amount);

            var dataBalance = (await _odataService.GetBalanceDataByCustomerAndCreditControlArea(selectBalanceQueryBuilder, model.CustomerNo, fromDateStr, toDateStr, model.CreditControlArea)).ToList();
            
            var dataCollection = (await _odataService.GetCollectionDataByCustomerAndCreditControlArea(selectCollectionQueryBuilder, model.CustomerNo, startPostingDate: fromDateStr, endPostingDate: toDateStr, creditControlArea: model.CreditControlArea)).ToList();

            var result = new List<BalanceConfirmationSummaryResultModel>();

            for (DateTime date = fromDate; date <= toDate; date = date.AddDays(1))
            {
                IEnumerable<DateTime> dateTimes = dataBalance.Select(x =>CustomConvertExtension.ObjectToDateTime(x.PostingDateDoc));

                var dataBal = dataBalance.Where(x => CustomConvertExtension.ObjectToDateTime(x.PostingDateDoc).Date == date.Date).ToList();
                var dataCol = dataCollection.Where(x => CustomConvertExtension.ObjectToDateTime(x.PostingDate).Date == date.Date).ToList();

                if (dataBal.Any() || dataCol.Any())
                {
                    var res = new BalanceConfirmationSummaryResultModel();
                    res.Date = date.DateFormat("dd-MM-yyyy");
                    res.OpeningBalance = (dataBal.Where(w => w.LineText == ConstantsValue.BalanceLineTextOpening)
                                            .Sum(s => CustomConvertExtension.ObjectToDecimal(s.Amount)));
                    res.ClosingBalance = (dataBal.Where(w => w.LineText == ConstantsValue.BalanceLineTextClosing)
                                            .Sum(s => CustomConvertExtension.ObjectToDecimal(s.Amount)));
                    res.InvoiceBalance = dataCol.Where(w => w.blart == ConstantsValue.CollectionInvoice)
                                            .Sum(s => CustomConvertExtension.ObjectToDecimal(s.Amount));
                    res.PaymentBalance = dataCol.Where(w => w.blart == ConstantsValue.CollectionMoneyReceipt)
                                            .Sum(s => CustomConvertExtension.ObjectToDecimal(s.Amount));

                    result.Add(res);
                }
            }

            return result;
        }

        public async Task<IList<ChequeBounceResultModel>> GetChequeBounce(ChequeBounceSearchModel model)
        {
            var currentDate = new DateTime(model.Year, model.Month, 1);
            var fromDate = currentDate.GetCYFD().DateTimeFormat();
            var toDate = currentDate.GetCYLD().DateTimeFormat();

            var selectQueryBuilder = new SelectQueryOptionBuilder();
            selectQueryBuilder.AddProperty(CollectionColDef.CustomerNo)
                                .AddProperty(CollectionColDef.CustomerName)
                                .AddProperty(CollectionColDef.DocNumber)
                                .AddProperty(CollectionColDef.ChequeNo)
                                .AddProperty(CollectionColDef.BankName)
                                .AddProperty(CollectionColDef.ClearDate)
                                .AddProperty(CollectionColDef.Amount)
                                .AddProperty(CollectionColDef.CreditControlArea);

            var data = (await _odataService.GetCollectionDataByCustomerAndCreditControlArea(selectQueryBuilder, model.CustomerNo, startClearDate: fromDate, endClearDate: toDate, bounceStatus: "Z1")).ToList();

            var result = data.Select(x =>
                                new ChequeBounceResultModel()
                                {
                                    ReversalDate = CustomConvertExtension.ObjectToDateTime(x.PostingDate).DateFormat("dd MMM yyyy"),
                                    CustomerNo = x.CustomerNo,
                                    CustomerName = x.CustomerName,
                                    CreditControlArea = x.CreditControlArea,
                                    Amount = CustomConvertExtension.ObjectToDecimal(x.Amount),
                                    InstrumentNo = x.ChequeNo,
                                    DocumentNo = x.DocNumber,
                                    BankName = x.BankName,
                                    Reason = "Cheque Bounce-Insuff"
                                }).ToList();

            #region Credit Control Area 
            var creditControlAreas = await _odataCommonService.GetAllCreditControlAreasAsync();

            foreach (var item in result)
            {
                item.CreditControlAreaName = creditControlAreas.FirstOrDefault(f => f.CreditControlAreaId.ToString() == item.CreditControlArea)?.Description ?? string.Empty;
            }
            #endregion

            return result;
        }

        public async Task<ChequeSummaryResultModel> GetChequeSummary(ChequeSummarySearchModel model)
        {
            var currentDate = new DateTime(model.Year, model.Month, 1);

            var cmfd = currentDate.GetCYFD().DateTimeFormat();
            var cfyfd = currentDate.GetCFYFD().DateTimeFormat();
            var toDate = currentDate.GetCYLD().DateTimeFormat();

            var dataCm = new List<CollectionDataModel>();
            var dataCy = new List<CollectionDataModel>();
            var dataBounceCm = new List<CollectionDataModel>();
            var dataBounceCy = new List<CollectionDataModel>();

            var selectQueryBuilder = new SelectQueryOptionBuilder();
            selectQueryBuilder.AddProperty(CollectionColDef.CustomerNo)
                                .AddProperty(CollectionColDef.CustomerName)
                                .AddProperty(CollectionColDef.PostingDate)
                                .AddProperty(CollectionColDef.ClearDate)
                                .AddProperty(CollectionColDef.Amount)
                                .AddProperty(CollectionColDef.ChequeNo);

            dataCm = (await _odataService.GetCollectionDataByCustomerAndCreditControlArea(selectQueryBuilder, model.CustomerNo, cmfd, toDate)).ToList();

            dataCy = (await _odataService.GetCollectionDataByCustomerAndCreditControlArea(selectQueryBuilder, model.CustomerNo, cfyfd, toDate)).ToList();

            dataBounceCm = (await _odataService.GetCollectionDataByCustomerAndCreditControlArea(selectQueryBuilder, model.CustomerNo, startClearDate: cmfd, endClearDate: toDate, bounceStatus: "Z1")).ToList();

            dataBounceCy = (await _odataService.GetCollectionDataByCustomerAndCreditControlArea(selectQueryBuilder, model.CustomerNo, startClearDate: cfyfd, endClearDate: toDate, bounceStatus: "Z1")).ToList();

            var result = new ChequeSummaryResultModel();

            result.CustomerNo = dataCy.FirstOrDefault()?.CustomerNo ?? string.Empty;
            result.CustomerName = dataCy.FirstOrDefault()?.CustomerName ?? string.Empty;

            #region Cheque Details
            result.ChequeDetails = new List<ChequeSummaryChequeDetailsModel>();

            var totalChqRec = new ChequeSummaryChequeDetailsModel();
            totalChqRec.ChequeDetailsName = "Total Chq Rec";
            totalChqRec.MTDNoOfCheque = dataCm.Count();
            totalChqRec.YTDNoOfCheque = dataCy.Count();
            totalChqRec.MTDTotalChequeValue = dataCm.Sum(s => CustomConvertExtension.ObjectToDecimal(s.Amount));
            totalChqRec.YTDTotalChequeValue = dataCy.Sum(s => CustomConvertExtension.ObjectToDecimal(s.Amount));

            var totalChqBncd = new ChequeSummaryChequeDetailsModel();
            totalChqBncd.ChequeDetailsName = "Total Chq Bncd";
            totalChqBncd.MTDNoOfCheque = dataBounceCm.Count();
            totalChqBncd.YTDNoOfCheque = dataBounceCy.Count();
            totalChqBncd.MTDTotalChequeValue = dataBounceCm.Sum(s => CustomConvertExtension.ObjectToDecimal(s.Amount));
            totalChqBncd.YTDTotalChequeValue = dataBounceCy.Sum(s => CustomConvertExtension.ObjectToDecimal(s.Amount));

            var bncdPercent = new ChequeSummaryChequeDetailsModel();
            bncdPercent.ChequeDetailsName = "Bncd Percent";
            bncdPercent.MTDNoOfCheque = _odataService.GetPercentage(totalChqRec.MTDNoOfCheque, totalChqBncd.MTDNoOfCheque);
            bncdPercent.YTDNoOfCheque = _odataService.GetPercentage(totalChqRec.YTDNoOfCheque, totalChqBncd.YTDNoOfCheque);
            bncdPercent.MTDTotalChequeValue = _odataService.GetPercentage(totalChqRec.MTDTotalChequeValue, totalChqBncd.MTDTotalChequeValue);
            bncdPercent.YTDTotalChequeValue = _odataService.GetPercentage(totalChqRec.YTDTotalChequeValue, totalChqBncd.YTDTotalChequeValue);

            result.ChequeDetails.Add(totalChqRec);
            result.ChequeDetails.Add(totalChqBncd);
            result.ChequeDetails.Add(bncdPercent);
            #endregion

            #region Cheque Bounce Details
            result.ChequeBounceDetails = dataBounceCm.Select(s => new ChequeSummaryChequeBounceDetailsModel()
                                                        {
                                                            CustomerNo = s.CustomerNo,
                                                            CustomerName = s.CustomerName,
                                                            ReversalDate = CustomConvertExtension.ObjectToDateTime(s.PostingDate).DateFormat("dd MMM yyyy"),
                                                            ChequeNo = s.ChequeNo,
                                                            Amount = s.Amount
                                                        }).ToList();
            #endregion

            return result;
        }
    }
}
