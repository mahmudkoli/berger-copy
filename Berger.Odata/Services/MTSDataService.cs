using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Berger.Common.Extensions;
using Berger.Data.MsfaEntity.SAPReports;
using Berger.Odata.Common;
using Berger.Odata.Extensions;
using Berger.Odata.Model;

namespace Berger.Odata.Services
{
    public class MTSDataService : IMTSDataService
    {
        private readonly IODataService _odataService;
        private readonly IODataBrandService _odataBrandService;
        private readonly ISalesDataService _salesDataService;
        public MTSDataService(
            IODataService odataService,
            IODataBrandService odataBrandService, ISalesDataService salesDataService)
        {
            _odataService = odataService;
            _odataBrandService = odataBrandService;
            _salesDataService = salesDataService;
        }

        public async Task<IList<MTSResultModel>> GetMTSUpdate(MTSSearchModelBase model)
        {
            var currentDateStr = $"{string.Format("{0:0000}", model.Year)}.{string.Format("{0:00}", model.Month)}";
            var currentDate = (new DateTime(model.Year, model.Month, 1));
            var fromDate = currentDate.GetCYFD().DateFormat();//.SalesSearchDateFormat();
            var toDate = currentDate.GetCYLD().DateFormat();//.SalesSearchDateFormat();
            var mtsBrandCodes = new List<string>();

            var selectQueryBuilder = new SelectQueryOptionBuilder();
            selectQueryBuilder.AddProperty(DataColumnDef.MTS_CustomerNo)
                                .AddProperty(DataColumnDef.MTS_CustomerName)
                                .AddProperty(DataColumnDef.MTS_MatarialGroupOrBrand)
                                .AddProperty(DataColumnDef.MTS_TargetVolume);

            mtsBrandCodes = (await _odataBrandService.GetMTSBrandCodesAsync()).ToList();

            var dataTarget = (await _odataService.GetMTSDataByCustomerAndDate(selectQueryBuilder, model.CustomerNo, currentDateStr, mtsBrandCodes)).ToList();


            var dataActual = await _salesDataService.GetCustomerWiseRevenue(x => new CustomerPerformanceReport
            {
                Brand = x.Brand,
                Volume = x.Volume,
                CustomerName = x.CustomerName,
                CustomerNo = x.CustomerNo,
            }, model.CustomerNo, fromDate, toDate, "-1", mtsBrandCodes);


            //var dataActual = await GetSalesDataValueVolume(model.CustomerNo, fromDate, toDate, mtsBrandCodes);

            var result = new List<MTSResultModel>();

            var brandFamilyInfos = (await _odataBrandService.GetBrandFamilyInfosAsync()).ToList();

            foreach (var item in dataActual)
            {
                item.Brand = brandFamilyInfos.FirstOrDefault(x => x.MatarialGroupOrBrand == item.Brand)?
                                                    .MatarialGroupOrBrandFamily ?? item.Brand;
            }

            foreach (var item in dataTarget)
            {
                item.MatarialGroupOrBrand = brandFamilyInfos.FirstOrDefault(x => x.MatarialGroupOrBrand == item.MatarialGroupOrBrand)?
                                                    .MatarialGroupOrBrandFamily ?? item.MatarialGroupOrBrand;
            }

            var brandCodes = dataTarget.Select(x => x.MatarialGroupOrBrand)
                                .Concat(dataActual.Select(x => x.Brand))
                                    .Distinct().ToList();

            foreach (var brandCode in brandCodes)
            {
                var res = new MTSResultModel();

                if (dataTarget.Any(x => x.MatarialGroupOrBrand == brandCode))
                {
                    var volTar = dataTarget.Where(x => x.MatarialGroupOrBrand == brandCode).Sum(x => CustomConvertExtension.ObjectToDecimal(x.TargetVolume));
                    var brandNameTar = dataTarget.FirstOrDefault(x => x.MatarialGroupOrBrand == brandCode).MatarialGroupOrBrand;
                    var custNoTar = dataTarget.FirstOrDefault(x => x.MatarialGroupOrBrand == brandCode).CustomerNo;
                    var custNameTar = dataTarget.FirstOrDefault(x => x.MatarialGroupOrBrand == brandCode).CustomerName;

                    res.MatarialGroupOrBrand = string.IsNullOrEmpty(res.MatarialGroupOrBrand) ? brandNameTar : res.MatarialGroupOrBrand;
                    res.CustomerNo = string.IsNullOrEmpty(res.CustomerNo) ? custNoTar : res.CustomerNo;
                    res.CustomerName = string.IsNullOrEmpty(res.CustomerName) ? custNameTar : res.CustomerName;
                    res.TargetVolume = volTar;
                }

                if (dataActual.Any(x => x.Brand == brandCode))
                {
                    var volAct = dataActual.Where(x => x.Brand == brandCode).Sum(x => x.Volume);
                    var brandNameAct = dataActual.FirstOrDefault(x => x.Brand == brandCode).Brand;
                    var custNoAct = dataActual.FirstOrDefault(x => x.Brand == brandCode).CustomerNo;
                    var custNameAct = dataActual.FirstOrDefault(x => x.Brand == brandCode).CustomerName;

                    res.MatarialGroupOrBrand = string.IsNullOrEmpty(res.MatarialGroupOrBrand) ? brandNameAct : res.MatarialGroupOrBrand;
                    res.CustomerNo = string.IsNullOrEmpty(res.CustomerNo) ? custNoAct : res.CustomerNo;
                    res.CustomerName = string.IsNullOrEmpty(res.CustomerName) ? custNameAct : res.CustomerName;
                    res.ActualVolume = volAct;
                }

                res.DifferenceVolume = res.TargetVolume - res.ActualVolume;

                result.Add(res);
            }

            #region get brand data
            if (result.Any())
            {
                foreach (var item in result)
                {
                    var brandFamilyData = brandFamilyInfos.Where(x => x.MatarialGroupOrBrandFamily == item.MatarialGroupOrBrand);
                    item.MatarialGroupOrBrand = string.Join(", ", brandFamilyData.Select(x => $"{x.MatarialGroupOrBrandName} ({x.MatarialGroupOrBrand})"));
                }
            }
            #endregion

            return result;
        }

        public async Task<IList<PerformanceResultModel>> GetPremiumBrandPerformance(MTSSearchModel model)
        {
            var currentDateStr = $"{string.Format("{0:0000}", model.Year)}.{string.Format("{0:00}", model.Month)}";
            var currentDate = (new DateTime(model.Year, model.Month, 1));
            var lyfd = currentDate.GetLYFD().DateFormat();//.SalesSearchDateFormat();
            var lyld = currentDate.GetLYLD().DateFormat();//.SalesSearchDateFormat();
            var cyfd = currentDate.GetCYFD().DateFormat();//.SalesSearchDateFormat();
            var cyld = currentDate.GetCYLD().DateFormat();//.SalesSearchDateFormat();
            List<string> premiumBrandCodes;

            var selectQueryBuilder = new SelectQueryOptionBuilder();
            selectQueryBuilder.AddProperty(DataColumnDef.MTS_CustomerNo)
                                .AddProperty(DataColumnDef.MTS_CustomerName)
                                .AddProperty(DataColumnDef.MTS_MatarialGroupOrBrand)
                                .AddProperty(DataColumnDef.MTS_TargetVolume)
                                .AddProperty(DataColumnDef.MTS_TargetValue);

            premiumBrandCodes = (await _odataBrandService.GetPremiumBrandCodesAsync()).ToList();

            var dataTargetCy = (await _odataService.GetMTSDataByCustomerAndDate(selectQueryBuilder, model.CustomerNo, currentDateStr, premiumBrandCodes)).ToList();

            var dataActualLy = await _salesDataService.GetCustomerWiseRevenue(x => new CustomerPerformanceReport()
            {
                Brand = x.Brand,
                Value = x.Value,
                Volume = x.Volume
            }, model.CustomerNo, lyfd, lyld, "-1", premiumBrandCodes);

            var dataActualCy = await _salesDataService.GetCustomerWiseRevenue(x => new CustomerPerformanceReport()
            {
                Brand = x.Brand,
                Value = x.Value,
                Volume = x.Volume
            }, model.CustomerNo, cyfd, cyld, "-1", premiumBrandCodes);


            //var dataActualLy = await GetSalesDataValueVolume(model.CustomerNo, lyfd, lyld, premiumBrandCodes);

            // var dataActualCy = await GetSalesDataValueVolume(model.CustomerNo, cyfd, cyld, premiumBrandCodes);

            var result = new List<PerformanceResultModel>();

            var brandCodes = dataTargetCy.Select(x => x.MatarialGroupOrBrand)
                                .Concat(dataActualLy.Select(x => x.Brand))
                                    .Concat(dataActualCy.Select(x => x.Brand))
                                            .Distinct().ToList();


            Func<CustomerPerformanceReport, decimal> sumFunc
                = x => model.VolumeOrValue == EnumVolumeOrValue.Value ? CustomConvertExtension.ObjectToDecimal(x.Value) //revenue
                : CustomConvertExtension.ObjectToDecimal(x.Volume); // volume

            Func<MTSDataModel, decimal> funcTarget = x =>
                model.VolumeOrValue == EnumVolumeOrValue.Value
                    ? CustomConvertExtension.ObjectToDecimal(x.TargetValue)
                    : CustomConvertExtension.ObjectToDecimal(x.TargetVolume);



            foreach (var brandCode in brandCodes)
            {
                var res = new PerformanceResultModel();

                if (dataTargetCy.Any(x => x.MatarialGroupOrBrand == brandCode))
                {
                    var volTarCy = dataTargetCy.Where(x => x.MatarialGroupOrBrand == brandCode).Sum(x => funcTarget(x));

                    var dataTargetSingle = dataTargetCy.FirstOrDefault(x => x.MatarialGroupOrBrand == brandCode);

                    var brandNameTarCy = dataTargetSingle.MatarialGroupOrBrand;
                    //var custNoTarCy = dataTargetSingle.CustomerNo;
                    //var custNameTarCy = dataTargetSingle.CustomerName;

                    res.MatarialGroupOrBrand = string.IsNullOrEmpty(res.MatarialGroupOrBrand) ? brandNameTarCy : res.MatarialGroupOrBrand;
                    res.CMTarget = volTarCy;
                }

                if (dataActualCy.Any(x => x.Brand == brandCode))
                {
                    var volActCy = dataActualCy.Where(x => x.Brand == brandCode).Sum(x => sumFunc(x));
                    var brandNameActCy = dataActualCy.FirstOrDefault(x => x.Brand == brandCode).Brand;
                    res.MatarialGroupOrBrand = string.IsNullOrEmpty(res.MatarialGroupOrBrand) ? brandNameActCy : res.MatarialGroupOrBrand;
                    res.CMActual = volActCy;
                }

                if (dataActualLy.Any(x => x.Brand == brandCode))
                {
                    var volActLy = dataActualLy.Where(x => x.Brand == brandCode).Sum(x => sumFunc(x));
                    var brandNameActLy = dataActualLy.FirstOrDefault(x => x.Brand == brandCode).Brand;
                    res.MatarialGroupOrBrand = string.IsNullOrEmpty(res.MatarialGroupOrBrand) ? brandNameActLy : res.MatarialGroupOrBrand;
                    res.LYMTD = volActLy;
                }
                res.RemainingTarget = res.CMTarget - res.CMActual;
                res.TillDateGrowth = _odataService.GetTillDateGrowth(res.LYMTD, res.CMActual,
                                        currentDate.GetCYLD().Day, currentDate.GetCYLCD().Day);

                result.Add(res);
            }

            #region get brand data
            if (result.Any())
            {
                var brands = result.Select(x => x.MatarialGroupOrBrand).Distinct().ToList();

                var allBrandFamilyData = await _odataBrandService.GetBrandFamilyInfosAsync(x => brands.Contains(x.MatarialGroupOrBrand));

                //var allBrandFamilyData = (await _odataService.GetBrandFamilyDataByBrands(brands)).ToList();

                foreach (var item in result)
                {
                    var brandFamilyData = allBrandFamilyData.FirstOrDefault(x => x.MatarialGroupOrBrand == item.MatarialGroupOrBrand);
                    if (brandFamilyData != null)
                    {
                        item.MatarialGroupOrBrand = $"{brandFamilyData.MatarialGroupOrBrandName} ({brandFamilyData.MatarialGroupOrBrand})";
                    }
                }
            }
            #endregion

            return result;
        }

        public async Task<IList<ValueTargetResultModel>> GetMonthlyValueTarget(MTSSearchModelBase model)
        {
            var currentDateStr = $"{string.Format("{0:0000}", model.Year)}.{string.Format("{0:00}", model.Month)}";
            var currentDate = (new DateTime(model.Year, model.Month, 1));
            var fromDate = currentDate.GetCYFD().DateFormat();
            var toDate = currentDate.GetCYLD().DateFormat();

            var selectQueryBuilder = new SelectQueryOptionBuilder();
            selectQueryBuilder.AddProperty(DataColumnDef.MTS_CustomerNo)
                                .AddProperty(DataColumnDef.MTS_CustomerName)
                                .AddProperty(DataColumnDef.MTS_MatarialGroupOrBrand)
                                .AddProperty(DataColumnDef.MTS_TargetValue);

            var dataTarget = (await _odataService.GetMTSDataByCustomerAndDate(selectQueryBuilder, model.CustomerNo, currentDateStr)).ToList();

            var dataActual = await _salesDataService.GetCustomerWiseRevenue(x => new CustomerPerformanceReport()
            {
                Brand = x.Brand,
                Value = x.Value
            }, model.CustomerNo, fromDate, toDate, "-1");

            //  var dataActual = await GetSalesDataValueVolume(model.CustomerNo, fromDate, toDate);

            var result = new List<ValueTargetResultModel>();

            var brandCodes = dataTarget.Select(x => x.MatarialGroupOrBrand)
                                .Concat(dataActual.Select(x => x.Brand))
                                    .Distinct().ToList();

            foreach (var brandCode in brandCodes)
            {
                var res = new ValueTargetResultModel();

                if (dataTarget.Any(x => x.MatarialGroupOrBrand == brandCode))
                {
                    var volTar = dataTarget.Where(x => x.MatarialGroupOrBrand == brandCode).Sum(x => CustomConvertExtension.ObjectToDecimal(x.TargetValue));
                    //var brandNameTar = dataTarget.FirstOrDefault(x => x.MatarialGroupOrBrand == brandCode).MatarialGroupOrBrand;
                    //var custNoTar = dataTarget.FirstOrDefault(x => x.MatarialGroupOrBrand == brandCode).CustomerNo;
                    //var custNameTar = dataTarget.FirstOrDefault(x => x.MatarialGroupOrBrand == brandCode).CustomerName;

                    //res.MatarialGroupOrBrand = string.IsNullOrEmpty(res.MatarialGroupOrBrand) ? brandNameTar : res.MatarialGroupOrBrand;
                    // res.CustomerNo = string.IsNullOrEmpty(res.CustomerNo) ? custNoTar : res.CustomerNo;
                    // res.CustomerName = string.IsNullOrEmpty(res.CustomerName) ? custNameTar : res.CustomerName;
                    res.TargetValue = volTar;
                }

                if (dataActual.Any(x => x.Brand == brandCode))
                {
                    var volAct = dataActual.Where(x => x.Brand == brandCode).Sum(x => x.Value);
                    // var brandNameAct = dataActual.FirstOrDefault(x => x.MatarialGroupOrBrand == brandCode).MatarialGroupOrBrand;
                    //var custNoAct = dataActual.FirstOrDefault(x => x.MatarialGroupOrBrand == brandCode).CustomerNo;
                    //var custNameAct = dataActual.FirstOrDefault(x => x.MatarialGroupOrBrand == brandCode).CustomerName;

                    //res.MatarialGroupOrBrand = string.IsNullOrEmpty(res.MatarialGroupOrBrand) ? brandNameAct : res.MatarialGroupOrBrand;
                    //  res.CustomerNo = string.IsNullOrEmpty(res.CustomerNo) ? custNoAct : res.CustomerNo;
                    // res.CustomerName = string.IsNullOrEmpty(res.CustomerName) ? custNameAct : res.CustomerName;
                    res.ActualValue = volAct;
                }

                res.DifferenceValue = res.TargetValue - res.ActualValue;

                result.Add(res);
            }

            var returnResult = new List<ValueTargetResultModel>();
            if (result.Any())
            {
                var res = new ValueTargetResultModel();
                //res.CustomerNo = result.FirstOrDefault().CustomerNo;
                //res.CustomerName = result.FirstOrDefault().CustomerName;
                res.TargetValue = result.Sum(x => x.TargetValue);
                res.ActualValue = result.Sum(x => x.ActualValue);
                res.DifferenceValue = res.TargetValue - res.ActualValue;
                returnResult.Add(res);
            }

            return returnResult;
        }

        //private async Task<IList<(string MatarialGroupOrBrand, string CustomerNo, string CustomerName, decimal ActualValue, decimal ActualVolume)>> GetSalesDataValueVolume(string customerNo, string fromDate, string toDate, List<string> brands = null)
        //{
        //    var selectQueryBuilder = new SelectQueryOptionBuilder();
        //    selectQueryBuilder.AddProperty(DataColumnDef.CustomerNo)
        //                        .AddProperty(DataColumnDef.CustomerName)
        //                        .AddProperty(DataColumnDef.MatarialGroupOrBrand)
        //                        .AddProperty(DataColumnDef.NetAmount)
        //                        .AddProperty(DataColumnDef.Volume);

        //    var data = (await _odataService.GetSalesDataByCustomerAndDivision(selectQueryBuilder, customerNo, fromDate, toDate, brands: brands)).ToList();

        //    var result = data.GroupBy(x => x.MatarialGroupOrBrand).Select(x =>
        //                                                            (
        //                                                                MatarialGroupOrBrand: x.Key,
        //                                                                CustomerNo: x.FirstOrDefault().CustomerNo,
        //                                                                CustomerName: x.FirstOrDefault().CustomerName,
        //                                                                ActualValue: x.Sum(s => CustomConvertExtension.ObjectToDecimal(s.NetAmount)),
        //                                                                ActualVolume: x.Sum(s => CustomConvertExtension.ObjectToDecimal(s.Volume))
        //                                                            )).ToList();

        //    return result;
        //}

        public async Task<IList<MTSDataModel>> GetMTDTarget(AppAreaSearchCommonModel area, DateTime fromDate, DateTime toDate,
            string division, EnumVolumeOrValue volumeOrValue, EnumBrandCategory? category, EnumBrandType? type)
        {
            var fromDateStr = fromDate.MTSSearchDateFormat();
            var toDateStr = toDate.MTSSearchDateFormat();

            var selectQueryBuilder = new SelectQueryOptionBuilder();
            selectQueryBuilder.AddProperty(DataColumnDef.MTS_PlantOrBusinessArea)
                                .AddProperty(DataColumnDef.MTS_Date)
                                .AddProperty(volumeOrValue == EnumVolumeOrValue.Volume
                                            ? DataColumnDef.MTS_TargetVolume
                                            : DataColumnDef.MTS_TargetValue);

            if (type.HasValue) selectQueryBuilder.AddProperty(DataColumnDef.MTS_MatarialGroupOrBrand);

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

            var result = await _odataService.GetMTSData(selectQueryBuilder, fromDateStr, toDateStr,
                            depots: area.Depots, territories: area.Territories, zones: area.Zones,
                            brands: brands, division: division);

            return result;
        }
    }
}

