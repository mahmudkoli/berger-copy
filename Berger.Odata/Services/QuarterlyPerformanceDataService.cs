using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Berger.Common.Extensions;
using Berger.Common.HttpClient;
using Berger.Common.JSONParser;
using Berger.Data.MsfaEntity.Master;
using Berger.Odata.Common;
using Berger.Odata.Extensions;
using Berger.Odata.Model;
using Berger.Odata.Repositories;
using Microsoft.Extensions.Options;

namespace Berger.Odata.Services
{
    public class QuarterlyPerformanceDataService : IQuarterlyPerformanceDataService
    {
        private readonly IODataService _odataService;
        private readonly IODataBrandService _odataBrandService;
        private readonly IODataRepository<Division> _oDataDivisionRepository;

        public QuarterlyPerformanceDataService(
            IODataService odataService,
            IODataBrandService odataBrandService, IODataRepository<Division> oDataDivisionRepository
            )
        {
            _odataService = odataService;
            _odataBrandService = odataBrandService;
            _oDataDivisionRepository = oDataDivisionRepository;
        }

        #region App Report
        public async Task<IList<QuarterlyPerformanceDataResultModel>> GetMTSValueTargetAchivement(QuarterlyPerformanceSearchModel model)
        {
            var fromDate = (new DateTime(model.FromYear, model.FromMonth, 1));
            //var toDate = (new DateTime(model.ToYear, model.ToMonth, 1)).GetCYLD().DateTimeFormat();
            var monthCount = 3;
            var mtsBrands = new List<string>();
            var monthlyDictTarget = new Dictionary<string, IList<MTSDataModel>>();
            var monthlyDictActual = new Dictionary<string, IList<SalesDataModel>>();

            var selectTargetQueryBuilder = new SelectQueryOptionBuilder();
            selectTargetQueryBuilder
                                //.AddProperty(DataColumnDef.MTS_Territory)
                                //.AddProperty(DataColumnDef.MTS_CustomerNo)
                                //.AddProperty(DataColumnDef.MTS_CustomerName)
                                .AddProperty(DataColumnDef.MTS_TargetValue);

            var selectActualQueryBuilder = new SelectQueryOptionBuilder();
            selectActualQueryBuilder
                                //.AddProperty(DataColumnDef.Territory)
                                //.AddProperty(DataColumnDef.CustomerNo)
                                //.AddProperty(DataColumnDef.CustomerName)
                                .AddProperty(DataColumnDef.NetAmount);

            mtsBrands = (await _odataBrandService.GetMTSBrandCodesAsync()).ToList();

            for (var i = 0; i < monthCount; i++)
            {
                int number = i;
                var dateFull = fromDate.GetMonthDate(number).GetCYFD();
                var date = $"{string.Format("{0:0000}", dateFull.Year)}.{string.Format("{0:00}", dateFull.Month)}";

                var data = (await _odataService.GetMTSDataByArea(selectTargetQueryBuilder, date, territory: model.Territory, brands: mtsBrands)).ToList();
                var monthName = fromDate.GetMonthName(number);

                monthlyDictTarget.Add(monthName, data);
            }

            for (var i = 0; i < monthCount; i++)
            {
                int number = i;
                var startDate = fromDate.GetMonthDate(number).GetCYFD().DateFormat();
                var endDate = fromDate.GetMonthDate(number).GetCYLD().DateFormat();

                var data = (await _odataService.GetSalesDataByArea(selectActualQueryBuilder, startDate, endDate, territory: model.Territory, brands: mtsBrands)).ToList();
                var monthName = fromDate.GetMonthName(number);

                monthlyDictActual.Add(monthName, data);
            }

            var result = new List<QuarterlyPerformanceDataResultModel>();

            if (monthlyDictTarget.Any())
            {
                var res = new QuarterlyPerformanceDataResultModel();

                for (var i = 0; i < monthCount; i++)
                {
                    int number = i;
                    var monthName = fromDate.GetMonthName(number);
                    var dictDataTarget = monthlyDictTarget[monthName].ToList();
                    var dictDataActual = monthlyDictActual[monthName].ToList();

                    res.MonthlyTargetData.Add(new MonthlyDataModel()
                    {
                        MonthName = $"{monthName} Target",
                        Amount = dictDataTarget.Sum(s => CustomConvertExtension.ObjectToDecimal(s.TargetValue))
                    });

                    res.MonthlyActualData.Add(new MonthlyDataModel()
                    {
                        MonthName = $"{monthName} Actual",
                        Amount = dictDataActual.Sum(s => CustomConvertExtension.ObjectToDecimal(s.NetAmount))
                    });
                }

                res.TotalTarget = res.MonthlyTargetData.Sum(s => s.Amount);
                res.TotalActual = res.MonthlyActualData.Sum(s => s.Amount);

                res.AchivementOrGrowth = res.TotalTarget > 0 ? ((res.TotalActual / res.TotalTarget)) * 100 : decimal.Zero;

                result.Add(res);
            }

            return result;
        }
        #endregion

        #region Portal Report
        public async Task<IList<PortalQuarterlyPerformanceDataResultModel>> GetMTSValueTargetAchivement(PortalQuarterlyPerformanceSearchModel model)
        {
            var fromDate = (new DateTime(model.FromYear, model.FromMonth, 1));
            //var toDate = (new DateTime(model.ToYear, model.ToMonth, 1)).GetCYLD().DateTimeFormat();
            var monthCount = 3;
            var mtsBrands = new List<string>();
            var monthlyDictTarget = new Dictionary<string, IList<MTSDataModel>>();
            var monthlyDictActual = new Dictionary<string, IList<SalesDataModel>>();

            var selectTargetQueryBuilder = new SelectQueryOptionBuilder();
            selectTargetQueryBuilder
                                //.AddProperty(DataColumnDef.MTS_Territory)
                                //.AddProperty(DataColumnDef.MTS_CustomerNo)
                                //.AddProperty(DataColumnDef.MTS_CustomerName)
                                .AddProperty(DataColumnDef.MTS_TargetValue);

            var selectActualQueryBuilder = new SelectQueryOptionBuilder();
            selectActualQueryBuilder
                                //.AddProperty(DataColumnDef.Territory)
                                //.AddProperty(DataColumnDef.CustomerNo)
                                .AddProperty(DataColumnDef.Date)
                                .AddProperty(DataColumnDef.NetAmount);

            mtsBrands = (await _odataBrandService.GetMTSBrandCodesAsync()).ToList();

            for (var i = 0; i < monthCount; i++)
            {
                int number = i;
                var dateFull = fromDate.GetMonthDate(number).GetCYFD();
                var date = $"{string.Format("{0:0000}", dateFull.Year)}.{string.Format("{0:00}", dateFull.Month)}";

                var data = (await _odataService.GetMTSDataByArea(selectTargetQueryBuilder, date, territory: model.Territory, brands: mtsBrands, depot: model.Depot, salesGroup: model.SalesGroup, model.SalesOffice, zone: model.Zone)).ToList();
                var monthName = fromDate.GetMonthName(number);

                monthlyDictTarget.Add(monthName, data);
            }

            DateTime toDate = fromDate.AddMonths(2);
            toDate = toDate.AddDays(DateTime.DaysInMonth(toDate.Year, toDate.Month)).AddDays(-1);

            var sellsData = (await _odataService.GetSalesDataByArea(selectActualQueryBuilder, fromDate.DateFormat(), toDate.DateFormat(), territory: model.Territory, brands: mtsBrands, depot: model.Depot, salesGroup: model.SalesGroup, model.SalesOffice, zone: model.Zone)).ToList();

            for (var i = 0; i < monthCount; i++)
            {
                int number = i;
                var startDate = fromDate.GetMonthDate(number).GetCYFD();
                var endDate = fromDate.GetMonthDate(number).GetCYLD();

                var data = sellsData.Where(x => x.Date.DateFormatDate() >= startDate && x.Date.DateFormatDate() <= endDate).ToList();
                var monthName = fromDate.GetMonthName(number);

                monthlyDictActual.Add(monthName, data);
            }

            var result = new List<QuarterlyPerformanceDataResultModel>();

            if (monthlyDictTarget.Any())
            {
                var res = new QuarterlyPerformanceDataResultModel();

                for (var i = 0; i < monthCount; i++)
                {
                    int number = i;
                    var monthName = fromDate.GetMonthName(number);
                    var dictDataTarget = monthlyDictTarget[monthName].ToList();
                    var dictDataActual = monthlyDictActual[monthName].ToList();

                    res.MonthlyTargetData.Add(new MonthlyDataModel()
                    {
                        MonthName = $"{monthName} Target",
                        Amount = dictDataTarget.Sum(s => CustomConvertExtension.ObjectToDecimal(s.TargetValue))
                    });

                    res.MonthlyActualData.Add(new MonthlyDataModel()
                    {
                        MonthName = $"{monthName} Actual",
                        Amount = dictDataActual.Sum(s => CustomConvertExtension.ObjectToDecimal(s.NetAmount))
                    });
                }

                res.TotalTarget = res.MonthlyTargetData.Sum(s => s.Amount);
                res.TotalActual = res.MonthlyActualData.Sum(s => s.Amount);

                res.AchivementOrGrowth = res.TotalTarget > 0 ? ((res.TotalActual / res.TotalTarget)) * 100 : decimal.Zero;

                result.Add(res);
            }

            return await ToPortalModel(result);
        }

        public async Task<IList<PortalQuarterlyPerformanceDataResultModel>> GetBillingDealerQuarterlyGrowth(PortalQuarterlyPerformanceSearchModel model)
        {
            var fromDate = (new DateTime(model.FromYear, model.FromMonth, 1));
            //var toDate = (new DateTime(model.ToYear, model.ToMonth, 1)).GetCYLD().DateTimeFormat();
            var monthCount = 3;
            var enamelBrands = new List<string>();
            var monthlyDictLY = new Dictionary<string, IList<SalesDataModel>>();
            var monthlyDictCY = new Dictionary<string, IList<SalesDataModel>>();

            var selectQueryBuilder = new SelectQueryOptionBuilder();
            selectQueryBuilder
                                    .AddProperty(DataColumnDef.CustomerNo)
                                    .AddProperty(DataColumnDef.NetAmount)
                                    .AddProperty(DataColumnDef.Date);

            enamelBrands = (await _odataBrandService.GetEnamelBrandCodesAsync()).ToList();


            var lyFd = fromDate.GetMonthDate(0).GetLYFD().DateFormat();
            var lyEd = fromDate.GetMonthDate(2).GetLYLD().DateFormat();

            var lyData = await _odataService.GetSalesDataByArea(selectQueryBuilder, lyFd, lyEd, territory: model.Territory, brands: enamelBrands, depot: model.Depot, salesGroup: model.SalesGroup, model.SalesOffice, zone: model.Zone);

            for (var i = 0; i < monthCount; i++)
            {
                int number = i;

                var startDate = fromDate.GetMonthDate(number).GetLYFD();
                var endDate = fromDate.GetMonthDate(number).GetLYLD();

                var data = lyData.Where(x => x.Date.DateFormatDate() >= startDate && x.Date.DateFormatDate() <= endDate).ToList();

                var monthName = fromDate.GetMonthName(number);

                monthlyDictLY.Add(monthName, data);
            }

            var cyFd = fromDate.GetMonthDate(0).GetLYFD().DateFormat();
            var cyEd = fromDate.GetMonthDate(2).GetLYLD().DateFormat();

            var cyData = (await _odataService.GetSalesDataByArea(selectQueryBuilder, cyFd, cyEd, territory: model.Territory, brands: enamelBrands, depot: model.Depot, salesGroup: model.SalesGroup, model.SalesOffice, zone: model.Zone)).ToList();

            for (var i = 0; i < monthCount; i++)
            {
                int number = i;
                var startDate = fromDate.GetMonthDate(number).GetCYFD();
                var endDate = fromDate.GetMonthDate(number).GetCYLD();

                var data = cyData.Where(x => x.Date.DateFormatDate() >= startDate && x.Date.DateFormatDate() <= endDate).ToList();
                var monthName = fromDate.GetMonthName(number);

                monthlyDictCY.Add(monthName, data);
            }


            var result = new List<QuarterlyPerformanceDataResultModel>();

            if (monthlyDictLY.Any())
            {
                var res = new QuarterlyPerformanceDataResultModel();

                for (var i = 0; i < monthCount; i++)
                {
                    int number = i;
                    var monthName = fromDate.GetMonthName(number);
                    var dictDataLY = monthlyDictLY[monthName].ToList();
                    var dictDataCY = monthlyDictCY[monthName].ToList();

                    res.MonthlyTargetData.Add(new MonthlyDataModel()
                    {
                        MonthName = $"{monthName} LY",
                        Amount = dictDataLY.Select(s => s.CustomerNo).Distinct().Count()
                    });

                    res.MonthlyActualData.Add(new MonthlyDataModel()
                    {
                        MonthName = $"{monthName} CY",
                        Amount = dictDataCY.Select(s => s.CustomerNo).Distinct().Count()
                    });
                }

                res.TotalTarget = res.MonthlyTargetData.Sum(s => s.Amount);
                res.TotalActual = res.MonthlyActualData.Sum(s => s.Amount);

                res.AchivementOrGrowth = _odataService.GetGrowth(res.TotalTarget, res.TotalActual);

                result.Add(res);
            }

            return await ToPortalModel(result);
        }

        public async Task<IList<PortalQuarterlyPerformanceDataResultModel>> GetEnamelPaintsQuarterlyGrowth(PortalQuarterlyPerformanceSearchModel model)
        {
            var fromDate = (new DateTime(model.FromYear, model.FromMonth, 1));
            //var toDate = (new DateTime(model.ToYear, model.ToMonth, 1)).GetCYLD().DateTimeFormat();
            var monthCount = 3;
            var enamelBrands = new List<string>();
            var monthlyDictTarget = new Dictionary<string, IList<SalesDataModel>>();
            var monthlyDictActual = new Dictionary<string, IList<SalesDataModel>>();

            var selectActualQueryBuilder = new SelectQueryOptionBuilder();
            selectActualQueryBuilder
                .AddProperty(DataColumnDef.Date)
                .AddProperty(DataColumnDef.Volume);

            enamelBrands = (await _odataBrandService.GetEnamelBrandCodesAsync()).ToList();

            var lyFd = fromDate.GetMonthDate(0).GetLYFD().DateFormat();
            var lyEd = fromDate.GetMonthDate(2).GetLYLD().DateFormat();

            var lyData = await _odataService.GetSalesDataByArea(selectActualQueryBuilder, lyFd, lyEd, territory: model.Territory, brands: enamelBrands, depot: model.Depot, salesGroup: model.SalesGroup, model.SalesOffice, zone: model.Zone);

            for (var i = 0; i < monthCount; i++)
            {
                int number = i;

                var startDate = fromDate.GetMonthDate(number).GetCYFD();
                var endDate = fromDate.GetMonthDate(number).GetCYLD();

                var data = lyData.Where(x => x.Date.DateFormatDate() >= startDate && x.Date.DateFormatDate() <= endDate).ToList();

                var monthName = fromDate.GetMonthName(number);

                monthlyDictTarget.Add(monthName, data);
            }

            var cyFd = fromDate.GetMonthDate(0).GetLYFD().DateFormat();
            var cyEd = fromDate.GetMonthDate(2).GetLYLD().DateFormat();

            var cyData = await _odataService.GetSalesDataByArea(selectActualQueryBuilder, cyFd, cyEd, territory: model.Territory, brands: enamelBrands, depot: model.Depot, salesGroup: model.SalesGroup, model.SalesOffice, zone: model.Zone);

            for (var i = 0; i < monthCount; i++)
            {
                int number = i;
                var startDate = fromDate.GetMonthDate(number).GetLYFD();
                var endDate = fromDate.GetMonthDate(number).GetLYLD();

                var data = cyData.Where(x => x.Date.DateFormatDate() >= startDate && x.Date.DateFormatDate() <= endDate).ToList();
                var monthName = fromDate.GetMonthName(number);

                monthlyDictActual.Add(monthName, data);
            }


            var result = new List<QuarterlyPerformanceDataResultModel>();

            if (monthlyDictTarget.Any())
            {
                var res = new QuarterlyPerformanceDataResultModel();

                for (var i = 0; i < monthCount; i++)
                {
                    int number = i;
                    var monthName = fromDate.GetMonthName(number);
                    var dictDataTarget = monthlyDictTarget[monthName].ToList();
                    var dictDataActual = monthlyDictActual[monthName].ToList();

                    res.MonthlyTargetData.Add(new MonthlyDataModel()
                    {
                        MonthName = $"{monthName} CY",
                        Amount = dictDataTarget.Sum(s => CustomConvertExtension.ObjectToDecimal(s.Volume))
                    });

                    res.MonthlyActualData.Add(new MonthlyDataModel()
                    {
                        MonthName = $"{monthName} LY",
                        Amount = dictDataActual.Sum(s => CustomConvertExtension.ObjectToDecimal(s.Volume))
                    });
                }

                res.TotalTarget = res.MonthlyTargetData.Sum(s => s.Amount);
                res.TotalActual = res.MonthlyActualData.Sum(s => s.Amount);

                res.AchivementOrGrowth = _odataService.GetGrowth(res.TotalActual, res.TotalTarget);

                result.Add(res);
            }

            return await ToPortalModel(result);
        }

        public async Task<IList<PortalQuarterlyPerformanceDataResultModel>> GetPremiumBrandsContribution(PortalQuarterlyPerformanceSearchModel model)
        {
            var fromDate = (new DateTime(model.FromYear, model.FromMonth, 1));
            //var toDate = (new DateTime(model.ToYear, model.ToMonth, 1)).GetCYLD().DateTimeFormat();
            var monthCount = 3;
            var brands = new List<string>();
            var monthlyDictTarget = new Dictionary<string, IList<SalesDataModel>>();
            var monthlyActData = new Dictionary<string, IList<SalesDataModel>>();
            var monthlyDictActual = new Dictionary<string, IList<SalesDataModel>>();

            var selectActualQueryBuilder = new SelectQueryOptionBuilder();
            selectActualQueryBuilder
                .AddProperty(DataColumnDef.Division)
                .AddProperty(DataColumnDef.NetAmount) 
                .AddProperty(DataColumnDef.Date);

            brands = (await _odataBrandService.GetPremiumBrandCodesAsync()).ToList();

            Division division = await _oDataDivisionRepository.FindAsync(x => x.Description == "Decorative");
            string divisionCode = division != null ? division.DivisionCode.ToString() : "";

            var lyFd = fromDate.GetMonthDate(0).GetLYFD().DateFormat();
            var lyEd = fromDate.GetMonthDate(2).GetLYLD().DateFormat();

            var lyData = await _odataService.GetSalesDataByArea(selectActualQueryBuilder, lyFd, lyEd, territory: model.Territory, brands: brands, depot: model.Depot, salesGroup: model.SalesGroup, model.SalesOffice, zone: model.Zone);
            
            for (var i = 0; i < monthCount; i++)
            {
                int number = i;

                var startDate = fromDate.GetMonthDate(number).GetCYFD();
                var endDate = fromDate.GetMonthDate(number).GetCYLD();

                var data = lyData.Where(x => x.Date.DateFormatDate() >= startDate && x.Date.DateFormatDate() <= endDate).ToList();

                var monthName = fromDate.GetMonthName(number);

                monthlyActData.Add(monthName, data);


                data = data.Where(x => x.Division == divisionCode).ToList();

                monthlyDictTarget.Add(monthName, data);
            }

            for (var i = 0; i < monthCount; i++)
            {
                int number = i;

                var monthName = fromDate.GetMonthName(number);
                var data = monthlyActData[monthName];

                monthlyDictActual.Add(monthName, data);
            }


            var result = new List<QuarterlyPerformanceDataResultModel>();

            if (monthlyDictTarget.Any())
            {
                var res = new QuarterlyPerformanceDataResultModel();

                for (var i = 0; i < monthCount; i++)
                {
                    int number = i;
                    var monthName = fromDate.GetMonthName(number);
                    var dictDataTarget = monthlyDictTarget[monthName].ToList();
                    var dictDataActual = monthlyDictActual[monthName].ToList();

                    res.MonthlyTargetData.Add(new MonthlyDataModel()
                    {
                        MonthName = $"{monthName} (Total Deco Sales at his Territory)",
                        Amount = dictDataTarget.Sum(s => CustomConvertExtension.ObjectToDecimal(s.NetAmount))
                    });

                    res.MonthlyActualData.Add(new MonthlyDataModel()
                    {
                        MonthName = $"{monthName} (Premium Brand actual Sales at his Territory)",
                        Amount = dictDataActual.Sum(s => CustomConvertExtension.ObjectToDecimal(s.NetAmount))
                    });
                }

                res.TotalTarget = res.MonthlyTargetData.Sum(s => s.Amount);
                res.TotalActual = res.MonthlyActualData.Sum(s => s.Amount);

                res.AchivementOrGrowth = _odataService.GetContribution(res.TotalActual, res.TotalTarget);

                result.Add(res);
            }

            return await ToPortalModel(result);
        }

        public async Task<IList<PortalQuarterlyPerformanceDataResultModel>> GetPremiumBrandsGrowth(PortalQuarterlyPerformanceSearchModel model)
        {
            var fromDate = (new DateTime(model.FromYear, model.FromMonth, 1));
            //var toDate = (new DateTime(model.ToYear, model.ToMonth, 1)).GetCYLD().DateTimeFormat();
            var monthCount = 3;
            var brands = new List<string>();
            var monthlyDictTarget = new Dictionary<string, IList<SalesDataModel>>();
            var monthlyDictActual = new Dictionary<string, IList<SalesDataModel>>();

            var selectActualQueryBuilder = new SelectQueryOptionBuilder();
            selectActualQueryBuilder
                .AddProperty(DataColumnDef.Date)
                .AddProperty(DataColumnDef.Volume);

            brands = (await _odataBrandService.GetPremiumBrandCodesAsync()).ToList();

            var lyFd = fromDate.GetMonthDate(0).GetLYFD().DateFormat();
            var lyEd = fromDate.GetMonthDate(2).GetLYLD().DateFormat();

            var lyData = await _odataService.GetSalesDataByArea(selectActualQueryBuilder, lyFd, lyEd, territory: model.Territory, brands: brands, depot: model.Depot, salesGroup: model.SalesGroup, model.SalesOffice, zone: model.Zone);



            for (var i = 0; i < monthCount; i++)
            {
                int number = i;

                var startDate = fromDate.GetMonthDate(number).GetCYFD();
                var endDate = fromDate.GetMonthDate(number).GetCYLD();

                var data = lyData.Where(x => x.Date.DateFormatDate() >= startDate && x.Date.DateFormatDate() <= endDate).ToList();

                var monthName = fromDate.GetMonthName(number);

                monthlyDictTarget.Add(monthName, data);
            }

            var cyFd = fromDate.GetMonthDate(0).GetLYFD().DateFormat();
            var cyEd = fromDate.GetMonthDate(2).GetLYLD().DateFormat();

            var cyData = await _odataService.GetSalesDataByArea(selectActualQueryBuilder, cyFd, cyEd, territory: model.Territory, brands: brands, depot: model.Depot, salesGroup: model.SalesGroup, model.SalesOffice, zone: model.Zone);


            for (var i = 0; i < monthCount; i++)
            {
                int number = i;
                var startDate = fromDate.GetMonthDate(number).GetLYFD();
                var endDate = fromDate.GetMonthDate(number).GetLYLD();

                var data = cyData.Where(x => x.Date.DateFormatDate() >= startDate && x.Date.DateFormatDate() <= endDate).ToList();

                var monthName = fromDate.GetMonthName(number);

                monthlyDictActual.Add(monthName, data);
            }


            var result = new List<QuarterlyPerformanceDataResultModel>();

            if (monthlyDictTarget.Any())
            {
                var res = new QuarterlyPerformanceDataResultModel();

                for (var i = 0; i < monthCount; i++)
                {
                    int number = i;
                    var monthName = fromDate.GetMonthName(number);
                    var dictDataTarget = monthlyDictTarget[monthName].ToList();
                    var dictDataActual = monthlyDictActual[monthName].ToList();

                    res.MonthlyTargetData.Add(new MonthlyDataModel()
                    {
                        MonthName = $"{monthName} CY",
                        Amount = dictDataTarget.Sum(s => CustomConvertExtension.ObjectToDecimal(s.Volume))
                    });

                    res.MonthlyActualData.Add(new MonthlyDataModel()
                    {
                        MonthName = $"{monthName} LY",
                        Amount = dictDataActual.Sum(s => CustomConvertExtension.ObjectToDecimal(s.Volume))
                    });
                }

                res.TotalTarget = res.MonthlyTargetData.Sum(s => s.Amount);
                res.TotalActual = res.MonthlyActualData.Sum(s => s.Amount);

                res.AchivementOrGrowth = _odataService.GetGrowth(res.TotalActual, res.TotalTarget);

                result.Add(res);
            }

            return await ToPortalModel(result);
        }

        public async Task<IList<PortalQuarterlyPerformanceDataResultModel>> ToPortalModel(List<QuarterlyPerformanceDataResultModel> data)
        {
            var result = new List<PortalQuarterlyPerformanceDataResultModel>();

            foreach (var item in data)
            {
                var res = new PortalQuarterlyPerformanceDataResultModel();
                res.FirstMonthTargetName = item.MonthlyTargetData[0].MonthName;
                res.SecondMonthTargetName = item.MonthlyTargetData[1].MonthName;
                res.ThirdMonthTargetName = item.MonthlyTargetData[2].MonthName;
                res.FirstMonthTargetAmount = item.MonthlyTargetData[0].Amount;
                res.SecondMonthTargetAmount = item.MonthlyTargetData[1].Amount;
                res.ThirdMonthTargetAmount = item.MonthlyTargetData[2].Amount;

                res.FirstMonthActualName = item.MonthlyActualData[0].MonthName;
                res.SecondMonthActualName = item.MonthlyActualData[1].MonthName;
                res.ThirdMonthActualName = item.MonthlyActualData[2].MonthName;
                res.FirstMonthActualAmount = item.MonthlyActualData[0].Amount;
                res.SecondMonthActualAmount = item.MonthlyActualData[1].Amount;
                res.ThirdMonthActualAmount = item.MonthlyActualData[2].Amount;

                res.TotalTarget = item.TotalTarget;
                res.TotalActual = item.TotalActual;
                res.AchivementOrGrowth = item.AchivementOrGrowth;

                result.Add(res);
            }

            return result;
        }
        #endregion
    }
}
