using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public FinancialDataService(
            IODataService odataService
            )
        {
            _odataService = odataService;
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
                                    Amount = x.Amount
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
                                    Amount = x.Amount
                                }).ToList();

            return result;
        }

        public async Task<IList<OutstandingSummaryResultModel>> GetOutstandingSummary(OutstandingSummarySearchModel model)
        {
            var currentDate = DateTime.Now;
            var fromDate = currentDate.AddDays(-30).DateTimeFormat();
            var toDate = currentDate.DateTimeFormat();

            var selectQueryBuilder = new SelectQueryOptionBuilder();
            selectQueryBuilder.AddProperty(FinancialColDef.CustomerNo)
                                .AddProperty(FinancialColDef.CustomerName)
                                .AddProperty(FinancialColDef.CreditControlArea)
                                .AddProperty(FinancialColDef.DayLimit)
                                .AddProperty(FinancialColDef.Amount);

            var data = (await _odataService.GetFinancialDataByCustomerAndCreditControlArea(selectQueryBuilder, model.CustomerNo, fromDate, toDate)).ToList();

            var result = data.Select(x =>
                                new OutstandingSummaryResultModel()
                                {
                                    Division = x.CreditControlArea,
                                    DaysLimit = x.DayLimit,
                                }).ToList();

            return result;
        }
    }
}
