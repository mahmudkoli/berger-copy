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
    public class FinancialDataService : IFinancialDataService
    {
        private readonly IODataService _odataService;
        private readonly IODataCommonService _odataCommonService;

        public FinancialDataService(
            IODataService odataService,
            IODataCommonService odataCommonService
            )
        {
            _odataService = odataService;
            _odataCommonService = odataCommonService;
        }

        public async Task<IList<OutstandingDetailsResultModel>> GetOutstandingDetails(OutstandingDetailsSearchModel model)
        {
            //var currentDate = DateTime.Now;
            var fromDate = (new DateTime(2011, 01, 01)).DateTimeFormat(); // need to get all data so date not fixed

            var selectQueryBuilder = new SelectQueryOptionBuilder();
            selectQueryBuilder.AddProperty(FinancialColDef.InvoiceNo)
                                .AddProperty(FinancialColDef.CreditControlArea)
                                .AddProperty(FinancialColDef.Age)
                                .AddProperty(FinancialColDef.PostingDate)
                                .AddProperty(FinancialColDef.Amount);

            var data = (await _odataService.GetFinancialDataByCustomerAndCreditControlArea(selectQueryBuilder, model.CustomerNo, fromDate)).ToList();

            Func<FinancialDataModel, bool> predicateFunc = x => (model.Days switch
            {
                EnumOutstandingDetailsAgeDays._0_To_30_Days => CustomConvertExtension.ObjectToInt(x.Age) >= 0 &&
                                                                    CustomConvertExtension.ObjectToInt(x.Age) <= 30,
                EnumOutstandingDetailsAgeDays._31_To_60_Days => CustomConvertExtension.ObjectToInt(x.Age) >= 31 &&
                                                                    CustomConvertExtension.ObjectToInt(x.Age) <= 60,
                EnumOutstandingDetailsAgeDays._61_To_90_Days => CustomConvertExtension.ObjectToInt(x.Age) >= 61 &&
                                                                    CustomConvertExtension.ObjectToInt(x.Age) <= 90,
                EnumOutstandingDetailsAgeDays._GT_90_Days => CustomConvertExtension.ObjectToInt(x.Age) > 90,
                _ => true
            });
            Func<FinancialDataModel, bool> predicateFuncFinal = x => predicateFunc(x) && x.CreditControlArea == model.CreditControlArea;

            var result = data.Where(predicateFuncFinal).Select(x =>
                                new OutstandingDetailsResultModel()
                                {
                                    InvoiceNo = x.InvoiceNo,
                                    Age = x.Age,
                                    PostingDate = x.PostingDate.ReturnDateFormatDate(format: "yyyy-MM-ddTHH:mm:ssZ"),
                                    Amount = CustomConvertExtension.ObjectToDecimal(x.Amount)
                                }).ToList();

            return result;
        }

        public async Task<IList<OutstandingSummaryResultModel>> GetOutstandingSummary(OutstandingSummarySearchModel model)
        {
            //var currentDate = DateTime.Now;
            var fromDate = (new DateTime(2011, 01, 01)).DateTimeFormat(); // need to get all data so date not fixed

            var selectCustomerQueryBuilder = new SelectQueryOptionBuilder();
            foreach (var prop in typeof(CustomerDataModel).GetProperties())
            {
                selectCustomerQueryBuilder.AddProperty(prop.Name);
            }

            var selectQueryBuilder = new SelectQueryOptionBuilder();
            selectQueryBuilder.AddProperty(FinancialColDef.CustomerNo)
                                .AddProperty(FinancialColDef.CustomerName)
                                .AddProperty(FinancialColDef.CreditControlArea)
                                .AddProperty(FinancialColDef.DayLimit)
                                .AddProperty(FinancialColDef.Age)
                                .AddProperty(FinancialColDef.Amount);

            var customerData = (await _odataService.GetCustomerDataByCustomerNo(selectCustomerQueryBuilder, model.CustomerNo)).ToList();
            var data = (await _odataService.GetFinancialDataByCustomerAndCreditControlArea(selectQueryBuilder, model.CustomerNo, fromDate)).ToList();

            var groupData = data.GroupBy(x => x.CreditControlArea).ToList();

            var result = groupData.Select(x =>
                                    {
                                        var osModel = new OutstandingSummaryResultModel();
                                        osModel.CreditControlArea = x.FirstOrDefault()?.CreditControlArea ?? string.Empty;
                                        osModel.DaysLimit = x.FirstOrDefault()?.DayLimit ?? string.Empty;
                                        osModel.ValueLimit = customerData.FirstOrDefault(f => f.CreditControlArea == osModel.CreditControlArea)?.CreditLimit ?? (decimal)0;
                                        osModel.NetDue = x.Sum(s => CustomConvertExtension.ObjectToDecimal(s.Amount));
                                        osModel.Slippage = x.Where(w => CustomConvertExtension.ObjectToInt(w.DayLimit) > CustomConvertExtension.ObjectToInt(w.Age))
                                                            .Sum(s => CustomConvertExtension.ObjectToDecimal(s.Amount));
                                        osModel.HighestDaysInvoice = x.Max(m => CustomConvertExtension.ObjectToInt(m.Age)).ToString();
                                        return osModel;
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

        public async Task<IList<ReportOutstandingSummaryResultModel>> GetReportOutstandingSummary(IList<int> dealerIds)
        {
            //var currentDate = DateTime.Now;
            var fromDate = (new DateTime(2011, 01, 01)).DateTimeFormat(); // need to get all data so date not fixed

            var selectCustomerQueryBuilder = new SelectQueryOptionBuilder();
            foreach (var prop in typeof(CustomerDataModel).GetProperties())
            {
                selectCustomerQueryBuilder.AddProperty(prop.Name);
            }

            var selectQueryBuilder = new SelectQueryOptionBuilder();
            selectQueryBuilder.AddProperty(FinancialColDef.CustomerNo)
                                .AddProperty(FinancialColDef.CustomerName)
                                .AddProperty(FinancialColDef.CreditControlArea)
                                .AddProperty(FinancialColDef.DayLimit)
                                .AddProperty(FinancialColDef.Age)
                                .AddProperty(FinancialColDef.Amount);

            var customerData = (await _odataService.GetCustomerDataByMultipleCustomerNo(selectCustomerQueryBuilder, dealerIds)).ToList();
            var data = (await _odataService.GetFinancialDataByMultipleCustomerAndCreditControlArea(selectQueryBuilder, dealerIds, fromDate)).ToList();

            var groupData = data.GroupBy(x => x.CreditControlArea).ToList();

            var result = groupData.Select(x =>
                                    {
                                        var osModel = new ReportOutstandingSummaryResultModel();
                                        osModel.CreditControlArea = x.FirstOrDefault()?.CreditControlArea ?? string.Empty;
                                        osModel.ValueLimit = customerData.GroupBy(g => g.CustomerNo)
                                                                .Sum(s => s.FirstOrDefault(f => f.CreditControlArea == osModel.CreditControlArea)?.CreditLimit ?? (decimal)0);
                                        osModel.NetDue = x.Sum(s => CustomConvertExtension.ObjectToDecimal(s.Amount));
                                        osModel.Slippage = x.Where(w => CustomConvertExtension.ObjectToInt(w.DayLimit) > CustomConvertExtension.ObjectToInt(w.Age))
                                                                .Sum(s => CustomConvertExtension.ObjectToDecimal(s.Amount));
                                        osModel.OSOver90Days = x.Where(m => CustomConvertExtension.ObjectToInt(m.Age) > 90)
                                                                .Sum(s => CustomConvertExtension.ObjectToDecimal(s.Amount));
                                        return osModel;
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

        public async Task<IList<ReportOSOver90DaysResultModel>> GetReportOSOver90Days(OSOver90DaysSearchModel model, IList<int> dealerIds)
        {
            var currentDate = DateTime.Now;
            var fmDate = currentDate.AddMonths(-3);
            var smDate = currentDate.AddMonths(-2);
            var tmDate = currentDate.AddMonths(-1);

            var fromDateFM = fmDate.GetCYFD().DateTimeFormat(); // First month
            var toDateFM = fmDate.GetCYLD().DateTimeFormat();
            var fromDateSM = smDate.GetCYFD().DateTimeFormat(); // Second month
            var toDateSM = smDate.GetCYLD().DateTimeFormat();
            var fromDateTM = tmDate.GetCYFD().DateTimeFormat(); // Third month
            var toDateTM = tmDate.GetCYLD().DateTimeFormat();

            var selectQueryBuilder = new SelectQueryOptionBuilder();
            selectQueryBuilder.AddProperty(FinancialColDef.CustomerNo)
                                .AddProperty(FinancialColDef.CustomerName)
                                .AddProperty(FinancialColDef.CreditControlArea)
                                .AddProperty(FinancialColDef.DayLimit)
                                .AddProperty(FinancialColDef.Age)
                                .AddProperty(FinancialColDef.Amount);

            var dataFM = (await _odataService.GetFinancialDataByMultipleCustomerAndCreditControlArea(selectQueryBuilder, dealerIds, fromDateFM, toDateFM, model.CreditControlArea)).ToList();
            var dataSM = (await _odataService.GetFinancialDataByMultipleCustomerAndCreditControlArea(selectQueryBuilder, dealerIds, fromDateSM, toDateSM, model.CreditControlArea)).ToList();
            var dataTM = (await _odataService.GetFinancialDataByMultipleCustomerAndCreditControlArea(selectQueryBuilder, dealerIds, fromDateTM, toDateTM, model.CreditControlArea)).ToList();

            var result = new List<ReportOSOver90DaysResultModel>();

            var res = new ReportOSOver90DaysResultModel();
            res.FirstMonthName = fmDate.ToString("MMMM");
            res.SecondMonthName = smDate.ToString("MMMM");
            res.ThirdMonthName = tmDate.ToString("MMMM");
            res.FirstMonthAmount = dataFM.Where(x => CustomConvertExtension.ObjectToInt(x.Age) > 90).Sum(s => CustomConvertExtension.ObjectToDecimal(s.Amount));
            res.SecondMonthAmount = dataSM.Where(x => CustomConvertExtension.ObjectToInt(x.Age) > 90).Sum(s => CustomConvertExtension.ObjectToDecimal(s.Amount));
            res.ThirdMonthAmount = dataTM.Where(x => CustomConvertExtension.ObjectToInt(x.Age) > 90).Sum(s => CustomConvertExtension.ObjectToDecimal(s.Amount));
            res.SecondMonthChangeAmount = res.SecondMonthAmount - res.FirstMonthAmount;
            res.ThirdMonthChangeAmount = res.ThirdMonthAmount - res.SecondMonthAmount;
            result.Add(res);

            return result;
        }

        public async Task<IList<ReportPaymentFollowUpResultModel>> GetReportPaymentFollowUp(PaymentFollowUpSearchModel model, IList<int> dealerIds)
        {
            //var currentDate = DateTime.Now;
            var fromDate = (new DateTime(2011, 01, 01)).DateTimeFormat(); // need to get all data so date not fixed

            var selectQueryBuilder = new SelectQueryOptionBuilder();
            selectQueryBuilder.AddProperty(FinancialColDef.CustomerNo)
                                .AddProperty(FinancialColDef.CustomerName)
                                .AddProperty(FinancialColDef.InvoiceNo)
                                .AddProperty(FinancialColDef.PostingDate)
                                .AddProperty(FinancialColDef.Age)
                                .AddProperty(FinancialColDef.DayLimit);

            var data = (await _odataService.GetFinancialDataByMultipleCustomerAndCreditControlArea(selectQueryBuilder, dealerIds, fromDate)).ToList();

            var result = data.Select(x =>
                                new ReportPaymentFollowUpResultModel()
                                {
                                    CustomerNo = x.CustomerNo,
                                    CustomerName = x.CustomerName,
                                    InvoiceNo = x.InvoiceNo,
                                    InvoiceDate = x.PostingDate.DateFormatDate(format: "yyyy-MM-ddTHH:mm:ssZ").DateFormat("dd.MM.yyyy"),
                                    InvoiceAge = x.Age,
                                    DayLimit = x.DayLimit
                                }).ToList();

            if (model.PaymentFollowUpType == EnumPaymentFollowUpTypeModel.RPRS)
            {
                var rprsDayPolicy = await _odataCommonService.GetAllRPRSPoliciesAsync();

                foreach (var item in result)
                {
                    var dayCount = rprsDayPolicy.FirstOrDefault(x => CustomConvertExtension.ObjectToInt(item.DayLimit) >= x.FromDaysLimit &&
                                                    CustomConvertExtension.ObjectToInt(item.DayLimit) <= x.ToDaysLimit)?.RPRSDays ?? 0;
                    item.RPRSDate = item.InvoiceDate.DateFormatDate("dd.MM.yyyy").AddDays(dayCount).DateFormat("dd.MM.yyyy");
                }
            }

            return result;
        }

        public async Task<IList<FinancialDataModel>> GetOsOver90DaysTrend(IList<int> dealerIds, DateTime fromDate, DateTime toDate)
        {
            var selectQueryBuilder = new SelectQueryOptionBuilder();
            selectQueryBuilder.AddProperty(FinancialColDef.CustomerNo)
                .AddProperty(FinancialColDef.CustomerNo).AddProperty(FinancialColDef.Amount)
                .AddProperty(FinancialColDef.Age);

            var data = (await _odataService.GetFinancialDataByMultipleCustomerAndCreditControlArea(selectQueryBuilder, dealerIds,
                fromDate.DateTimeFormat(), toDate.DateTimeFormat())).ToList();

            return data.Where(x => CustomConvertExtension.ObjectToInt(x.Age) > 90).ToList();
        }


    }
}
