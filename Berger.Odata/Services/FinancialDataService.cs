using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    public class FinancialDataService : IFinancialDataService
    {
        private readonly IODataService _odataService;
        private readonly IODataCommonService _odataCommonService;

        public FinancialDataService(
            IODataService odataService,
            IODataCommonService odataCommonService
            )
        {
            _odataService = odataService;
            _odataCommonService = odataCommonService;
        }

        public async Task<IList<CollectionHistoryResultModel>> GetCollectionHistory(CollectionHistorySearchModel model)
        {
            var currentDate = DateTime.Now;
            var fromDate = currentDate.AddDays(-30).DateTimeFormat();
            var toDate = currentDate.DateTimeFormat();

            var selectQueryBuilder = new SelectQueryOptionBuilder();
            selectQueryBuilder.AddProperty(FinancialColDef.InvoiceNo)
                                .AddProperty(FinancialColDef.CustomerNo)
                                .AddProperty(FinancialColDef.CustomerName)
                                .AddProperty(FinancialColDef.CreditControlArea)
                                .AddProperty(FinancialColDef.PostingDate)
                                .AddProperty(FinancialColDef.Amount);

            var data = (await _odataService.GetFinancialDataByCustomerAndCreditControlArea(selectQueryBuilder, model.CustomerNo, fromDate, toDate, model.Division)).ToList();

            var result = data.Select(x =>
                                new CollectionHistoryResultModel()
                                {
                                    InvoiceNo = x.InvoiceNo,
                                    CustomerNo = x.CustomerNo,
                                    CustomerName = x.CustomerName,
                                    Division = x.CreditControlArea,
                                    //DivisionName = x.CreditControlAreaName,
                                    PostingDate = x.PostingDate,
                                    Amount = CustomConvertExtension.ObjectToDecimal(x.Amount)
                                }).ToList();

            return result;
        }

        public async Task<IList<OutstandingDetailsResultModel>> GetOutstandingDetails(OutstandingDetailsSearchModel model)
        {
            var currentDate = DateTime.Now;
            var fromDate = (model.Days switch
            {
                EnumOutstandingDetailsDaysCount._0_To_30_Days => currentDate.AddDays(-30),
                EnumOutstandingDetailsDaysCount._31_To_60_Days => currentDate.AddDays(-60),
                EnumOutstandingDetailsDaysCount._61_To_90_Days => currentDate.AddDays(-90),
                _ => default(DateTime)
            }).DateTimeFormat();

            var toDate = (model.Days switch
            {
                EnumOutstandingDetailsDaysCount._31_To_60_Days => currentDate.AddDays(-31),
                EnumOutstandingDetailsDaysCount._61_To_90_Days => currentDate.AddDays(-61),
                EnumOutstandingDetailsDaysCount._GT_90_Days => currentDate.AddDays(-91),
                _ => currentDate
            }).DateTimeFormat();

            var selectQueryBuilder = new SelectQueryOptionBuilder();
            selectQueryBuilder.AddProperty(FinancialColDef.InvoiceNo)
                                .AddProperty(FinancialColDef.Age)
                                .AddProperty(FinancialColDef.PostingDate)
                                .AddProperty(FinancialColDef.Amount);

            var data = (await _odataService.GetFinancialDataByCustomerAndCreditControlArea(selectQueryBuilder, model.CustomerNo, fromDate, toDate, model.Division)).ToList();

            var result = data.Select(x =>
                                new OutstandingDetailsResultModel()
                                {
                                    InvoiceNo = x.InvoiceNo,
                                    Age = x.Age,
                                    PostingDate = x.PostingDate,
                                    Amount = CustomConvertExtension.ObjectToDecimal(x.Amount)
                                }).ToList();

            return result;
        }

        public async Task<IList<OutstandingSummaryResultModel>> GetOutstandingSummary(OutstandingSummarySearchModel model)
        {
            //var currentDate = DateTime.Now;
            var fromDate = (new DateTime(2011, 01, 01)).DateTimeFormat();

            var selectCustomerQueryBuilder = new SelectQueryOptionBuilder();
            foreach (var prop in typeof(CustomerDataModel).GetProperties())
            {
                selectCustomerQueryBuilder.AddProperty(prop.Name);
            }

            var selectQueryBuilder = new SelectQueryOptionBuilder();
            selectQueryBuilder.AddProperty(FinancialColDef.CustomerNo)
                                .AddProperty(FinancialColDef.CustomerName)
                                .AddProperty(FinancialColDef.CreditControlArea)
                                .AddProperty(FinancialColDef.DayLimit)
                                .AddProperty(FinancialColDef.Age)
                                .AddProperty(FinancialColDef.Amount);

            var customerData = (await _odataService.GetCustomerDataByCustomerNo(selectCustomerQueryBuilder, model.CustomerNo)).ToList();
            var data = (await _odataService.GetFinancialDataByCustomerAndCreditControlArea(selectQueryBuilder, model.CustomerNo, fromDate)).ToList();

            var groupData = data.GroupBy(x => x.CreditControlArea).ToList();

            var result = groupData.Select(x =>
                                    {
                                        var osModel = new OutstandingSummaryResultModel();
                                        osModel.Division = x.FirstOrDefault()?.CreditControlArea ?? string.Empty;
                                        osModel.DaysLimit = x.FirstOrDefault()?.DayLimit ?? string.Empty;
                                        osModel.ValueLimit = customerData.FirstOrDefault(f => f.CreditControlArea == osModel.Division)?.CreditLimit ?? (decimal)0;
                                        osModel.NetDue = x.Sum(s => CustomConvertExtension.ObjectToDecimal(s.Amount));
                                        osModel.Slippage = x.Where(w => CustomConvertExtension.ObjectToInt(w.DayLimit) > CustomConvertExtension.ObjectToInt(w.Age))
                                                            .Sum(s => CustomConvertExtension.ObjectToDecimal(s.Amount));
                                        osModel.HighestDaysInvoice = x.Max(m => CustomConvertExtension.ObjectToInt(m.Age)).ToString();
                                        return osModel;
                                    }).OrderBy(o => o.Division).ToList();

            #region Credit Control Area 
            var creditControlAreas = await _odataCommonService.GetAllCreditControlAreasAsync();

            foreach (var item in result)
            {
                item.DivisionName = creditControlAreas.FirstOrDefault(f => f.CreditControlAreaId.ToString() == item.Division)?.Description ?? string.Empty;
            }
            #endregion

            return result;
        }
    }
}
