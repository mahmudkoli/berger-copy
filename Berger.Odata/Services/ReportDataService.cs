using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Berger.Common.Extensions;
using Berger.Common.Model;
using Berger.Odata.Extensions;
using Berger.Odata.Model;

namespace Berger.Odata.Services
{
    public class ReportDataService : IReportDataService
    {
        private readonly ISalesDataService _salesDataService;
        private readonly IMTSDataService _mtsDataService;
        private readonly IODataService _odataService;
        private readonly IODataCommonService _odataCommonService;

        public ReportDataService(
            ISalesDataService salesDataService, 
            IMTSDataService mtsDataService,
            IODataService odataService,
            IODataCommonService odataCommonService)
        {
            _salesDataService = salesDataService;
            _mtsDataService = mtsDataService;
            _odataService = odataService;
            _odataCommonService = odataCommonService;
        }

        public async Task<IList<TargetReportResultModel>> MyTarget(MyTargetSearchModel model, IList<string> dealerIds)
        {
            //TODO: need to modify
            int monthDay = DateTime.DaysInMonth(model.Year, model.Month);
            DateTime fromDate = new DateTime(model.Year, model.Month, 1);
            DateTime endDate = new DateTime(model.Year, model.Month, DateTime.Now.Day);

            DateTime previousYearFromDate = fromDate.AddYears(-1);
            DateTime previousYearEndDate = new DateTime(model.Year - 1, model.Month, DateTime.DaysInMonth(model.Year - 1, model.Month));


            IEnumerable<SalesDataModel> salesTargetData = await _salesDataService
                .GetMyTargetSales(fromDate, endDate, model.Division, model.VolumeOrValue, model.ReportType, dealerIds, model.BrandType);

            var previousYearSalesData = await _salesDataService
                .GetMyTargetSales(previousYearFromDate, previousYearEndDate, model.Division, model.VolumeOrValue, model.ReportType, dealerIds, model.BrandType);


            var result = salesTargetData.Select(x => new TargetReportResultModel
            {
                TerritoryNumber = x.Territory,
                Zone = x.Zone,
                Brand = x.MatarialGroupOrBrand
            }).ToList();

            var mtsTargetData = await _mtsDataService.GetMyTargetMts(fromDate, dealerIds, model.Division, model.ReportType, model.VolumeOrValue, model.BrandType);

            mtsTargetData = mtsTargetData.GroupBy(x => new { x.Territory, x.Zone, x.MatarialGroupOrBrand })
                .Select(x => new MTSDataModel()
                {
                    Territory = x.Key.Territory,
                    Zone = x.Key.Zone,
                    MatarialGroupOrBrand = x.Key.MatarialGroupOrBrand,
                    TargetVolume = x.Sum(y => decimal.Parse(y.TargetVolume ?? "0")).ToString("0.##"),
                    TargetValue = x.Sum(y => decimal.Parse(y.TargetValue ?? "0")).ToString("0.##"),
                }).ToList();

            var oldTerritory = mtsTargetData.Where(x =>
                    !result.Select(y => y.TerritoryNumber + "-" + y.Zone + "-" + y.Brand)
                    .Contains(x.Territory + "-" + x.Zone + "-" + x.MatarialGroupOrBrand))
                .Select(x => new TargetReportResultModel
                {
                    TerritoryNumber = x.Territory,
                    Zone = x.Zone,
                    Brand = x.MatarialGroupOrBrand
                }).ToList();

            result.AddRange(oldTerritory);

            foreach (var item in result)
            {
                if (model.VolumeOrValue == EnumVolumeOrValue.Value)
                {
                    item.TotalMTSTarget = decimal.Parse(mtsTargetData
                        .FirstOrDefault(x => x.Territory == item.TerritoryNumber && x.MatarialGroupOrBrand == item.Brand && x.Zone == item.Zone)
                        ?.TargetValue ?? "0");

                    item.TillDateMTSAchieved = decimal.Parse(salesTargetData.FirstOrDefault(x => x.Territory == item.TerritoryNumber
                    && x.Zone == item.Zone && x.MatarialGroupOrBrand == item.Brand
                    )?.NetAmount ?? "0");

                    item.LYSMAchieved = decimal.Parse(previousYearSalesData
                        .FirstOrDefault(x =>
                            x.Zone == item.Zone &&
                            x.Territory == item.TerritoryNumber &&
                            x.MatarialGroupOrBrand == item.Brand)?.NetAmount ?? "0");
                }
                else
                {
                    item.TotalMTSTarget = decimal.Parse(mtsTargetData
                        .FirstOrDefault(x => x.Territory == item.TerritoryNumber && x.MatarialGroupOrBrand == item.Brand && x.Zone == item.Zone)
                        ?.TargetVolume ?? "0");
                    item.TillDateMTSAchieved = decimal.Parse(salesTargetData.FirstOrDefault(x => x.Territory == item.TerritoryNumber
                                                                                                     && x.Zone == item.Zone && x.MatarialGroupOrBrand == item.Brand)?.Volume ?? "0");

                    item.LYSMAchieved = decimal.Parse(previousYearSalesData
                        .FirstOrDefault(x =>
                            x.Zone == item.Zone &&
                            x.Territory == item.TerritoryNumber &&
                            x.MatarialGroupOrBrand == item.Brand)?.Volume ?? "0");
                }

                item.DayTarget = decimal.Parse((item.TotalMTSTarget / monthDay).ToString("0.##"));
                item.DaySales = decimal.Parse((item.TillDateMTSAchieved / DateTime.Now.Day).ToString("0.##"));
                item.TillDateTarget = decimal.Parse(((item.TotalMTSTarget / monthDay) * DateTime.Now.Day).ToString("0.##"));
                item.TillDateIdealAchieved = item.TotalMTSTarget == 0 ? 0 : decimal.Parse(((item.TillDateTarget / item.TotalMTSTarget)*100).ToString("0.##"));
                item.TillDateActualAchieved = item.TotalMTSTarget == 0 ? 0 : decimal.Parse(((item.TillDateMTSAchieved / item.TotalMTSTarget)*100).ToString("0.##"));
                item.TillDateMTSAchieved = decimal.Parse(item.TillDateMTSAchieved.ToString("0.##"));
                item.TotalMTSTarget = decimal.Parse(item.TotalMTSTarget.ToString("0.##"));
            }

            #region get brand data
            if (result.Any() && model.ReportType == MyTargetReportType.BrandWise)
            {
                var brands = result.Select(x => x.Brand).Distinct().ToList();

                var allBrandFamilyData = (await _odataService.GetBrandFamilyDataByBrands(brands)).ToList();

                foreach (var item in result)
                {
                    var brandFamilyData = allBrandFamilyData.FirstOrDefault(x => x.MatarialGroupOrBrand == item.Brand);
                    if (brandFamilyData != null)
                    {
                        item.Brand = $"{brandFamilyData.MatarialGroupOrBrandName} ({item.Brand})";
                    }
                }
            }
            #endregion

            return result;
        }

        public async Task<IList<MTDTargetSummaryReportResultModel>> MTDTargetSummary(MTDTargetSummarySearchModel model)
        {
            var currentDate = DateTime.Now;
            var cyfd = currentDate.GetCYFD();
            var cylcd = currentDate.GetCYLCD();
            var cyld = currentDate.GetCYLD();
            var lyfd = currentDate.GetLYFD();
            var lylcd = currentDate.GetLYLCD();
            var lyld = currentDate.GetLYLD();

            var cyDataActual = await _salesDataService.GetMTDActual(model, cyfd, cyld, model.Division, model.VolumeOrValue, model.Category);
            var lyDataActual = await _salesDataService.GetMTDActual(model, lyfd, lyld, model.Division, model.VolumeOrValue, model.Category);
            var cyDataTarget = await _mtsDataService.GetMTDTarget(model, cyfd, cyld, model.Division, model.VolumeOrValue, model.Category);


            var result = new List<MTDTargetSummaryReportResultModel>();

            var depots = cyDataActual.Select(x => x.PlantOrBusinessArea)
                            .Concat(lyDataActual.Select(x => x.PlantOrBusinessArea))
                                .Concat(cyDataTarget.Select(x => x.PlantOrBusinessArea))
                                    .Distinct().ToList();

            var tillDateCyActual = cyDataActual.Where(x => x.Date.SalesResultDateFormat().Date >= cyfd.Date
                                                        && x.Date.SalesResultDateFormat().Date <= cylcd.Date)
                                                .Sum(x => model.VolumeOrValue == EnumVolumeOrValue.Volume
                                                ? CustomConvertExtension.ObjectToDecimal(x.Volume)
                                                : CustomConvertExtension.ObjectToDecimal(x.NetAmount));

            var tillDateLyActual = lyDataActual.Where(x => x.Date.SalesResultDateFormat().Date >= lyfd.Date
                                                        && x.Date.SalesResultDateFormat().Date <= lylcd.Date)
                                                .Sum(x => model.VolumeOrValue == EnumVolumeOrValue.Volume
                                                ? CustomConvertExtension.ObjectToDecimal(x.Volume)
                                                : CustomConvertExtension.ObjectToDecimal(x.NetAmount));

            var tempResult = new MTDTargetSummaryReportResultModel();
            tempResult.Depots = depots;
            tempResult.Depot = depots.FirstOrDefault();
            tempResult.LYMTD = lyDataActual.Sum(x => model.VolumeOrValue == EnumVolumeOrValue.Volume
                                                ? CustomConvertExtension.ObjectToDecimal(x.Volume)
                                                : CustomConvertExtension.ObjectToDecimal(x.NetAmount));
            tempResult.CMTarget = cyDataTarget.Sum(x => model.VolumeOrValue == EnumVolumeOrValue.Volume
                                                ? CustomConvertExtension.ObjectToDecimal(x.TargetVolume)
                                                : CustomConvertExtension.ObjectToDecimal(x.TargetValue));
            tempResult.CMActual = cyDataActual.Sum(x => model.VolumeOrValue == EnumVolumeOrValue.Volume
                                                ? CustomConvertExtension.ObjectToDecimal(x.Volume)
                                                : CustomConvertExtension.ObjectToDecimal(x.NetAmount));
            tempResult.TillDateGrowth = _odataService.GetGrowth(tillDateLyActual, tillDateCyActual);
            tempResult.AskingPerDay = (tempResult.CMTarget - tempResult.CMActual) / currentDate.RemainingDays();
            tempResult.TillDatePerformacePerDay = tempResult.CMActual / currentDate.TillDays();

            result.Add(tempResult);

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
    }
}