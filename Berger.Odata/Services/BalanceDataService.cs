using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Berger.Common.Extensions;
using Berger.Odata.Common;
using Berger.Odata.Extensions;
using Berger.Odata.Model;

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

        public async Task<IList<CollectionHistoryResultModel>> GetMRHistory(CollectionHistorySearchModel model)
        {
            //var currentDate = DateTime.Now;
            //var fromDate = currentDate.AddMonths(-1).GetCYFD().DateTimeFormat();
            //var toDate = currentDate.AddMonths(-1).GetCYLD().DateTimeFormat(); 

            var fromDate = model.FromDate.DateTimeFormat();
            var toDate = model.ToDate.DateTimeFormat();

            var selectQueryBuilder = new SelectQueryOptionBuilder();
            selectQueryBuilder.AddProperty(CollectionColDef.DocNumber)
                                //.AddProperty(CollectionColDef.CustomerNo)
                                //.AddProperty(CollectionColDef.CustomerName)
                                .AddProperty(CollectionColDef.ChequeNo)
                                .AddProperty(CollectionColDef.BankName)
                                .AddProperty(CollectionColDef.Depot)
                                .AddProperty(CollectionColDef.CreditControlArea)
                                .AddProperty(CollectionColDef.PostingDate)
                                .AddProperty(CollectionColDef.Amount);

            var data = (await _odataService.GetCollectionDataByCustomerAndCreditControlArea(selectQueryBuilder, model.CustomerNo, startPostingDate: fromDate, endPostingDate: toDate, creditControlArea: model.CreditControlArea)).ToList();

            var result = data.Select(x =>
                                new CollectionHistoryResultModel()
                                {
                                    MrNo = x.DocNumber,
                                    ChequeNo = x.ChequeNo,
                                    BankName = x.BankName,
                                    Division = x.CreditControlArea,
                                    Date = CustomConvertExtension.ObjectToDateTime(x.PostingDate).DateFormat("dd MMM yyyy"),
                                    MrAmount = CustomConvertExtension.ObjectToDecimal(x.Amount)
                                }).ToList();

            #region Credit Control Area 
            var creditControlAreas = await _odataCommonService.GetAllCreditControlAreasAsync();

            foreach (var item in result)
            {
                item.Division = creditControlAreas.FirstOrDefault(f => f.CreditControlAreaId.ToString() == item.Division)?.Description ?? string.Empty;
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

            var selectQueryBuilder = new SelectQueryOptionBuilder();
            selectQueryBuilder.AddProperty(BalanceColDef.LineText)
                                .AddProperty(BalanceColDef.DocType)
                                .AddProperty(BalanceColDef.TransactionDescription)
                                .AddProperty(BalanceColDef.CustomerNo)
                                .AddProperty(BalanceColDef.CustomerName)
                                .AddProperty(BalanceColDef.CreditControlArea)
                                .AddProperty(BalanceColDef.PostingDateDoc)
                                .AddProperty(BalanceColDef.Amount);

            var data = (await _odataService.GetBalanceDataByCustomerAndCreditControlArea(selectQueryBuilder, model.CustomerNo, fromDateStr, toDateStr, model.CreditControlArea)).ToList();

            var result = data.Select(x =>
            {
                var res = new BalanceConfirmationSummaryResultModel();

                res.Date = CustomConvertExtension.ObjectToDateTime(x.PostingDateDoc).DateFormat("dd-MM-yyyy");
                res.OpeningBalance = x.LineText == ConstantsValue.BalanceLineTextOpening ? CustomConvertExtension.ObjectToDecimal(x.Amount) : 0;
                res.ClosingBalance = x.LineText == ConstantsValue.BalanceLineTextClosing ? CustomConvertExtension.ObjectToDecimal(x.Amount) : 0;
                res.InvoiceBalance = x.DocType == ConstantsValue.BalanceDocTypeInvoice ? CustomConvertExtension.ObjectToDecimal(x.Amount) : 0;
                res.PaymentBalance = x.DocType == ConstantsValue.BalanceDocTypeMoneyReceipt ? CustomConvertExtension.ObjectToDecimal(x.Amount) : 0;
                res.TransactionDescription = x.TransactionDescription;

                return res;
            }).ToList();

            result = result.OrderBy(x => x.Date.DateFormatDate("dd-MM-yyyy")).ToList();

            return result;
        }

        public async Task<ChecqueBounceResultModel> GetChequeBounce(ChequeBounceSearchModel model)
        {
            var currentDate = new DateTime(model.Year, model.Month, 1);
            var fromDate = currentDate.GetCFYFD().DateTimeFormat();
            var toDate = currentDate.GetCYLD().DateTimeFormat();

            var bounceSelectQueryBuilder = new SelectQueryOptionBuilder();
            bounceSelectQueryBuilder.AddProperty(CollectionColDef.CustomerNo)
                                .AddProperty(CollectionColDef.CustomerName)
                                .AddProperty(CollectionColDef.DocNumber)
                                .AddProperty(CollectionColDef.ChequeNo)
                                .AddProperty(CollectionColDef.BankName)
                                .AddProperty(CollectionColDef.ClearDate)
                                .AddProperty(CollectionColDef.Amount)
                                .AddProperty(CollectionColDef.CreditControlArea);


            var receiveSelectQueryBuilder = new SelectQueryOptionBuilder();
            receiveSelectQueryBuilder.AddProperty(CollectionColDef.CustomerNo)
                                .AddProperty(CollectionColDef.CustomerName)
                                .AddProperty(CollectionColDef.DocNumber)
                                .AddProperty(CollectionColDef.ChequeNo)
                                .AddProperty(CollectionColDef.BankName)
                                .AddProperty(CollectionColDef.PostingDate)
                                .AddProperty(CollectionColDef.Amount)
                                .AddProperty(CollectionColDef.CreditControlArea);

            var chequeBounce = (await _odataService.GetCollectionDataByCustomerAndCreditControlArea(bounceSelectQueryBuilder, model.CustomerNo, startClearDate: fromDate, endClearDate: toDate, bounceStatus: ConstantsValue.ChequeBounceStatus, creditControlArea: model.CreditControlArea)).ToList();
            var chequeReceived = (await _odataService.GetCollectionDataByCustomerAndCreditControlArea(receiveSelectQueryBuilder, model.CustomerNo, startPostingDate: fromDate, endPostingDate: toDate, creditControlArea: model.CreditControlArea)).ToList();

            var result = new ChecqueBounceResultModel();


            var totalChqRec = new ChequeBounceSummaryResultModel
            {
                Category = "Total Cheque Receive",
                MTDNoOfCheque = chequeReceived.Count(x => CustomConvertExtension.ObjectToDateTime(x.PostingDate) >= currentDate &&
                                                          CustomConvertExtension.ObjectToDateTime(x.PostingDate) <=
                                                          currentDate.GetMonthLastDate()),
                YTDNoOfCheque = chequeReceived.Count(),
                MTDChequeValue = chequeReceived
                    .Where(x => CustomConvertExtension.ObjectToDateTime(x.PostingDate) >= currentDate &&
                                CustomConvertExtension.ObjectToDateTime(x.PostingDate) <=
                                currentDate.GetMonthLastDate())
                    .Sum(s => CustomConvertExtension.ObjectToDecimal(s.Amount)),
                YTDChequeValue = chequeReceived.Sum(s => CustomConvertExtension.ObjectToDecimal(s.Amount))
            };

            var totalChqBncd = new ChequeBounceSummaryResultModel
            {
                Category = "Total Cheque Bounced",
                MTDNoOfCheque = chequeBounce.Count(x => CustomConvertExtension.ObjectToDateTime(x.ClearDate) >= currentDate &&
                                                        CustomConvertExtension.ObjectToDateTime(x.ClearDate) <=
                                                        currentDate.GetMonthLastDate()),
                YTDNoOfCheque = chequeBounce.Count(),
                MTDChequeValue = chequeBounce.Where(x => CustomConvertExtension.ObjectToDateTime(x.ClearDate) >= currentDate &&
                                                         CustomConvertExtension.ObjectToDateTime(x.ClearDate) <=
                                                         currentDate.GetMonthLastDate())
                    .Sum(s => CustomConvertExtension.ObjectToDecimal(s.Amount)),
                YTDChequeValue = chequeBounce.Sum(s => CustomConvertExtension.ObjectToDecimal(s.Amount))
            };

            var bncdPercent = new ChequeBounceSummaryResultModel
            {
                Category = "Bounce %",
                MTDNoOfCheque = _odataService.GetPercentage(totalChqRec.MTDNoOfCheque, totalChqBncd.MTDNoOfCheque),
                YTDNoOfCheque = _odataService.GetPercentage(totalChqRec.YTDNoOfCheque, totalChqBncd.YTDNoOfCheque),
                MTDChequeValue =
                    _odataService.GetPercentage(totalChqRec.MTDChequeValue, totalChqBncd.YTDChequeValue),
                YTDChequeValue =
                    _odataService.GetPercentage(totalChqRec.MTDChequeValue, totalChqBncd.YTDChequeValue)
            };

            result.ChequeBounceSummaryResultModels.Add(totalChqRec);
            result.ChequeBounceSummaryResultModels.Add(totalChqBncd);
            result.ChequeBounceSummaryResultModels.Add(bncdPercent);


            result.ChequeBounceDetailResultModels = chequeBounce.Where(x => CustomConvertExtension.ObjectToDateTime(x.ClearDate) >= currentDate &&
                                              CustomConvertExtension.ObjectToDateTime(x.ClearDate) <=
                                              currentDate.GetMonthLastDate()).Select(x =>
                                new ChequeBounceDetailResultModel()
                                {
                                    Date = CustomConvertExtension.ObjectToDateTime(x.PostingDate).DateFormat("dd.MM.yyyy"),
                                    CreditControlArea = x.CreditControlArea,
                                    Amount = CustomConvertExtension.ObjectToDecimal(x.Amount),
                                    ChequeNo = x.ChequeNo,
                                    MrNumber = x.DocNumber,
                                    BankName = x.BankName
                                }).ToList();

            #region Credit Control Area 
            var creditControlAreas = await _odataCommonService.GetAllCreditControlAreasAsync();

            foreach (var item in result.ChequeBounceDetailResultModels)
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

            dataBounceCm = (await _odataService.GetCollectionDataByCustomerAndCreditControlArea(selectQueryBuilder, model.CustomerNo, startClearDate: cmfd, endClearDate: toDate, bounceStatus: ConstantsValue.ChequeBounceStatus)).ToList();

            dataBounceCy = (await _odataService.GetCollectionDataByCustomerAndCreditControlArea(selectQueryBuilder, model.CustomerNo, startClearDate: cfyfd, endClearDate: toDate, bounceStatus: ConstantsValue.ChequeBounceStatus)).ToList();

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
                Amount = CustomConvertExtension.ObjectToDecimal(s.Amount)
            }).ToList();
            #endregion

            return result;
        }

        public async Task<ChequeSummaryReportResultModel> GetChequeSummaryReport(ChequeSummaryReportSearchModel model)
        {
            var filterDate = new DateTime(model.Year, model.Month, 01);

            var cyfd = filterDate.GetCYFD().CollectionSearchDateTimeFormat();
            var cfyfd = filterDate.GetCFYFD().CollectionSearchDateTimeFormat();
            var cyld = filterDate.GetCYLD().CollectionSearchDateTimeFormat();

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

            #region Filter by area and customer no
            //dataCm = (await _odataService.GetCollectionData(selectQueryBuilder,
            //                    depots: model.Depots, territories: model.Territories,
            //                    customerNos: model.CustomerNos,
            //                    startPostingDate: cyfd, endPostingDate: cyld)).ToList();

            //dataCy = (await _odataService.GetCollectionData(selectQueryBuilder,
            //                    depots: model.Depots, territories: model.Territories,
            //                    customerNos: model.CustomerNos,
            //                    startPostingDate: cfyfd, endPostingDate: cyld)).ToList();

            //dataBounceCm = (await _odataService.GetCollectionData(selectQueryBuilder,
            //                        depots: model.Depots, territories: model.Territories,
            //                        customerNos: model.CustomerNos,
            //                        startClearDate: cyfd, endClearDate: cyld,
            //                        bounceStatus: ConstantsValue.ChequeBounceStatus)).ToList();

            //dataBounceCy = (await _odataService.GetCollectionData(selectQueryBuilder,
            //                        depots: model.Depots, territories: model.Territories,
            //                        customerNos: model.CustomerNos,
            //                        startClearDate: cfyfd, endClearDate: cyld,
            //                        bounceStatus: ConstantsValue.ChequeBounceStatus)).ToList();
            #endregion

            #region Filter by only customer no
            var customerNos = model.CustomerNos;

            if (customerNos == null || !customerNos.Any())
            {
                var selectCustomerQueryBuilder = new SelectQueryOptionBuilder();
                selectCustomerQueryBuilder.AddProperty(nameof(CustomerDataModel.CustomerNo));

                var customerData = (await _odataService.GetCustomerData(selectCustomerQueryBuilder,
                                    depots: model.Depots, territories: model.Territories, zones: model.Zones,
                                    channel: ConstantsValue.DistrbutionChannelDealer)).ToList();

                customerNos = customerData.Select(x => x.CustomerNo).Distinct().ToList();
            }

            dataCm = (await _odataService.GetCollectionData(selectQueryBuilder,
                                customerNos: customerNos,
                                startPostingDate: cyfd, endPostingDate: cyld)).ToList();

            dataCy = (await _odataService.GetCollectionData(selectQueryBuilder,
                                customerNos: customerNos,
                                startPostingDate: cfyfd, endPostingDate: cyld)).ToList();

            dataBounceCm = (await _odataService.GetCollectionData(selectQueryBuilder,
                                    customerNos: customerNos,
                                    startClearDate: cyfd, endClearDate: cyld,
                                    bounceStatus: ConstantsValue.ChequeBounceStatus)).ToList();

            dataBounceCy = (await _odataService.GetCollectionData(selectQueryBuilder,
                                    customerNos: customerNos,
                                    startClearDate: cfyfd, endClearDate: cyld,
                                    bounceStatus: ConstantsValue.ChequeBounceStatus)).ToList();
            #endregion

            var result = new ChequeSummaryReportResultModel();

            //result.CustomerNo = dataCy.FirstOrDefault()?.CustomerNo ?? string.Empty;
            //result.CustomerName = dataCy.FirstOrDefault()?.CustomerName ?? string.Empty;

            #region Cheque Details
            result.ChequeDetails = new List<ChequeSummaryChequeDetailsReportModel>();

            var totalChqRec = new ChequeSummaryChequeDetailsReportModel();
            totalChqRec.ChequeDetailsName = "Total Chq Rec";
            totalChqRec.MTDNoOfCheque = dataCm.Count();
            totalChqRec.YTDNoOfCheque = dataCy.Count();
            totalChqRec.MTDTotalChequeValue = dataCm.Sum(s => CustomConvertExtension.ObjectToDecimal(s.Amount));
            totalChqRec.YTDTotalChequeValue = dataCy.Sum(s => CustomConvertExtension.ObjectToDecimal(s.Amount));

            var totalChqBncd = new ChequeSummaryChequeDetailsReportModel();
            totalChqBncd.ChequeDetailsName = "Total Chq Bncd";
            totalChqBncd.MTDNoOfCheque = dataBounceCm.Count();
            totalChqBncd.YTDNoOfCheque = dataBounceCy.Count();
            totalChqBncd.MTDTotalChequeValue = dataBounceCm.Sum(s => CustomConvertExtension.ObjectToDecimal(s.Amount));
            totalChqBncd.YTDTotalChequeValue = dataBounceCy.Sum(s => CustomConvertExtension.ObjectToDecimal(s.Amount));

            var bncdPercent = new ChequeSummaryChequeDetailsReportModel();
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
            result.ChequeBounceDetails = dataBounceCm.Select(s => new ChequeSummaryChequeBounceDetailsReportModel()
            {
                CustomerNo = s.CustomerNo,
                CustomerName = s.CustomerName,
                Date = CustomConvertExtension.ObjectToDateTime(s.PostingDate).DateFormat("dd MMM yyyy"),
                ChequeNo = s.ChequeNo,
                Amount = CustomConvertExtension.ObjectToDecimal(s.Amount)
            }).ToList();
            #endregion

            return result;
        }

        public async Task<CustomerCreditStatusResultModel> GetCustomerCreditStatus(CustomerCreditStatusSearchModel model)
        {
            var selectQueryBuilder = new SelectQueryOptionBuilder();
            foreach (var prop in typeof(CustomerCreditDataModel).GetProperties())
            {
                selectQueryBuilder.AddProperty(prop.Name);
            }

            var data = (await _odataService.GetCustomerCreditData(selectQueryBuilder, model.CustomerNo, model.CreditControlArea)).ToList();

            var result = data.FirstOrDefault();

            var modelResult = new CustomerCreditStatusResultModel();

            if (result != null)
            {
                modelResult.CreditLimit = CustomConvertExtension.ObjectToDecimal(result.CreditLimit);
                modelResult.LastPayment = CustomConvertExtension.ObjectToDecimal(result.LastPayment);
                modelResult.LastPaymentDate = string.IsNullOrEmpty(result.LastPaymentDate) || result.LastPaymentDate == "00000000"
                                                ? string.Empty
                                                : result.LastPaymentDate.DateFormatDate("yyyyMMdd").DateFormat("dd.MM.yyyy");

                modelResult.CreditLimitUsed = CustomConvertExtension.ObjectToDecimal(result.Receivable) + CustomConvertExtension.ObjectToDecimal(result.OpenDelivery) +
                                                CustomConvertExtension.ObjectToDecimal(result.OpenOrder) + CustomConvertExtension.ObjectToDecimal(result.OpenBill);
                modelResult.RemainingLimit = modelResult.CreditLimit - modelResult.CreditLimitUsed;
                modelResult.CreditLimitUsedPercentage = modelResult.CreditLimit == 0 ? 0 : (modelResult.CreditLimitUsed / modelResult.CreditLimit) * 100;
            }

            return modelResult;
        }
    }
}
