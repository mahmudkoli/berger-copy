using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Berger.Common.Extensions;
using Berger.Common.Model;
using Berger.Data.MsfaEntity.SAPReports;
using Berger.Odata.Extensions;
using Berger.Odata.Model;
using Berger.Odata.Model.SPModels;
using Berger.Odata.Repositories;

namespace Berger.Odata.Services
{
    public class ReportDataService : IReportDataService
    {
        private readonly ISalesDataService _salesDataService;
        private readonly IMTSDataService _mtsDataService;
        private readonly IODataService _odataService;
        private readonly IODataCommonService _odataCommonService;
        private readonly IODataBrandService _odataBrandService;
        private readonly IODataSAPRepository<SummaryPerformanceReport> _summaryPerformanceReportRepo;

        public ReportDataService(
            ISalesDataService salesDataService, 
            IMTSDataService mtsDataService,
            IODataService odataService,
            IODataCommonService odataCommonService,
            IODataBrandService odataBrandService,
            IODataSAPRepository<SummaryPerformanceReport> summaryPerformanceReportRepo)
        {
            _salesDataService = salesDataService;
            _mtsDataService = mtsDataService;
            _odataService = odataService;
            _odataCommonService = odataCommonService;
            _odataBrandService = odataBrandService;
            _summaryPerformanceReportRepo = summaryPerformanceReportRepo;
        }

        public async Task<IList<MTDTargetSummaryReportResultModel>> MTDTargetSummary(MTDTargetSummarySearchModel model)
        {
            var currentDate = DateTime.Now;
            var filterDate = new DateTime(model.Year, model.Month, 01);
            var cyfd = filterDate.GetCYFD();
            var cylcd = filterDate.GetCYLCD();
            var cyld = filterDate.GetCYLD();
            var lyfd = filterDate.GetLYFD();
            var lylcd = filterDate.GetLYLCD();
            var lyld = filterDate.GetLYLD();

            //var cyDataActual = await _salesDataService.GetMTDActual(model, cyfd, cyld, model.Division, model.VolumeOrValue, model.Category, null);
            //var lyDataActual = await _salesDataService.GetMTDActual(model, lyfd, lyld, model.Division, model.VolumeOrValue, model.Category, null);

            var dataActual = _summaryPerformanceReportRepo.GetDataBySP<spMTDTargetSummaryReport>("spGetMTDTargetSummaryReports", new List<(string, object)>
            {
                ("Depots", string.Join(",",model.Depots)),
                ("Territories", string.Join(",",model.Territories)),
                ("Zones", string.Join(",",model.Zones)),
                ("Division", model.Division),
                ("Year", model.Year),
                ("Month", model.Month),
                ("VolumeOrValue", model.VolumeOrValue),
                ("BrandCategory", model.Category),
            });
            var cyDataTarget = await _mtsDataService.GetMTDTarget(model, cyfd, cyld, model.Division, model.VolumeOrValue, model.Category, null);

            var result = new List<MTDTargetSummaryReportResultModel>();

            var depots = dataActual.Select(x => x.Depot)
                            //.Concat(lyDataActual.Select(x => x.PlantOrBusinessArea))
                                .Concat(cyDataTarget.Select(x => x.PlantOrBusinessArea))
                                    .Distinct().ToList();

            //var tillDateCyActual = cyDataActual.Where(x => x.Date.SalesResultDateFormat().Date >= cyfd.Date
            //                                            && x.Date.SalesResultDateFormat().Date <= cylcd.Date)
            //                                    .Sum(x => model.VolumeOrValue == EnumVolumeOrValue.Volume
            //                                    ? CustomConvertExtension.ObjectToDecimal(x.Volume)
            //                                    : CustomConvertExtension.ObjectToDecimal(x.NetAmount));

            //var tillDateLyActual = lyDataActual.Where(x => x.Date.SalesResultDateFormat().Date >= lyfd.Date
            //                                            && x.Date.SalesResultDateFormat().Date <= lylcd.Date)
            //                                    .Sum(x => model.VolumeOrValue == EnumVolumeOrValue.Volume
            //                                    ? CustomConvertExtension.ObjectToDecimal(x.Volume)
            //                                    : CustomConvertExtension.ObjectToDecimal(x.NetAmount));

            if (dataActual.Any() || cyDataTarget.Any())
            {
                var tillDateLyActual = dataActual.Sum(x => x.TillDateLYActual);
                var tillDateCyActual = dataActual.Sum(x => x.TillDateCYActual);

                var tempResult = new MTDTargetSummaryReportResultModel();
                tempResult.Depots = depots;
                tempResult.Depot = tempResult.Depots.FirstOrDefault();
                tempResult.LYMTD = dataActual.Sum(x => x.LYMTD);
                tempResult.CMTarget = cyDataTarget.Sum(x => model.VolumeOrValue == EnumVolumeOrValue.Volume
                                                    ? CustomConvertExtension.ObjectToDecimal(x.TargetVolume)
                                                    : CustomConvertExtension.ObjectToDecimal(x.TargetValue));
                tempResult.CMActual = dataActual.Sum(x => x.CMActual);
                tempResult.TillDateGrowth = _odataService.GetGrowth(tillDateLyActual, tillDateCyActual);
                tempResult.AskingPerDay = (tempResult.CMTarget - tempResult.CMActual) / currentDate.RemainingDays();
                tempResult.TillDatePerformacePerDay = tempResult.CMActual / currentDate.TillDays();

                result.Add(tempResult);
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

        public async Task<IList<MTDBrandPerformanceReportResultModel>> MTDBrandPerformance(MTDBrandPerformanceSearchModel model)
        {
            var currentDate = DateTime.Now;
            var filterDate = new DateTime(model.Year, model.Month, 01);
            var cyfd = filterDate.GetCYFD();
            var cylcd = filterDate.GetCYLCD();
            var cyld = filterDate.GetCYLD();
            var lyfd = filterDate.GetLYFD();
            var lylcd = filterDate.GetLYLCD();
            var lyld = filterDate.GetLYLD();

            var cyDataActual = await _salesDataService.GetMTDActual(model, cyfd, cyld, model.Division, model.VolumeOrValue, null, model.Type);
            var lyDataActual = await _salesDataService.GetMTDActual(model, lyfd, lyld, model.Division, model.VolumeOrValue, null, model.Type);
            var cyDataTarget = await _mtsDataService.GetMTDTarget(model, cyfd, cyld, model.Division, model.VolumeOrValue, null, model.Type);


            var result = new List<MTDBrandPerformanceReportResultModel>();

            var brands = cyDataActual.Select(x => x.MatarialGroupOrBrand)
                            .Concat(lyDataActual.Select(x => x.MatarialGroupOrBrand))
                                .Concat(cyDataTarget.Select(x => x.MatarialGroupOrBrand))
                                    .Distinct().ToList();

            var depots = cyDataActual.Select(x => x.PlantOrBusinessArea)
                            .Concat(lyDataActual.Select(x => x.PlantOrBusinessArea))
                                .Concat(cyDataTarget.Select(x => x.PlantOrBusinessArea))
                                    .Distinct().ToList();

            //var brandFamilyInfos = await _odataBrandService.GetBrandFamilyInfosAsync(x => brands.Any(b => b == x.MatarialGroupOrBrand));
            var brandFamilyInfos = await _odataBrandService.GetBrandFamilyInfosAsync();

            Func<SalesDataModel, string, bool> predicateActual = (x, brand) => x.MatarialGroupOrBrand == brand;
            Func<MTSDataModel, string, bool> predicateTarget = (x, brand) => x.MatarialGroupOrBrand == brand;

            #region brand family group
            if (model.Type == EnumBrandType.MTSBrands)
            {
                // MatarialGroupOrBrandFamily as CustomerGroup
                foreach (var item in cyDataActual)
                {
                    item.CustomerGroup = brandFamilyInfos.FirstOrDefault(x => x.MatarialGroupOrBrand == item.MatarialGroupOrBrand)?
                                                        .MatarialGroupOrBrandFamily ?? item.MatarialGroupOrBrand;
                }
                foreach (var item in lyDataActual)
                {
                    item.CustomerGroup = brandFamilyInfos.FirstOrDefault(x => x.MatarialGroupOrBrand == item.MatarialGroupOrBrand)?
                                                        .MatarialGroupOrBrandFamily ?? item.MatarialGroupOrBrand;
                }
                foreach (var item in cyDataTarget)
                {
                    item.CustomerGroup = brandFamilyInfos.FirstOrDefault(x => x.MatarialGroupOrBrand == item.MatarialGroupOrBrand)?
                                                        .MatarialGroupOrBrandFamily ?? item.MatarialGroupOrBrand;
                }

                brands = cyDataActual.Select(x => x.CustomerGroup)
                            .Concat(lyDataActual.Select(x => x.CustomerGroup))
                                .Concat(cyDataTarget.Select(x => x.CustomerGroup))
                                    .Distinct().ToList();

                predicateActual = (x, brand) => x.CustomerGroup == brand;
                predicateTarget = (x, brand) => x.CustomerGroup == brand;
            }
            #endregion

            foreach (var brand in brands)
            {
                var tillDateCyActual = cyDataActual.Where(x => predicateActual(x, brand) &&
                                                        x.Date.SalesResultDateFormat().Date >= cyfd.Date
                                                        && x.Date.SalesResultDateFormat().Date <= cylcd.Date)
                                                .Sum(x => model.VolumeOrValue == EnumVolumeOrValue.Volume
                                                ? CustomConvertExtension.ObjectToDecimal(x.Volume)
                                                : CustomConvertExtension.ObjectToDecimal(x.NetAmount));

                var tillDateLyActual = lyDataActual.Where(x => predicateActual(x, brand) && 
                                                            x.Date.SalesResultDateFormat().Date >= lyfd.Date
                                                            && x.Date.SalesResultDateFormat().Date <= lylcd.Date)
                                                    .Sum(x => model.VolumeOrValue == EnumVolumeOrValue.Volume
                                                    ? CustomConvertExtension.ObjectToDecimal(x.Volume)
                                                    : CustomConvertExtension.ObjectToDecimal(x.NetAmount));

                var brandFamilyObj = brandFamilyInfos.FirstOrDefault(x => model.Type == EnumBrandType.MTSBrands
                                                        ? x.MatarialGroupOrBrandFamily == brand
                                                        : x.MatarialGroupOrBrand == brand);
                var brandName = brandFamilyObj != null 
                                    ? model.Type == EnumBrandType.MTSBrands 
                                        ? $"{brandFamilyObj.MatarialGroupOrBrandFamilyName} ({brandFamilyObj.MatarialGroupOrBrandFamily})" 
                                        : $"{brandFamilyObj.MatarialGroupOrBrandName} ({brandFamilyObj.MatarialGroupOrBrand})"
                                    : brand;

                // for showing all group item name code
                if (model.Type == EnumBrandType.MTSBrands && brandFamilyObj != null)
                {
                    brandName = string.Join(", ", brandFamilyInfos.Where(x => x.MatarialGroupOrBrandFamily == brand)
                                                        .Select(x => $"{x.MatarialGroupOrBrandName} ({x.MatarialGroupOrBrand})"));
                }

                var tempResult = new MTDBrandPerformanceReportResultModel();
                tempResult.Depots = cyDataActual.Where(x => predicateActual(x, brand))
                                            .Select(x => x.PlantOrBusinessArea)
                                    .Concat(lyDataActual.Where(x => predicateActual(x, brand))
                                            .Select(x => x.PlantOrBusinessArea))
                                    .Concat(cyDataTarget.Where(x =>predicateTarget(x, brand))
                                            .Select(x => x.PlantOrBusinessArea))
                                    .Distinct().ToList();
                tempResult.Depot = tempResult.Depots.FirstOrDefault();
                tempResult.Brand = brandName;
                tempResult.LYMTD = lyDataActual.Where(x => predicateActual(x, brand))
                                                .Sum(x => model.VolumeOrValue == EnumVolumeOrValue.Volume
                                                    ? CustomConvertExtension.ObjectToDecimal(x.Volume)
                                                    : CustomConvertExtension.ObjectToDecimal(x.NetAmount));
                tempResult.CMTarget = cyDataTarget.Where(x => predicateTarget(x, brand))
                                                    .Sum(x => model.VolumeOrValue == EnumVolumeOrValue.Volume
                                                    ? CustomConvertExtension.ObjectToDecimal(x.TargetVolume)
                                                    : CustomConvertExtension.ObjectToDecimal(x.TargetValue));
                tempResult.CMActual = cyDataActual.Where(x => predicateActual(x, brand))
                                                    .Sum(x => model.VolumeOrValue == EnumVolumeOrValue.Volume
                                                    ? CustomConvertExtension.ObjectToDecimal(x.Volume)
                                                    : CustomConvertExtension.ObjectToDecimal(x.NetAmount));
                tempResult.TillDateGrowth = _odataService.GetGrowth(tillDateLyActual, tillDateCyActual);
                tempResult.AskingPerDay = (tempResult.CMTarget - tempResult.CMActual) / currentDate.RemainingDays();
                tempResult.TillDatePerformacePerDay = tempResult.CMActual / currentDate.TillDays();

                result.Add(tempResult);
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
    }
}