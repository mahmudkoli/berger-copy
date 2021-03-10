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
            //model.FromDate = "2011.09.01";//(new DateTime(2011, 09, 01)).DateFormat()
            //model.ToDate = "2011.10.01";//(new DateTime(2011, 10, 01)).DateFormat()
            //model.CustomerNo = "24";
            //model.Division = "10";

            //var fromDate = model.FromDate.DateFormat();
            //var toDate = model.ToDate.DateFormat();
            var currentDate = DateTime.Now;
            var fromDate = currentDate.AddDays(-30).DateFormat();
            var toDate = currentDate.DateFormat();

            var selectQueryBuilder = new SelectQueryOptionBuilder();
            selectQueryBuilder.AddProperty(DataColumnDef.CustomerNoOrSoldToParty)
                                .AddProperty(DataColumnDef.CustomerName)
                                .AddProperty(DataColumnDef.Division)
                                .AddProperty(DataColumnDef.DivisionName)
                                .AddProperty(DataColumnDef.InvoiceNoOrBillNo)
                                .AddProperty(DataColumnDef.Date)
                                .AddProperty(DataColumnDef.NetAmount);

            var data = (await _odataService.GetSalesDataByCustomerAndDivision(selectQueryBuilder, model.CustomerNo, fromDate, toDate, model.Division)).ToList();

            var result = data.Select(x => 
                                new InvoiceHistoryResultModel()
                                {
                                    CustomerNo = x.CustomerNoOrSoldToParty,
                                    CustomerName = x.CustomerName,
                                    Division = x.Division,
                                    DivisionName = x.DivisionName,
                                    InvoiceNoOrBillNo = x.InvoiceNoOrBillNo,
                                    Date = x.Date,
                                    NetAmount = CustomConvertExtension.ObjectToDecimal(x.NetAmount)
                                }).ToList();

            #region get driver data
            if(result.Any())
            {
                var invoiceNos = result.Select(x => x.InvoiceNoOrBillNo).Distinct().ToList();

                var allDriverData = await _odataService.GetDriverDataByInvoiceNos(invoiceNos);

                foreach (var item in result)
                {
                    var driverData = allDriverData.FirstOrDefault(x => x.InvoiceNoOrBillNo == item.InvoiceNoOrBillNo);
                    if (driverData != null)
                    {
                        item.DriverName = driverData.DriverName;
                        item.DriverMobileNo = driverData.DriverMobileNo;
                    }
                }
            }
            #endregion

            return result;
        }

        public async Task<IList<InvoiceItemDetailsResultModel>> GetInvoiceItemDetails(InvoiceItemDetailsSearchModel model)
        {
            var filterQueryBuilder = new FilterQueryOptionBuilder();
            filterQueryBuilder.Equal(DataColumnDef.InvoiceNoOrBillNo, model.InvoiceNo);

            var selectQueryBuilder = new SelectQueryOptionBuilder();
            selectQueryBuilder.AddProperty(DataColumnDef.NetAmount)
                                .AddProperty(DataColumnDef.Quantity)
                                .AddProperty(DataColumnDef.MatrialCode)
                                .AddProperty(DataColumnDef.MatarialDescription);

            //var topQuery = $"$top=5";

            var queryBuilder = new QueryOptionBuilder();
            queryBuilder.AppendQuery(filterQueryBuilder.Filter)
                        //.AppendQuery(topQuery)
                        .AppendQuery(selectQueryBuilder.Select);

            var data = await _odataService.GetSalesData(queryBuilder.Query);

            var result = data.Select(x => new InvoiceItemDetailsResultModel()
                                            {
                                                NetAmount = CustomConvertExtension.ObjectToDecimal(x.NetAmount),
                                                Quantity = CustomConvertExtension.ObjectToDecimal(x.Quantity),
                                                MatrialCode = x.MatrialCode,
                                                MatarialDescription = x.MatarialDescription,
                                            }).ToList();

            return result;
        }

        public async Task<IList<BrandWiseMTDResultModel>> GetBrandWiseMTDDetails(BrandWiseMTDSearchModel model)
        {
            //var currentdate = new DateTime(2011, 09, 21);
            //var currentdate = model.Date;
            var currentdate = DateTime.Now;
            var previousMonthCount = 3;
            var cbMaterialCodes = new List<string>();

            var cyfd = currentdate.GetCYFD().DateFormat();
            var cylcd = currentdate.GetCYLCD().DateFormat();

            var lyfd = currentdate.GetLYFD().DateFormat();
            var lylcd = currentdate.GetLYLCD().DateFormat();

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
                //TODO: get CB material codes and add to list
                cbMaterialCodes = (await _odataBrandService.GetCBMaterialCodesAsync()).ToList();
            }

            dataLy = (await _odataService.GetSalesDataByCustomerAndDivision(selectQueryBuilder, model.CustomerNo, lyfd, lylcd, model.Division, cbMaterialCodes)).ToList();
            
            dataCy = (await _odataService.GetSalesDataByCustomerAndDivision(selectQueryBuilder, model.CustomerNo, cyfd, cylcd, model.Division, cbMaterialCodes)).ToList();
            
            for (var i = 1; i <= previousMonthCount; i++)
            {
                int number = i * -1;
                var startDate = currentdate.GetMonthDate(number).GetCYFD().DateFormat();
                var endDate = currentdate.GetMonthDate(number).GetCYLD().DateFormat();

                var data = (await _odataService.GetSalesDataByCustomerAndDivision(selectQueryBuilder, model.CustomerNo, startDate, endDate, model.Division, cbMaterialCodes)).ToList();
                var monthName = currentdate.GetMonthName(number);

                previousMonthDict.Add(monthName, data);
            }

            Func<SalesDataModel, decimal> calcFunc = x => CustomConvertExtension.ObjectToDecimal(x.NetAmount);
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
                    var mtdAmtLy = dataLy.Where(x => x.MatarialGroupOrBrand == brandCode).Sum(calcFunc);
                    var brandNameLy = dataLy.FirstOrDefault(x => x.MatarialGroupOrBrand == brandCode).MatarialGroupOrBrandName;

                    res.MatarialGroupOrBrand = string.IsNullOrEmpty(res.MatarialGroupOrBrand) ? brandNameLy : res.MatarialGroupOrBrand;
                    res.LYMTD = mtdAmtLy;
                }

                if (dataCy.Any(x => x.MatarialGroupOrBrand == brandCode))
                {
                    var mtdAmtCy = dataCy.Where(x => x.MatarialGroupOrBrand == brandCode).Sum(calcFunc);
                    var brandNameCy = dataCy.FirstOrDefault(x => x.MatarialGroupOrBrand == brandCode).MatarialGroupOrBrandName;

                    res.MatarialGroupOrBrand = string.IsNullOrEmpty(res.MatarialGroupOrBrand) ? brandNameCy : res.MatarialGroupOrBrand;
                    res.CYMTD = mtdAmtCy;
                }

                for (var i = 1; i <= previousMonthCount; i++)
                {
                    int number = i * -1;
                    var monthName = currentdate.GetMonthName(number);
                    var dictData = previousMonthDict[monthName].ToList();
                    var mtdAmt = decimal.Zero;

                    if (dictData.Any(x => x.MatarialGroupOrBrand == brandCode))
                    {
                        mtdAmt = dictData.Where(x => x.MatarialGroupOrBrand == brandCode).Sum(calcFunc);
                        var brandName = dictData.FirstOrDefault(x => x.MatarialGroupOrBrand == brandCode).MatarialGroupOrBrandName;

                        res.MatarialGroupOrBrand = string.IsNullOrEmpty(res.MatarialGroupOrBrand) ? brandName : res.MatarialGroupOrBrand;
                    }

                    res.PreviousMonthData.Add(new BrandWiseMTDPreviousModel() { MonthName = monthName, Amount = mtdAmt });
                }

                res.Growth =  res.LYMTD > 0 && res.CYMTD > 0 ? ((res.CYMTD - res.LYMTD) * 100) / res.LYMTD : 
                                res.LYMTD <= 0 && res.CYMTD > 0 ? decimal.Parse("100.000") : 
                                    decimal.Zero;
                result.Add(res);
            }

            return result;
        }

        public async Task<IList<BrandOrDivisionWiseMTDResultModel>> GetBrandOrDivisionWisePerformance(BrandOrDivisionWiseMTDSearchModel model)
        {
            //var currentdate = new DateTime(2011, 09, 21);
            var firstMonthInYear = 4;
            //var currentdate = model.Date;
            var currentdate = DateTime.Now;
            var mtsBrandCodes = new List<string>();

            var cyfd = currentdate.GetCYFD().DateFormat();
            var cylcd = currentdate.GetCYLCD().DateFormat();
            var cyld = currentdate.GetCYLD().DateFormat();

            var lyfd = currentdate.GetLYFD().DateFormat();
            var lylcd = currentdate.GetLYLCD().DateFormat();
            var lyld = currentdate.GetLYLD().DateFormat();

            var lfyfd = currentdate.GetLFYFD(firstMonthInYear).DateFormat();
            var lfylcd = currentdate.GetLFYLCD(firstMonthInYear).DateFormat();
            var lfyld = currentdate.GetLFYLD(firstMonthInYear).DateFormat();

            var cfyfd = currentdate.GetCFYFD(firstMonthInYear).DateFormat();
            var cfylcd = currentdate.GetCFYLCD(firstMonthInYear).DateFormat();
            var cfyld = currentdate.GetCFYLD(firstMonthInYear).DateFormat();

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

            if(model.BrandOrDivision == EnumBrandOrDivision.MTS_Brand)
            {
                //TODO: get MTS brand codes and add to list
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

            var brandsOrDivisions = dataLyMtd.Select(selectFunc)
                                .Concat(dataCyMtd.Select(selectFunc))
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

                res.GrowthMTD = res.LYMTD > 0 && res.CYMTD > 0 ? ((res.CYMTD - res.LYMTD) * 100) / res.LYMTD :
                                res.LYMTD <= 0 && res.CYMTD > 0 ? decimal.Parse("100.000") :
                                    decimal.Zero;

                res.GrowthYTD = res.LYYTD > 0 && res.CYYTD > 0 ? ((res.CYYTD - res.LYYTD) * 100) / res.LYYTD :
                                res.LYYTD <= 0 && res.CYYTD > 0 ? decimal.Parse("100.000") :
                                    decimal.Zero;
                result.Add(res);
            }

            return result;
        }
    }
}

