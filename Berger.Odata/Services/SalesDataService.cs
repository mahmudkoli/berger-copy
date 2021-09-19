using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Berger.Common.Extensions;
using Berger.Common.Model;
using Berger.Data.MsfaEntity.Master;
using Berger.Data.MsfaEntity.SAPReports;
using Berger.Data.MsfaEntity.SAPTables;
using Berger.Data.ViewModel;
using Berger.Odata.Common;
using Berger.Odata.Extensions;
using Berger.Odata.Model;
using Berger.Odata.Model.SPModels;
using Berger.Odata.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Berger.Odata.Services
{
    public class SalesDataService : ISalesDataService
    {
        private readonly IODataService _odataService;
        private readonly IODataBrandService _odataBrandService;
        private readonly IODataCommonService _odataCommonService;
        private readonly IODataSAPRepository<SummaryPerformanceReport> _summaryPerformanceReportRepo;
        private readonly IODataSAPRepository<CustomerPerformanceReport> _customerPerformanceReportRepository;
        private readonly IODataSAPRepository<ColorBankPerformanceReport> _colorBankPerformanceSapRepository;
        private readonly IODataSAPRepository<CategoryWisePerformanceReport> _categoryWisePerformanceReportSapRepository;
        private readonly IODataSAPRepository<CustomerInvoiceReport> _customerInvoiceReportRepository;
        private readonly IODataSAPRepository<SAPSalesInfo> _sapSalesInfoRepository;
        private readonly IODataApplicationRepository<DealerInfo> _dealarInfoRepository;
        public SalesDataService(
            IODataService odataService,
            IODataBrandService odataBrandService,
            IODataCommonService odataCommonService, IODataSAPRepository<SummaryPerformanceReport> summaryPerformanceReportRepo, IODataSAPRepository<CustomerPerformanceReport> customerPerformanceReportRepository, IODataSAPRepository<ColorBankPerformanceReport> colorBankPerformanceSapRepository, IODataSAPRepository<CategoryWisePerformanceReport> categoryWisePerformanceReportSapRepository, IODataSAPRepository<CustomerInvoiceReport> customerInvoiceReportRepository, IODataSAPRepository<SAPSalesInfo> sapSalesInfoRepository, IODataApplicationRepository<DealerInfo> dealarInfoRepository)
        {
            _odataService = odataService;
            _odataBrandService = odataBrandService;
            _odataCommonService = odataCommonService;
            _summaryPerformanceReportRepo = summaryPerformanceReportRepo;
            _customerPerformanceReportRepository = customerPerformanceReportRepository;
            _colorBankPerformanceSapRepository = colorBankPerformanceSapRepository;
            _categoryWisePerformanceReportSapRepository = categoryWisePerformanceReportSapRepository;
            _customerInvoiceReportRepository = customerInvoiceReportRepository;
            _sapSalesInfoRepository = sapSalesInfoRepository;
            _dealarInfoRepository = dealarInfoRepository;
        }

        #region During dealer visit
        public async Task<IList<InvoiceHistoryResultModel>> GetInvoiceHistory(InvoiceHistorySearchModel model)
        {
            var currentDate = DateTime.Now;
            //var fromDate = currentDate.AddMonths(-1).GetCYFD().DateFormat();
            //var toDate = currentDate.GetCYLD().DateFormat();

            //var fromDate = model.FromDate.SalesSearchDateFormat();
            //var toDate = model.ToDate.SalesSearchDateFormat();

            //var selectQueryBuilder = new SelectQueryOptionBuilder();
            //selectQueryBuilder.AddProperty(DataColumnDef.CustomerNoOrSoldToParty)
            //                    .AddProperty(DataColumnDef.InvoiceNoOrBillNo)
            //                    .AddProperty(DataColumnDef.Date)
            //                    .AddProperty(DataColumnDef.NetAmount)
            //                    .AddProperty(DataColumnDef.Time);

            //var data = (await _odataService.GetSalesDataByCustomerAndDivision(selectQueryBuilder, model.CustomerNo, fromDate, toDate, model.Division)).ToList();
            var data = await _customerInvoiceReportRepository.GetAllIncludeAsync(
                x => new { x.InvoiceNoOrBillNo, x.Date, x.Value, x.Time },
                x => x.CustomerNo == model.CustomerNo
                    && (string.IsNullOrEmpty(model.Division) || x.Division == model.Division)
                    && x.Date.Date >= model.FromDate.Date && x.Date.Date <= model.ToDate.Date,
                null, null, true);

            var groupData = data.GroupBy(x => x.InvoiceNoOrBillNo).ToList();

            var result = groupData.Select(x =>
                                new InvoiceHistoryResultModel()
                                {
                                    InvoiceNoOrBillNo = x.Key,
                                    Date = (x.FirstOrDefault()?.Date??default(DateTime)).ToString("dd-MM-yyyy"),
                                    NetAmount = x.Sum(s => s.Value),
                                    Time = x.FirstOrDefault()?.Time??string.Empty
                                }).ToList();

            return result;
        }

        public async Task<InvoiceDetailsResultModel> GetInvoiceDetails(InvoiceDetailsSearchModel model)
        {
            //var filterQueryBuilder = new FilterQueryOptionBuilder();
            //filterQueryBuilder.Equal(DataColumnDef.InvoiceNoOrBillNo, model.InvoiceNo);

            //var selectQueryBuilder = new SelectQueryOptionBuilder();
            //selectQueryBuilder.AddProperty(DataColumnDef.CustomerNoOrSoldToParty)
            //                    .AddProperty(DataColumnDef.CustomerName)
            //                    .AddProperty(DataColumnDef.Division)
            //                    .AddProperty(DataColumnDef.Date)
            //                    .AddProperty(DataColumnDef.DivisionName)
            //                    .AddProperty(DataColumnDef.InvoiceNoOrBillNo)
            //                    .AddProperty(DataColumnDef.LineNumber)
            //                    .AddProperty(DataColumnDef.NetAmount)
            //                    .AddProperty(DataColumnDef.Quantity)
            //                    .AddProperty(DataColumnDef.MatrialCode)
            //                    .AddProperty(DataColumnDef.MatarialDescription)
            //                    .AddProperty(DataColumnDef.UnitOfMeasure);

            //var queryBuilder = new QueryOptionBuilder();
            //queryBuilder.AppendQuery(filterQueryBuilder.Filter)
            //            .AppendQuery(selectQueryBuilder.Select);

            //var data = await _odataService.GetSalesData(queryBuilder.Query);

            var data = await _sapSalesInfoRepository.GetAllIncludeAsync(
                x => new { x.InvoiceNoOrBillNo, x.Date, x.NetAmount, x.Time, x.MatrialCode, 
                    x.MatarialDescription, x.Quantity, x.UnitOfMeasure, x.LineNumber, x.CustomerNo, x.CustomerName, x.Division, x.DivisionName },
                x => x.InvoiceNoOrBillNo == model.InvoiceNo,
                null, null, true);

            var result = data.Select(x => new InvoiceItemDetailsResultModel()
            {
                NetAmount = x.NetAmount,
                Quantity = x.Quantity,
                MatrialCode = x.MatrialCode,
                MatarialDescription = x.MatarialDescription,
                Unit = x.UnitOfMeasure,
                LineNumber = CustomConvertExtension.ObjectToInt(x.LineNumber).ToString(),
            }).ToList();

            var returnResult = new InvoiceDetailsResultModel();

            if (data.Any())
            {
                returnResult.InvoiceNoOrBillNo = data.FirstOrDefault()?.InvoiceNoOrBillNo??string.Empty;
                returnResult.Date = (data.FirstOrDefault()?.Date ?? default(DateTime)).ToString("dd-MM-yyyy");
                returnResult.NetAmount = data.Sum(x => x.NetAmount);
                returnResult.CustomerNo = data.FirstOrDefault()?.CustomerNo ?? string.Empty;
                returnResult.CustomerName = data.FirstOrDefault()?.CustomerName ?? string.Empty;
                returnResult.Division = data.FirstOrDefault()?.Division ?? string.Empty;
                returnResult.DivisionName = data.FirstOrDefault()?.DivisionName ?? string.Empty;

                returnResult.InvoiceItemDetails = result;

                #region get driver data
                var driverData = (await _odataService.GetDriverDataByInvoiceNo(returnResult.InvoiceNoOrBillNo));
                if (driverData != null)
                {
                    returnResult.DriverName = driverData.DriverName;
                    returnResult.DriverMobileNo = driverData.DriverMobileNo;
                }
                #endregion
            }

            return returnResult;
        }

        public async Task<IList<CustomerPerformanceReport>> GetCustomerWiseRevenue(Expression<Func<CustomerPerformanceReport,
            CustomerPerformanceReport>> selectProperty, string customerNo, string startDate, string endDate, string division = "-1", List<string> brands = null)
        {
            brands ??= new List<string>();
            division = string.IsNullOrWhiteSpace(division) ? "-1" : division;
            DateTime stDate = DateTime.ParseExact(startDate, "yyyy.MM.dd", null);
            DateTime edDate = DateTime.ParseExact(endDate, "yyyy.MM.dd", null);
            List<string> yearMonthString = new List<string>();
            while (stDate < edDate)
            {
                yearMonthString.Add(stDate.Year + "-" + stDate.Month);
                stDate = stDate.AddMonths(1);
            }

            return await _customerPerformanceReportRepository.GetAllIncludeAsync(selectProperty, x =>
                    yearMonthString.Contains((x.Year.ToString() + "-" + x.Month.ToString())) &&
                    (string.IsNullOrWhiteSpace(customerNo) || x.CustomerNo == customerNo) &&
                    (division == "-1" || x.Division == division) &&
                    (!brands.Any() || brands.Contains(x.Brand)), null, null, true
            );
        }

        public async Task<IList<ColorBankPerformanceReport>> GetCbProductReport(Expression<Func<ColorBankPerformanceReport,
            ColorBankPerformanceReport>> selectProperty, string customerNo, string startDate, string endDate, string division = "-1", List<string> brands = null,
            List<string> depots = null,
            List<string> territories = null,
            List<string> salesGroup = null,
            List<string> zones = null
            )
        {
            brands ??= new List<string>();
            depots ??= new List<string>();
            territories ??= new List<string>();
            salesGroup ??= new List<string>();
            zones ??= new List<string>();

            division = string.IsNullOrWhiteSpace(division) ? "-1" : division;
            DateTime stDate = DateTime.ParseExact(startDate, "yyyy.MM.dd", null);
            DateTime edDate = DateTime.ParseExact(endDate, "yyyy.MM.dd", null);
            List<string> yearMonthString = new List<string>();
            while (stDate < edDate)
            {
                yearMonthString.Add(stDate.Year + "-" + stDate.Month);
                stDate = stDate.AddMonths(1);
            }

            return await _colorBankPerformanceSapRepository.GetAllIncludeAsync(selectProperty, x =>
                    yearMonthString.Contains((x.Year.ToString() + "-" + x.Month.ToString())) &&
                    (string.IsNullOrWhiteSpace(customerNo) || x.CustomerNo == customerNo) &&
                    (division == "-1" || x.Division == division) &&
                    (!depots.Any() || depots.Contains(x.Depot)) &&
                    (!territories.Any() || territories.Contains(x.Territory)) &&
                    (!salesGroup.Any() || salesGroup.Contains(x.SalesGroup)) &&
                    (!brands.Any() || brands.Contains(x.Brand)) &&
                    (!zones.Any() || zones.Contains(x.Zone)), null, null, true
            );
        }


        public async Task<IList<CustomerPerformanceReport>> GetCbProductCustomerWiseRevenue(string customerNo, string startDate, string endDate, string division = "-1")
        {
            division = string.IsNullOrWhiteSpace(division) ? "-1" : division;


            DateTime stDate = DateTime.ParseExact(startDate, "yyyy.MM.dd", null);
            DateTime edDate = DateTime.ParseExact(endDate, "yyyy.MM.dd", null);


            List<string> yearMonthString = new List<string>();
            while (stDate < edDate)
            {
                yearMonthString.Add(stDate.Year + "-" + stDate.Month);
                stDate = stDate.AddMonths(1);
            }


            return await _colorBankPerformanceSapRepository.FindByCondition(x =>
                    yearMonthString.Contains((x.Year.ToString() + "-" + x.Month.ToString())) &&
                                                                (string.IsNullOrWhiteSpace(customerNo) ||
                                                                 x.CustomerNo == customerNo) &&
                                                                (division == "-1" || x.Division == division))
                   .GroupBy(x => new { x.Brand, Year = x.Year, Month = x.Month })
                   .Select(x => new CustomerPerformanceReport
                   {
                       Brand = x.Key.Brand,
                       TillDateValue = x.Sum(y => y.TillDateValue),
                       Year = x.Key.Year,
                       Month = x.Key.Month,
                   }).ToListAsync();
        }



        public async Task<IList<BrandWiseMTDResultModel>> GetBrandWiseMTDDetails(BrandWiseMTDSearchModel model)
        {
            var currentDate = DateTime.Now;
            var previousMonthCount = 3;
            var cbMaterialCodes = new List<string>();

            var cyfd = currentDate.GetCYFD().DateFormat();//.SalesSearchDateFormat();
            var cylcd = currentDate.GetCYLCD().DateFormat();//.SalesSearchDateFormat();

            var lyfd = currentDate.GetLYFD().DateFormat();//.SalesSearchDateFormat();
            var lylcd = currentDate.GetLYLCD().DateFormat();//.SalesSearchDateFormat();

            IList<CustomerPerformanceReport> dataLy = new List<CustomerPerformanceReport>();
            IList<CustomerPerformanceReport> dataCy = new List<CustomerPerformanceReport>();
            var previousMonthDict = new Dictionary<string, IList<CustomerPerformanceReport>>();

            //var selectQueryBuilder = new SelectQueryOptionBuilder();
            //selectQueryBuilder.AddProperty(DataColumnDef.CustomerNoOrSoldToParty)
            //                    .AddProperty(DataColumnDef.Division)
            //                    .AddProperty(DataColumnDef.InvoiceNoOrBillNo)
            //                    .AddProperty(DataColumnDef.Date)
            //                    .AddProperty(DataColumnDef.NetAmount)
            //                    .AddProperty(DataColumnDef.MatarialGroupOrBrand)
            //                    .AddProperty(DataColumnDef.MatarialGroupOrBrandName);



            var startDate = currentDate.GetMonthDate(-3).GetCYFD().DateFormat();
            var endDate = currentDate.GetMonthDate(-1).GetCYLD().DateFormat();

            IList<CustomerPerformanceReport> data = new List<CustomerPerformanceReport>();
            var brandFamilyInfos = await _odataBrandService.GetBrandFamilyInfosAsync();

            if (model.IsOnlyCBMaterial)
            {
                dataCy = await GetCbProductCustomerWiseRevenue(model.CustomerNo, cyfd, cylcd, model.Division);
                dataLy = await GetCbProductCustomerWiseRevenue(model.CustomerNo, lyfd, lylcd, model.Division);
                data = await GetCbProductCustomerWiseRevenue(model.CustomerNo, startDate, endDate, model.Division);
            }
            else
            {

                dataCy = await GetCustomerWiseRevenue(x => new CustomerPerformanceReport
                {
                    Brand = x.Brand,
                    TillDateValue = x.TillDateValue
                }, model.CustomerNo, cyfd, cylcd, model.Division);

                dataLy = await GetCustomerWiseRevenue(x => new CustomerPerformanceReport
                {
                    Brand = x.Brand,
                    TillDateValue = x.TillDateValue
                }, model.CustomerNo, lyfd, lylcd, model.Division);

                data = await GetCustomerWiseRevenue(x => new CustomerPerformanceReport
                {
                    Brand = x.Brand,
                    TillDateValue = x.TillDateValue,
                    Year = x.Year,
                    Month = x.Month
                }, model.CustomerNo, startDate,
                   endDate, model.Division);
            }

            //dataLy = (await _odataService.GetSalesDataByCustomerAndDivision(selectQueryBuilder, model.CustomerNo, lyfd, lylcd, model.Division, cbMaterialCodes)).ToList();
            // dataCy = (await _odataService.GetSalesDataByCustomerAndDivision(selectQueryBuilder, model.CustomerNo, cyfd, cylcd, model.Division, cbMaterialCodes)).ToList();





            for (int i = 1; i <= previousMonthCount; i++)
            {
                int number = i * -1;
                DateTime date = currentDate.AddMonths(number);
                var monthName = currentDate.GetMonthName(number);

                previousMonthDict.Add(monthName, data.Where(x => x.Year == date.Year && x.Month == date.Month).ToList());
            }



            //for (var i = 1; i <= previousMonthCount; i++)
            //{
            //    int number = i * -1;
            //    //  var startDate = currentDate.GetMonthDate(number).GetCYFD().DateFormat();
            //    //  var endDate = currentDate.GetMonthDate(number).GetCYLD().DateFormat();

            //    var data = (await _odataService.GetSalesDataByCustomerAndDivision(selectQueryBuilder, model.CustomerNo, startDate, endDate, model.Division, cbMaterialCodes)).ToList();
            //    var monthName = currentDate.GetMonthName(number);

            //    previousMonthDict.Add(monthName, data);
            //}

            var result = new List<BrandWiseMTDResultModel>();

            var brandCodes = dataLy.Select(x => x.Brand)
                                .Concat(dataCy.Select(x => x.Brand))
                                    .Concat(previousMonthDict.Values.SelectMany(x => x).Select(x => x.Brand))
                                        .Distinct().ToList();

            foreach (var brandCode in brandCodes)
            {
                var res = new BrandWiseMTDResultModel { PreviousMonthData = new List<BrandWiseMTDPreviousModel>() };

                if (dataLy.Any(x => x.Brand == brandCode))
                {
                    var mtdAmtLy = dataLy.Where(x => x.Brand == brandCode).Sum(x => CustomConvertExtension.ObjectToDecimal(x.TillDateValue));
                    var lastYearSingleData = dataLy.FirstOrDefault(x => x.Brand == brandCode);

                    if (lastYearSingleData != null)
                    {
                        // var brandNameLy = $"{lastYearSingleData.Brand} ({lastYearSingleData.Brand })";

                        res.MatarialGroupOrBrand = string.IsNullOrEmpty(res.MatarialGroupOrBrand) ? lastYearSingleData.Brand : res.MatarialGroupOrBrand;
                    }

                    res.LYMTD = mtdAmtLy;
                }

                if (dataCy.Any(x => x.Brand == brandCode))
                {
                    var mtdAmtCy = dataCy.Where(x => x.Brand == brandCode).Sum(x => CustomConvertExtension.ObjectToDecimal(x.TillDateValue));

                    var cySingleData = dataCy.FirstOrDefault(x => x.Brand == brandCode);

                    if (cySingleData != null)
                    {
                        // var brandNameCy = $"{cySingleData.Brand} ({cySingleData.Brand })";

                        res.MatarialGroupOrBrand = string.IsNullOrEmpty(res.MatarialGroupOrBrand) ? cySingleData.Brand : res.MatarialGroupOrBrand;
                    }

                    res.CYMTD = mtdAmtCy;
                }

                for (var i = 1; i <= previousMonthCount; i++)
                {
                    int number = i * -1;
                    var monthName = currentDate.GetMonthName(number);
                    var dictData = previousMonthDict[monthName].ToList();
                    var mtdAmt = decimal.Zero;

                    if (dictData.Any(x => x.Brand == brandCode))
                    {
                        mtdAmt = dictData.Where(x => x.Brand == brandCode).Sum(x => CustomConvertExtension.ObjectToDecimal(x.TillDateValue));

                        var mtdSingleData = dictData.FirstOrDefault(x => x.Brand == brandCode);

                        if (mtdSingleData != null)
                        {
                            //var brandName = $"{mtdSingleData.Brand} ({mtdSingleData.Brand })";

                            res.MatarialGroupOrBrand = string.IsNullOrEmpty(res.MatarialGroupOrBrand) ? mtdSingleData.Brand : res.MatarialGroupOrBrand;
                        }
                    }

                    res.PreviousMonthData.Add(new BrandWiseMTDPreviousModel() { MonthName = monthName, Amount = mtdAmt });
                }

                res.Growth = _odataService.GetGrowth(res.LYMTD, res.CYMTD);

                result.Add(res);
            }


            result.ForEach(x => x.MatarialGroupOrBrand = model.IsOnlyCBMaterial
                ? null
                : (brandFamilyInfos.FirstOrDefault(y => y.MatarialGroupOrBrand == x.MatarialGroupOrBrand)?
                    .MatarialGroupOrBrandName ?? x.MatarialGroupOrBrand) + $" ({x.MatarialGroupOrBrand})");

            return result;
        }

        public async Task<IList<BrandWisePerformanceResultModel>> GetBrandWisePerformance(BrandWisePerformanceSearchModel model)
        {
            //var filterDate = DateTime.Now.AddMonths(-1);
            var filterDate = new DateTime(model.Year, model.Month, 01);
            var mtsBrandCodes = new List<string>();

            var cyfd = filterDate.GetCYFD().DateFormat();//.SalesSearchDateFormat();
            var cfyfd = filterDate.GetCFYFD().DateFormat();//.SalesSearchDateFormat();
            var cyld = filterDate.GetCYLD().DateFormat();//.SalesSearchDateFormat();

            var lyfd = filterDate.GetLYFD().DateFormat();//.SalesSearchDateFormat();
            var lfyfd = filterDate.GetLFYFD().DateFormat();//.SalesSearchDateFormat();
            var lyld = filterDate.GetLYLD().DateFormat();//.SalesSearchDateFormat();

            //var selectQueryBuilder = new SelectQueryOptionBuilder();
            //selectQueryBuilder.AddProperty(DataColumnDef.PlantOrBusinessArea)
            //                        .AddProperty(model.VolumeOrValue == EnumVolumeOrValue.Volume
            //                                    ? DataColumnDef.Volume
            //                                    : DataColumnDef.NetAmount)
            //                        .AddProperty(model.BrandOrDivision == EnumBrandOrDivision.Division
            //                                    ? DataColumnDef.Division
            //                                    : DataColumnDef.MatarialGroupOrBrand);

            if (model.BrandOrDivision == EnumBrandOrDivision.MTSBrands)
            {
                mtsBrandCodes = (await _odataBrandService.GetMTSBrandCodesAsync()).ToList();
            }



            var dataLyMtd = await GetCustomerWiseRevenue(x => new CustomerPerformanceReport()
            {
                Brand = x.Brand,
                Value = x.Value,
                Volume = x.Volume,
                Division = x.Division
            }, model.CustomerNo, lyfd, lyld, model.Division, mtsBrandCodes);

            var dataCyMtd = await GetCustomerWiseRevenue(x => new CustomerPerformanceReport()
            {
                Brand = x.Brand,
                Value = x.Value,
                Volume = x.Volume,
                Division = x.Division
            }, model.CustomerNo, cyfd, cyld, model.Division, mtsBrandCodes);

            var dataLyYtd = await GetCustomerWiseRevenue(x => new CustomerPerformanceReport()
            {
                Brand = x.Brand,
                Value = x.Value,
                Volume = x.Volume,
                Division = x.Division
            }, model.CustomerNo, lfyfd, lyld, model.Division, mtsBrandCodes);

            var dataCyYtd = await GetCustomerWiseRevenue(x => new CustomerPerformanceReport()
            {
                Brand = x.Brand,
                Value = x.Value,
                Volume = x.Volume,
                Division = x.Division
            }, model.CustomerNo, cfyfd, cyld, model.Division, mtsBrandCodes);



            //var dataLyMtd = (await _odataService.GetSalesDataByCustomerAndDivision(selectQueryBuilder, model.CustomerNo, lyfd, lyld,
            //                        division: model.Division, brands: mtsBrandCodes)).ToList();

            //var dataCyMtd = (await _odataService.GetSalesDataByCustomerAndDivision(selectQueryBuilder, model.CustomerNo, cyfd, cyld,
            //                        division: model.Division, brands: mtsBrandCodes)).ToList();

            //var dataLyYtd = (await _odataService.GetSalesDataByCustomerAndDivision(selectQueryBuilder, model.CustomerNo, lfyfd, lyld,
            //                        division: model.Division, brands: mtsBrandCodes)).ToList();

            //var dataCyYtd = (await _odataService.GetSalesDataByCustomerAndDivision(selectQueryBuilder, model.CustomerNo, cfyfd, cyld,
            //                        division: model.Division, brands: mtsBrandCodes)).ToList();

            Func<CustomerPerformanceReport, decimal> calcFunc = x => CustomConvertExtension.ObjectToDecimal(
                                                            model.VolumeOrValue == EnumVolumeOrValue.Volume ? x.Volume : x.Value);
            Func<CustomerPerformanceReport, string> selectFunc = x => model.BrandOrDivision == EnumBrandOrDivision.Division ?
                                                            x.Division : x.Brand;
            Func<CustomerPerformanceReport, string, bool> predicateFunc = (x, val) => model.BrandOrDivision == EnumBrandOrDivision.Division ?
                                                                    x.Division == val : x.Brand == val;

            var brandsOrDivisions = dataLyMtd.Select(selectFunc)
                                        .Concat(dataCyMtd.Select(selectFunc))
                                            .Concat(dataLyYtd.Select(selectFunc))
                                                .Concat(dataCyYtd.Select(selectFunc))
                                                    .Distinct().ToList();

            //var depots = dataLyMtd.Select(x => x.PlantOrBusinessArea)
            //                .Concat(dataCyMtd.Select(x => x.PlantOrBusinessArea))
            //                    .Concat(dataLyYtd.Select(x => x.PlantOrBusinessArea))
            //                        .Concat(dataCyYtd.Select(x => x.PlantOrBusinessArea))
            //                            .Distinct().ToList();

            var brandFamilyInfos = new List<BrandFamilyInfo>();
            var divisions = new List<Division>();

            if (model.BrandOrDivision == EnumBrandOrDivision.Division)
            {
                divisions = (await _odataCommonService.GetAllDivisionsAsync()).ToList();
            }
            else
            {
                //brandFamilyInfos = (await _odataBrandService.GetBrandFamilyInfosAsync(x => brandsOrDivisions.Any(b => b == x.MatarialGroupOrBrand))).ToList();
                brandFamilyInfos = (await _odataBrandService.GetBrandFamilyInfosAsync()).ToList();
            }

            #region brand family group
            if (model.BrandOrDivision == EnumBrandOrDivision.MTSBrands)
            {
                foreach (var item in dataLyMtd)
                {
                    item.Brand = brandFamilyInfos.FirstOrDefault(x => x.MatarialGroupOrBrand == item.Brand)?
                                                        .MatarialGroupOrBrandFamily ?? item.Brand;
                }
                foreach (var item in dataCyMtd)
                {
                    item.Brand = brandFamilyInfos.FirstOrDefault(x => x.MatarialGroupOrBrand == item.Brand)?
                                                        .MatarialGroupOrBrandFamily ?? item.Brand;
                }
                foreach (var item in dataLyYtd)
                {
                    item.Brand = brandFamilyInfos.FirstOrDefault(x => x.MatarialGroupOrBrand == item.Brand)?
                                                        .MatarialGroupOrBrandFamily ?? item.Brand;
                }
                foreach (var item in dataCyYtd)
                {
                    item.Brand = brandFamilyInfos.FirstOrDefault(x => x.MatarialGroupOrBrand == item.Brand)?
                                                        .MatarialGroupOrBrandFamily ?? item.Brand;
                }

                brandsOrDivisions = dataLyMtd.Select(x => x.Brand)
                                    .Concat(dataCyMtd.Select(x => x.Brand))
                                        .Concat(dataLyYtd.Select(x => x.Brand))
                                            .Concat(dataCyYtd.Select(x => x.Brand))
                                                .Distinct().ToList();
            }
            #endregion

            var result = new List<BrandWisePerformanceResultModel>();

            foreach (var brandOrDiv in brandsOrDivisions)
            {
                var brandOrDivName = brandOrDiv;


                if (model.BrandOrDivision == EnumBrandOrDivision.Division)
                {
                    var divObj = divisions.FirstOrDefault(x => (x.DivisionCode ?? 0).ToString() == brandOrDiv);

                    brandOrDivName = divObj != null
                                        ? $"{divObj.Description} ({divObj.DivisionCode ?? 0})"
                                        : brandOrDiv;
                }
                else
                {
                    var brandFamilyObj = brandFamilyInfos.FirstOrDefault(x => model.BrandOrDivision == EnumBrandOrDivision.MTSBrands
                                                            ? x.MatarialGroupOrBrandFamily == brandOrDiv
                                                            : x.MatarialGroupOrBrand == brandOrDiv);
                    brandOrDivName = brandFamilyObj != null
                                        ? model.BrandOrDivision == EnumBrandOrDivision.MTSBrands
                                            ? $"{brandFamilyObj.MatarialGroupOrBrandFamilyName} ({brandFamilyObj.MatarialGroupOrBrandFamily})"
                                            : $"{brandFamilyObj.MatarialGroupOrBrandName} ({brandFamilyObj.MatarialGroupOrBrand})"
                                        : brandOrDiv;

                    // for showing all group item name code
                    if (model.BrandOrDivision == EnumBrandOrDivision.MTSBrands && brandFamilyObj != null)
                    {
                        brandOrDivName = string.Join(", ", brandFamilyInfos.Where(x => x.MatarialGroupOrBrandFamily == brandOrDiv)
                                                            .Select(x => $"{x.MatarialGroupOrBrandName} ({x.MatarialGroupOrBrand})"));
                    }
                }

                var res = new BrandWisePerformanceResultModel();
                res.BrandOrDivision = brandOrDivName;
                res.LYMTD = dataLyMtd.Where(x => predicateFunc(x, brandOrDiv)).Sum(calcFunc);
                res.CYMTD = dataCyMtd.Where(x => predicateFunc(x, brandOrDiv)).Sum(calcFunc);
                res.LYYTD = dataLyYtd.Where(x => predicateFunc(x, brandOrDiv)).Sum(calcFunc);
                res.CYYTD = dataCyYtd.Where(x => predicateFunc(x, brandOrDiv)).Sum(calcFunc);
                res.GrowthMTD = _odataService.GetGrowth(res.LYMTD, res.CYMTD);
                res.GrowthYTD = _odataService.GetGrowth(res.LYYTD, res.CYYTD);

                result.Add(res);
            }

            return result;
        }
        #endregion

        public async Task<IList<YTDBrandPerformanceSearchModelResultModel>> GetYTDBrandPerformance(YTDBrandPerformanceSearchModelSearchModel model)
        {
            //var filterDate = DateTime.Now.AddMonths(-1);
            //var filterDate = new DateTime(model.Year, model.Month, 01);
            //var mtsBrandCodes = new List<string>();

            //var cyfd = filterDate.GetCYFD().SalesSearchDateFormat();
            //var cfyfd = filterDate.GetCFYFD().SalesSearchDateFormat();
            //var cyld = filterDate.GetCYLD().SalesSearchDateFormat();

            //var lyfd = filterDate.GetLYFD().SalesSearchDateFormat();
            //var lfyfd = filterDate.GetLFYFD().SalesSearchDateFormat();
            //var lyld = filterDate.GetLYLD().SalesSearchDateFormat();

            //var selectQueryBuilder = new SelectQueryOptionBuilder();
            //selectQueryBuilder.AddProperty(DataColumnDef.PlantOrBusinessArea)
            //                        .AddProperty(model.VolumeOrValue == EnumVolumeOrValue.Volume
            //                                    ? DataColumnDef.Volume
            //                                    : DataColumnDef.NetAmount)
            //                        .AddProperty(model.BrandOrDivision == EnumBrandOrDivision.Division
            //                                    ? DataColumnDef.Division
            //                                    : DataColumnDef.MatarialGroupOrBrand);

            //if (model.BrandOrDivision == EnumBrandOrDivision.MTSBrands)
            //{
            //    mtsBrandCodes = (await _odataBrandService.GetMTSBrandCodesAsync()).ToList();
            //}

            //var dataLyMtd = (await _odataService.GetSalesData(selectQueryBuilder, lyfd, lyld,
            //                        depots: model.Depots, territories: model.Territories, zones: model.Zones,
            //                        division: model.Division, brands: mtsBrandCodes)).ToList();

            //var dataCyMtd = (await _odataService.GetSalesData(selectQueryBuilder, cyfd, cyld,
            //                        depots: model.Depots, territories: model.Territories, zones: model.Zones,
            //                        division: model.Division, brands: mtsBrandCodes)).ToList();

            //var dataLyYtd = (await _odataService.GetSalesData(selectQueryBuilder, lfyfd, lyld,
            //                        depots: model.Depots, territories: model.Territories, zones: model.Zones,
            //                        division: model.Division, brands: mtsBrandCodes)).ToList();

            //var dataCyYtd = (await _odataService.GetSalesData(selectQueryBuilder, cfyfd, cyld,
            //                        depots: model.Depots, territories: model.Territories, zones: model.Zones,
            //                        division: model.Division, brands: mtsBrandCodes)).ToList();

            var dataActual = _summaryPerformanceReportRepo.GetDataBySP<spYTDTBrnadPerformanceReport>("spGetYTDBrandPerformanceReports", new List<(string, object)>
            {
                ("Depots", string.Join(",",model.Depots)),
                ("Territories", string.Join(",",model.Territories)),
                ("Zones", string.Join(",",model.Zones)),
                ("Division", model.Division),
                ("Year", model.Year),
                ("Month", model.Month),
                ("VolumeOrValue", model.VolumeOrValue),
                ("BrandOrDivision", model.BrandOrDivision),
            },
            nameof(spYTDTBrnadPerformanceReport.Depots),
            nameof(spYTDTBrnadPerformanceReport.TempBrand));
            foreach (var item in dataActual)
            {
                item.Depots = item.Depot.Split(',').ToList();
            }

            //Func<SalesDataModel, decimal> calcFunc = x => CustomConvertExtension.ObjectToDecimal(
            //                                                model.VolumeOrValue == EnumVolumeOrValue.Volume ? x.Volume : x.NetAmount);
            //Func<SalesDataModel, string> selectFunc = x => model.BrandOrDivision == EnumBrandOrDivision.Division ?
            //                                                x.Division : x.MatarialGroupOrBrand;
            //Func<SalesDataModel, string, bool> predicateFunc = (x, val) => model.BrandOrDivision == EnumBrandOrDivision.Division ?
            //                                                        x.Division == val : x.MatarialGroupOrBrand == val;
            Func<spYTDTBrnadPerformanceReport, string> selectFunc = x => x.BrandOrDivision;
            Func<spYTDTBrnadPerformanceReport, string, bool> predicateFunc = (x, val) => x.BrandOrDivision == val;

            var brandsOrDivisions = dataActual.Select(selectFunc)
                                                    //.Concat(dataCyMtd.Select(selectFunc))
                                                    //    .Concat(dataLyYtd.Select(selectFunc))
                                                    //        .Concat(dataCyYtd.Select(selectFunc))
                                                    .Distinct().ToList();

            var depots = dataActual.SelectMany(x => x.Depots)
                                        //.Concat(dataCyMtd.Select(x => x.PlantOrBusinessArea))
                                        //    .Concat(dataLyYtd.Select(x => x.PlantOrBusinessArea))
                                        //        .Concat(dataCyYtd.Select(x => x.PlantOrBusinessArea))
                                        .Distinct().ToList();

            var brandFamilyInfos = new List<BrandFamilyInfo>();
            var divisions = new List<Division>();

            if (model.BrandOrDivision == EnumBrandOrDivision.Division)
            {
                divisions = (await _odataCommonService.GetAllDivisionsAsync()).ToList();
            }
            else
            {
                //brandFamilyInfos = (await _odataBrandService.GetBrandFamilyInfosAsync(x => brandsOrDivisions.Any(b => b == x.MatarialGroupOrBrand))).ToList();
                brandFamilyInfos = (await _odataBrandService.GetBrandFamilyInfosAsync()).ToList();
            }

            #region brand family group
            if (model.BrandOrDivision == EnumBrandOrDivision.MTSBrands)
            {
                foreach (var item in dataActual)
                {
                    item.BrandOrDivision = brandFamilyInfos.FirstOrDefault(x => x.MatarialGroupOrBrand == item.BrandOrDivision)?
                                                        .MatarialGroupOrBrandFamily ?? item.BrandOrDivision;
                }

                brandsOrDivisions = dataActual.Select(x => x.BrandOrDivision)
                                                //.Concat(dataCyMtd.Select(x => x.MatarialGroupOrBrand))
                                                //    .Concat(dataLyYtd.Select(x => x.MatarialGroupOrBrand))
                                                //        .Concat(dataCyYtd.Select(x => x.MatarialGroupOrBrand))
                                                .Distinct().ToList();
            }
            #endregion

            var result = new List<YTDBrandPerformanceSearchModelResultModel>();

            foreach (var brandOrDiv in brandsOrDivisions)
            {
                var brandOrDivName = brandOrDiv;


                if (model.BrandOrDivision == EnumBrandOrDivision.Division)
                {
                    var divObj = divisions.FirstOrDefault(x => (x.DivisionCode ?? 0).ToString() == brandOrDiv);

                    brandOrDivName = divObj != null
                                        ? $"{divObj.Description} ({divObj.DivisionCode ?? 0})"
                                        : brandOrDiv;
                }
                else
                {
                    var brandFamilyObj = brandFamilyInfos.FirstOrDefault(x => model.BrandOrDivision == EnumBrandOrDivision.MTSBrands
                                                            ? x.MatarialGroupOrBrandFamily == brandOrDiv
                                                            : x.MatarialGroupOrBrand == brandOrDiv);
                    brandOrDivName = brandFamilyObj != null
                                        ? model.BrandOrDivision == EnumBrandOrDivision.MTSBrands
                                            ? $"{brandFamilyObj.MatarialGroupOrBrandFamilyName} ({brandFamilyObj.MatarialGroupOrBrandFamily})"
                                            : $"{brandFamilyObj.MatarialGroupOrBrandName} ({brandFamilyObj.MatarialGroupOrBrand})"
                                        : brandOrDiv;

                    // for showing all group item name code
                    if (model.BrandOrDivision == EnumBrandOrDivision.MTSBrands && brandFamilyObj != null)
                    {
                        brandOrDivName = string.Join(", ", brandFamilyInfos.Where(x => x.MatarialGroupOrBrandFamily == brandOrDiv)
                                                            .Select(x => $"{x.MatarialGroupOrBrandName} ({x.MatarialGroupOrBrand})"));
                    }
                }

                var res = new YTDBrandPerformanceSearchModelResultModel();
                res.Depots = dataActual.Where(x => predicateFunc(x, brandOrDiv))
                                        .SelectMany(x => x.Depots)
                                //.Concat(dataCyMtd.Where(x => predicateFunc(x, brandOrDiv))
                                //        .Select(x => x.PlantOrBusinessArea))
                                //.Concat(dataLyYtd.Where(x => predicateFunc(x, brandOrDiv))
                                //        .Select(x => x.PlantOrBusinessArea))
                                //.Concat(dataCyYtd.Where(x => predicateFunc(x, brandOrDiv))
                                //        .Select(x => x.PlantOrBusinessArea))
                                .Distinct().ToList();
                res.Depot = res.Depots.FirstOrDefault();
                res.BrandOrDivision = brandOrDivName;
                res.LYMTD = dataActual.Where(x => predicateFunc(x, brandOrDiv)).Sum(x => x.LYMTD);
                res.CYMTD = dataActual.Where(x => predicateFunc(x, brandOrDiv)).Sum(x => x.CYMTD);
                res.LYYTD = dataActual.Where(x => predicateFunc(x, brandOrDiv)).Sum(x => x.LYYTD);
                res.CYYTD = dataActual.Where(x => predicateFunc(x, brandOrDiv)).Sum(x => x.CYYTD);
                res.GrowthMTD = _odataService.GetGrowth(res.LYMTD, res.CYMTD);
                res.GrowthYTD = _odataService.GetGrowth(res.LYYTD, res.CYYTD);

                result.Add(res);
            }

            #region get depot data
            if (result.Any())
            {
                var depotsCodeName = await _odataCommonService.GetAllDepotsAsync(x => depots.Contains(x.Werks));

                foreach (var item in result)
                {
                    item.Depots = item.Depots.Select(x =>
                                    depotsCodeName.Any(d => d.Code == x)
                                    ? $"{depotsCodeName.FirstOrDefault(d => d.Code == x).Name} ({x})"
                                    : x).ToList();

                    item.Depot = string.Join(", ", item.Depots);
                }
            }
            #endregion

            return result;
        }

        public async Task<IList<CategoryWiseDealerPerformanceResultModel>> GetCategoryWiseDealerPerformance(CategoryWiseDealerPerformanceSearchModel model)
        {
            //var filterDate = DateTime.Now.AddMonths(-1);
            //var filterDate = new DateTime(model.Year, model.Month, 01);
            //var customerCount = 10;
            //var notPurchasedFromDate = filterDate.AddMonths(-2).GetCYFD();
            //var notPurchasedToDate = filterDate.GetCYLD();

            var customerClassification = model.Category switch
            {
                EnumCustomerClassification.All => string.Empty,
                EnumCustomerClassification.Exclusive => ConstantsValue.CustomerClassificationExclusive,
                EnumCustomerClassification.NonExclusive => ConstantsValue.CustomerClassificationNonExclusive,
                _ => string.Empty
            };

            //var cyfd = filterDate.GetCYFD().SalesSearchDateFormat();
            //var cfyfd = filterDate.GetCFYFD().SalesSearchDateFormat();
            //var cyld = filterDate.GetCYLD().SalesSearchDateFormat();

            //var lyfd = filterDate.GetLYFD().SalesSearchDateFormat();
            //var lfyfd = filterDate.GetLFYFD().SalesSearchDateFormat();
            //var lyld = filterDate.GetLYLD().SalesSearchDateFormat();

            //var selectQueryBuilder = new SelectQueryOptionBuilder();
            //selectQueryBuilder.AddProperty(DataColumnDef.CustomerNoOrSoldToParty)
            //                    .AddProperty(DataColumnDef.CustomerName)
            //                    .AddProperty(DataColumnDef.Date)
            //                    .AddProperty(DataColumnDef.NetAmount);

            //var dataLyMtd = (await _odataService.GetSalesData(selectQueryBuilder, lyfd, lyld,
            //                        depots: model.Depots, territories: model.Territories, zones: model.Zones,
            //                        classification: customerClassification)).ToList();

            //var dataCyMtd = (await _odataService.GetSalesData(selectQueryBuilder, cyfd, cyld,
            //                        depots: model.Depots, territories: model.Territories, zones: model.Zones,
            //                        classification: customerClassification)).ToList();

            //var dataLyYtd = (await _odataService.GetSalesData(selectQueryBuilder, lfyfd, lyld,
            //                        depots: model.Depots, territories: model.Territories, zones: model.Zones,
            //                        classification: customerClassification)).ToList();

            //var dataCyYtd = (await _odataService.GetSalesData(selectQueryBuilder, cfyfd, cyld,
            //                        depots: model.Depots, territories: model.Territories, zones: model.Zones,
            //                        classification: customerClassification)).ToList();


            var dataActual = _summaryPerformanceReportRepo.GetDataBySP<spCategoryWisePerformanceReport>("spGetCategoryWiseDealerPerformanceReports", new List<(string, object)>
            {
                ("Depots", string.Join(",",model.Depots)),
                ("Territories", string.Join(",",model.Territories)),
                ("Zones", string.Join(",",model.Zones)),
                ("Year", model.Year),
                ("Month", model.Month),
                ("CustomerClassification", customerClassification),
                ("PerformanceCategory", model.PerformanceCategory),
            });

            var result = new List<CategoryWiseDealerPerformanceResultModel>();

            #region from sap
            //if (model.PerformanceCategory == EnumDealerPerformanceCategory.TopPerformer || model.PerformanceCategory == EnumDealerPerformanceCategory.BottomPerformer)
            //{
            //var dataLyGroup = dataLyYtd.GroupBy(x => x.CustomerNoOrSoldToParty).Select(x =>
            //                            new CategoryWiseDealerPerformanceResultModel()
            //                            {
            //                                CustomerNo = x.Key,
            //                                CustomerName = x.FirstOrDefault()?.CustomerName ?? string.Empty,
            //                                LYYTD = x.Sum(s => CustomConvertExtension.ObjectToDecimal(s.NetAmount))
            //                            });

            //var performerData = model.PerformanceCategory == EnumDealerPerformanceCategory.TopPerformer
            //                        ? dataLyGroup.OrderByDescending(o => o.LYYTD).Take(customerCount)
            //                        : dataLyGroup.OrderBy(o => o.LYYTD).Take(customerCount);

            //    var ranking = 1;

            //    foreach (var item in performerData)
            //    {
            //        var res = new CategoryWiseDealerPerformanceResultModel();
            //        res.Ranking = ranking++;
            //        res.CustomerNo = item.CustomerNo;
            //        res.CustomerName = item.CustomerName;
            //        res.CYMTD = dataCyMtd.Where(f => f.CustomerNo == item.CustomerNo).Sum(x => CustomConvertExtension.ObjectToDecimal(x.NetAmount));
            //        res.LYMTD = dataLyMtd.Where(f => f.CustomerNo == item.CustomerNo).Sum(x => CustomConvertExtension.ObjectToDecimal(x.NetAmount));
            //        res.CYYTD = dataCyYtd.Where(f => f.CustomerNo == item.CustomerNo).Sum(x => CustomConvertExtension.ObjectToDecimal(x.NetAmount));
            //        res.LYYTD = item.LYYTD;
            //        res.GrowthMTD = _odataService.GetGrowth(res.LYMTD, res.CYMTD);
            //        res.GrowthYTD = _odataService.GetGrowth(res.LYYTD, res.CYYTD);

            //        result.Add(res);
            //    }
            //}
            //else if (model.PerformanceCategory == EnumDealerPerformanceCategory.NotPurchasedLastMonth)
            //{
            //    // without sales last month
            //    var notPurchasedCyData = dataCyYtd.Where(x => !(x.Date.SalesResultDateFormat().Date >= notPurchasedFromDate.Date
            //                                                    && x.Date.SalesResultDateFormat().Date <= notPurchasedToDate.Date));

            //    // not sales only last month
            //    var notPurchasedCyGroupData = dataCyYtd.Where(x => !notPurchasedCyData.Any(y => y.CustomerNoOrSoldToParty == x.CustomerNoOrSoldToParty))
            //                                    .GroupBy(x => x.CustomerNoOrSoldToParty).Select(x =>
            //                                    new CategoryWiseDealerPerformanceResultModel()
            //                                    {
            //                                        CustomerNo = x.Key,
            //                                        CustomerName = x.FirstOrDefault()?.CustomerName ?? string.Empty,
            //                                        CYYTD = x.Sum(s => CustomConvertExtension.ObjectToDecimal(s.NetAmount))
            //                                    })
            //                                    .OrderByDescending(x => x.CYYTD);

            //    var ranking = 1;

            //    foreach (var item in notPurchasedCyGroupData)
            //    {
            //        var res = new CategoryWiseDealerPerformanceResultModel();
            //        res.Ranking = ranking++;
            //        res.CustomerNo = item.CustomerNo;
            //        res.CustomerName = item.CustomerName;
            //        res.LYMTD = dataLyMtd.Where(f => f.CustomerNo == item.CustomerNo).Sum(x => CustomConvertExtension.ObjectToDecimal(x.NetAmount));
            //        res.CYMTD = dataCyMtd.Where(f => f.CustomerNo == item.CustomerNo).Sum(x => CustomConvertExtension.ObjectToDecimal(x.NetAmount));
            //        res.LYYTD = dataLyYtd.Where(f => f.CustomerNo == item.CustomerNo).Sum(x => CustomConvertExtension.ObjectToDecimal(x.NetAmount));
            //        res.CYYTD = item.CYYTD;
            //        res.GrowthMTD = _odataService.GetGrowth(res.LYMTD, res.CYMTD);
            //        res.GrowthYTD = _odataService.GetGrowth(res.LYYTD, res.CYYTD);

            //        result.Add(res);
            //    }
            //}
            #endregion

            var ranking = 1;

            foreach (var item in dataActual)
            {
                var res = new CategoryWiseDealerPerformanceResultModel();
                res.Ranking = ranking++;
                res.CustomerNo = item.CustomerNo;
                res.CustomerName = item.CustomerName;
                res.LYMTD = dataActual.Where(f => f.CustomerNo == item.CustomerNo).Sum(x => x.LYMTD);
                res.CYMTD = dataActual.Where(f => f.CustomerNo == item.CustomerNo).Sum(x => x.CYMTD);
                res.LYYTD = dataActual.Where(f => f.CustomerNo == item.CustomerNo).Sum(x => x.LYYTD);
                res.CYYTD = dataActual.Where(f => f.CustomerNo == item.CustomerNo).Sum(x => x.CYYTD);
                res.GrowthMTD = _odataService.GetGrowth(res.LYMTD, res.CYMTD);
                res.GrowthYTD = _odataService.GetGrowth(res.LYYTD, res.CYYTD);

                result.Add(res);
            }

            return result;
        }

        //public async Task<IList<ReportDealerPerformanceResultModel>> GetReportDealerPerformance(IList<string> dealerIds, DealerPerformanceReportType dealerPerformanceReportType)
        //{
        //    var currentDate = DateTime.Now.AddMonths(-1);
        //    var mtsBrandCodes = new List<string>();

        //    var cyfd = currentDate.GetCYFD().DateFormat();
        //    var cylcd = currentDate.GetCYLCD().DateFormat();
        //    var cyld = currentDate.GetCYLD().DateFormat();

        //    var lyfd = currentDate.GetLYFD().DateFormat();
        //    var lylcd = currentDate.GetLYLCD().DateFormat();
        //    var lyld = currentDate.GetLYLD().DateFormat();

        //    var lfyfd = currentDate.GetLFYFD().DateFormat();
        //    var lfylcd = currentDate.GetLFYLCD().DateFormat();
        //    var lfyld = currentDate.GetLFYLD().DateFormat();

        //    var cfyfd = currentDate.GetCFYFD().DateFormat();
        //    var cfylcd = currentDate.GetCFYLCD().DateFormat();
        //    var cfyld = currentDate.GetCFYLD().DateFormat();

        //    var dataLyMtd = new List<SalesDataModel>();
        //    var dataCyMtd = new List<SalesDataModel>();
        //    var dataLyYtd = new List<SalesDataModel>();
        //    var dataCyYtd = new List<SalesDataModel>();

        //    var selectQueryBuilder = new SelectQueryOptionBuilder();
        //    selectQueryBuilder
        //        .AddProperty(DataColumnDef.NetAmount)
        //        .AddProperty(DataColumnDef.Territory);

        //    if (dealerPerformanceReportType == DealerPerformanceReportType.ClubSupremeTerritoryAndDealerWise)
        //    {
        //        selectQueryBuilder
        //            .AddProperty(DataColumnDef.CustomerNo)
        //            .AddProperty(DataColumnDef.CustomerName);
        //    }

        //    string division = "-1";

        //    dataLyMtd = (await _odataService.GetSalesDataByMultipleCustomerAndDivision(selectQueryBuilder, dealerIds, lyfd, lylcd, division, brands: mtsBrandCodes)).ToList();

        //    dataCyMtd = (await _odataService.GetSalesDataByMultipleCustomerAndDivision(selectQueryBuilder, dealerIds, cyfd, cylcd, division, brands: mtsBrandCodes)).ToList();

        //    dataLyYtd = (await _odataService.GetSalesDataByMultipleCustomerAndDivision(selectQueryBuilder, dealerIds, lfyfd, lfylcd, division, brands: mtsBrandCodes)).ToList();

        //    dataCyYtd = (await _odataService.GetSalesDataByMultipleCustomerAndDivision(selectQueryBuilder, dealerIds, cfyfd, cfylcd, division, brands: mtsBrandCodes)).ToList();

        //    Func<SalesDataModel, SalesDataModel> selectFunc = x => new SalesDataModel
        //    {
        //        NetAmount = x.NetAmount,
        //        Territory = x.Territory,
        //        CustomerNo = x.CustomerNo,
        //        CustomerName = x.CustomerName
        //    };

        //    Func<SalesDataModel, decimal> calcFunc = x => CustomConvertExtension.ObjectToDecimal(x.NetAmount);
        //    Func<SalesDataModel, SalesDataModel, bool> predicateFunc = (x, val) => x.Territory == val.Territory && x.CustomerName == val.CustomerName && x.CustomerNo == val.CustomerNo;

        //    var concatAllList = dataLyMtd.Select(selectFunc)
        //        .Concat(dataCyMtd.Select(selectFunc))
        //        .Concat(dataLyYtd.Select(selectFunc))
        //        .Concat(dataCyYtd.Select(selectFunc))
        //        .GroupBy(p => new { p.Territory, p.CustomerName, p.CustomerNo })
        //        .Select(g => g.First());

        //    var result = new List<ReportDealerPerformanceResultModel>();


        //    foreach (var item in concatAllList)
        //    {
        //        var res = new ReportDealerPerformanceResultModel();

        //        if (dataLyMtd.Any(x => predicateFunc(x, item)))
        //        {
        //            var amtLyMtd = dataLyMtd.Where(x => predicateFunc(x, item)).Sum(calcFunc);
        //            res.LYMTD = amtLyMtd;
        //        }

        //        if (dataCyMtd.Any(x => predicateFunc(x, item)))
        //        {
        //            var amtCyMtd = dataCyMtd.Where(x => predicateFunc(x, item)).Sum(calcFunc);
        //            res.CYMTD = amtCyMtd;
        //        }

        //        if (dataLyYtd.Any(x => predicateFunc(x, item)))
        //        {
        //            var amtLyYtd = dataLyYtd.Where(x => predicateFunc(x, item)).Sum(calcFunc);
        //            res.LYYTD = amtLyYtd;
        //        }

        //        if (dataCyYtd.Any(x => predicateFunc(x, item)))
        //        {
        //            var amtCyYtd = dataCyYtd.Where(x => predicateFunc(x, item)).Sum(calcFunc);
        //            res.CYYTD = amtCyYtd;
        //        }

        //        res.Territory = item.Territory;
        //        res.DealerId = item.CustomerNo;
        //        res.DealerName = item.CustomerName;
        //        res.GrowthMTD = _odataService.GetGrowth(res.LYMTD, res.CYMTD);
        //        res.GrowthYTD = _odataService.GetGrowth(res.LYYTD, res.CYYTD);

        //        result.Add(res);
        //    }

        //    return result;
        //}


        public async Task<IList<CategoryWisePerformanceReport>> GetCategoryWisePerformanceReports(Expression<Func<CategoryWisePerformanceReport,
            CategoryWisePerformanceReport>> selectProperty, string startDate, string endDate, IList<string> depots = null, IList<string> territories = null, IList<string> zones = null, IList<string> customerNos = null)
        {
            depots ??= new List<string>();
            territories ??= new List<string>();
            zones ??= new List<string>();
            customerNos ??= new List<string>();

            DateTime stDate = DateTime.ParseExact(startDate, "yyyy.MM.dd", null);
            DateTime edDate = DateTime.ParseExact(endDate, "yyyy.MM.dd", null);
            List<string> yearMonthString = new List<string>();
            while (stDate < edDate)
            {
                yearMonthString.Add(stDate.Year + "-" + stDate.Month);
                stDate = stDate.AddMonths(1);
            }

            return await _categoryWisePerformanceReportSapRepository.GetAllIncludeAsync(selectProperty, x =>
                yearMonthString.Contains((x.Year.ToString() + "-" + x.Month.ToString())) &&
                (!depots.Any() || depots.Contains(x.Depot)) &&
                (!customerNos.Any() || customerNos.Contains(x.CustomerNo)) &&
                (!territories.Any() || territories.Contains(x.Territory)) &&
                (!zones.Any() || zones.Contains(x.Zone)), null, null, true);
        }
        public async Task<IList<RptLastYearAppointDlerPerformanceSummaryResultModel>> GetReportLastYearAppointedDealerPerformanceSummary(LastYearAppointedDealerPerformanceSearchModel model,
            List<string> lastYearAppointedDealer)
        {
            var currentDate = new DateTime(model.Year, model.Month, 1);

            var cyfd = currentDate.GetCYFD().DateFormat();//.SalesSearchDateFormat();
            var cyld = currentDate.GetCYLD().DateFormat();//.SalesSearchDateFormat();

            var lyfd = currentDate.GetLYFD().DateFormat();//.SalesSearchDateFormat();
            var lyld = currentDate.GetLYLD().DateFormat();//.SalesSearchDateFormat();

            var lfyfd = currentDate.GetLFYFD().DateFormat();//.SalesSearchDateFormat();
            var cfyfd = currentDate.GetCFYFD().DateFormat();//.SalesSearchDateFormat();



            var dealerSelect = new SelectQueryOptionBuilder()
                .AddProperty(nameof(CustomerDataModel.CustomerNo))
                .AddProperty(nameof(CustomerDataModel.CreditControlArea))
                .AddProperty(nameof(CustomerDataModel.BusinessArea));

            var selectQueryBuilder = new SelectQueryOptionBuilder();
            selectQueryBuilder
                .AddProperty(DataColumnDef.NetAmount)
                .AddProperty(DataColumnDef.PlantOrBusinessArea)
                .AddProperty(DataColumnDef.CustomerNo);


            //  var dataCyMtd = (await _odataService.GetSalesData(selectQueryBuilder, cyfd, cyld, depots: model.Depots, territories: model.Territories, zones: model.Zones)).ToList();
            //  var dataLyMtd = (await _odataService.GetSalesData(selectQueryBuilder, lyfd, lyld, depots: model.Depots, territories: model.Territories, zones: model.Zones)).ToList();


            var dataCyMtd = (await GetCategoryWisePerformanceReports(x => new CategoryWisePerformanceReport()
            {
                Value = x.Value,
                Depot = x.Depot,
                CustomerNo = x.CustomerNo
            }, cyfd, cyld, depots: model.Depots, territories: model.Territories, zones: model.Zones, lastYearAppointedDealer)).ToList();
            var dataLyMtd = (await GetCategoryWisePerformanceReports(x => new CategoryWisePerformanceReport()
            {
                Value = x.Value,
                Depot = x.Depot,
                CustomerNo = x.CustomerNo
            }, lyfd, lyld, depots: model.Depots, territories: model.Territories, zones: model.Zones, lastYearAppointedDealer)).ToList();

            var dataLyYtd = (await GetCategoryWisePerformanceReports(x => new CategoryWisePerformanceReport()
            {
                Value = x.Value,
                Depot = x.Depot,
                CustomerNo = x.CustomerNo
            }, lfyfd, lyld, depots: model.Depots, territories: model.Territories, zones: model.Zones, lastYearAppointedDealer)).ToList();
            var dataCyYtd = (await GetCategoryWisePerformanceReports(x => new CategoryWisePerformanceReport()
            {
                Value = x.Value,
                Depot = x.Depot,
                CustomerNo = x.CustomerNo
            }, cfyfd, cyld, depots: model.Depots, territories: model.Territories, zones: model.Zones, lastYearAppointedDealer)).ToList();

            //var dataLyYtd = (await _odataService.GetSalesData(selectQueryBuilder, lfyfd, lyld, depots: model.Depots, territories: model.Territories, zones: model.Zones)).ToList();
            //var dataCyYtd = (await _odataService.GetSalesData(selectQueryBuilder, cfyfd, cyld, depots: model.Depots, territories: model.Territories, zones: model.Zones)).ToList();


            //var dataCyMtdTask = (_odataService.GetSalesData(selectQueryBuilder, cyfd, cyld, depots: model.Depots, territories: model.Territories, zones: model.Zones));
            //var dataLyMtdTask = (_odataService.GetSalesData(selectQueryBuilder, lyfd, lyld, depots: model.Depots, territories: model.Territories, zones: model.Zones));

            //var dataLyYtdTask = (_odataService.GetSalesData(selectQueryBuilder, lfyfd, lyld, depots: model.Depots, territories: model.Territories, zones: model.Zones));
            //var dataCyYtdTask = (_odataService.GetSalesData(selectQueryBuilder, cfyfd, cyld, depots: model.Depots, territories: model.Territories, zones: model.Zones));


            //await TaskExt.WhenAll(dataCyMtdTask, dataLyMtdTask, dataLyYtdTask, dataCyYtdTask);

            //var dataCyMtd = await dataCyMtdTask;
            //var dataLyMtd = await dataLyMtdTask;

            //var dataLyYtd = await dataLyYtdTask;
            //var dataCyYtd = await dataCyYtdTask;

            Func<CategoryWisePerformanceReport, CategoryWisePerformanceReport> concatSelectFunc = x => new CategoryWisePerformanceReport
            {
                Value = x.Value,
                Depot = x.Depot,
                CustomerNo = x.CustomerNo
            };

            Func<CategoryWisePerformanceReport, CategoryWisePerformanceReport> selectFunc = x => new CategoryWisePerformanceReport
            {
                Value = x.Value,
                Depot = x.Depot
            };

            Func<CategoryWisePerformanceReport, decimal> calcFunc = x => CustomConvertExtension.ObjectToDecimal(x.Value);
            Func<CategoryWisePerformanceReport, CategoryWisePerformanceReport, bool> predicateFunc = (x, val) => x.Depot == val.Depot;

            var concatAllList = dataLyMtd.Select(concatSelectFunc)
                .Concat(dataCyMtd.Select(concatSelectFunc))
                .Concat(dataLyYtd.Select(concatSelectFunc))
                .Concat(dataCyYtd.Select(concatSelectFunc))
                .GroupBy(p => new { p.Depot, p.CustomerNo })
                .Select(g => g.First());


            concatAllList = concatAllList.Where(x => lastYearAppointedDealer.Contains(x.CustomerNo)).ToList();

            concatAllList = concatAllList.Select(selectFunc).GroupBy(p => p.Depot).Select(g => g.First()).ToList();

            var result = new List<RptLastYearAppointDlerPerformanceSummaryResultModel>();

            model.Zones = model.Zones ??= new List<string>();
            model.Depots = model.Depots ??= new List<string>();
            model.Territories = model.Territories ??= new List<string>();

            var dealer = await _dealarInfoRepository.FindByCondition(x =>

                (!model.Zones.Any() || model.Zones.Contains(x.CustZone)) &&
                (!model.Territories.Any() || model.Territories.Contains(x.Territory)) &&
                (!model.Depots.Any() || model.Depots.Contains(x.BusinessArea)) &&
                x.Channel == ConstantsValue.DistrbutionChannelDealer
            ).Select(x => new
            {
                x.BusinessArea,
                x.CustomerNo
            }).Distinct().ToListAsync();


            //var dealer = await _odataService.GetCustomerData(dealerSelect, depots: model.Depots, territories: model.Territories,
            //    zones: model.Zones, channel: ConstantsValue.DistrbutionChannelDealer);


            foreach (var item in concatAllList)
            {
                var res = new RptLastYearAppointDlerPerformanceSummaryResultModel();

                if (dataLyMtd.Any(x => predicateFunc(x, item)))
                {
                    var amtLyMtd = dataLyMtd.Where(x => predicateFunc(x, item)).Sum(calcFunc);
                    res.LYMTD = amtLyMtd;
                }

                if (dataCyMtd.Any(x => predicateFunc(x, item)))
                {
                    var amtCyMtd = dataCyMtd.Where(x => predicateFunc(x, item)).Sum(calcFunc);
                    res.CYMTD = amtCyMtd;
                }

                if (dataLyYtd.Any(x => predicateFunc(x, item)))
                {
                    var amtLyYtd = dataLyYtd.Where(x => predicateFunc(x, item)).Sum(calcFunc);
                    res.LYYTD = amtLyYtd;
                }

                if (dataCyYtd.Any(x => predicateFunc(x, item)))
                {
                    var amtCyYtd = dataCyYtd.Where(x => predicateFunc(x, item)).Sum(calcFunc);
                    res.CYYTD = amtCyYtd;
                }

                res.DepotCode = item.Depot;
                res.GrowthMTD = _odataService.GetGrowth(res.LYMTD, res.CYMTD);
                res.GrowthYTD = _odataService.GetGrowth(res.LYYTD, res.CYYTD);
                //res.NumberOfDealer = dealer.Count(x => x.BusinessArea == item.PlantOrBusinessArea);
                res.NumberOfDealer = dealer.Where(x => x.BusinessArea == item.Depot).Select(x => x.CustomerNo).Distinct().Count();
                result.Add(res);
            }

            return result;
        }

        public async Task<IList<RptLastYearAppointDlrPerformanceDetailResultModel>> GetReportLastYearAppointedDealerPerformanceDetail(LastYearAppointedDealerPerformanceSearchModel model,
            List<string> lastYearAppointedDealer)
        {
            var currentDate = new DateTime(model.Year, model.Month, 1);

            var cyfd = currentDate.GetCYFD().DateFormat();//.SalesSearchDateFormat();
            var cyld = currentDate.GetCYLD().DateFormat();//.SalesSearchDateFormat();

            var lyfd = currentDate.GetLYFD().DateFormat();//.SalesSearchDateFormat();
            var lyld = currentDate.GetLYLD().DateFormat();//.SalesSearchDateFormat();

            var lfyfd = currentDate.GetLFYFD().DateFormat();//.SalesSearchDateFormat();

            var cfyfd = currentDate.GetCFYFD().DateFormat();//.SalesSearchDateFormat();

            var selectQueryBuilder = new SelectQueryOptionBuilder();
            selectQueryBuilder
                .AddProperty(DataColumnDef.PlantOrBusinessArea)
                .AddProperty(DataColumnDef.Territory)
                .AddProperty(DataColumnDef.Zone)
                .AddProperty(DataColumnDef.CustomerNo)
                .AddProperty(DataColumnDef.NetAmount)
                .AddProperty(DataColumnDef.CustomerName);



            var dataCyMtd = (await GetCategoryWisePerformanceReports(x => new CategoryWisePerformanceReport()
            {
                Depot = x.Depot,
                Territory = x.Territory,
                Zone = x.Zone,
                Value = x.Value,
                CustomerNo = x.CustomerNo,
                CustomerName = x.CustomerName
            }, cyfd, cyld, depots: model.Depots, territories: model.Territories, zones: model.Zones, lastYearAppointedDealer)).ToList();
            var dataLyMtd = (await GetCategoryWisePerformanceReports(x => new CategoryWisePerformanceReport()
            {
                Depot = x.Depot,
                Territory = x.Territory,
                Zone = x.Zone,
                Value = x.Value,
                CustomerNo = x.CustomerNo,
                CustomerName = x.CustomerName
            }, lyfd, lyld, depots: model.Depots, territories: model.Territories, zones: model.Zones, lastYearAppointedDealer)).ToList();

            var dataLyYtd = (await GetCategoryWisePerformanceReports(x => new CategoryWisePerformanceReport()
            {
                Depot = x.Depot,
                Territory = x.Territory,
                Zone = x.Zone,
                Value = x.Value,
                CustomerNo = x.CustomerNo,
                CustomerName = x.CustomerName
            }, lfyfd, lyld, depots: model.Depots, territories: model.Territories, zones: model.Zones, lastYearAppointedDealer)).ToList();
            var dataCyYtd = (await GetCategoryWisePerformanceReports(x => new CategoryWisePerformanceReport()
            {
                Depot = x.Depot,
                Territory = x.Territory,
                Zone = x.Zone,
                Value = x.Value,
                CustomerNo = x.CustomerNo,
                CustomerName = x.CustomerName
            }, cfyfd, cyld, depots: model.Depots, territories: model.Territories, zones: model.Zones, lastYearAppointedDealer)).ToList();






            //var dataCyMtd = (await _odataService.GetSalesData(selectQueryBuilder, cyfd, cyld, depots: model.Depots, territories: model.Territories, zones: model.Zones)).ToList();
            //var dataLyMtd = (await _odataService.GetSalesData(selectQueryBuilder, lyfd, lyld, depots: model.Depots, territories: model.Territories, zones: model.Zones)).ToList();

            //var dataLyYtd = (await _odataService.GetSalesData(selectQueryBuilder, lfyfd, lyld, depots: model.Depots, territories: model.Territories, zones: model.Zones)).ToList();
            //var dataCyYtd = (await _odataService.GetSalesData(selectQueryBuilder, cfyfd, cyld, depots: model.Depots, territories: model.Territories, zones: model.Zones)).ToList();


            Func<CategoryWisePerformanceReport, CategoryWisePerformanceReport> selectFunc = x => new CategoryWisePerformanceReport
            {
                Value = x.Value,
                Depot = x.Depot,
                Territory = x.Territory,
                Zone = x.Zone,
                CustomerName = x.CustomerName,
                CustomerNo = x.CustomerNo
            };

            var concatAllList = dataLyMtd.Select(selectFunc)
                .Concat(dataCyMtd.Select(selectFunc))
                .GroupBy(p => new { p.Depot, p.Territory, p.Zone, p.CustomerName, p.CustomerNo })
                .Select(g => g.First());

            concatAllList = concatAllList.Where(x => lastYearAppointedDealer.Contains(x.CustomerNo)).ToList();

            var result = new List<RptLastYearAppointDlrPerformanceDetailResultModel>();
            Func<CategoryWisePerformanceReport, CategoryWisePerformanceReport, bool> predicateFunc = (x, val) => x.Depot == val.Depot && x.Territory == val.Territory
                && x.CustomerNo == val.CustomerNo && x.Zone == val.Zone;
            Func<CategoryWisePerformanceReport, decimal> calcFunc = x => CustomConvertExtension.ObjectToDecimal(x.Value);

            foreach (var item in concatAllList)
            {

                var res = new RptLastYearAppointDlrPerformanceDetailResultModel
                {
                    DepotCode = item.Depot,
                    Territory = item.Territory,
                    Zone = item.Zone,
                    CustomerNo = item.CustomerNo,
                    CustomerName = item.CustomerName,
                };

                if (dataLyMtd.Any(x => predicateFunc(x, item)))
                {
                    var amtLyMtd = dataLyMtd.Where(x => predicateFunc(x, item)).Sum(calcFunc);
                    res.LYMTD = amtLyMtd;
                }

                if (dataCyMtd.Any(x => predicateFunc(x, item)))
                {
                    var amtCyMtd = dataCyMtd.Where(x => predicateFunc(x, item)).Sum(calcFunc);
                    res.CYMTD = amtCyMtd;
                }

                if (dataLyYtd.Any(x => predicateFunc(x, item)))
                {
                    var amtLyYtd = dataLyYtd.Where(x => predicateFunc(x, item)).Sum(calcFunc);
                    res.LYYTD = amtLyYtd;
                }

                if (dataCyYtd.Any(x => predicateFunc(x, item)))
                {
                    var amtCyYtd = dataCyYtd.Where(x => predicateFunc(x, item)).Sum(calcFunc);
                    res.CYYTD = amtCyYtd;
                }

                res.GrowthMTD = _odataService.GetGrowth(res.LYMTD, res.CYMTD);
                res.GrowthYTD = _odataService.GetGrowth(res.LYYTD, res.CYYTD);
            }

            return result;
        }
        public async Task<IList<ReportClubSupremePerformance>> GetReportClubSupremePerformance(ClubSupremePerformanceSearchModel model,
            List<CustNClubMappingVm> clubSupremeDealers, ClubSupremeReportType reportType)
        {
            var currentDate = new DateTime(model.Year, model.Month, 1);

            var cyfd = currentDate.GetCYFD().DateFormat();//.SalesSearchDateFormat();
            var cyld = currentDate.GetCYLD().DateFormat();//.SalesSearchDateFormat();

            var lyfd = currentDate.GetLYFD().DateFormat();//.SalesSearchDateFormat();
            var lyld = currentDate.GetLYLD().DateFormat();//.SalesSearchDateFormat();

            var lfyfd = currentDate.GetLFYFD().DateFormat();//.SalesSearchDateFormat();
            var cfyfd = currentDate.GetCFYFD().DateFormat();//.SalesSearchDateFormat();

            var selectQueryBuilder = new SelectQueryOptionBuilder();

            Func<CategoryWisePerformanceReport, CategoryWisePerformanceReport> selectFunc;

            var dealerSelect = new SelectQueryOptionBuilder()
                .AddProperty(nameof(CustomerDataModel.CustomerNo));




            IList<CustomerDataModel> dealer = new List<CustomerDataModel>();

            if (reportType == ClubSupremeReportType.Summary)
            {
                selectQueryBuilder
                    .AddProperty(DataColumnDef.CustomerNo)
                    .AddProperty(DataColumnDef.NetAmount);

                selectFunc = x => new CategoryWisePerformanceReport
                {
                    Value = x.Value,
                    CustomerName = x.CustomerName,
                    CustomerNo = x.CustomerNo
                };

                model.Zones = model.Zones ??= new List<string>();
                model.Depots = model.Depots ??= new List<string>();
                model.Territories = model.Territories ??= new List<string>();

               // dealer = await _dealarInfoRepository.FindByCondition(x =>
               //     (!model.Zones.Any() || model.Zones.Contains(x.CustZone)) &&
               //    (!model.Territories.Any() || model.Territories.Contains(x.Territory)) &&
               //    (!model.Depots.Any() || model.Depots.Contains(x.BusinessArea)) &&
               //    x.Channel == ConstantsValue.DistrbutionChannelDealer
               //).Select(x => new CustomerDataModel()
               //{
               //    BusinessArea = x.BusinessArea,
               //    CustomerNo = x.CustomerNo
               //}).Distinct().ToListAsync();

            }
            else
            {
                selectQueryBuilder
                    .AddProperty(DataColumnDef.PlantOrBusinessArea)
                    .AddProperty(DataColumnDef.Territory)
                    .AddProperty(DataColumnDef.Zone)
                    .AddProperty(DataColumnDef.CustomerNo)
                    .AddProperty(DataColumnDef.NetAmount)
                    .AddProperty(DataColumnDef.CustomerName);

                selectFunc = x => new CategoryWisePerformanceReport
                {
                    Value = x.Value,
                    Depot = x.Depot,
                    Territory = x.Territory,
                    Zone = x.Zone,
                    CustomerName = x.CustomerName,
                    CustomerNo = x.CustomerNo
                };

            }


            var dataCyMtd = (await GetCategoryWisePerformanceReports(x => new CategoryWisePerformanceReport()
            {
                Depot = x.Depot,
                Territory = x.Territory,
                Zone = x.Zone,
                Value = x.Value,
                CustomerNo = x.CustomerNo,
                CustomerName = x.CustomerName
            }, cyfd, cyld, depots: model.Depots, territories: model.Territories, zones: model.Zones)).ToList();
            
            var dataLyMtd = (await GetCategoryWisePerformanceReports(x => new CategoryWisePerformanceReport()
            {
                Depot = x.Depot,
                Territory = x.Territory,
                Zone = x.Zone,
                Value = x.Value,
                CustomerNo = x.CustomerNo,
                CustomerName = x.CustomerName
            }, lyfd, lyld, depots: model.Depots, territories: model.Territories, zones: model.Zones)).ToList();
            
            var dataLyYtd = (await GetCategoryWisePerformanceReports(x => new CategoryWisePerformanceReport()
            {
                Depot = x.Depot,
                Territory = x.Territory,
                Zone = x.Zone,
                Value = x.Value,
                CustomerNo = x.CustomerNo,
                CustomerName = x.CustomerName
            }, lfyfd, lyld, depots: model.Depots, territories: model.Territories, zones: model.Zones)).ToList();
            
            var dataCyYtd = (await GetCategoryWisePerformanceReports(x => new CategoryWisePerformanceReport()
            {
                Depot = x.Depot,
                Territory = x.Territory,
                Zone = x.Zone,
                Value = x.Value,
                CustomerNo = x.CustomerNo,
                CustomerName = x.CustomerName
            }, cfyfd, cyld, depots: model.Depots, territories: model.Territories, zones: model.Zones)).ToList();





            //var dataCyMtd = (await _odataService.GetSalesData(selectQueryBuilder, cyfd, cyld, depots: model.Depots, territories: model.Territories, zones: model.Zones)).ToList();
            //var dataLyMtd = (await _odataService.GetSalesData(selectQueryBuilder, lyfd, lyld, depots: model.Depots, territories: model.Territories, zones: model.Zones)).ToList();
            //var dataLyYtd = (await _odataService.GetSalesData(selectQueryBuilder, lfyfd, lyld, depots: model.Depots, territories: model.Territories, zones: model.Zones)).ToList();
            //var dataCyYtd = (await _odataService.GetSalesData(selectQueryBuilder, cfyfd, cyld, depots: model.Depots, territories: model.Territories, zones: model.Zones)).ToList();


            //var concatAllList = dataLyMtd.Select(selectFunc)
            //    .Concat(dataCyMtd.Select(selectFunc))
            //    .Concat(dataCyYtd.Select(selectFunc))
            //    .Concat(dataLyYtd.Select(selectFunc))
            //    .GroupBy(p => new { p.Depot, p.Territory, p.Zone, p.CustomerName, p.CustomerNo })
            //    .Select(g => g.First());

            //concatAllList = concatAllList.Where(x => clubSupremeDealers.Select(y => y.CustomerNo).Contains(x.CustomerNo)).ToList();

            var result = new List<ReportClubSupremePerformance>();
            

            Func<CategoryWisePerformanceReport, CustNClubMappingVm, bool> predicateFunc = (x, val) => x.Depot == val.DepotCode && x.Territory == val.Territory
                && x.CustomerNo == val.CustomerNo && x.Zone == val.Zone;

            Func<CategoryWisePerformanceReport, decimal> calcFunc = x => CustomConvertExtension.ObjectToDecimal(x.Value);
            

            foreach (var item in clubSupremeDealers)
            {
                var res = reportType == ClubSupremeReportType.Detail
                    ? (ReportClubSupremePerformance)new ReportClubSupremePerformanceDetail
                    {
                        DepotCode = item.DepotCode,
                        Territory = item.Territory,
                        Zone = item.Zone,
                        CustomerNo = item.CustomerNo,
                        CustomerName = item.CustomerName
                    }
                    : new ReportClubSupremePerformanceSummary()
                    {
                        NumberOfDealer = 1
                    };

                res.ClubStatus = EnumExtension.GetEnumDescription(clubSupremeDealers.FirstOrDefault(x => x.CustomerNo == item.CustomerNo)?.ClubSupreme ?? 0);

                if (dataLyMtd.Any(x => predicateFunc(x, item)))
                {
                    var amtLyMtd = dataLyMtd.Where(x => predicateFunc(x, item)).Sum(calcFunc);
                    res.LYMTD = amtLyMtd;
                }

                if (dataCyMtd.Any(x => predicateFunc(x, item)))
                {
                    var amtCyMtd = dataCyMtd.Where(x => predicateFunc(x, item)).Sum(calcFunc);
                    res.CYMTD = amtCyMtd;
                }

                if (dataLyYtd.Any(x => predicateFunc(x, item)))
                {
                    var amtLyYtd = dataLyYtd.Where(x => predicateFunc(x, item)).Sum(calcFunc);
                    res.LYYTD = amtLyYtd;
                }

                if (dataCyYtd.Any(x => predicateFunc(x, item)))
                {
                    var amtCyYtd = dataCyYtd.Where(x => predicateFunc(x, item)).Sum(calcFunc);
                    res.CYYTD = amtCyYtd;
                }

                if (reportType == ClubSupremeReportType.Detail)
                {
                    res.GrowthMTD = _odataService.GetGrowth(res.LYMTD, res.CYMTD);
                    res.GrowthYTD = _odataService.GetGrowth(res.LYYTD, res.CYYTD);
                }
                result.Add(res);
            }

            if (reportType == ClubSupremeReportType.Summary)
            {
                result = new List<ReportClubSupremePerformance>(result.Cast<ReportClubSupremePerformanceSummary>().GroupBy((x) => new
                {
                    x.ClubStatus
                }).Select(x => new ReportClubSupremePerformanceSummary
                {
                    CYMTD = x.Sum(y => y.CYMTD),
                    LYMTD = x.Sum(y => y.LYMTD),
                    LYYTD = x.Sum(y => y.LYYTD),
                    CYYTD = x.Sum(y => y.CYYTD),
                    ClubStatus = x.Key.ClubStatus,
                    NumberOfDealer = x.Sum(y => y.NumberOfDealer)
                }).ToList());

                result.ForEach(x =>
                {
                    x.GrowthMTD = _odataService.GetGrowth(x.LYMTD, x.CYMTD);
                    x.GrowthYTD = _odataService.GetGrowth(x.LYYTD, x.CYYTD);
                });
            }


            return result;
        }

        //public async Task<IList<KPIStrikRateKPIReportResultModel>> GetKPIStrikeRateKPIReport(int year, int month, string depot, List<string> salesGroups, List<string> territories, List<string> zones, List<string> brands)
        //{
        //    var currentDate = new DateTime(year, month, 01);
        //    var fromDate = currentDate.GetCYFD().DateFormat();
        //    var toDate = currentDate.GetCYLD().DateFormat();

        //    var selectQueryBuilder = new SelectQueryOptionBuilder();
        //    selectQueryBuilder.AddProperty(DataColumnDef.CustomerNoOrSoldToParty)
        //                        .AddProperty(DataColumnDef.InvoiceNoOrBillNo)
        //                        .AddProperty(DataColumnDef.Date)
        //                        .AddProperty(DataColumnDef.NetAmount)
        //                        .AddProperty(DataColumnDef.CustomerClassification)
        //                        .AddProperty(DataColumnDef.MatarialGroupOrBrand);

        //    var data = (await _odataService.GetSalesDataByMultipleArea(selectQueryBuilder, fromDate, toDate, depot, salesGroups: salesGroups, territories: territories, zones: zones, brands: brands)).ToList();

        //    var result = data.Select(x =>
        //                        new KPIStrikRateKPIReportResultModel()
        //                        {
        //                            CustomerNo = x.CustomerNoOrSoldToParty,
        //                            InvoiceNoOrBillNo = x.InvoiceNoOrBillNo,
        //                            DateTime = x.Date.DateFormatDate(),
        //                            Date = x.Date.ReturnDateFormatDate(),
        //                            NetAmount = CustomConvertExtension.ObjectToDecimal(x.NetAmount),
        //                            CustomerClassification = x.CustomerClassification,
        //                            MatarialGroupOrBrand = x.MatarialGroupOrBrand,
        //                        }).ToList();

        //    return result;
        //}

        //public async Task<IList<KPIBusinessAnalysisKPIReportResultModel>> GetKPIBusinessAnalysisKPIReport(int year, int month, string depot, List<string> salesGroups, List<string> territories)
        //{
        //    var filterDate = new DateTime(year, month, 01);
        //    var fromDate = filterDate.GetCYFD().SalesSearchDateFormat();
        //    var toDate = filterDate.GetCYLD().SalesSearchDateFormat();

        //    var selectQueryBuilder = new SelectQueryOptionBuilder();
        //    selectQueryBuilder.AddProperty(DataColumnDef.InvoiceNoOrBillNo)
        //                        .AddProperty(DataColumnDef.CustomerNoOrSoldToParty)
        //                        .AddProperty(DataColumnDef.Date)
        //                        .AddProperty(DataColumnDef.NetAmount);

        //    var data = (await _odataService.GetSalesData(selectQueryBuilder, fromDate, toDate,
        //                                    depots: new List<string> { depot }, salesGroups: salesGroups, territories: territories)).ToList();

        //    var result = data.Select(x =>
        //                        new KPIBusinessAnalysisKPIReportResultModel()
        //                        {
        //                            CustomerNo = x.CustomerNoOrSoldToParty,
        //                        }).ToList();

        //    return result;
        //}

        public async Task<int> NoOfBillingDealer(AreaSearchCommonModel area, string division = "", string channel = "")
        {
            var currentDate = DateTime.Now;
            //var fromDate = currentDate.SalesSearchDateFormat();
            //var toDate = currentDate.SalesSearchDateFormat();

            //var selectQueryBuilder = new SelectQueryOptionBuilder();
            //selectQueryBuilder.AddProperty(DataColumnDef.CustomerNoOrSoldToParty)
            //                .AddProperty(DataColumnDef.NetAmount);

            //var data = await _odataService.GetSalesData(selectQueryBuilder, fromDate, toDate,
            //                                        depots: area.Depots, salesOffices: area.SalesOffices, salesGroups: area.SalesGroups,
            //                                        territories: area.Territories, zones: area.Zones,
            //                                        division: division, channel: channel);

            var data = await _customerInvoiceReportRepository.GetAllIncludeAsync(x => x.CustomerNo,
                                x => x.Date.Date == currentDate.Date
                                    && (!area.Depots.Any() || area.Depots.Contains(x.Depot))
                                    && (!area.SalesOffices.Any() || area.SalesOffices.Contains(x.SalesOffice))
                                    && (!area.SalesGroups.Any() || area.SalesGroups.Contains(x.SalesGroup))
                                    && (!area.Territories.Any() || area.Territories.Contains(x.Territory))
                                    && (!area.Zones.Any() || area.Zones.Contains(x.Zone))
                                    && x.Division == division
                                    && x.DistributionChannel == channel,
                                null, null, true);

            //var result = data.Select(x => x.CustomerNoOrSoldToParty).Distinct().Count();
            var result = data.Distinct().Count();

            return result;
        }

        public async Task<IList<TodaysInvoiceValueResultModel>> GetTodaysActivityInvoiceValue(TodaysInvoiceValueSearchModel model, AreaSearchCommonModel area)
        {
            var currentDate = DateTime.Now;
            //var fromDate = currentDate.SalesSearchDateFormat();
            //var toDate = currentDate.SalesSearchDateFormat();

            //var selectQueryBuilder = new SelectQueryOptionBuilder();
            //selectQueryBuilder.AddProperty(DataColumnDef.CustomerNoOrSoldToParty)
            //                    .AddProperty(DataColumnDef.CustomerName)
            //                    .AddProperty(DataColumnDef.InvoiceNoOrBillNo)
            //                    .AddProperty(DataColumnDef.NetAmount);

            //var data = await _odataService.GetSalesData(selectQueryBuilder, fromDate, toDate,
            //                                        depots: area.Depots, salesOffices: area.SalesOffices, salesGroups: area.SalesGroups,
            //                                        territories: area.Territories, zones: area.Zones,
            //                                        division: model.Division);

            var data = await _customerInvoiceReportRepository.GetAllIncludeAsync(x => 
                                new { x.InvoiceNoOrBillNo, x.CustomerNo, x.CustomerName, x.Value },
                                x => x.Date.Date == currentDate.Date
                                    && (!area.Depots.Any() || area.Depots.Contains(x.Depot))
                                    && (!area.SalesOffices.Any() || area.SalesOffices.Contains(x.SalesOffice))
                                    && (!area.SalesGroups.Any() || area.SalesGroups.Contains(x.SalesGroup))
                                    && (!area.Territories.Any() || area.Territories.Contains(x.Territory))
                                    && (!area.Zones.Any() || area.Zones.Contains(x.Zone))
                                    && x.Division == model.Division,
                                null, null, true);

            var groupData = data.GroupBy(x => x.InvoiceNoOrBillNo).ToList();

            var result = groupData.Select(x =>
                                new TodaysInvoiceValueResultModel()
                                {
                                    InvoiceNoOrBillNo = x.Key,
                                    CustomerNo = x.FirstOrDefault()?.CustomerNo ?? string.Empty,
                                    CustomerName = x.FirstOrDefault()?.CustomerName ?? string.Empty,
                                    NetAmount = x.Sum(s => s.Value)
                                }).ToList();

            return result;
        }

        //public async Task<IList<SalesDataModel>> GetMTDActual(AppAreaSearchCommonModel area, DateTime fromDate, DateTime toDate,
        //    string division, EnumVolumeOrValue volumeOrValue, EnumBrandCategory? category, EnumBrandType? type)
        //{
        //    var fromDateStr = fromDate.SalesSearchDateFormat();
        //    var toDateStr = toDate.SalesSearchDateFormat();

        //    var selectQueryBuilder = new SelectQueryOptionBuilder();
        //    selectQueryBuilder.AddProperty(DataColumnDef.PlantOrBusinessArea)
        //                        .AddProperty(DataColumnDef.Date)
        //                        .AddProperty(volumeOrValue == EnumVolumeOrValue.Volume
        //                                    ? DataColumnDef.Volume
        //                                    : DataColumnDef.NetAmount);

        //    if (type.HasValue) selectQueryBuilder.AddProperty(DataColumnDef.MatarialGroupOrBrand);

        //    var brands = new List<string>();

        //    if (category.HasValue && category.Value == EnumBrandCategory.Liquid)
        //    {
        //        brands = (await _odataBrandService.GetLiquidBrandCodesAsync()).ToList();
        //    }
        //    else if (category.HasValue && category.Value == EnumBrandCategory.Powder)
        //    {
        //        brands = (await _odataBrandService.GetPowderBrandCodesAsync()).ToList();
        //    }

        //    if (type.HasValue && type.Value == EnumBrandType.MTSBrands)
        //    {
        //        brands = (await _odataBrandService.GetMTSBrandCodesAsync()).ToList();
        //    }

        //    var result = await _odataService.GetSalesData(selectQueryBuilder, fromDateStr, toDateStr,
        //                    depots: area.Depots, territories: area.Territories, zones: area.Zones,
        //                    brands: brands, division: division);

        //    return result;
        //}

        public async Task<IList<CustomerDeliveryNoteResultModel>> GetCustomerDeliveryNote(CustomerDeliveryNoteSearchModel model)
        {
            var fromDateStr = model.DeliveryFromDate.DeliverySearchDateTimeFormat();
            var toDateStr = model.DeliveryToDate.DeliverySearchDateTimeFormat();

            var selectQueryBuilder = new SelectQueryOptionBuilder();
            selectQueryBuilder.AddProperty(CustomerDeliveryColDef.InvoiceDate)
                                .AddProperty(CustomerDeliveryColDef.InvoiceCreateTime)
                                .AddProperty(CustomerDeliveryColDef.InvoiceNumber)
                                .AddProperty(CustomerDeliveryColDef.Volume)
                                .AddProperty(CustomerDeliveryColDef.DeliveryDate)
                                .AddProperty(CustomerDeliveryColDef.DeliveryTime)
                                .AddProperty(CustomerDeliveryColDef.DriverName)
                                .AddProperty(CustomerDeliveryColDef.DriverMobileNo);

            var data = await _odataService.GetCustomerDeliveryData(selectQueryBuilder, model.CustomerNo, fromDateStr, toDateStr);

            var result = data.Select(x => new CustomerDeliveryNoteResultModel()
            {
                InvoiceDate = CustomConvertExtension.ObjectToDateTime(x.InvoiceDate).DateFormat("dd.MM.yyyy"),
                InvoiceCreateTime = x.InvoiceCreateTime
                                                                    .Replace("PT", "").Replace("H", ":").Replace("M", ":").Replace("S", ""),
                InvoiceNumber = x.InvoiceNumber,
                Volume = CustomConvertExtension.ObjectToDecimal(x.Volume),
                DeliveryDate = CustomConvertExtension.ObjectToDateTime(x.DeliveryDate).DateFormat("dd.MM.yyyy"),
                DeliveryTime = x.DeliveryTime
                                                                .Replace("PT", "").Replace("H", ":").Replace("M", ":").Replace("S", ""),
                DriverName = x.DriverName,
                DriverMobileNo = x.DriverMobileNo
            }).ToList();

            return result;
        }
    }
}

