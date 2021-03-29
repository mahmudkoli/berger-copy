﻿using System;
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

        #region During dealer visit
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
            //selectQueryBuilder.AddProperty(DataColumnDef.CustomerNoOrSoldToParty)
            //                .AddProperty(DataColumnDef.Division)
            //                .AddProperty(DataColumnDef.DivisionName)
            //                .AddProperty(DataColumnDef.InvoiceNoOrBillNo)
            //                .AddProperty(DataColumnDef.Date)
            //                .AddProperty(DataColumnDef.NetAmount)
            //                .AddProperty(DataColumnDef.Volume)
            //                .AddProperty(DataColumnDef.MatarialGroupOrBrand)
            //                .AddProperty(DataColumnDef.MatarialGroupOrBrandName);

            if (model.VolumeOrValue == EnumVolumeOrValue.Volume)
            {
                selectQueryBuilder.AddProperty(DataColumnDef.Volume);
            } 
            else
            {
                selectQueryBuilder.AddProperty(DataColumnDef.NetAmount);
            }

            if (model.BrandOrDivision == EnumBrandOrDivision.Division)
            {
                selectQueryBuilder.AddProperty(DataColumnDef.Division)
                                    .AddProperty(DataColumnDef.DivisionName);
            } 
            else
            {
                selectQueryBuilder.AddProperty(DataColumnDef.MatarialGroupOrBrand)
                                    .AddProperty(DataColumnDef.MatarialGroupOrBrandName);
            }

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
        #endregion

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

        public async Task<IList<TotalInvoiceValueResultModel>> GetReportTotalInvoiceValue(TotalInvoiceValueSearchModel model, IList<int> dealerIds)
        {
            var currentDate = DateTime.Now;
            var fromDate = currentDate.DateFormat();
            var toDate = currentDate.DateFormat();

            var selectQueryBuilder = new SelectQueryOptionBuilder();
            selectQueryBuilder.AddProperty(DataColumnDef.CustomerNoOrSoldToParty)
                                .AddProperty(DataColumnDef.CustomerName)
                                .AddProperty(DataColumnDef.InvoiceNoOrBillNo)
                                .AddProperty(DataColumnDef.NetAmount);

            var data = (await _odataService.GetSalesDataByMultipleCustomerAndDivision(selectQueryBuilder, dealerIds, fromDate, toDate, model.Division)).ToList();

            var result = data.Select(x =>
                                new TotalInvoiceValueResultModel()
                                {
                                    InvoiceNoOrBillNo = x.InvoiceNoOrBillNo,
                                    CustomerNo = x.CustomerNoOrSoldToParty,
                                    CustomerName = x.CustomerName,
                                    NetAmount = CustomConvertExtension.ObjectToDecimal(x.NetAmount)
                                }).ToList();

            return result;
        }

        public async Task<IList<BrandOrDivisionWisePerformanceResultModel>> GetReportBrandOrDivisionWisePerformance(BrandOrDivisionWisePerformanceSearchModel model, IList<int> dealerIds)
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

            var dataLyMtd = new List<SalesDataModel>();
            var dataCyMtd = new List<SalesDataModel>();
            var dataLyYtd = new List<SalesDataModel>();
            var dataCyYtd = new List<SalesDataModel>();

            var selectQueryBuilder = new SelectQueryOptionBuilder();
            selectQueryBuilder
                                //.AddProperty(DataColumnDef.CustomerNoOrSoldToParty)
                                .AddProperty(DataColumnDef.Division)
                                .AddProperty(DataColumnDef.DivisionName)
                                //.AddProperty(DataColumnDef.InvoiceNoOrBillNo)
                                //.AddProperty(DataColumnDef.Date)
                                .AddProperty(DataColumnDef.NetAmount)
                                .AddProperty(DataColumnDef.Volume)
                                .AddProperty(DataColumnDef.MatarialGroupOrBrand)
                                .AddProperty(DataColumnDef.MatarialGroupOrBrandName);

            if (model.BrandOrDivision == EnumBrandOrDivision.MTS_Brand)
            {
                mtsBrandCodes = (await _odataBrandService.GetMTSBrandCodesAsync()).ToList();
            }

            dataLyMtd = (await _odataService.GetSalesDataByMultipleCustomerAndDivision(selectQueryBuilder, dealerIds, lyfd, lylcd, model.Division, brands: mtsBrandCodes)).ToList();

            dataCyMtd = (await _odataService.GetSalesDataByMultipleCustomerAndDivision(selectQueryBuilder, dealerIds, cyfd, cylcd, model.Division, brands: mtsBrandCodes)).ToList();

            dataLyYtd = (await _odataService.GetSalesDataByMultipleCustomerAndDivision(selectQueryBuilder, dealerIds, lfyfd, lfylcd, model.Division, brands: mtsBrandCodes)).ToList();

            dataCyYtd = (await _odataService.GetSalesDataByMultipleCustomerAndDivision(selectQueryBuilder, dealerIds, cfyfd, cfylcd, model.Division, brands: mtsBrandCodes)).ToList();

            Func<SalesDataModel, decimal> calcFunc = x => CustomConvertExtension.ObjectToDecimal(
                                                            model.VolumeOrValue == EnumVolumeOrValue.Value ? x.NetAmount : x.Volume);
            Func<SalesDataModel, string> selectFunc = x => model.BrandOrDivision == EnumBrandOrDivision.Division ?
                                                            x.DivisionName : x.MatarialGroupOrBrandName;
            Func<SalesDataModel, string, bool> predicateFunc = (x, val) => model.BrandOrDivision == EnumBrandOrDivision.Division ?
                                                                    x.DivisionName == val : x.MatarialGroupOrBrandName == val;
            var result = new List<BrandOrDivisionWisePerformanceResultModel>();

            var brandsOrDivisions = dataLyMtd.Select(selectFunc)
                                        .Concat(dataCyMtd.Select(selectFunc))
                                            .Concat(dataLyYtd.Select(selectFunc))
                                                .Concat(dataCyYtd.Select(selectFunc))
                                                    .Distinct().ToList();

            foreach (var brandOrDiv in brandsOrDivisions)
            {
                var res = new BrandOrDivisionWisePerformanceResultModel();

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

        public async Task<IList<DealerPerformanceResultModel>> GetReportDealerPerformance(DealerPerformanceSearchModel model, IList<int> dealerIds)
        {
            var currentDate = DateTime.Now;
            var customerCount = 10;

            var customerClassification = model.DealerCategory switch
            {
                EnumDealerClassificationCategory.All => "-1",
                EnumDealerClassificationCategory.Exclusive => ConstantsValue.CustomerClassificationExclusive,
                EnumDealerClassificationCategory.NonExclusive => ConstantsValue.CustomerClassificationNonExclusive,
                _ => "-1"
            };

            var fromDate = currentDate.AddDays(-30);
            var toDate = currentDate;

            var lfyfd = currentDate.GetLFYFD().DateFormat();
            var lfylcd = currentDate.GetLFYLCD().DateFormat();
            var lfyld = currentDate.GetLFYLD().DateFormat();

            var cfyfd = currentDate.GetCFYFD().DateFormat();
            var cfylcd = currentDate.GetCFYLCD().DateFormat();
            var cfyld = currentDate.GetCFYLD().DateFormat();

            var dataLy = new List<SalesDataModel>();
            var dataCy = new List<SalesDataModel>();

            var selectQueryBuilder = new SelectQueryOptionBuilder();
            selectQueryBuilder.AddProperty(DataColumnDef.CustomerNoOrSoldToParty)
                                .AddProperty(DataColumnDef.CustomerName)
                                .AddProperty(DataColumnDef.CustomerClassification)
                                .AddProperty(DataColumnDef.Date)
                                .AddProperty(DataColumnDef.NetAmount);

            dataLy = (await _odataService.GetSalesDataByMultipleCustomerAndDivision(selectQueryBuilder, dealerIds, lfyfd, lfyld, customerClassification: customerClassification, territory: model.Territory)).ToList();

            dataCy = (await _odataService.GetSalesDataByMultipleCustomerAndDivision(selectQueryBuilder, dealerIds, cfyfd, cfyld, customerClassification: customerClassification, territory: model.Territory)).ToList();

            var dataLyGroup = dataLy.GroupBy(x => x.CustomerNoOrSoldToParty).Select(s =>
                                        new DealerPerformanceResultModel()
                                        {
                                            CustomerNo = s.Key,
                                            CustomerName = s.FirstOrDefault()?.CustomerName ?? string.Empty,
                                            LYSales = s.Sum(s => CustomConvertExtension.ObjectToDecimal(s.NetAmount))
                                        });

            var result = new List<DealerPerformanceResultModel>();

            if (model.DealerPerformanceCategory == EnumDealerPerformanceCategory.Top_10_Performer || model.DealerPerformanceCategory == EnumDealerPerformanceCategory.Bottom_10_Performer)
            {
                var dataCyGroup = dataCy.GroupBy(x => x.CustomerNoOrSoldToParty).Select(s =>
                                            new DealerPerformanceResultModel()
                                            {
                                                CustomerNo = s.Key,
                                                CustomerName = s.FirstOrDefault()?.CustomerName ?? string.Empty,
                                                CYSales = s.Sum(s => CustomConvertExtension.ObjectToDecimal(s.NetAmount))
                                            });

                var performerData = model.DealerPerformanceCategory == EnumDealerPerformanceCategory.Top_10_Performer ?
                            dataCyGroup.OrderByDescending(o => o.CYSales).Take(customerCount) : dataCyGroup.OrderBy(o => o.CYSales).Take(customerCount);

                var slNo = 1;

                foreach (var item in performerData)
                {
                    var res = new DealerPerformanceResultModel();
                    res.SLNo = slNo++;
                    res.CustomerNo = item.CustomerNo;
                    res.CustomerName = item.CustomerName;
                    res.CYSales = item.CYSales;
                    res.LYSales = dataLyGroup.FirstOrDefault(f => f.CustomerNo == item.CustomerNo)?.LYSales ?? decimal.Zero;
                    res.Growth = _odataService.GetGrowth(res.LYSales, res.CYSales);
                    result.Add(res);
                }
            }
            else if (model.DealerPerformanceCategory == EnumDealerPerformanceCategory.NotPurchasedLastMonth)
            {
                var notPurchasedCyData = dataCy.Where(x => !(x.Date.DateFormatDate("yyyyMMdd") >= fromDate && x.Date.DateFormatDate("yyyyMMdd") <= toDate));

                var notPurchasedCyGroupData = notPurchasedCyData.GroupBy(x => x.CustomerNoOrSoldToParty).Select(s =>
                                            new DealerPerformanceResultModel()
                                            {
                                                CustomerNo = s.Key,
                                                CustomerName = s.FirstOrDefault()?.CustomerName ?? string.Empty,
                                                CYSales = s.Sum(s => CustomConvertExtension.ObjectToDecimal(s.NetAmount))
                                            });

                var slNo = 1;

                foreach (var item in notPurchasedCyGroupData)
                {
                    var res = new DealerPerformanceResultModel();
                    res.SLNo = slNo++;
                    res.CustomerNo = item.CustomerNo;
                    res.CustomerName = item.CustomerName;
                    res.CYSales = item.CYSales;
                    res.LYSales = dataLyGroup.FirstOrDefault(f => f.CustomerNo == item.CustomerNo)?.LYSales ?? decimal.Zero;
                    res.Growth = _odataService.GetGrowth(res.LYSales, res.CYSales);
                    result.Add(res);
                }
            }

            return result;
        }

        public async Task<IList<ReportDealerPerformanceResultModel>> GetReportDealerPerformance(IList<int> dealerIds, DealerPerformanceReportType dealerPerformanceReportType)
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

            var dataLyMtd = new List<SalesDataModel>();
            var dataCyMtd = new List<SalesDataModel>();
            var dataLyYtd = new List<SalesDataModel>();
            var dataCyYtd = new List<SalesDataModel>();

            var selectQueryBuilder = new SelectQueryOptionBuilder();
            selectQueryBuilder
                .AddProperty(DataColumnDef.NetAmount)
                .AddProperty(DataColumnDef.Territory);

            if (dealerPerformanceReportType == DealerPerformanceReportType.ClubSupremeTerritoryAndDealerWise)
            {
                selectQueryBuilder
                    .AddProperty(DataColumnDef.CustomerNo)
                    .AddProperty(DataColumnDef.CustomerName);
            }

            string division = "-1";

            dataLyMtd = (await _odataService.GetSalesDataByMultipleCustomerAndDivision(selectQueryBuilder, dealerIds, lyfd, lylcd, division, brands: mtsBrandCodes)).ToList();

            dataCyMtd = (await _odataService.GetSalesDataByMultipleCustomerAndDivision(selectQueryBuilder, dealerIds, cyfd, cylcd, division, brands: mtsBrandCodes)).ToList();

            dataLyYtd = (await _odataService.GetSalesDataByMultipleCustomerAndDivision(selectQueryBuilder, dealerIds, lfyfd, lfylcd, division, brands: mtsBrandCodes)).ToList();

            dataCyYtd = (await _odataService.GetSalesDataByMultipleCustomerAndDivision(selectQueryBuilder, dealerIds, cfyfd, cfylcd, division, brands: mtsBrandCodes)).ToList();

            Func<SalesDataModel, SalesDataModel> selectFunc = x => new SalesDataModel
            {
                NetAmount = x.NetAmount,
                Territory = x.Territory,
                CustomerNo = x.CustomerNo,
                CustomerName = x.CustomerName
            };

            Func<SalesDataModel, decimal> calcFunc = x => CustomConvertExtension.ObjectToDecimal(x.NetAmount);
            Func<SalesDataModel, SalesDataModel, bool> predicateFunc = (x, val) => x.Territory == val.Territory && x.CustomerName == val.CustomerName && x.CustomerNo == val.CustomerNo;

            var concatAllList = dataLyMtd.Select(selectFunc)
                .Concat(dataCyMtd.Select(selectFunc))
                .Concat(dataLyYtd.Select(selectFunc))
                .Concat(dataCyYtd.Select(selectFunc))
                .GroupBy(p => new { p.Territory, p.CustomerName, p.CustomerNo })
                .Select(g => g.First());

            var result = new List<ReportDealerPerformanceResultModel>();


            foreach (var item in concatAllList)
            {
                var res = new ReportDealerPerformanceResultModel();

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

                res.Territory = item.Territory;
                res.DealerId = item.CustomerNo;
                res.DealerName = item.CustomerName;
                res.GrowthMTD = _odataService.GetGrowth(res.LYMTD, res.CYMTD);
                res.GrowthYTD = _odataService.GetGrowth(res.LYYTD, res.CYYTD);

                result.Add(res);
            }

            return result;
        }

        public async Task<int> NoOfBillingDealer(IList<int> dealerIds)
        {
            var selectQueryBuilder = new SelectQueryOptionBuilder();
            //selectQueryBuilder.AddProperty(DataColumnDef.CustomerNoOrSoldToParty);
            selectQueryBuilder.AddProperty(DataColumnDef.CustomerNo).AddProperty(DataColumnDef.Volume);
            var fromDate = DateTime.Now.DateFormat();
            var toDate = DateTime.Now.DateFormat();
            IList<SalesDataModel> salesDataByMultipleCustomerAndDivision = await _odataService.GetSalesDataByMultipleCustomerAndDivision(selectQueryBuilder, dealerIds, fromDate, toDate);
            return salesDataByMultipleCustomerAndDivision.Select(x => x.CustomerNo).Distinct().Count();
        }
    }
}

