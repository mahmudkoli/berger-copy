using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Berger.Common.Extensions;
using Berger.Common.Model;
using Berger.Data.MsfaEntity.Master;
using Berger.Data.MsfaEntity.SAPTables;
using Berger.Data.ViewModel;
using Berger.Odata.Common;
using Berger.Odata.Extensions;
using Berger.Odata.Model;

namespace Berger.Odata.Services
{
    public class SalesDataService : ISalesDataService
    {
        private readonly IODataService _odataService;
        private readonly IODataBrandService _odataBrandService;
        private readonly IODataCommonService _odataCommonService;

        public SalesDataService(
            IODataService odataService,
            IODataBrandService odataBrandService,
            IODataCommonService odataCommonService
        )
        {
            _odataService = odataService;
            _odataBrandService = odataBrandService;
            _odataCommonService = odataCommonService;
        }

        #region During dealer visit
        public async Task<IList<InvoiceHistoryResultModel>> GetInvoiceHistory(InvoiceHistorySearchModel model)
        {
            var currentDate = DateTime.Now;
            var fromDate = currentDate.AddMonths(-1).GetCYFD().DateFormat();
            var toDate = currentDate.GetCYLD().DateFormat();

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

            if (model.BrandOrDivision == EnumBrandOrDivision.MTSBrands)
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

        public async Task<IList<YTDBrandPerformanceSearchModelResultModel>> GetYTDBrandPerformance(YTDBrandPerformanceSearchModelSearchModel model)
        {
            //var filterDate = DateTime.Now.AddMonths(-1);
            var filterDate = new DateTime(model.Year, model.Month, 01);
            var mtsBrandCodes = new List<string>();

            var cyfd = filterDate.GetCYFD().SalesSearchDateFormat();
            var cfyfd = filterDate.GetCFYFD().SalesSearchDateFormat();
            var cyld = filterDate.GetCYLD().SalesSearchDateFormat();

            var lyfd = filterDate.GetLYFD().SalesSearchDateFormat();
            var lfyfd = filterDate.GetLFYFD().SalesSearchDateFormat();
            var lyld = filterDate.GetLYLD().SalesSearchDateFormat();

            var selectQueryBuilder = new SelectQueryOptionBuilder();
            selectQueryBuilder.AddProperty(DataColumnDef.PlantOrBusinessArea)
                                    .AddProperty(model.VolumeOrValue == EnumVolumeOrValue.Volume
                                                ? DataColumnDef.Volume
                                                : DataColumnDef.NetAmount)
                                    .AddProperty(model.BrandOrDivision == EnumBrandOrDivision.Division
                                                ? DataColumnDef.Division
                                                : DataColumnDef.MatarialGroupOrBrand);

            if (model.BrandOrDivision == EnumBrandOrDivision.MTSBrands)
            {
                mtsBrandCodes = (await _odataBrandService.GetMTSBrandCodesAsync()).ToList();
            }

            var dataLyMtd = (await _odataService.GetSalesData(selectQueryBuilder, lyfd, lyld,
                                    depots: model.Depots, territories: model.Territories, zones: model.Zones,
                                    division: model.Division, brands: mtsBrandCodes)).ToList();

            var dataCyMtd = (await _odataService.GetSalesData(selectQueryBuilder, cyfd, cyld,
                                    depots: model.Depots, territories: model.Territories, zones: model.Zones,
                                    division: model.Division, brands: mtsBrandCodes)).ToList();

            var dataLyYtd = (await _odataService.GetSalesData(selectQueryBuilder, lfyfd, lyld,
                                    depots: model.Depots, territories: model.Territories, zones: model.Zones,
                                    division: model.Division, brands: mtsBrandCodes)).ToList();

            var dataCyYtd = (await _odataService.GetSalesData(selectQueryBuilder, cfyfd, cyld,
                                    depots: model.Depots, territories: model.Territories, zones: model.Zones,
                                    division: model.Division, brands: mtsBrandCodes)).ToList();

            Func<SalesDataModel, decimal> calcFunc = x => CustomConvertExtension.ObjectToDecimal(
                                                            model.VolumeOrValue == EnumVolumeOrValue.Volume ? x.Volume : x.NetAmount);
            Func<SalesDataModel, string> selectFunc = x => model.BrandOrDivision == EnumBrandOrDivision.Division ?
                                                            x.Division : x.MatarialGroupOrBrand;
            Func<SalesDataModel, string, bool> predicateFunc = (x, val) => model.BrandOrDivision == EnumBrandOrDivision.Division ?
                                                                    x.Division == val : x.MatarialGroupOrBrand == val;

            var brandsOrDivisions = dataLyMtd.Select(selectFunc)
                                        .Concat(dataCyMtd.Select(selectFunc))
                                            .Concat(dataLyYtd.Select(selectFunc))
                                                .Concat(dataCyYtd.Select(selectFunc))
                                                    .Distinct().ToList();

            var depots = dataLyMtd.Select(x => x.PlantOrBusinessArea)
                            .Concat(dataCyMtd.Select(x => x.PlantOrBusinessArea))
                                .Concat(dataLyYtd.Select(x => x.PlantOrBusinessArea))
                                    .Concat(dataCyYtd.Select(x => x.PlantOrBusinessArea))
                                        .Distinct().ToList();

            var brandFamilyInfos = new List<BrandFamilyInfo>();
            var divisions = new List<Division>();

            if (model.BrandOrDivision == EnumBrandOrDivision.Division)
            {
                divisions = (await _odataCommonService.GetAllDivisionsAsync()).ToList();
            }
            else
            {
                brandFamilyInfos = (await _odataBrandService.GetBrandFamilyInfosAsync(x => brandsOrDivisions.Any(b => b == x.MatarialGroupOrBrand))).ToList();
            }

            #region brand family group
            if (model.BrandOrDivision == EnumBrandOrDivision.MTSBrands)
            {
                foreach (var item in dataLyMtd)
                {
                    item.MatarialGroupOrBrand = brandFamilyInfos.FirstOrDefault(x => x.MatarialGroupOrBrand == item.MatarialGroupOrBrand)?
                                                        .MatarialGroupOrBrandFamily ?? item.MatarialGroupOrBrand;
                }
                foreach (var item in dataCyMtd)
                {
                    item.MatarialGroupOrBrand = brandFamilyInfos.FirstOrDefault(x => x.MatarialGroupOrBrand == item.MatarialGroupOrBrand)?
                                                        .MatarialGroupOrBrandFamily ?? item.MatarialGroupOrBrand;
                }
                foreach (var item in dataLyYtd)
                {
                    item.MatarialGroupOrBrand = brandFamilyInfos.FirstOrDefault(x => x.MatarialGroupOrBrand == item.MatarialGroupOrBrand)?
                                                        .MatarialGroupOrBrandFamily ?? item.MatarialGroupOrBrand;
                }
                foreach (var item in dataCyYtd)
                {
                    item.MatarialGroupOrBrand = brandFamilyInfos.FirstOrDefault(x => x.MatarialGroupOrBrand == item.MatarialGroupOrBrand)?
                                                        .MatarialGroupOrBrandFamily ?? item.MatarialGroupOrBrand;
                }

                brandsOrDivisions = dataLyMtd.Select(x => x.MatarialGroupOrBrand)
                                    .Concat(dataCyMtd.Select(x => x.MatarialGroupOrBrand))
                                        .Concat(dataLyYtd.Select(x => x.MatarialGroupOrBrand))
                                            .Concat(dataCyYtd.Select(x => x.MatarialGroupOrBrand))
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
                }

                var res = new YTDBrandPerformanceSearchModelResultModel();
                res.Depots = dataLyMtd.Where(x => predicateFunc(x, brandOrDiv))
                                        .Select(x => x.PlantOrBusinessArea)
                                .Concat(dataCyMtd.Where(x => predicateFunc(x, brandOrDiv))
                                        .Select(x => x.PlantOrBusinessArea))
                                .Concat(dataLyYtd.Where(x => predicateFunc(x, brandOrDiv))
                                        .Select(x => x.PlantOrBusinessArea))
                                .Concat(dataCyYtd.Where(x => predicateFunc(x, brandOrDiv))
                                        .Select(x => x.PlantOrBusinessArea))
                                .Distinct().ToList();
                res.Depot = res.Depots.FirstOrDefault();
                res.BrandOrDivision = brandOrDivName;
                res.LYMTD = dataLyMtd.Where(x => predicateFunc(x, brandOrDiv)).Sum(calcFunc);
                res.CYMTD = dataCyMtd.Where(x => predicateFunc(x, brandOrDiv)).Sum(calcFunc);
                res.LYYTD = dataLyYtd.Where(x => predicateFunc(x, brandOrDiv)).Sum(calcFunc);
                res.CYYTD = dataCyYtd.Where(x => predicateFunc(x, brandOrDiv)).Sum(calcFunc);
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
            var filterDate = new DateTime(model.Year, model.Month, 01);
            var customerCount = 10;
            var notPurchasedFromDate = filterDate.AddMonths(-2).GetCYFD();
            var notPurchasedToDate = filterDate.GetCYLD();

            var customerClassification = model.Category switch
            {
                EnumCustomerClassification.All => string.Empty,
                EnumCustomerClassification.Exclusive => ConstantsValue.CustomerClassificationExclusive,
                EnumCustomerClassification.NonExclusive => ConstantsValue.CustomerClassificationNonExclusive,
                _ => string.Empty
            };

            var cyfd = filterDate.GetCYFD().SalesSearchDateFormat();
            var cfyfd = filterDate.GetCFYFD().SalesSearchDateFormat();
            var cyld = filterDate.GetCYLD().SalesSearchDateFormat();

            var lyfd = filterDate.GetLYFD().SalesSearchDateFormat();
            var lfyfd = filterDate.GetLFYFD().SalesSearchDateFormat();
            var lyld = filterDate.GetLYLD().SalesSearchDateFormat();

            var selectQueryBuilder = new SelectQueryOptionBuilder();
            selectQueryBuilder.AddProperty(DataColumnDef.CustomerNoOrSoldToParty)
                                .AddProperty(DataColumnDef.CustomerName)
                                .AddProperty(DataColumnDef.Date)
                                .AddProperty(DataColumnDef.NetAmount);

            var dataLyMtd = (await _odataService.GetSalesData(selectQueryBuilder, lyfd, lyld,
                                    depots: model.Depots, territories: model.Territories, zones: model.Zones,
                                    classification: customerClassification)).ToList();

            var dataCyMtd = (await _odataService.GetSalesData(selectQueryBuilder, cyfd, cyld,
                                    depots: model.Depots, territories: model.Territories, zones: model.Zones,
                                    classification: customerClassification)).ToList();

            var dataLyYtd = (await _odataService.GetSalesData(selectQueryBuilder, lfyfd, lyld,
                                    depots: model.Depots, territories: model.Territories, zones: model.Zones,
                                    classification: customerClassification)).ToList();

            var dataCyYtd = (await _odataService.GetSalesData(selectQueryBuilder, cfyfd, cyld,
                                    depots: model.Depots, territories: model.Territories, zones: model.Zones,
                                    classification: customerClassification)).ToList();

            var result = new List<CategoryWiseDealerPerformanceResultModel>();

            if (model.PerformanceCategory == EnumDealerPerformanceCategory.TopPerformer || model.PerformanceCategory == EnumDealerPerformanceCategory.BottomPerformer)
            {
                var dataLyGroup = dataLyYtd.GroupBy(x => x.CustomerNoOrSoldToParty).Select(x =>
                                            new CategoryWiseDealerPerformanceResultModel()
                                            {
                                                CustomerNo = x.Key,
                                                CustomerName = x.FirstOrDefault()?.CustomerName ?? string.Empty,
                                                LYYTD = x.Sum(s => CustomConvertExtension.ObjectToDecimal(s.NetAmount))
                                            });

                var performerData = model.PerformanceCategory == EnumDealerPerformanceCategory.TopPerformer
                                        ? dataLyGroup.OrderByDescending(o => o.LYYTD).Take(customerCount)
                                        : dataLyGroup.OrderBy(o => o.LYYTD).Take(customerCount);

                var ranking = 1;

                foreach (var item in performerData)
                {
                    var res = new CategoryWiseDealerPerformanceResultModel();
                    res.Ranking = ranking++;
                    res.CustomerNo = item.CustomerNo;
                    res.CustomerName = item.CustomerName;
                    res.CYMTD = dataCyMtd.Where(f => f.CustomerNo == item.CustomerNo).Sum(x => CustomConvertExtension.ObjectToDecimal(x.NetAmount));
                    res.LYMTD = dataLyMtd.Where(f => f.CustomerNo == item.CustomerNo).Sum(x => CustomConvertExtension.ObjectToDecimal(x.NetAmount));
                    res.CYYTD = dataCyYtd.Where(f => f.CustomerNo == item.CustomerNo).Sum(x => CustomConvertExtension.ObjectToDecimal(x.NetAmount));
                    res.LYYTD = item.LYYTD;
                    res.GrowthMTD = _odataService.GetGrowth(res.LYMTD, res.CYMTD);
                    res.GrowthYTD = _odataService.GetGrowth(res.LYYTD, res.CYYTD);

                    result.Add(res);
                }
            }
            else if (model.PerformanceCategory == EnumDealerPerformanceCategory.NotPurchasedLastMonth)
            {
                // without sales last month
                var notPurchasedCyData = dataCyYtd.Where(x => !(x.Date.SalesResultDateFormat().Date >= notPurchasedFromDate.Date
                                                                && x.Date.SalesResultDateFormat().Date <= notPurchasedToDate.Date));

                // not sales only last month
                var notPurchasedCyGroupData = dataCyYtd.Where(x => !notPurchasedCyData.Any(y => y.CustomerNoOrSoldToParty == x.CustomerNoOrSoldToParty))
                                                .GroupBy(x => x.CustomerNoOrSoldToParty).Select(x =>
                                                new CategoryWiseDealerPerformanceResultModel()
                                                {
                                                    CustomerNo = x.Key,
                                                    CustomerName = x.FirstOrDefault()?.CustomerName ?? string.Empty,
                                                    CYYTD = x.Sum(s => CustomConvertExtension.ObjectToDecimal(s.NetAmount))
                                                })
                                                .OrderByDescending(x => x.CYYTD);

                var ranking = 1;

                foreach (var item in notPurchasedCyGroupData)
                {
                    var res = new CategoryWiseDealerPerformanceResultModel();
                    res.Ranking = ranking++;
                    res.CustomerNo = item.CustomerNo;
                    res.CustomerName = item.CustomerName;
                    res.LYMTD = dataLyMtd.Where(f => f.CustomerNo == item.CustomerNo).Sum(x => CustomConvertExtension.ObjectToDecimal(x.NetAmount));
                    res.CYMTD = dataCyMtd.Where(f => f.CustomerNo == item.CustomerNo).Sum(x => CustomConvertExtension.ObjectToDecimal(x.NetAmount));
                    res.LYYTD = dataLyYtd.Where(f => f.CustomerNo == item.CustomerNo).Sum(x => CustomConvertExtension.ObjectToDecimal(x.NetAmount));
                    res.CYYTD = item.CYYTD;
                    res.GrowthMTD = _odataService.GetGrowth(res.LYMTD, res.CYMTD);
                    res.GrowthYTD = _odataService.GetGrowth(res.LYYTD, res.CYYTD);

                    result.Add(res);
                }
            }

            return result;
        }

        public async Task<IList<ReportDealerPerformanceResultModel>> GetReportDealerPerformance(IList<string> dealerIds, DealerPerformanceReportType dealerPerformanceReportType)
        {
            var currentDate = DateTime.Now.AddMonths(-1);
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

        public async Task<IList<RptLastYearAppointDlerPerformanceSummaryResultModel>> GetReportLastYearAppointedDealerPerformanceSummary(LastYearAppointedDealerPerformanceSearchModel model, List<string> lastYearAppointedDealer)
        {
            var currentDate = new DateTime(model.Year, model.Month, 1);

            var cyfd = currentDate.GetCYFD().SalesSearchDateFormat();
            var cyld = currentDate.GetCYLD().SalesSearchDateFormat();

            var lyfd = currentDate.GetLYFD().SalesSearchDateFormat();
            var lyld = currentDate.GetLYLD().SalesSearchDateFormat();

            var lfyfd = currentDate.GetLFYFD().SalesSearchDateFormat();
            var cfyfd = currentDate.GetCFYFD().SalesSearchDateFormat();



            var dealerSelect = new SelectQueryOptionBuilder()
                .AddProperty(nameof(CustomerDataModel.CustomerNo))
                .AddProperty(nameof(CustomerDataModel.CreditControlArea))
                .AddProperty(nameof(CustomerDataModel.BusinessArea));

            var selectQueryBuilder = new SelectQueryOptionBuilder();
            selectQueryBuilder
                .AddProperty(DataColumnDef.NetAmount)
                .AddProperty(DataColumnDef.PlantOrBusinessArea)
                .AddProperty(DataColumnDef.CustomerNo);


            var dataCyMtd = (await _odataService.GetSalesData(selectQueryBuilder, cyfd, cyld, depots: model.Depots, territories: model.Territories, zones: model.Zones)).ToList();
            var dataLyMtd = (await _odataService.GetSalesData(selectQueryBuilder, lyfd, lyld, depots: model.Depots, territories: model.Territories, zones: model.Zones)).ToList();

            var dataLyYtd = (await _odataService.GetSalesData(selectQueryBuilder, lfyfd, lyld, depots: model.Depots, territories: model.Territories, zones: model.Zones)).ToList();
            var dataCyYtd = (await _odataService.GetSalesData(selectQueryBuilder, cfyfd, cyld, depots: model.Depots, territories: model.Territories, zones: model.Zones)).ToList();


            //var dataCyMtdTask = (_odataService.GetSalesData(selectQueryBuilder, cyfd, cyld, depots: model.Depots, territories: model.Territories, zones: model.Zones));
            //var dataLyMtdTask = (_odataService.GetSalesData(selectQueryBuilder, lyfd, lyld, depots: model.Depots, territories: model.Territories, zones: model.Zones));

            //var dataLyYtdTask = (_odataService.GetSalesData(selectQueryBuilder, lfyfd, lyld, depots: model.Depots, territories: model.Territories, zones: model.Zones));
            //var dataCyYtdTask = (_odataService.GetSalesData(selectQueryBuilder, cfyfd, cyld, depots: model.Depots, territories: model.Territories, zones: model.Zones));


            //await TaskExt.WhenAll(dataCyMtdTask, dataLyMtdTask, dataLyYtdTask, dataCyYtdTask);

            //var dataCyMtd = await dataCyMtdTask;
            //var dataLyMtd = await dataLyMtdTask;

            //var dataLyYtd = await dataLyYtdTask;
            //var dataCyYtd = await dataCyYtdTask;

            Func<SalesDataModel, SalesDataModel> concatSelectFunc = x => new SalesDataModel
            {
                NetAmount = x.NetAmount,
                PlantOrBusinessArea = x.PlantOrBusinessArea,
                CustomerNo = x.CustomerNo
            };

            Func<SalesDataModel, SalesDataModel> selectFunc = x => new SalesDataModel
            {
                NetAmount = x.NetAmount,
                PlantOrBusinessArea = x.PlantOrBusinessArea
            };

            Func<SalesDataModel, decimal> calcFunc = x => CustomConvertExtension.ObjectToDecimal(x.NetAmount);
            Func<SalesDataModel, SalesDataModel, bool> predicateFunc = (x, val) => x.PlantOrBusinessArea == val.PlantOrBusinessArea;

            var concatAllList = dataLyMtd.Select(concatSelectFunc)
                .Concat(dataCyMtd.Select(concatSelectFunc))
                .Concat(dataLyYtd.Select(concatSelectFunc))
                .Concat(dataCyYtd.Select(concatSelectFunc))
                .GroupBy(p => new { p.PlantOrBusinessArea, p.CustomerNo })
                .Select(g => g.First());


            concatAllList = concatAllList.Where(x => lastYearAppointedDealer.Contains(x.CustomerNo)).ToList();

            concatAllList = concatAllList.Select(selectFunc).GroupBy(p => p.PlantOrBusinessArea).Select(g => g.First()).ToList();

            var result = new List<RptLastYearAppointDlerPerformanceSummaryResultModel>();

            var dealer = await _odataService.GetCustomerData(dealerSelect, depots: model.Depots, territories: model.Territories,
                zones: model.Zones, channel: ConstantsValue.DistrbutionChannelDealer);


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

                res.DepotCode = item.PlantOrBusinessArea;
                res.GrowthMTD = _odataService.GetGrowth(res.LYMTD, res.CYMTD);
                res.GrowthYTD = _odataService.GetGrowth(res.LYYTD, res.CYYTD);
                res.NumberOfDealer = dealer.Count(x => x.BusinessArea == item.PlantOrBusinessArea);
                result.Add(res);
            }

            return result;
        }
        public async Task<IList<RptLastYearAppointDlrPerformanceDetailResultModel>> GetReportLastYearAppointedDealerPerformanceDetail(LastYearAppointedDealerPerformanceSearchModel model, List<string> lastYearAppointedDealer)
        {
            var currentDate = new DateTime(model.Year, model.Month, 1);

            var cyfd = currentDate.GetCYFD().SalesSearchDateFormat();
            var cyld = currentDate.GetCYLD().SalesSearchDateFormat();

            var lyfd = currentDate.GetLYFD().SalesSearchDateFormat();
            var lyld = currentDate.GetLYLD().SalesSearchDateFormat();

            var lfyfd = currentDate.GetLFYFD().SalesSearchDateFormat();

            var cfyfd = currentDate.GetCFYFD().SalesSearchDateFormat();

            var selectQueryBuilder = new SelectQueryOptionBuilder();
            selectQueryBuilder
                .AddProperty(DataColumnDef.PlantOrBusinessArea)
                .AddProperty(DataColumnDef.Territory)
                .AddProperty(DataColumnDef.Zone)
                .AddProperty(DataColumnDef.CustomerNo)
                .AddProperty(DataColumnDef.NetAmount)
                .AddProperty(DataColumnDef.CustomerName);


            var dataCyMtd = (await _odataService.GetSalesData(selectQueryBuilder, cyfd, cyld, depots: model.Depots, territories: model.Territories, zones: model.Zones)).ToList();
            var dataLyMtd = (await _odataService.GetSalesData(selectQueryBuilder, lyfd, lyld, depots: model.Depots, territories: model.Territories, zones: model.Zones)).ToList();

            var dataLyYtd = (await _odataService.GetSalesData(selectQueryBuilder, lfyfd, lyld, depots: model.Depots, territories: model.Territories, zones: model.Zones)).ToList();
            var dataCyYtd = (await _odataService.GetSalesData(selectQueryBuilder, cfyfd, cyld, depots: model.Depots, territories: model.Territories, zones: model.Zones)).ToList();


            Func<SalesDataModel, SalesDataModel> selectFunc = x => new SalesDataModel
            {
                NetAmount = x.NetAmount,
                PlantOrBusinessArea = x.PlantOrBusinessArea,
                Territory = x.Territory,
                Zone = x.Zone,
                CustomerName = x.CustomerName,
                CustomerNo = x.CustomerNo
            };

            var concatAllList = dataLyMtd.Select(selectFunc)
                .Concat(dataCyMtd.Select(selectFunc))
                .GroupBy(p => new { p.PlantOrBusinessArea, p.Territory, p.Zone, p.CustomerName, p.CustomerNo })
                .Select(g => g.First());

            concatAllList = concatAllList.Where(x => lastYearAppointedDealer.Contains(x.CustomerNo)).ToList();

            var result = new List<RptLastYearAppointDlrPerformanceDetailResultModel>();
            Func<SalesDataModel, SalesDataModel, bool> predicateFunc = (x, val) => x.PlantOrBusinessArea == val.PlantOrBusinessArea && x.Territory == val.Territory
                && x.CustomerNo == val.CustomerNo && x.Zone == val.Zone;
            Func<SalesDataModel, decimal> calcFunc = x => CustomConvertExtension.ObjectToDecimal(x.NetAmount);

            foreach (var item in concatAllList)
            {

                var res = new RptLastYearAppointDlrPerformanceDetailResultModel
                {
                    DepotCode = item.PlantOrBusinessArea,
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
        public async Task<IList<ReportClubSupremePerformance>> GetReportClubSupremePerformance(ClubSupremePerformanceSearchModel model, List<CustNClubMappingVm> clubSupremeDealers, ClubSupremeReportType reportType)
        {
            var currentDate = new DateTime(model.Year, model.Month, 1);

            var cyfd = currentDate.GetCYFD().SalesSearchDateFormat();
            var cyld = currentDate.GetCYLD().SalesSearchDateFormat();

            var lyfd = currentDate.GetLYFD().SalesSearchDateFormat();
            var lyld = currentDate.GetLYLD().SalesSearchDateFormat();

            var lfyfd = currentDate.GetLFYFD().SalesSearchDateFormat();
            var cfyfd = currentDate.GetCFYFD().SalesSearchDateFormat();

            var selectQueryBuilder = new SelectQueryOptionBuilder();

            Func<SalesDataModel, SalesDataModel> selectFunc;

            var dealerSelect = new SelectQueryOptionBuilder()
                .AddProperty(nameof(CustomerDataModel.CustomerNo));

            IList<CustomerDataModel> dealer = new List<CustomerDataModel>();

            if (reportType == ClubSupremeReportType.Summary)
            {
                selectQueryBuilder
                    .AddProperty(DataColumnDef.CustomerNo)
                    .AddProperty(DataColumnDef.NetAmount);

                selectFunc = x => new SalesDataModel
                {
                    NetAmount = x.NetAmount,
                    CustomerName = x.CustomerName,
                    CustomerNo = x.CustomerNo
                };

                dealer = await _odataService.GetCustomerData(dealerSelect, depots: model.Depots, territories: model.Territories,
                   zones: model.Zones, channel: ConstantsValue.DistrbutionChannelDealer);

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

                selectFunc = x => new SalesDataModel
                {
                    NetAmount = x.NetAmount,
                    PlantOrBusinessArea = x.PlantOrBusinessArea,
                    Territory = x.Territory,
                    Zone = x.Zone,
                    CustomerName = x.CustomerName,
                    CustomerNo = x.CustomerNo
                };

            }

            var dataCyMtd = (await _odataService.GetSalesData(selectQueryBuilder, cyfd, cyld, depots: model.Depots, territories: model.Territories, zones: model.Zones)).ToList();
            var dataLyMtd = (await _odataService.GetSalesData(selectQueryBuilder, lyfd, lyld, depots: model.Depots, territories: model.Territories, zones: model.Zones)).ToList();
            var dataLyYtd = (await _odataService.GetSalesData(selectQueryBuilder, lfyfd, lyld, depots: model.Depots, territories: model.Territories, zones: model.Zones)).ToList();
            var dataCyYtd = (await _odataService.GetSalesData(selectQueryBuilder, cfyfd, cyld, depots: model.Depots, territories: model.Territories, zones: model.Zones)).ToList();


            var concatAllList = dataLyMtd.Select(selectFunc)
                .Concat(dataCyMtd.Select(selectFunc))
                .GroupBy(p => new { p.PlantOrBusinessArea, p.Territory, p.Zone, p.CustomerName, p.CustomerNo })
                .Select(g => g.First());

            concatAllList = concatAllList.Where(x => clubSupremeDealers.Select(y => y.CustomerNo).Contains(x.CustomerNo)).ToList();

            var result = new List<ReportClubSupremePerformance>();

            Func<SalesDataModel, SalesDataModel, bool> predicateFunc = (x, val) => x.PlantOrBusinessArea == val.PlantOrBusinessArea && x.Territory == val.Territory
                && x.CustomerNo == val.CustomerNo && x.Zone == val.Zone;
            Func<SalesDataModel, decimal> calcFunc = x => CustomConvertExtension.ObjectToDecimal(x.NetAmount);

            foreach (var item in concatAllList)
            {
                var res = reportType == ClubSupremeReportType.Detail
                    ? (ReportClubSupremePerformance)new ReportClubSupremePerformanceDetail
                    {
                        DepotCode = item.PlantOrBusinessArea,
                        Territory = item.Territory,
                        Zone = item.Zone,
                        CustomerNo = item.CustomerNo,
                        CustomerName = item.CustomerName
                    }
                    : new ReportClubSupremePerformanceSummary()
                    {
                        NumberOfDealer = dealer.FirstOrDefault(x => x.CustomerNo == item.CustomerNo) == null ? 0 : 1
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

        public async Task<IList<KPIStrikRateKPIReportResultModel>> GetKPIStrikeRateKPIReport(int year, int month, string depot, List<string> salesGroups, List<string> territories, List<string> zones, List<string> brands)
        {
            var currentDate = new DateTime(year, month, 01);
            var fromDate = currentDate.GetCYFD().DateFormat();
            var toDate = currentDate.GetCYLD().DateFormat();

            var selectQueryBuilder = new SelectQueryOptionBuilder();
            selectQueryBuilder.AddProperty(DataColumnDef.CustomerNoOrSoldToParty)
                                .AddProperty(DataColumnDef.InvoiceNoOrBillNo)
                                .AddProperty(DataColumnDef.Date)
                                .AddProperty(DataColumnDef.NetAmount)
                                .AddProperty(DataColumnDef.CustomerClassification)
                                .AddProperty(DataColumnDef.MatarialGroupOrBrand);

            var data = (await _odataService.GetSalesDataByMultipleArea(selectQueryBuilder, fromDate, toDate, depot, salesGroups: salesGroups, territories: territories, zones: zones, brands: brands)).ToList();

            var result = data.Select(x =>
                                new KPIStrikRateKPIReportResultModel()
                                {
                                    CustomerNo = x.CustomerNoOrSoldToParty,
                                    InvoiceNoOrBillNo = x.InvoiceNoOrBillNo,
                                    DateTime = x.Date.DateFormatDate(),
                                    Date = x.Date.ReturnDateFormatDate(),
                                    NetAmount = CustomConvertExtension.ObjectToDecimal(x.NetAmount),
                                    CustomerClassification = x.CustomerClassification,
                                    MatarialGroupOrBrand = x.MatarialGroupOrBrand,
                                }).ToList();

            return result;
        }

        public async Task<IList<KPIBusinessAnalysisKPIReportResultModel>> GetKPIBusinessAnalysisKPIReport(int year, int month, string depot, List<string> salesGroups, List<string> territories, List<string> zones)
        {
            var currentDate = new DateTime(year, month, 01);
            var fromDate = currentDate.GetCYFD().DateFormat();
            var toDate = currentDate.GetCYLD().DateFormat();

            var selectQueryBuilder = new SelectQueryOptionBuilder();
            selectQueryBuilder.AddProperty(DataColumnDef.InvoiceNoOrBillNo)
                                .AddProperty(DataColumnDef.CustomerNoOrSoldToParty)
                                .AddProperty(DataColumnDef.Date)
                                .AddProperty(DataColumnDef.NetAmount);

            var data = (await _odataService.GetSalesDataByMultipleArea(selectQueryBuilder, fromDate, toDate, depot, salesGroups: salesGroups, territories: territories, zones: zones)).ToList();

            var result = data.Select(x =>
                                new KPIBusinessAnalysisKPIReportResultModel()
                                {
                                    CustomerNo = x.CustomerNoOrSoldToParty,
                                }).ToList();

            return result;
        }

        public async Task<int> NoOfBillingDealer(AreaSearchCommonModel area, string division = "", string channel = "")
        {
            var currentDate = DateTime.Now;
            var fromDate = currentDate.SalesSearchDateFormat();
            var toDate = currentDate.SalesSearchDateFormat();

            var selectQueryBuilder = new SelectQueryOptionBuilder();
            selectQueryBuilder.AddProperty(DataColumnDef.CustomerNoOrSoldToParty)
                            .AddProperty(DataColumnDef.NetAmount);

            var data = await _odataService.GetSalesData(selectQueryBuilder, fromDate, toDate,
                                                    depots: area.Depots, salesOffices: area.SalesOffices, salesGroups: area.SalesGroups,
                                                    territories: area.Territories, zones: area.Zones,
                                                    division: division, channel: channel);

            var result = data.Select(x => x.CustomerNoOrSoldToParty).Distinct().Count();

            return result;
        }

        public async Task<IList<TodaysInvoiceValueResultModel>> GetTodaysActivityInvoiceValue(TodaysInvoiceValueSearchModel model, AreaSearchCommonModel area)
        {
            var currentDate = DateTime.Now;
            var fromDate = currentDate.SalesSearchDateFormat();
            var toDate = currentDate.SalesSearchDateFormat();

            var selectQueryBuilder = new SelectQueryOptionBuilder();
            selectQueryBuilder.AddProperty(DataColumnDef.CustomerNoOrSoldToParty)
                                .AddProperty(DataColumnDef.CustomerName)
                                .AddProperty(DataColumnDef.InvoiceNoOrBillNo)
                                .AddProperty(DataColumnDef.NetAmount);

            var data = await _odataService.GetSalesData(selectQueryBuilder, fromDate, toDate,
                                                    depots: area.Depots, salesOffices: area.SalesOffices, salesGroups: area.SalesGroups,
                                                    territories: area.Territories, zones: area.Zones,
                                                    division: model.Division);

            var result = data.Select(x =>
                                new TodaysInvoiceValueResultModel()
                                {
                                    InvoiceNoOrBillNo = x.InvoiceNoOrBillNo,
                                    CustomerNo = x.CustomerNoOrSoldToParty,
                                    CustomerName = x.CustomerName,
                                    NetAmount = CustomConvertExtension.ObjectToDecimal(x.NetAmount)
                                }).ToList();

            return result;
        }

        public async Task<IList<SalesDataModel>> GetMTDActual(AppAreaSearchCommonModel area, DateTime fromDate, DateTime toDate,
            string division, EnumVolumeOrValue volumeOrValue, EnumBrandCategory? category, EnumBrandType? type)
        {
            var fromDateStr = fromDate.SalesSearchDateFormat();
            var toDateStr = toDate.SalesSearchDateFormat();

            var selectQueryBuilder = new SelectQueryOptionBuilder();
            selectQueryBuilder.AddProperty(DataColumnDef.PlantOrBusinessArea)
                                .AddProperty(DataColumnDef.Date)
                                .AddProperty(volumeOrValue == EnumVolumeOrValue.Volume
                                            ? DataColumnDef.Volume
                                            : DataColumnDef.NetAmount);

            if (type.HasValue) selectQueryBuilder.AddProperty(DataColumnDef.MatarialGroupOrBrand);

            var brands = new List<string>();

            if (category.HasValue && category.Value == EnumBrandCategory.Liquid)
            {
                brands = (await _odataBrandService.GetLiquidBrandCodesAsync()).ToList();
            }
            else if (category.HasValue && category.Value == EnumBrandCategory.Powder)
            {
                brands = (await _odataBrandService.GetPowderBrandCodesAsync()).ToList();
            }

            if (type.HasValue && type.Value == EnumBrandType.MTSBrands)
            {
                brands = (await _odataBrandService.GetMTSBrandCodesAsync()).ToList();
            }

            var result = await _odataService.GetSalesData(selectQueryBuilder, fromDateStr, toDateStr,
                            depots: area.Depots, territories: area.Territories, zones: area.Zones,
                            brands: brands, division: division);

            return result;
        }
    }
}

