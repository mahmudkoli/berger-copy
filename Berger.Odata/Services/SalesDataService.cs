using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
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
    public class SalesDataService : ISalesDataService
    {
        private readonly IODataService _odataService;
        private readonly IODataBrandService _odataBrandService;

        public SalesDataService(
            IODataService odataService,
            IODataBrandService odataBrandService
            )
        {
            _odataService = odataService;
            _odataBrandService = odataBrandService;
        }

        public async Task<IList<InvoiceHistoryResultModel>> GetInvoiceHistory(InvoiceHistorySearchModel model)
        {
            var currentDate = DateTime.Now;
            var fromDate = currentDate.AddMonths(-1).GetCYFD().DateFormat();
            var toDate = currentDate.AddMonths(-1).GetCYLD().DateFormat();

            var selectQueryBuilder = new SelectQueryOptionBuilder();
            selectQueryBuilder.AddProperty(DataColumnDef.CustomerNoOrSoldToParty)
                                .AddProperty(DataColumnDef.InvoiceNoOrBillNo)
                                .AddProperty(DataColumnDef.Date)
                                .AddProperty(DataColumnDef.NetAmount)
                                .AddProperty(DataColumnDef.Time);

            var data = (await _odataService.GetSalesDataByCustomerAndDivision(selectQueryBuilder, model.CustomerNo, fromDate, toDate, model.Division)).ToList();

            var result = data.Select(x =>
                                new InvoiceHistoryResultModel()
                                {
                                    InvoiceNoOrBillNo = x.InvoiceNoOrBillNo,
                                    Date = x.Date.ReturnDateFormatDate(),
                                    NetAmount = CustomConvertExtension.ObjectToDecimal(x.NetAmount),
                                    Time = x.Time.ReturnDateFormatTime()
                                }).ToList();

            return result;
        }

        public async Task<InvoiceDetailsResultModel> GetInvoiceDetails(InvoiceDetailsSearchModel model)
        {
            var filterQueryBuilder = new FilterQueryOptionBuilder();
            filterQueryBuilder.Equal(DataColumnDef.InvoiceNoOrBillNo, model.InvoiceNo);

            var selectQueryBuilder = new SelectQueryOptionBuilder();
            selectQueryBuilder.AddProperty(DataColumnDef.CustomerNoOrSoldToParty)
                                .AddProperty(DataColumnDef.CustomerName)
                                .AddProperty(DataColumnDef.Division)
                                .AddProperty(DataColumnDef.Date)
                                .AddProperty(DataColumnDef.DivisionName)
                                .AddProperty(DataColumnDef.InvoiceNoOrBillNo)
                                .AddProperty(DataColumnDef.LineNumber)
                                .AddProperty(DataColumnDef.NetAmount)
                                .AddProperty(DataColumnDef.Quantity)
                                .AddProperty(DataColumnDef.MatrialCode)
                                .AddProperty(DataColumnDef.MatarialDescription)
                                .AddProperty(DataColumnDef.UnitOfMeasure);

            var queryBuilder = new QueryOptionBuilder();
            queryBuilder.AppendQuery(filterQueryBuilder.Filter)
                        .AppendQuery(selectQueryBuilder.Select);

            var data = await _odataService.GetSalesData(queryBuilder.Query);

            var result = data.Select(x => new InvoiceItemDetailsResultModel()
            {
                NetAmount = CustomConvertExtension.ObjectToDecimal(x.NetAmount),
                Quantity = CustomConvertExtension.ObjectToDecimal(x.Quantity),
                MatrialCode = x.MatrialCode,
                MatarialDescription = x.MatarialDescription,
                Unit = x.UnitOfMeasure,
                LineNumber = x.LineNumber,
            }).ToList();

            var returnResult = new InvoiceDetailsResultModel();

            if (data.Any())
            {
                returnResult.InvoiceNoOrBillNo = data.FirstOrDefault().InvoiceNoOrBillNo;
                returnResult.Date = data.FirstOrDefault().Date.ReturnDateFormatDate();
                returnResult.NetAmount = data.Sum(x => CustomConvertExtension.ObjectToDecimal(x.NetAmount));
                returnResult.CustomerNo = data.FirstOrDefault().CustomerNoOrSoldToParty;
                returnResult.CustomerName = data.FirstOrDefault().CustomerName;
                returnResult.Division = data.FirstOrDefault().Division;
                returnResult.DivisionName = data.FirstOrDefault().DivisionName;

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

        public async Task<IList<BrandWiseMTDResultModel>> GetBrandWiseMTDDetails(BrandWiseMTDSearchModel model)
        {
            var currentDate = DateTime.Now;
            var previousMonthCount = 3;
            var cbMaterialCodes = new List<string>();

            var cyfd = currentDate.GetCYFD().DateFormat();
            var cylcd = currentDate.GetCYLCD().DateFormat();

            var lyfd = currentDate.GetLYFD().DateFormat();
            var lylcd = currentDate.GetLYLCD().DateFormat();

            var dataLy = new List<SalesDataModel>();
            var dataCy = new List<SalesDataModel>();
            var previousMonthDict = new Dictionary<string, IList<SalesDataModel>>();

            var selectQueryBuilder = new SelectQueryOptionBuilder();
            selectQueryBuilder.AddProperty(DataColumnDef.CustomerNoOrSoldToParty)
                                .AddProperty(DataColumnDef.Division)
                                .AddProperty(DataColumnDef.InvoiceNoOrBillNo)
                                .AddProperty(DataColumnDef.Date)
                                .AddProperty(DataColumnDef.NetAmount)
                                .AddProperty(DataColumnDef.MatarialGroupOrBrand)
                                .AddProperty(DataColumnDef.MatarialGroupOrBrandName);

            if (model.IsOnlyCBMaterial)
            {
                cbMaterialCodes = (await _odataBrandService.GetCBMaterialCodesAsync()).ToList();
            }

            dataLy = (await _odataService.GetSalesDataByCustomerAndDivision(selectQueryBuilder, model.CustomerNo, lyfd, lylcd, model.Division, cbMaterialCodes)).ToList();

            dataCy = (await _odataService.GetSalesDataByCustomerAndDivision(selectQueryBuilder, model.CustomerNo, cyfd, cylcd, model.Division, cbMaterialCodes)).ToList();

            for (var i = 1; i <= previousMonthCount; i++)
            {
                int number = i * -1;
                var startDate = currentDate.GetMonthDate(number).GetCYFD().DateFormat();
                var endDate = currentDate.GetMonthDate(number).GetCYLD().DateFormat();

                var data = (await _odataService.GetSalesDataByCustomerAndDivision(selectQueryBuilder, model.CustomerNo, startDate, endDate, model.Division, cbMaterialCodes)).ToList();
                var monthName = currentDate.GetMonthName(number);

                previousMonthDict.Add(monthName, data);
            }

            var result = new List<BrandWiseMTDResultModel>();

            var brandCodes = dataLy.Select(x => x.MatarialGroupOrBrand)
                                .Concat(dataCy.Select(x => x.MatarialGroupOrBrand))
                                    .Concat(previousMonthDict.Values.SelectMany(x => x).Select(x => x.MatarialGroupOrBrand))
                                        .Distinct().ToList();

            foreach (var brandCode in brandCodes)
            {
                var res = new BrandWiseMTDResultModel();
                res.PreviousMonthData = new List<BrandWiseMTDPreviousModel>();

                if (dataLy.Any(x => x.MatarialGroupOrBrand == brandCode))
                {
                    var mtdAmtLy = dataLy.Where(x => x.MatarialGroupOrBrand == brandCode).Sum(x => CustomConvertExtension.ObjectToDecimal(x.NetAmount));
                    var brandNameLy = dataLy.FirstOrDefault(x => x.MatarialGroupOrBrand == brandCode).MatarialGroupOrBrandName;

                    res.MatarialGroupOrBrand = string.IsNullOrEmpty(res.MatarialGroupOrBrand) ? brandNameLy : res.MatarialGroupOrBrand;
                    res.LYMTD = mtdAmtLy;
                }

                if (dataCy.Any(x => x.MatarialGroupOrBrand == brandCode))
                {
                    var mtdAmtCy = dataCy.Where(x => x.MatarialGroupOrBrand == brandCode).Sum(x => CustomConvertExtension.ObjectToDecimal(x.NetAmount));
                    var brandNameCy = dataCy.FirstOrDefault(x => x.MatarialGroupOrBrand == brandCode).MatarialGroupOrBrandName;

                    res.MatarialGroupOrBrand = string.IsNullOrEmpty(res.MatarialGroupOrBrand) ? brandNameCy : res.MatarialGroupOrBrand;
                    res.CYMTD = mtdAmtCy;
                }

                for (var i = 1; i <= previousMonthCount; i++)
                {
                    int number = i * -1;
                    var monthName = currentDate.GetMonthName(number);
                    var dictData = previousMonthDict[monthName].ToList();
                    var mtdAmt = decimal.Zero;

                    if (dictData.Any(x => x.MatarialGroupOrBrand == brandCode))
                    {
                        mtdAmt = dictData.Where(x => x.MatarialGroupOrBrand == brandCode).Sum(x => CustomConvertExtension.ObjectToDecimal(x.NetAmount));
                        var brandName = dictData.FirstOrDefault(x => x.MatarialGroupOrBrand == brandCode).MatarialGroupOrBrandName;

                        res.MatarialGroupOrBrand = string.IsNullOrEmpty(res.MatarialGroupOrBrand) ? brandName : res.MatarialGroupOrBrand;
                    }

                    res.PreviousMonthData.Add(new BrandWiseMTDPreviousModel() { MonthName = monthName, Amount = mtdAmt });
                }

                res.Growth = _odataService.GetGrowth(res.LYMTD, res.CYMTD);

                result.Add(res);
            }

            return result;
        }

        public async Task<IList<BrandOrDivisionWiseMTDResultModel>> GetBrandOrDivisionWisePerformance(BrandOrDivisionWiseMTDSearchModel model)
        {
            var currentDate = DateTime.Now;
            var mtsBrandCodes = new List<string>();

            var cyfd = currentDate.GetCYFD().DateFormat();
            var cylcd = currentDate.GetCYLCD().DateFormat();
            var cyld = currentDate.GetCYLD().DateFormat();

            var lyfd = currentDate.GetLYFD().DateFormat();
            var lylcd = currentDate.GetLYLCD().DateFormat();
            var lyld = currentDate.GetLYLD().DateFormat();

            var lfyfd = currentDate.GetLFYFD().DateFormat();
            var lfylcd = currentDate.GetLFYLCD().DateFormat();
            var lfyld = currentDate.GetLFYLD().DateFormat();

            var cfyfd = currentDate.GetCFYFD().DateFormat();
            var cfylcd = currentDate.GetCFYLCD().DateFormat();
            var cfyld = currentDate.GetCFYLD().DateFormat();

            var dataLySm = new List<SalesDataModel>();
            var dataLyMtd = new List<SalesDataModel>();
            var dataCyMtd = new List<SalesDataModel>();
            var dataLyYtd = new List<SalesDataModel>();
            var dataCyYtd = new List<SalesDataModel>();

            var selectQueryBuilder = new SelectQueryOptionBuilder();
            selectQueryBuilder.AddProperty(DataColumnDef.CustomerNoOrSoldToParty)
                                .AddProperty(DataColumnDef.Division)
                                .AddProperty(DataColumnDef.DivisionName)
                                .AddProperty(DataColumnDef.InvoiceNoOrBillNo)
                                .AddProperty(DataColumnDef.Date)
                                .AddProperty(DataColumnDef.NetAmount)
                                .AddProperty(DataColumnDef.Volume)
                                .AddProperty(DataColumnDef.MatarialGroupOrBrand)
                                .AddProperty(DataColumnDef.MatarialGroupOrBrandName);

            if (model.BrandOrDivision == EnumBrandOrDivision.MTS_Brand)
            {
                mtsBrandCodes = (await _odataBrandService.GetMTSBrandCodesAsync()).ToList();
            }

            dataLySm = (await _odataService.GetSalesDataByCustomerAndDivision(selectQueryBuilder, model.CustomerNo, lyfd, lyld, model.Division, brands: mtsBrandCodes)).ToList();

            dataLyMtd = (await _odataService.GetSalesDataByCustomerAndDivision(selectQueryBuilder, model.CustomerNo, lyfd, lylcd, model.Division, brands: mtsBrandCodes)).ToList();

            dataCyMtd = (await _odataService.GetSalesDataByCustomerAndDivision(selectQueryBuilder, model.CustomerNo, cyfd, cylcd, model.Division, brands: mtsBrandCodes)).ToList();

            dataLyYtd = (await _odataService.GetSalesDataByCustomerAndDivision(selectQueryBuilder, model.CustomerNo, lfyfd, lfylcd, model.Division, brands: mtsBrandCodes)).ToList();

            dataCyYtd = (await _odataService.GetSalesDataByCustomerAndDivision(selectQueryBuilder, model.CustomerNo, cfyfd, cfylcd, model.Division, brands: mtsBrandCodes)).ToList();

            Func<SalesDataModel, decimal> calcFunc = x => CustomConvertExtension.ObjectToDecimal(
                                                            model.VolumeOrValue == EnumVolumeOrValue.Value ? x.NetAmount : x.Volume);
            Func<SalesDataModel, string> selectFunc = x => model.BrandOrDivision == EnumBrandOrDivision.Division ?
                                                            x.DivisionName : x.MatarialGroupOrBrandName;
            Func<SalesDataModel, string, bool> predicateFunc = (x, val) => model.BrandOrDivision == EnumBrandOrDivision.Division ?
                                                                    x.DivisionName == val : x.MatarialGroupOrBrandName == val;
            var result = new List<BrandOrDivisionWiseMTDResultModel>();

            var brandsOrDivisions = dataLySm.Select(selectFunc)
                                        .Concat(dataLyMtd.Select(selectFunc))
                                            .Concat(dataCyMtd.Select(selectFunc))
                                                .Concat(dataLyYtd.Select(selectFunc))
                                                    .Concat(dataCyYtd.Select(selectFunc))
                                                        .Distinct().ToList();

            foreach (var brandOrDiv in brandsOrDivisions)
            {
                var res = new BrandOrDivisionWiseMTDResultModel();

                if (dataLySm.Any(x => predicateFunc(x, brandOrDiv)))
                {
                    var amtLySm = dataLySm.Where(x => predicateFunc(x, brandOrDiv)).Sum(calcFunc);
                    var brandOrDivNameLySm = dataLySm.Where(x => predicateFunc(x, brandOrDiv)).Select(selectFunc).FirstOrDefault();

                    res.MatarialGroupOrBrandOrDivision = string.IsNullOrEmpty(res.MatarialGroupOrBrandOrDivision) ? brandOrDivNameLySm : res.MatarialGroupOrBrandOrDivision;
                    res.LYSM = amtLySm;
                }

                if (dataLyMtd.Any(x => predicateFunc(x, brandOrDiv)))
                {
                    var amtLyMtd = dataLyMtd.Where(x => predicateFunc(x, brandOrDiv)).Sum(calcFunc);
                    var brandOrDivNameLyMtd = dataLyMtd.Where(x => predicateFunc(x, brandOrDiv)).Select(selectFunc).FirstOrDefault();

                    res.MatarialGroupOrBrandOrDivision = string.IsNullOrEmpty(res.MatarialGroupOrBrandOrDivision) ? brandOrDivNameLyMtd : res.MatarialGroupOrBrandOrDivision;
                    res.LYMTD = amtLyMtd;
                }

                if (dataCyMtd.Any(x => predicateFunc(x, brandOrDiv)))
                {
                    var amtCyMtd = dataCyMtd.Where(x => predicateFunc(x, brandOrDiv)).Sum(calcFunc);
                    var brandOrDivNameCyMtd = dataCyMtd.Where(x => predicateFunc(x, brandOrDiv)).Select(selectFunc).FirstOrDefault();

                    res.MatarialGroupOrBrandOrDivision = string.IsNullOrEmpty(res.MatarialGroupOrBrandOrDivision) ? brandOrDivNameCyMtd : res.MatarialGroupOrBrandOrDivision;
                    res.CYMTD = amtCyMtd;
                }

                if (dataLyYtd.Any(x => predicateFunc(x, brandOrDiv)))
                {
                    var amtLyYtd = dataLyYtd.Where(x => predicateFunc(x, brandOrDiv)).Sum(calcFunc);
                    var brandOrDivNameLyYtd = dataLyYtd.Where(x => predicateFunc(x, brandOrDiv)).Select(selectFunc).FirstOrDefault();

                    res.MatarialGroupOrBrandOrDivision = string.IsNullOrEmpty(res.MatarialGroupOrBrandOrDivision) ? brandOrDivNameLyYtd : res.MatarialGroupOrBrandOrDivision;
                    res.LYYTD = amtLyYtd;
                }

                if (dataCyYtd.Any(x => predicateFunc(x, brandOrDiv)))
                {
                    var amtCyYtd = dataCyYtd.Where(x => predicateFunc(x, brandOrDiv)).Sum(calcFunc);
                    var brandOrDivNameCyYtd = dataCyYtd.Where(x => predicateFunc(x, brandOrDiv)).Select(selectFunc).FirstOrDefault();

                    res.MatarialGroupOrBrandOrDivision = string.IsNullOrEmpty(res.MatarialGroupOrBrandOrDivision) ? brandOrDivNameCyYtd : res.MatarialGroupOrBrandOrDivision;
                    res.CYYTD = amtCyYtd;
                }

                res.GrowthMTD = _odataService.GetGrowth(res.LYMTD, res.CYMTD);

                res.GrowthYTD = _odataService.GetGrowth(res.LYYTD, res.CYYTD);

                result.Add(res);
            }

            return result;
        }

        public async Task<IList<SalesDataModel>> GetMyTargetSales(DateTime fromDate, DateTime endDate, string division, EnumVolumeOrValue volumeOrValue,
            MyTargetReportType targetReportType, IList<int> dealerIds)
        {
            var selectQueryBuilder = new SelectQueryOptionBuilder();

            switch (targetReportType)
            {
                case MyTargetReportType.TerritoryWiseTarget:
                    selectQueryBuilder.AddProperty(DataColumnDef.Territory);
                    break;
                case MyTargetReportType.ZoneWiseTarget:
                    selectQueryBuilder.AddProperty(DataColumnDef.Zone);
                    break;
                case MyTargetReportType.BrandWise:
                    selectQueryBuilder.AddProperty(DataColumnDef.MatarialGroupOrBrand);
                    break;
            }


            selectQueryBuilder.AddProperty(volumeOrValue == EnumVolumeOrValue.Volume
                ? DataColumnDef.Volume
                : DataColumnDef.NetAmount);

            var cyfd = fromDate.GetCYFD().DateFormat();
            var cyed = endDate.DateFormat();

            return await _odataService.GetSalesDataByMultipleCustomerAndDivision(selectQueryBuilder, dealerIds, cyfd, cyed, division);

        }
    }
}

