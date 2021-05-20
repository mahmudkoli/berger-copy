using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Berger.Odata.Model;

namespace Berger.Odata.Services
{
    public class ReportDataService : IReportDataService
    {
        private readonly ISalesDataService _salesDataService;
        private readonly IMTSDataService _mtsDataService;
        private readonly IODataService _odataService;

        public ReportDataService(ISalesDataService salesDataService, IMTSDataService mtsDataService,
            IODataService odataService)
        {
            _salesDataService = salesDataService;
            _mtsDataService = mtsDataService;
            this._odataService = odataService;
        }

        public async Task<IList<TargetReportResultModel>> MyTarget(MyTargetSearchModel model, IList<int> dealerIds)
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

       

    }
}