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
    public class AlertNotificationODataService : IAlertNotificationODataService
    {
        private readonly IODataService _odataService;

        public AlertNotificationODataService(IODataService odataService)
        {
            _odataService = odataService;
        }
        public async Task<IList<CollectionDataModel>> GetCustomerAndCreditControlArea(SelectQueryOptionBuilder selectQueryBuilder,
            string startPostingDate = "", string endPostingDate = "", string startClearDate = "", string endClearDate = "")
        {
            var filterQueryBuilder = new FilterQueryOptionBuilder();
            filterQueryBuilder.Equal(CollectionColDef.Company, ConstantsValue.BergerCompanyCode);

            if (!string.IsNullOrEmpty(startPostingDate) && !string.IsNullOrEmpty(endPostingDate))
            {
                filterQueryBuilder.And()
                                .StartGroup()
                                .GreaterThanOrEqualDateTime(CollectionColDef.PostingDate, startPostingDate)
                                .And()
                                .LessThanOrEqualDateTime(CollectionColDef.PostingDate, endPostingDate)
                                .EndGroup();
            }
            else if (!string.IsNullOrEmpty(startPostingDate))
            {
                filterQueryBuilder.And().GreaterThanOrEqualDateTime(CollectionColDef.PostingDate, startPostingDate);
            }
            else if (!string.IsNullOrEmpty(endPostingDate))
            {
                filterQueryBuilder.And().LessThanOrEqualDateTime(CollectionColDef.PostingDate, endPostingDate);
            }

            if (!string.IsNullOrEmpty(startClearDate) && !string.IsNullOrEmpty(endClearDate))
            {
                filterQueryBuilder.And()
                                .StartGroup()
                                .GreaterThanOrEqualDateTime(CollectionColDef.ClearDate, startClearDate)
                                .And()
                                .LessThanOrEqualDateTime(CollectionColDef.ClearDate, endClearDate)
                                .EndGroup();
            }
            else if (!string.IsNullOrEmpty(startClearDate))
            {
                filterQueryBuilder.And().GreaterThanOrEqualDateTime(CollectionColDef.ClearDate, startClearDate);
            }
            else if (!string.IsNullOrEmpty(endClearDate))
            {
                filterQueryBuilder.And().LessThanOrEqualDateTime(CollectionColDef.ClearDate, endClearDate);
            }

            var queryBuilder = new QueryOptionBuilder();
            queryBuilder.AppendQuery(filterQueryBuilder.Filter)
                        .AppendQuery(selectQueryBuilder.Select);

            var data = (await _odataService.GetCollectionData(queryBuilder.Query)).ToList();

            return data;
        }

        public async Task<IList<CustomerDataModel>> GetCustomerDataByMultipleCustomerNo(SelectQueryOptionBuilder selectQueryBuilder
            )
        {

            var queryBuilder = new QueryOptionBuilder();
            queryBuilder.AppendQuery(selectQueryBuilder.Select);

            var data = (await _odataService.GetCustomerData(queryBuilder.Query)).ToList();

            return data;
        }

        public async Task<IList<FinancialDataModel>> GetFinancialDataByCustomer(SelectQueryOptionBuilder selectQueryBuilder,
           string startDate = "", string endDate = "", string creditControlArea = "")
        {
            var filterQueryBuilder = new FilterQueryOptionBuilder();
            filterQueryBuilder.Equal(FinancialColDef.CompanyCode, ConstantsValue.BergerCompanyCode);


            if (!string.IsNullOrEmpty(creditControlArea))
            {
                filterQueryBuilder.And().Equal(FinancialColDef.CreditControlArea, creditControlArea);
            }

            if (!string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate))
            {
                filterQueryBuilder.And()
                                .StartGroup()
                                .GreaterThanOrEqualDateTime(FinancialColDef.Date, startDate)
                                .And()
                                .LessThanOrEqualDateTime(FinancialColDef.Date, endDate)
                                .EndGroup();
            }
            else if (!string.IsNullOrEmpty(startDate))
            {
                filterQueryBuilder.And().GreaterThanOrEqualDateTime(FinancialColDef.Date, startDate);
            }
            else if (!string.IsNullOrEmpty(endDate))
            {
                filterQueryBuilder.And().LessThanOrEqualDateTime(FinancialColDef.Date, endDate);
            }
            else if (string.IsNullOrEmpty(endDate))
            {
                endDate = DateTime.Now.DateTimeFormat();
                filterQueryBuilder.And().LessThanOrEqualDateTime(FinancialColDef.Date, endDate);
            }


            var queryBuilder = new QueryOptionBuilder();
            queryBuilder.AppendQuery(filterQueryBuilder.Filter)
                        //.AppendQuery(topQuery)
                        .AppendQuery(selectQueryBuilder.Select);

            var data = (await _odataService.GetFinancialData(queryBuilder.Query)).ToList();

            return data;
        }

        public async Task<IList<CustomerOccasionDataModel>> GetCustomerOccasionData(SelectQueryOptionBuilder selectQueryBuilder)
        {
            var queryBuilder = new QueryOptionBuilder();
            queryBuilder.AppendQuery(selectQueryBuilder.Select);

            var data = (await _odataService.GetCustomerOccasionData(queryBuilder.Query)).ToList();

            return data;
        }
    }

    public interface IAlertNotificationODataService
    {
        Task<IList<CollectionDataModel>> GetCustomerAndCreditControlArea(SelectQueryOptionBuilder selectQueryBuilder,
            string startPostingDate = "", string endPostingDate = "", string startClearDate = "", string endClearDate = "");

        Task<IList<CustomerDataModel>> GetCustomerDataByMultipleCustomerNo(SelectQueryOptionBuilder selectQueryBuilder
            );

        Task<IList<FinancialDataModel>> GetFinancialDataByCustomer(SelectQueryOptionBuilder selectQueryBuilder,
           string startDate = "", string endDate = "", string creditControlArea = "");

        Task<IList<CustomerOccasionDataModel>> GetCustomerOccasionData(SelectQueryOptionBuilder selectQueryBuilder);

    }
}
