using Berger.Common.Extensions;
using Berger.Odata.Common;
using Berger.Odata.Extensions;
using Berger.Odata.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Berger.Odata.Services
{
    public class KpiDataService : IKpiDataService
    {
        private readonly IODataService _odataService;
        private readonly IODataBrandService _odataBrandService;

        public KpiDataService(
            IODataService odataService,
            IODataBrandService odataBrandService
        )
        {
            _odataService = odataService;
            _odataBrandService = odataBrandService;
        }

        public async Task<TerritoryTargetAchievementResultModel> GetTerritoryTargetAchivement(TerritoryTargetAchievementSearchModel model)
        {
            var liquidBrands = new List<string>();
            var powderBrands = new List<string>();

            var liquidTarget = new List<MTSDataModel>();
            var powderTarget = new List<MTSDataModel>();
            var valueTarget = new List<MTSDataModel>();

            var liquidActual = new List<SalesDataModel>();
            var powderActual = new List<SalesDataModel>();
            var valueActual = new List<SalesDataModel>();

            var selectLiquidTargetQueryBuilder = new SelectQueryOptionBuilder();
            selectLiquidTargetQueryBuilder
                                        //.AddProperty(DataColumnDef.MTS_Territory)
                                        .AddProperty(DataColumnDef.MTS_TargetValue);

            var selectLiquidActualQueryBuilder = new SelectQueryOptionBuilder();
            selectLiquidActualQueryBuilder
                                        .AddProperty(DataColumnDef.Territory)
                                        //.AddProperty(DataColumnDef.Date)
                                        .AddProperty(DataColumnDef.NetAmount);

            var selectPowderTargetQueryBuilder = new SelectQueryOptionBuilder();
            selectPowderTargetQueryBuilder
                                        //.AddProperty(DataColumnDef.MTS_Territory)
                                        .AddProperty(DataColumnDef.MTS_TargetValue);

            var selectPowderActualQueryBuilder = new SelectQueryOptionBuilder();
            selectPowderActualQueryBuilder
                                        .AddProperty(DataColumnDef.Territory)
                                        //.AddProperty(DataColumnDef.Date)
                                        .AddProperty(DataColumnDef.NetAmount);

            var selectValueTargetQueryBuilder = new SelectQueryOptionBuilder();
            selectValueTargetQueryBuilder
                                        //.AddProperty(DataColumnDef.MTS_Territory)
                                        .AddProperty(DataColumnDef.MTS_TargetValue);

            var selectValueActualQueryBuilder = new SelectQueryOptionBuilder();
            selectValueActualQueryBuilder
                                        .AddProperty(DataColumnDef.Territory)
                                        //.AddProperty(DataColumnDef.Date)
                                        .AddProperty(DataColumnDef.NetAmount);

            liquidBrands = (await _odataBrandService.GetLiquidBrandCodesAsync()).ToList();
            powderBrands = (await _odataBrandService.GetPowderBrandCodesAsync()).ToList();

            var startDate = $"{string.Format("{0:0000}", model.FromDate.Year)}.{string.Format("{0:00}", model.FromDate.Month)}";
            var endDate = $"{string.Format("{0:0000}", model.ToDate.Year)}.{string.Format("{0:00}", model.ToDate.Month)}";

            liquidTarget = (await _odataService.GetMTSDataByTerritory(selectValueTargetQueryBuilder, startDate, endDate, model.Territory, liquidBrands)).ToList();
            powderTarget = (await _odataService.GetMTSDataByTerritory(selectValueTargetQueryBuilder, startDate, endDate, model.Territory, powderBrands)).ToList();
            valueTarget = (await _odataService.GetMTSDataByTerritory(selectValueTargetQueryBuilder, startDate, endDate, model.Territory)).ToList();

            liquidActual = (await _odataService.GetSalesDataByArea(selectValueActualQueryBuilder, model.FromDate.DateFormat(), model.ToDate.DateFormat(), model.Territory, liquidBrands)).ToList();
            powderActual = (await _odataService.GetSalesDataByArea(selectValueActualQueryBuilder, model.FromDate.DateFormat(), model.ToDate.DateFormat(), model.Territory, powderBrands)).ToList();
            valueActual = (await _odataService.GetSalesDataByArea(selectValueActualQueryBuilder, model.FromDate.DateFormat(), model.ToDate.DateFormat(), model.Territory)).ToList();
            
            var result = new TerritoryTargetAchievementResultModel();
            result.LiquidTargetInGallons = liquidTarget.Sum(x => CustomConvertExtension.ObjectToDecimal(x.TargetValue));
            result.LiquidActualInGallons = liquidActual.Sum(x => CustomConvertExtension.ObjectToDecimal(x.NetAmount));
            result.LiquidAcv = _odataService.GetAchivement(result.LiquidTargetInGallons, result.LiquidActualInGallons);

            result.PowderTargetInKg = powderTarget.Sum(x => CustomConvertExtension.ObjectToDecimal(x.TargetValue));
            result.PowderActualInKg = powderActual.Sum(x => CustomConvertExtension.ObjectToDecimal(x.NetAmount));
            result.PowderAcv = _odataService.GetAchivement(result.PowderTargetInKg, result.PowderActualInKg);

            result.ValueTargetInTk = valueTarget.Sum(x => CustomConvertExtension.ObjectToDecimal(x.TargetValue));
            result.ValueActualIngTk = valueActual.Sum(x => CustomConvertExtension.ObjectToDecimal(x.NetAmount));
            result.ValueAcv = _odataService.GetAchivement(result.ValueTargetInTk, result.ValueActualIngTk);

            return result;
        }

        public async Task<DealerWiseTargetAchievementResultModel> GetDealerWiseTargetAchivement(DealerWiseTargetAchievementSearchModel model)
        {
            var liquidBrands = new List<string>();
            var powderBrands = new List<string>();

            var liquidTarget = new List<MTSDataModel>();
            var powderTarget = new List<MTSDataModel>();
            var valueTarget = new List<MTSDataModel>();

            var liquidActual = new List<SalesDataModel>();
            var powderActual = new List<SalesDataModel>();
            var valueActual = new List<SalesDataModel>();

            var selectLiquidTargetQueryBuilder = new SelectQueryOptionBuilder();
            selectLiquidTargetQueryBuilder
                                        //.AddProperty(DataColumnDef.MTS_Territory)
                                        .AddProperty(DataColumnDef.MTS_TargetValue);

            var selectLiquidActualQueryBuilder = new SelectQueryOptionBuilder();
            selectLiquidActualQueryBuilder
                                        .AddProperty(DataColumnDef.Territory)
                                        //.AddProperty(DataColumnDef.Date)
                                        .AddProperty(DataColumnDef.NetAmount);

            var selectPowderTargetQueryBuilder = new SelectQueryOptionBuilder();
            selectPowderTargetQueryBuilder
                                        //.AddProperty(DataColumnDef.MTS_Territory)
                                        .AddProperty(DataColumnDef.MTS_TargetValue);

            var selectPowderActualQueryBuilder = new SelectQueryOptionBuilder();
            selectPowderActualQueryBuilder
                                        .AddProperty(DataColumnDef.Territory)
                                        //.AddProperty(DataColumnDef.Date)
                                        .AddProperty(DataColumnDef.NetAmount);

            var selectValueTargetQueryBuilder = new SelectQueryOptionBuilder();
            selectValueTargetQueryBuilder
                                        //.AddProperty(DataColumnDef.MTS_Territory)
                                        .AddProperty(DataColumnDef.MTS_TargetValue);

            var selectValueActualQueryBuilder = new SelectQueryOptionBuilder();
            selectValueActualQueryBuilder
                                        .AddProperty(DataColumnDef.Territory)
                                        //.AddProperty(DataColumnDef.Date)
                                        .AddProperty(DataColumnDef.NetAmount);

            liquidBrands = (await _odataBrandService.GetLiquidBrandCodesAsync()).ToList();
            powderBrands = (await _odataBrandService.GetPowderBrandCodesAsync()).ToList();

            var startDate = $"{string.Format("{0:0000}", model.FromDate.Year)}.{string.Format("{0:00}", model.FromDate.Month)}";
            var endDate = $"{string.Format("{0:0000}", model.ToDate.Year)}.{string.Format("{0:00}", model.ToDate.Month)}";

            liquidTarget = (await _odataService.GetMTSDataByDealer(selectValueTargetQueryBuilder, startDate, endDate, model.Territory, model.DealerId.ToString(), liquidBrands)).ToList();
            powderTarget = (await _odataService.GetMTSDataByDealer(selectValueTargetQueryBuilder, startDate, endDate, model.Territory, model.DealerId.ToString(), powderBrands)).ToList();
            valueTarget = (await _odataService.GetMTSDataByDealer(selectValueTargetQueryBuilder, startDate, endDate, model.Territory, model.DealerId.ToString())).ToList();

            liquidActual = (await _odataService.GetSalesDataByDealer(selectValueActualQueryBuilder, model.FromDate.DateFormat(), model.ToDate.DateFormat(), model.Territory, model.DealerId.ToString(), liquidBrands)).ToList();
            powderActual = (await _odataService.GetSalesDataByDealer(selectValueActualQueryBuilder, model.FromDate.DateFormat(), model.ToDate.DateFormat(), model.Territory, model.DealerId.ToString(), powderBrands)).ToList();
            valueActual = (await _odataService.GetSalesDataByDealer(selectValueActualQueryBuilder, model.FromDate.DateFormat(), model.ToDate.DateFormat(), model.Territory, model.DealerId.ToString())).ToList();

            var result = new DealerWiseTargetAchievementResultModel();
            result.LiquidTargetInGallons = liquidTarget.Sum(x => CustomConvertExtension.ObjectToDecimal(x.TargetValue));
            result.LiquidActualInGallons = liquidActual.Sum(x => CustomConvertExtension.ObjectToDecimal(x.NetAmount));
            result.LiquidAcv = _odataService.GetAchivement(result.LiquidTargetInGallons, result.LiquidActualInGallons);

            result.PowderTargetInKg = powderTarget.Sum(x => CustomConvertExtension.ObjectToDecimal(x.TargetValue));
            result.PowderActualInKg = powderActual.Sum(x => CustomConvertExtension.ObjectToDecimal(x.NetAmount));
            result.PowderAcv = _odataService.GetAchivement(result.PowderTargetInKg, result.PowderActualInKg);

            result.ValueTargetInTk = valueTarget.Sum(x => CustomConvertExtension.ObjectToDecimal(x.TargetValue));
            result.ValueActualIngTk = valueActual.Sum(x => CustomConvertExtension.ObjectToDecimal(x.NetAmount));
            result.ValueAcv = _odataService.GetAchivement(result.ValueTargetInTk, result.ValueActualIngTk);

            return result;
        }
    }
}
