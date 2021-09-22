using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Berger.Common.Constants;
using Berger.Common.Extensions;
using Berger.Common.HttpClient;
using Berger.Common.JSONParser;
using Berger.Data.MsfaEntity.SAPReports;
using Berger.Data.MsfaEntity.SAPTables;
using Berger.Odata.Common;
using Berger.Odata.Extensions;
using Berger.Odata.Model;
using Berger.Odata.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Berger.Odata.Services
{
    public class FinancialDataService : IFinancialDataService
    {
        private readonly IODataService _odataService;
        private readonly IODataCommonService _odataCommonService;
        private readonly IODataSAPRepository<SummaryPerformanceReport> _summaryPerformanceReportRepo;
        private readonly IODataApplicationRepository<DealerInfo> _dealarInfoRepository;

        public FinancialDataService(
            IODataService odataService,
            IODataCommonService odataCommonService,
            IODataSAPRepository<SummaryPerformanceReport> summaryPerformanceReportRepo,
            IODataApplicationRepository<DealerInfo> dealarInfoRepository
            )
        {
            _odataService = odataService;
            _odataCommonService = odataCommonService;
            _summaryPerformanceReportRepo = summaryPerformanceReportRepo;
            _dealarInfoRepository = dealarInfoRepository;
        }

        public async Task<IList<OutstandingDetailsResultModel>> GetOutstandingDetails(OutstandingDetailsSearchModel model)
        {
            //var currentDate = DateTime.Now;
            //var fromDate = (new DateTime(2011, 01, 01)).DateTimeFormat(); // need to get all data so date not fixed

            var selectQueryBuilder = new SelectQueryOptionBuilder();
            selectQueryBuilder.AddProperty(FinancialColDef.InvoiceNo)
                                .AddProperty(FinancialColDef.CreditControlArea)
                                .AddProperty(FinancialColDef.Age)
                                .AddProperty(FinancialColDef.PostingDate)
                                .AddProperty(FinancialColDef.Amount);

            //var data = (await _odataService.GetFinancialDataByCustomerAndCreditControlArea(selectQueryBuilder, model.CustomerNo, fromDate)).ToList();
            var data = (await _odataService.GetFinancialDataByCustomerAndCreditControlArea(selectQueryBuilder, model.CustomerNo, creditControlArea: model.CreditControlArea)).ToList();

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
            Func<FinancialDataModel, bool> predicateFuncFinal = x => predicateFunc(x);

            var result = data.Where(predicateFuncFinal).Select(x =>
                                new OutstandingDetailsResultModel()
                                {
                                    InvoiceNo = x.InvoiceNo,
                                    Age = x.Age,
                                    PostingDate = x.PostingDate.ReturnDateFormatDate(format: "yyyy-MM-ddTHH:mm:ssZ"),
                                    Amount = CustomConvertExtension.ObjectToDecimal(x.Amount)
                                }).ToList();

            result = result.OrderBy(x => x.PostingDate.DateFormatDate("yyyy-MM-ddTHH:mm:ssZ")).ToList();

            return result;
        }

        public async Task<IList<OutstandingSummaryResultModel>> GetOutstandingSummary(OutstandingSummarySearchModel model)
        {
            //var currentDate = DateTime.Now;
            //var fromDate = (new DateTime(2011, 01, 01)).DateTimeFormat(); // need to get all data so date not fixed

            var selectCustomerQueryBuilder = new SelectQueryOptionBuilder();
            //foreach (var prop in typeof(CustomerDataModel).GetProperties())
            //{
            //    selectCustomerQueryBuilder.AddProperty(prop.Name);
            //}
            selectCustomerQueryBuilder.AddProperty(nameof(CustomerDataModel.CustomerNo))
                                .AddProperty(nameof(CustomerDataModel.Channel))
                                .AddProperty(nameof(CustomerDataModel.CreditControlArea))
                                .AddProperty(nameof(CustomerDataModel.CreditLimit));

            var selectQueryBuilder = new SelectQueryOptionBuilder();
            selectQueryBuilder.AddProperty(FinancialColDef.CustomerNo)
                                .AddProperty(FinancialColDef.CustomerName)
                                .AddProperty(FinancialColDef.CreditControlArea)
                                .AddProperty(FinancialColDef.DayLimit)
                                .AddProperty(FinancialColDef.Age)
                                .AddProperty(FinancialColDef.Amount);

            var customerData = (await _odataService.GetCustomerDataByCustomerNo(selectCustomerQueryBuilder, model.CustomerNo)).ToList();
            //var data = (await _odataService.GetFinancialDataByCustomerAndCreditControlArea(selectQueryBuilder, model.CustomerNo, fromDate)).ToList();
            var data = (await _odataService.GetFinancialDataByCustomerAndCreditControlArea(selectQueryBuilder, model.CustomerNo)).ToList();

            var groupData = data.GroupBy(x => x.CreditControlArea).ToList();

            var result = groupData.Select(x =>
                                    {
                                        var osModel = new OutstandingSummaryResultModel();
                                        osModel.CreditControlArea = x.FirstOrDefault()?.CreditControlArea ?? string.Empty;
                                        osModel.DaysLimit = x.FirstOrDefault()?.DayLimit ?? string.Empty;
                                        osModel.ValueLimit = customerData.Where(f => f.Channel == ConstantsValue.DistrbutionChannelDealer && f.CreditControlArea == osModel.CreditControlArea)
                                                                            .GroupBy(g => g.CreditLimit).Sum(c => c.Key);
                                        osModel.NetDue = x.Sum(s => CustomConvertExtension.ObjectToDecimal(s.Amount));
                                        osModel.Slippage = x.Where(w => CustomConvertExtension.ObjectToInt(w.DayLimit) < CustomConvertExtension.ObjectToInt(w.Age))
                                                            .Sum(s => CustomConvertExtension.ObjectToDecimal(s.Amount));
                                        osModel.HighestDaysInvoice = x.Max(m => CustomConvertExtension.ObjectToInt(m.Age)).ToString();
                                        return osModel;
                                    }).ToList();

            #region Credit Control Area 
            var creditControlAreas = await _odataCommonService.GetAllCreditControlAreasAsync();

            foreach (var item in result)
            {
                item.CreditControlAreaName = $"{creditControlAreas.FirstOrDefault(f => f.CreditControlAreaId.ToString() == item.CreditControlArea)?.Description ?? string.Empty} ({ConstantsApplication.SpaceString}{item.CreditControlArea})";
            }
            #endregion

            result = result.OrderBy(x => x.CreditControlArea).ToList();

            return result;
        }

        public async Task<IList<OutstandingSummaryReportResultModel>> GetOutstandingSummaryReport(OutstandingSummaryReportSearchModel model)
        {
            var filterEndDate = DateTime.Now.FinancialSearchDateTimeFormat();

            var selectCustomerQueryBuilder = new SelectQueryOptionBuilder();
            selectCustomerQueryBuilder.AddProperty(nameof(CustomerDataModel.CustomerNo))
                                .AddProperty(nameof(CustomerDataModel.CreditControlArea))
                                .AddProperty(nameof(CustomerDataModel.CreditLimit));

            var selectQueryBuilder = new SelectQueryOptionBuilder();
            selectQueryBuilder.AddProperty(FinancialColDef.CustomerNo)
                                .AddProperty(FinancialColDef.CustomerName)
                                .AddProperty(FinancialColDef.CreditControlArea)
                                .AddProperty(FinancialColDef.DayLimit)
                                .AddProperty(FinancialColDef.Age)
                                .AddProperty(FinancialColDef.Amount);

            var customerData = (await _odataService.GetCustomerData(selectCustomerQueryBuilder,
                                depots: model.Depots, territories: model.Territories, zones: model.Zones,
                                channel: ConstantsValue.DistrbutionChannelDealer, creditControlArea: model.CreditControlArea)).ToList();

            var customerNos = customerData.Select(x => x.CustomerNo).Distinct().ToList();

            #region financial data call by single customer
            var data = new List<FinancialDataModel>();

            foreach (var customerNo in customerNos)
            {
                var dataSingle = (await _odataService.GetFinancialData(selectQueryBuilder, customerNo, filterEndDate, creditControlArea: model.CreditControlArea)).ToList();
                if(dataSingle.Any())
                {
                    data.AddRange(dataSingle);
                }
            }
            #endregion

            var groupData = data.GroupBy(x => x.CreditControlArea).ToList();

            var result = groupData.Select(x =>
                                    {
                                        var osModel = new OutstandingSummaryReportResultModel();
                                        osModel.CreditControlArea = x.FirstOrDefault()?.CreditControlArea ?? string.Empty;
                                        osModel.ValueLimit = customerData.GroupBy(g => g.CustomerNo)
                                                                .Sum(s => s.Where(f => f.CreditControlArea == osModel.CreditControlArea)
                                                                            .GroupBy(g => g.CreditLimit).Sum(c => c.Key));
                                        osModel.NetDue = x.Sum(s => CustomConvertExtension.ObjectToDecimal(s.Amount));
                                        osModel.Slippage = x.Where(w => CustomConvertExtension.ObjectToInt(w.DayLimit) < CustomConvertExtension.ObjectToInt(w.Age))
                                                                .Sum(s => CustomConvertExtension.ObjectToDecimal(s.Amount));
                                        osModel.OSOver90Days = x.Where(m => CustomConvertExtension.ObjectToInt(m.Age) > 90)
                                                                .Sum(s => CustomConvertExtension.ObjectToDecimal(s.Amount));
                                        return osModel;
                                    }).ToList();

            #region Credit Control Area 
            var creditControlAreas = await _odataCommonService.GetAllCreditControlAreasAsync();

            foreach (var item in result)
            {
                var creditControlArea = creditControlAreas.FirstOrDefault(f => f.CreditControlAreaId.ToString() == item.CreditControlArea);

                item.CreditControlArea = creditControlArea != null ?
                                            $"{creditControlArea.Description} ({creditControlArea.CreditControlAreaId})"
                                            : item.CreditControlArea;
            }
            #endregion

            return result;
        }

        public async Task<IList<OSOver90DaysTrendReportResultModel>> GetOSOver90DaysTrendReport(OSOver90DaysTrendSearchModel model)
        {
            if (model.CreditControlArea == "-1") model.CreditControlArea = string.Empty;
            var currentDate = DateTime.Now;
            var fmDate = currentDate.AddMonths(-3);
            var smDate = currentDate.AddMonths(-2);
            var tmDate = currentDate.AddMonths(-1);

            var salesDates = new List<string>
            {
                fmDate.Year.ToString()+fmDate.Month.ToString(),
                smDate.Year.ToString()+smDate.Month.ToString(),
                tmDate.Year.ToString()+tmDate.Month.ToString()
            };

            var filterFinancialEndDateFM = fmDate.GetCYLD().FinancialSearchDateTimeFormat();
            var filterFinancialEndDateSM = smDate.GetCYLD().FinancialSearchDateTimeFormat();
            var filterFinancialEndDateTM = tmDate.GetCYLD().FinancialSearchDateTimeFormat();

            //var filterSalesFromDate = fmDate.GetCYFD().SalesSearchDateFormat();
            //var filterSalesToDate = tmDate.GetCYLD().SalesSearchDateFormat();
            //var fmFromDate = fmDate.GetCYFD();
            //var fmToDate = fmDate.GetCYLD();
            //var smFromDate = smDate.GetCYFD();
            //var smToDate = smDate.GetCYLD();
            //var tmFromDate = tmDate.GetCYFD();
            //var tmToDate = tmDate.GetCYLD();

            //var selectCustomerQueryBuilder = new SelectQueryOptionBuilder();
            //selectCustomerQueryBuilder.AddProperty(nameof(CustomerDataModel.CustomerNo));

            //var selectSalesQueryBuilder = new SelectQueryOptionBuilder();
            //selectSalesQueryBuilder.AddProperty(DataColumnDef.CustomerNoOrSoldToParty)
            //                            .AddProperty(DataColumnDef.Date)
            //                            .AddProperty(DataColumnDef.NetAmount);

            var selectFinancialQueryBuilder = new SelectQueryOptionBuilder();
            selectFinancialQueryBuilder.AddProperty(FinancialColDef.CustomerNo)
                                        .AddProperty(FinancialColDef.Age)
                                        .AddProperty(FinancialColDef.Amount);

            //var salesData = (await _odataService.GetSalesData(selectSalesQueryBuilder,
            //                    filterSalesFromDate, filterSalesToDate,
            //                    depots: model.Depots, territories: model.Territories, zones: model.Zones,
            //                    creditControlArea: model.CreditControlArea)).ToList();

            var salesData = await _summaryPerformanceReportRepo.FindAll(
                x => salesDates.Contains(x.Year.ToString()+x.Month.ToString())
                    && (!model.Depots.Any() || model.Depots.Contains(x.Depot))
                    && (!model.Territories.Any() || model.Territories.Contains(x.Territory))
                    && (!model.Zones.Any() || model.Zones.Contains(x.Zone))
                    && (string.IsNullOrEmpty(model.CreditControlArea) || model.CreditControlArea == x.CreditControlArea)
                ).GroupBy(x=>x.Month).Select(x => new { Month = x.Key, Value = x.Sum(y=>y.Value) }).ToListAsync();

            //var customerData = (await _odataService.GetCustomerData(selectCustomerQueryBuilder,
            //                    depots: model.Depots, territories: model.Territories, zones: model.Zones,
            //                    channel: ConstantsValue.DistrbutionChannelDealer, creditControlArea: model.CreditControlArea)).ToList();

            var customerData = (await _dealarInfoRepository.GetAllIncludeAsync(x => new { x.CustomerNo },
                                x => (model.Depots.Any() || model.Depots.Contains(x.BusinessArea))
                                && (model.Territories.Any() || model.Territories.Contains(x.Territory))
                                && (model.Zones.Any() || model.Zones.Contains(x.CustZone))
                                && x.Channel == ConstantsValue.DistrbutionChannelDealer
                                && (string.IsNullOrEmpty(model.CreditControlArea) || x.CreditControlArea == model.CreditControlArea),
                                null, null, true));

            var customerNos = customerData.Select(x => x.CustomerNo).Distinct().ToList();

            #region financial data call by single customer
            var financialDataFM = new List<FinancialDataModel>();
            var financialDataSM = new List<FinancialDataModel>();
            var financialDataTM = new List<FinancialDataModel>();

            foreach (var customerNo in customerNos)
            {
                var dataSingle = (await _odataService.GetFinancialData(selectFinancialQueryBuilder, customerNo, filterFinancialEndDateFM, creditControlArea: model.CreditControlArea)).ToList();
                if (dataSingle.Any())
                {
                    financialDataFM.AddRange(dataSingle);
                }
            }

            foreach (var customerNo in customerNos)
            {
                var dataSingle = (await _odataService.GetFinancialData(selectFinancialQueryBuilder, customerNo, filterFinancialEndDateSM, creditControlArea: model.CreditControlArea)).ToList();
                if (dataSingle.Any())
                {
                    financialDataSM.AddRange(dataSingle);
                }
            }

            foreach (var customerNo in customerNos)
            {
                var dataSingle = (await _odataService.GetFinancialData(selectFinancialQueryBuilder, customerNo, filterFinancialEndDateTM, creditControlArea: model.CreditControlArea)).ToList();
                if (dataSingle.Any())
                {
                    financialDataTM.AddRange(dataSingle);
                }
            }
            #endregion

            var result = new List<OSOver90DaysTrendReportResultModel>();

            var resFM = new OSOver90DaysTrendReportResultModel();
            resFM.Month = fmDate.ToString("MMMM");
            resFM.OSOver90Days = financialDataFM.Where(x => CustomConvertExtension.ObjectToInt(x.Age) > 90).Sum(s => CustomConvertExtension.ObjectToDecimal(s.Amount));
            resFM.Difference = 0;
            resFM.Sales = salesData.Where(x => x.Month == fmDate.Month)
                                    .Sum(s => s.Value);
            resFM.OSPercentageWithSales = _odataService.GetPercentage(resFM.Sales, resFM.OSOver90Days);

            result.Add(resFM);

            var resSM = new OSOver90DaysTrendReportResultModel();
            resSM.Month = smDate.ToString("MMMM");
            resSM.OSOver90Days = financialDataSM.Where(x => CustomConvertExtension.ObjectToInt(x.Age) > 90).Sum(s => CustomConvertExtension.ObjectToDecimal(s.Amount));
            resSM.Difference = resFM.OSOver90Days - resSM.OSOver90Days;
            resSM.Sales = salesData.Where(x => x.Month == smDate.Month)
                                    .Sum(s => s.Value);
            resSM.OSPercentageWithSales = _odataService.GetPercentage(resSM.Sales, resSM.OSOver90Days);

            result.Add(resSM);

            var resTM = new OSOver90DaysTrendReportResultModel();
            resTM.Month = tmDate.ToString("MMMM");
            resTM.OSOver90Days = financialDataTM.Where(x => CustomConvertExtension.ObjectToInt(x.Age) > 90).Sum(s => CustomConvertExtension.ObjectToDecimal(s.Amount));
            resTM.Difference = resSM.OSOver90Days - resTM.OSOver90Days;
            resTM.Sales = salesData.Where(x => x.Month == tmDate.Month)
                                    .Sum(s => s.Value);
            resTM.OSPercentageWithSales = _odataService.GetPercentage(resTM.Sales, resTM.OSOver90Days);

            result.Add(resTM);

            return result;
        }
        
        public async Task<IList<PortalOSOver90DaysTrendResultModel>> GetPortalOSOver90DaysTrendReport(PortalOSOver90DaysTrendSearchModel model)
        {
            if (model.CreditControlArea == "-1") model.CreditControlArea = string.Empty;
            var currentDate = new DateTime(model.Year, model.Month, 01);
            var fmDate = currentDate.AddMonths(-2);
            var smDate = currentDate.AddMonths(-1);
            var tmDate = currentDate;

            var salesDates = new List<string>
            {
                fmDate.Year.ToString()+fmDate.Month.ToString(),
                smDate.Year.ToString()+smDate.Month.ToString(),
                tmDate.Year.ToString()+tmDate.Month.ToString()
            };

            var filterFinancialEndDateFM = fmDate.GetCYLD().FinancialSearchDateTimeFormat();
            var filterFinancialEndDateSM = smDate.GetCYLD().FinancialSearchDateTimeFormat();
            var filterFinancialEndDateTM = tmDate.GetCYLD().FinancialSearchDateTimeFormat();

            //var filterSalesFromDate = fmDate.GetCYFD().SalesSearchDateFormat();
            //var filterSalesToDate = tmDate.GetCYLD().SalesSearchDateFormat();
            //var fmFromDate = fmDate.GetCYFD();
            //var fmToDate = fmDate.GetCYLD();
            //var smFromDate = smDate.GetCYFD();
            //var smToDate = smDate.GetCYLD();
            //var tmFromDate = tmDate.GetCYFD();
            //var tmToDate = tmDate.GetCYLD();

            //var selectCustomerQueryBuilder = new SelectQueryOptionBuilder();
            //selectCustomerQueryBuilder.AddProperty(nameof(CustomerDataModel.CustomerNo));

            //var selectSalesQueryBuilder = new SelectQueryOptionBuilder();
            //selectSalesQueryBuilder.AddProperty(DataColumnDef.CustomerNoOrSoldToParty)
            //                            .AddProperty(DataColumnDef.Date)
            //                            .AddProperty(DataColumnDef.NetAmount);

            var selectFinancialQueryBuilder = new SelectQueryOptionBuilder();
            selectFinancialQueryBuilder.AddProperty(FinancialColDef.CustomerNo)
                                        .AddProperty(FinancialColDef.Age)
                                        .AddProperty(FinancialColDef.Amount);

            //var salesData = (await _odataService.GetSalesData(selectSalesQueryBuilder,
            //                    filterSalesFromDate, filterSalesToDate,
            //                    depots: new List<string> { model.Depot }, territories: model.Territories, zones: model.Zones,
            //                    creditControlArea: model.CreditControlArea)).ToList();

            var salesData = await _summaryPerformanceReportRepo.FindAll(
                x => salesDates.Contains(x.Year.ToString() + x.Month.ToString())
                    && (string.IsNullOrEmpty(model.Depot) || model.Depot==x.Depot)
                    && (!model.SalesGroups.Any() || model.SalesGroups.Contains(x.SalesGroup))
                    && (!model.Territories.Any() || model.Territories.Contains(x.Territory))
                    && (!model.Zones.Any() || model.Zones.Contains(x.Zone))
                    && (string.IsNullOrEmpty(model.CreditControlArea) || model.CreditControlArea == x.CreditControlArea)
                ).GroupBy(x => x.Month).Select(x => new { Month = x.Key, Value = x.Sum(y => y.Value) }).ToListAsync();

            //var customerData = (await _odataService.GetCustomerData(selectCustomerQueryBuilder,
            //                    depots: new List<string> { model.Depot }, salesGroups: model.SalesGroups, territories: model.Territories, zones: model.Zones,
            //                    channel: ConstantsValue.DistrbutionChannelDealer, creditControlArea: model.CreditControlArea)).ToList();

            var customerData = (await _dealarInfoRepository.GetAllIncludeAsync(x => new { x.CustomerNo },
                                x => (string.IsNullOrEmpty(model.Depot) || model.Depot == x.BusinessArea)
                                && (model.SalesGroups.Any() || model.SalesGroups.Contains(x.SalesGroup))
                                && (model.Territories.Any() || model.Territories.Contains(x.Territory))
                                && (model.Zones.Any() || model.Zones.Contains(x.CustZone))
                                && x.Channel == ConstantsValue.DistrbutionChannelDealer
                                && (string.IsNullOrEmpty(model.CreditControlArea) || x.CreditControlArea == model.CreditControlArea),
                                null, null, true));

            var customerNos = customerData.Select(x => x.CustomerNo).Distinct().ToList();

            #region financial data call by single customer
            var financialDataFM = new List<FinancialDataModel>();
            var financialDataSM = new List<FinancialDataModel>();
            var financialDataTM = new List<FinancialDataModel>();

            foreach (var customerNo in customerNos)
            {
                var dataSingle = (await _odataService.GetFinancialData(selectFinancialQueryBuilder, customerNo, filterFinancialEndDateFM, creditControlArea: model.CreditControlArea)).ToList();
                if (dataSingle.Any())
                {
                    financialDataFM.AddRange(dataSingle);
                }
            }

            foreach (var customerNo in customerNos)
            {
                var dataSingle = (await _odataService.GetFinancialData(selectFinancialQueryBuilder, customerNo, filterFinancialEndDateSM, creditControlArea: model.CreditControlArea)).ToList();
                if (dataSingle.Any())
                {
                    financialDataSM.AddRange(dataSingle);
                }
            }

            foreach (var customerNo in customerNos)
            {
                var dataSingle = (await _odataService.GetFinancialData(selectFinancialQueryBuilder, customerNo, filterFinancialEndDateTM, creditControlArea: model.CreditControlArea)).ToList();
                if (dataSingle.Any())
                {
                    financialDataTM.AddRange(dataSingle);
                }
            }
            #endregion

            var result = new List<PortalOSOver90DaysTrendResultModel>();

            var resFM = new PortalOSOver90DaysTrendResultModel();
            resFM.Month = fmDate.ToString("MMMM");
            resFM.OSOver90Days = financialDataFM.Where(x => CustomConvertExtension.ObjectToInt(x.Age) > 90).Sum(s => CustomConvertExtension.ObjectToDecimal(s.Amount));
            resFM.Difference = 0;
            resFM.Sales = salesData.Where(x => fmDate.Month==x.Month)
                                    .Sum(s => s.Value);
            resFM.OSPercentageWithSales = _odataService.GetPercentage(resFM.Sales, resFM.OSOver90Days);

            result.Add(resFM);

            var resSM = new PortalOSOver90DaysTrendResultModel();
            resSM.Month = smDate.ToString("MMMM");
            resSM.OSOver90Days = financialDataSM.Where(x => CustomConvertExtension.ObjectToInt(x.Age) > 90).Sum(s => CustomConvertExtension.ObjectToDecimal(s.Amount));
            resSM.Difference = resFM.OSOver90Days - resSM.OSOver90Days;
            resSM.Sales = salesData.Where(x => smDate.Month == x.Month)
                                    .Sum(s => s.Value);
            resSM.OSPercentageWithSales = _odataService.GetPercentage(resSM.Sales, resSM.OSOver90Days);

            result.Add(resSM);

            var resTM = new PortalOSOver90DaysTrendResultModel();
            resTM.Month = tmDate.ToString("MMMM");
            resTM.OSOver90Days = financialDataTM.Where(x => CustomConvertExtension.ObjectToInt(x.Age) > 90).Sum(s => CustomConvertExtension.ObjectToDecimal(s.Amount));
            resTM.Difference = resSM.OSOver90Days - resTM.OSOver90Days;
            resTM.Sales = salesData.Where(x => tmDate.Month == x.Month)
                                    .Sum(s => s.Value);
            resTM.OSPercentageWithSales = _odataService.GetPercentage(resTM.Sales, resTM.OSOver90Days);

            result.Add(resTM);

            return result;
        }

        public async Task<IList<PaymentFollowUpResultModel>> GetPaymentFollowUp(PaymentFollowUpSearchModel model)
        {
            var filterEndDate = DateTime.Now.FinancialSearchDateTimeFormat();

            var selectCustomerQueryBuilder = new SelectQueryOptionBuilder();
            selectCustomerQueryBuilder.AddProperty(nameof(CustomerDataModel.CustomerNo))
                                        .AddProperty(nameof(CustomerDataModel.PriceGroup));

            var selectQueryBuilder = new SelectQueryOptionBuilder();
            selectQueryBuilder.AddProperty(FinancialColDef.CustomerNo)
                                .AddProperty(FinancialColDef.CustomerName)
                                .AddProperty(FinancialColDef.InvoiceNo)
                                .AddProperty(FinancialColDef.PostingDate)
                                .AddProperty(FinancialColDef.Age)
                                .AddProperty(FinancialColDef.Amount)
                                .AddProperty(FinancialColDef.DayLimit);

            var customerData = (await _odataService.GetCustomerData(selectCustomerQueryBuilder, 
                                        depots: model.Depots, territories: model.Territories, zones: model.Zones, 
                                        customerNos: model.CustomerNos, 
                                        channel: ConstantsValue.DistrbutionChannelDealer)).ToList();

            customerData = customerData.Where(x => x.PriceGroup == ConstantsValue.PriceGroupCreditBuyer ||
                                                   x.PriceGroup == ConstantsValue.PriceGroupCashBuyer ||
                                                   x.PriceGroup == ConstantsValue.PriceGroupFastPayCarry).ToList();

            var customerNos = customerData.Select(x => x.CustomerNo).Distinct().ToList();

            #region data call by single customer
            var data = new List<FinancialDataModel>();

            foreach (var customerNo in customerNos)
            {
                var dataSingle = (await _odataService.GetFinancialData(selectQueryBuilder, customerNo, filterEndDate)).ToList();
                if (dataSingle.Any())
                {
                    data.AddRange(dataSingle);
                }
            }
            #endregion

            var result = data.Select(x =>
                                new PaymentFollowUpResultModel()
                                {
                                    CustomerNo = x.CustomerNo,
                                    CustomerName = x.CustomerName,
                                    InvoiceNo = x.InvoiceNo,
                                    InvoiceDateTime = CustomConvertExtension.ObjectToDateTime(x.PostingDate),
                                    InvoiceDate = CustomConvertExtension.ObjectToDateTime(x.PostingDate).DateFormat("dd.MM.yyyy"),
                                    InvoiceAge = CustomConvertExtension.ObjectToInt(x.Age),
                                    DayLimit = CustomConvertExtension.ObjectToInt(x.DayLimit),
                                    NetDue = CustomConvertExtension.ObjectToDecimal(x.Amount)
                                }).ToList();

            
            var rprsDayPolicy = await _odataCommonService.GetAllRPRSPoliciesAsync();

            var rprsCustomerNos = customerData.Where(x => x.PriceGroup == ConstantsValue.PriceGroupCreditBuyer)
                                                .Select(x => x.CustomerNo).Distinct().ToList();

            var fastPayCarryCustomerNos = customerData.Where(x => x.PriceGroup == ConstantsValue.PriceGroupCashBuyer ||
                                                                    x.PriceGroup == ConstantsValue.PriceGroupFastPayCarry)
                                                        .Select(x => x.CustomerNo).Distinct().ToList();
            foreach (var item in result)
            {
                var dayCount = 0;
                if (rprsCustomerNos.Any(x => x == item.CustomerNo))
                {
                    dayCount = rprsDayPolicy.FirstOrDefault(x => CustomConvertExtension.ObjectToInt(item.DayLimit) >= x.FromDaysLimit &&
                                                                CustomConvertExtension.ObjectToInt(item.DayLimit) <= x.ToDaysLimit)?.RPRSDays ?? 0;
                }
                else if (fastPayCarryCustomerNos.Any(x => x == item.CustomerNo))
                {
                    dayCount = 5;
                }

                item.DayLimitRPRS = dayCount;
                item.RPRSDate = item.InvoiceDateTime.AddDays(dayCount).DateFormat("dd.MM.yyyy");
            }
            
            return result;
        }

        public async Task<IList<FinancialDataModel>> GetOsOver90DaysTrend(string dealerId, DateTime fromDate, DateTime toDate, string creditControlArea = "")
        {
            var selectQueryBuilder = new SelectQueryOptionBuilder();
            selectQueryBuilder
                            .AddProperty(FinancialColDef.CustomerNo)
                            .AddProperty(FinancialColDef.Amount)
                            //.AddProperty(FinancialColDef.PostingDate)
                            .AddProperty(FinancialColDef.Date)
                            .AddProperty(FinancialColDef.CreditControlArea)
                            .AddProperty(FinancialColDef.Age);

            var data = (await _odataService.GetFinancialDataByCustomerAndCreditControlArea(selectQueryBuilder, dealerId,
                fromDate.DateTimeFormat(), toDate.DateTimeFormat(), creditControlArea: creditControlArea)).ToList();

            return data.Where(x => CustomConvertExtension.ObjectToInt(x.Age) > 90).ToList();
        }

        public async Task<IList<FinancialDataModel>> GetCustomerSlippageAmount(IList<string> dealerIds, DateTime endDate)
        {
            var endDateStr = endDate.DateTimeFormat();

            var selectQueryBuilder = new SelectQueryOptionBuilder();
            selectQueryBuilder.AddProperty(FinancialColDef.CustomerNo)
                                .AddProperty(FinancialColDef.Amount)
                                .AddProperty(FinancialColDef.DayLimit)
                                .AddProperty(FinancialColDef.Age);

            #region data call by single customer
            var data = new List<FinancialDataModel>();

            foreach (var dealerId in dealerIds)
            {
                var dataSingle = (await _odataService.GetFinancialDataByCustomerAndCreditControlArea(selectQueryBuilder, dealerId, endDate: endDateStr)).ToList();
                if (dataSingle.Any())
                {
                    data.AddRange(dataSingle);
                }
            }
            #endregion

            return data.Where(x => CustomConvertExtension.ObjectToInt(x.Age) > CustomConvertExtension.ObjectToInt(x.DayLimit)).ToList();
        }

        public async Task<(bool HasOS, bool HasSlippage)> CheckCustomerOSSlippage(string dealerId)
        {
            var selectQueryBuilder = new SelectQueryOptionBuilder();
            selectQueryBuilder.AddProperty(FinancialColDef.DayLimit)
                                .AddProperty(FinancialColDef.Age);

            var data = (await _odataService.GetFinancialDataByCustomerAndCreditControlArea(selectQueryBuilder, dealerId)).ToList();

            var hasOS = data.Any();
            var hasSlippage = data.Any(x => CustomConvertExtension.ObjectToInt(x.Age) > CustomConvertExtension.ObjectToInt(x.DayLimit));

            return (hasOS, hasSlippage);
        }
    }
}
