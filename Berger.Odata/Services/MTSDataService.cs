using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Berger.Common.Extensions;
using Berger.Common.HttpClient;
using Berger.Common.JSONParser;
using Berger.Common.Model;
using Berger.Odata.Common;
using Berger.Odata.Extensions;
using Berger.Odata.Model;
using Microsoft.Extensions.Options;

namespace Berger.Odata.Services
{
    public class MTSDataService : IMTSDataService
    {
        private readonly IODataService _odataService;
        private readonly IODataBrandService _odataBrandService;

        public MTSDataService(
            IODataService odataService,
            IODataBrandService odataBrandService
            )
        {
            _odataService = odataService;
            _odataBrandService = odataBrandService;
        }

        public async Task<IList<MTSResultModel>> GetMTSBrandsVolume(MTSSearchModel model)
        {
            var currentDateStr = $"{string.Format("{0:0000}", model.Year)}.{string.Format("{0:00}", model.Month)}";
            var currentDate = (new DateTime(model.Year, model.Month, 1));
            var fromDate = currentDate.GetCYFD().DateFormat();
            var toDate = currentDate.GetCYLD().DateFormat();
            var mtsBrandCodes = new List<string>();

            var selectQueryBuilder = new SelectQueryOptionBuilder();
            selectQueryBuilder.AddProperty(DataColumnDef.MTS_CustomerNo)
                                .AddProperty(DataColumnDef.MTS_CustomerName)
                                .AddProperty(DataColumnDef.MTS_MatarialGroupOrBrand)
                                .AddProperty(DataColumnDef.MTS_TargetVolume);

            mtsBrandCodes = (await _odataBrandService.GetMTSBrandCodesAsync()).ToList();

            var dataTarget = (await _odataService.GetMTSDataByCustomerAndDate(selectQueryBuilder, model.CustomerNo, currentDateStr, mtsBrandCodes)).ToList();

            var dataActual = await GetSalesDataValueVolume(model.CustomerNo, fromDate, toDate, mtsBrandCodes);

            var result = new List<MTSResultModel>();

            var brandCodes = dataTarget.Select(x => x.MatarialGroupOrBrand)
                                .Concat(dataActual.Select(x => x.MatarialGroupOrBrand))
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

                if (dataActual.Any(x => x.MatarialGroupOrBrand == brandCode))
                {
                    var volAct = dataActual.Where(x => x.MatarialGroupOrBrand == brandCode).Sum(x => x.ActualVolume);
                    var brandNameAct = dataActual.FirstOrDefault(x => x.MatarialGroupOrBrand == brandCode).MatarialGroupOrBrand;
                    var custNoAct = dataActual.FirstOrDefault(x => x.MatarialGroupOrBrand == brandCode).CustomerNo;
                    var custNameAct = dataActual.FirstOrDefault(x => x.MatarialGroupOrBrand == brandCode).CustomerName;

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
                var brands = result.Select(x => x.MatarialGroupOrBrand).Distinct().ToList();

                var allBrandFamilyData = (await _odataService.GetBrandFamilyDataByBrands(brands, true))
                                            .GroupBy(x => x.MatarialGroupOrBrandFamily).ToList();

                foreach (var item in result)
                {
                    var brandFamilyData = allBrandFamilyData.FirstOrDefault(x => x.Key == item.MatarialGroupOrBrand);
                    if (brandFamilyData != null)
                    {
                        item.MatarialGroupOrBrand = string.Join(", ", brandFamilyData.Select(x => x.MatarialGroupOrBrandName));
                    }
                }
            }
            #endregion

            return result;
        }

        public async Task<IList<PerformanceResultModel>> GetPremiumBrandPerformance(MTSSearchModel model)
        {
            var currentDateStr = $"{string.Format("{0:0000}", model.Year)}.{string.Format("{0:00}", model.Month)}";
            var currentDate = (new DateTime(model.Year, model.Month, 1));
            var lyfd = currentDate.GetLYFD().DateFormat();
            var lyld = currentDate.GetLYLD().DateFormat();
            var cyfd = currentDate.GetCYFD().DateFormat();
            var cyld = currentDate.GetCYLD().DateFormat();
            var premiumBrandCodes = new List<string>();

            var selectQueryBuilder = new SelectQueryOptionBuilder();
            selectQueryBuilder.AddProperty(DataColumnDef.MTS_CustomerNo)
                                .AddProperty(DataColumnDef.MTS_CustomerName)
                                .AddProperty(DataColumnDef.MTS_MatarialGroupOrBrand)
                                .AddProperty(DataColumnDef.MTS_TargetVolume);

            premiumBrandCodes = (await _odataBrandService.GetPremiumBrandCodesAsync()).ToList();

            var dataTargetCy = (await _odataService.GetMTSDataByCustomerAndDate(selectQueryBuilder, model.CustomerNo, currentDateStr, premiumBrandCodes)).ToList();

            var dataActualLy = await GetSalesDataValueVolume(model.CustomerNo, lyfd, lyld, premiumBrandCodes);

            var dataActualCy = await GetSalesDataValueVolume(model.CustomerNo, cyfd, cyld, premiumBrandCodes);

            var result = new List<PerformanceResultModel>();

            var brandCodes = dataTargetCy.Select(x => x.MatarialGroupOrBrand)
                                .Concat(dataActualLy.Select(x => x.MatarialGroupOrBrand))
                                    .Concat(dataActualCy.Select(x => x.MatarialGroupOrBrand))
                                            .Distinct().ToList();

            foreach (var brandCode in brandCodes)
            {
                var res = new PerformanceResultModel();

                if (dataTargetCy.Any(x => x.MatarialGroupOrBrand == brandCode))
                {
                    var volTarCy = dataTargetCy.Where(x => x.MatarialGroupOrBrand == brandCode).Sum(x => CustomConvertExtension.ObjectToDecimal(x.TargetVolume));
                    var brandNameTarCy = dataTargetCy.FirstOrDefault(x => x.MatarialGroupOrBrand == brandCode).MatarialGroupOrBrand;
                    var custNoTarCy = dataTargetCy.FirstOrDefault(x => x.MatarialGroupOrBrand == brandCode).CustomerNo;
                    var custNameTarCy = dataTargetCy.FirstOrDefault(x => x.MatarialGroupOrBrand == brandCode).CustomerName;

                    res.MatarialGroupOrBrand = string.IsNullOrEmpty(res.MatarialGroupOrBrand) ? brandNameTarCy : res.MatarialGroupOrBrand;
                    res.CustomerNo = string.IsNullOrEmpty(res.CustomerNo) ? custNoTarCy : res.CustomerNo;
                    res.CustomerName = string.IsNullOrEmpty(res.CustomerName) ? custNameTarCy : res.CustomerName;
                    res.TargetVolume = volTarCy;
                }

                if (dataActualCy.Any(x => x.MatarialGroupOrBrand == brandCode))
                {
                    var volActCy = dataActualCy.Where(x => x.MatarialGroupOrBrand == brandCode).Sum(x => x.ActualVolume);
                    var brandNameActCy = dataActualCy.FirstOrDefault(x => x.MatarialGroupOrBrand == brandCode).MatarialGroupOrBrand;
                    var custNoActCy = dataActualCy.FirstOrDefault(x => x.MatarialGroupOrBrand == brandCode).CustomerNo;
                    var custNameActCy = dataActualCy.FirstOrDefault(x => x.MatarialGroupOrBrand == brandCode).CustomerName;

                    res.MatarialGroupOrBrand = string.IsNullOrEmpty(res.MatarialGroupOrBrand) ? brandNameActCy : res.MatarialGroupOrBrand;
                    res.CustomerNo = string.IsNullOrEmpty(res.CustomerNo) ? custNoActCy : res.CustomerNo;
                    res.CustomerName = string.IsNullOrEmpty(res.CustomerName) ? custNameActCy : res.CustomerName;
                    res.ActualVolume = volActCy;
                }

                if (dataActualLy.Any(x => x.MatarialGroupOrBrand == brandCode))
                {
                    var volActLy = dataActualLy.Where(x => x.MatarialGroupOrBrand == brandCode).Sum(x => x.ActualVolume);
                    var brandNameActLy = dataActualLy.FirstOrDefault(x => x.MatarialGroupOrBrand == brandCode).MatarialGroupOrBrand;
                    var custNoActLy = dataActualLy.FirstOrDefault(x => x.MatarialGroupOrBrand == brandCode).CustomerNo;
                    var custNameActLy = dataActualLy.FirstOrDefault(x => x.MatarialGroupOrBrand == brandCode).CustomerName;

                    res.MatarialGroupOrBrand = string.IsNullOrEmpty(res.MatarialGroupOrBrand) ? brandNameActLy : res.MatarialGroupOrBrand;
                    res.CustomerNo = string.IsNullOrEmpty(res.CustomerNo) ? custNoActLy : res.CustomerNo;
                    res.CustomerName = string.IsNullOrEmpty(res.CustomerName) ? custNameActLy : res.CustomerName;
                    res.LYSMVolume = volActLy;
                }

                res.TargetAchievement = _odataService.GetAchivement(res.TargetVolume, res.ActualVolume);
                res.TillDateGrowth = _odataService.GetTillDateGrowth(res.LYSMVolume, res.ActualVolume,
                                        currentDate.GetCYLD().Day, currentDate.GetCYLCD().Day);

                result.Add(res);
            }

            #region get brand data
            if (result.Any())
            {
                var brands = result.Select(x => x.MatarialGroupOrBrand).Distinct().ToList();

                var allBrandFamilyData = (await _odataService.GetBrandFamilyDataByBrands(brands)).ToList();

                foreach (var item in result)
                {
                    var brandFamilyData = allBrandFamilyData.FirstOrDefault(x => x.MatarialGroupOrBrand == item.MatarialGroupOrBrand);
                    if (brandFamilyData != null)
                    {
                        item.MatarialGroupOrBrand = brandFamilyData.MatarialGroupOrBrandName;
                    }
                }
            }
            #endregion

            return result;
        }

        public async Task<IList<ValueTargetResultModel>> GetMonthlyValueTarget(MTSSearchModel model)
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

            var dataActual = await GetSalesDataValueVolume(model.CustomerNo, fromDate, toDate);

            var result = new List<ValueTargetResultModel>();

            var brandCodes = dataTarget.Select(x => x.MatarialGroupOrBrand)
                                .Concat(dataActual.Select(x => x.MatarialGroupOrBrand))
                                    .Distinct().ToList();

            foreach (var brandCode in brandCodes)
            {
                var res = new ValueTargetResultModel();

                if (dataTarget.Any(x => x.MatarialGroupOrBrand == brandCode))
                {
                    var volTar = dataTarget.Where(x => x.MatarialGroupOrBrand == brandCode).Sum(x => CustomConvertExtension.ObjectToDecimal(x.TargetValue));
                    var brandNameTar = dataTarget.FirstOrDefault(x => x.MatarialGroupOrBrand == brandCode).MatarialGroupOrBrand;
                    var custNoTar = dataTarget.FirstOrDefault(x => x.MatarialGroupOrBrand == brandCode).CustomerNo;
                    var custNameTar = dataTarget.FirstOrDefault(x => x.MatarialGroupOrBrand == brandCode).CustomerName;

                    //res.MatarialGroupOrBrand = string.IsNullOrEmpty(res.MatarialGroupOrBrand) ? brandNameTar : res.MatarialGroupOrBrand;
                    res.CustomerNo = string.IsNullOrEmpty(res.CustomerNo) ? custNoTar : res.CustomerNo;
                    res.CustomerName = string.IsNullOrEmpty(res.CustomerName) ? custNameTar : res.CustomerName;
                    res.TargetValue = volTar;
                }

                if (dataActual.Any(x => x.MatarialGroupOrBrand == brandCode))
                {
                    var volAct = dataActual.Where(x => x.MatarialGroupOrBrand == brandCode).Sum(x => x.ActualVolume);
                    var brandNameAct = dataActual.FirstOrDefault(x => x.MatarialGroupOrBrand == brandCode).MatarialGroupOrBrand;
                    var custNoAct = dataActual.FirstOrDefault(x => x.MatarialGroupOrBrand == brandCode).CustomerNo;
                    var custNameAct = dataActual.FirstOrDefault(x => x.MatarialGroupOrBrand == brandCode).CustomerName;

                    //res.MatarialGroupOrBrand = string.IsNullOrEmpty(res.MatarialGroupOrBrand) ? brandNameAct : res.MatarialGroupOrBrand;
                    res.CustomerNo = string.IsNullOrEmpty(res.CustomerNo) ? custNoAct : res.CustomerNo;
                    res.CustomerName = string.IsNullOrEmpty(res.CustomerName) ? custNameAct : res.CustomerName;
                    res.ActualValue = volAct;
                }

                res.DifferenceValue = res.TargetValue - res.ActualValue;

                result.Add(res);
            }

            var returnResult = new List<ValueTargetResultModel>();
            if (result.Any())
            {
                var res = new ValueTargetResultModel();
                res.CustomerNo = result.FirstOrDefault().CustomerNo;
                res.CustomerName = result.FirstOrDefault().CustomerName;
                res.TargetValue = result.Sum(x => x.TargetValue);
                res.ActualValue = result.Sum(x => x.ActualValue);
                res.DifferenceValue = res.TargetValue - res.ActualValue;
                returnResult.Add(res);
            }

            return returnResult;
        }

        private async Task<IList<(string MatarialGroupOrBrand, string CustomerNo, string CustomerName, decimal ActualValue, decimal ActualVolume)>> GetSalesDataValueVolume(string customerNo, string fromDate, string toDate, List<string> brands = null)
        {
            var selectQueryBuilder = new SelectQueryOptionBuilder();
            selectQueryBuilder.AddProperty(DataColumnDef.CustomerNo)
                                .AddProperty(DataColumnDef.CustomerName)
                                .AddProperty(DataColumnDef.MatarialGroupOrBrand)
                                .AddProperty(DataColumnDef.NetAmount)
                                .AddProperty(DataColumnDef.Volume);

            var data = (await _odataService.GetSalesDataByCustomerAndDivision(selectQueryBuilder, customerNo, fromDate, toDate, brands: brands)).ToList();

            var result = data.GroupBy(x => x.MatarialGroupOrBrand).Select(x =>
                                                                    (
                                                                        MatarialGroupOrBrand: x.Key,
                                                                        CustomerNo: x.FirstOrDefault().CustomerNo,
                                                                        CustomerName: x.FirstOrDefault().CustomerName,
                                                                        ActualValue: x.Sum(s => CustomConvertExtension.ObjectToDecimal(s.NetAmount)),
                                                                        ActualVolume: x.Sum(s => CustomConvertExtension.ObjectToDecimal(s.Volume))
                                                                    )).ToList();

            return result;
        }

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

