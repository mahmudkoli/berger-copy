using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Berger.Odata.Extensions;
using Berger.Odata.Model;

namespace Berger.Odata.Services
{
    public interface IODataService
    {
        #region get data
        Task<IList<SalesDataModel>> GetSalesData(string query);
        Task<IList<MTSDataModel>> GetMTSData(string query);
        Task<IList<DriverDataModel>> GetDriverData(string query);
        Task<IList<BrandFamilyDataModel>> GetBrandFamilyData(string query);
        Task<IList<FinancialDataModel>> GetFinancialData(string query);
        Task<IList<BalanceDataModel>> GetBalanceData(string query);
        Task<IList<CollectionDataModel>> GetCollectionData(string query);
        Task<IList<CustomerDataModel>> GetCustomerData(string query);
        #endregion

        #region get selectable data
        Task<DriverDataModel> GetDriverDataByInvoiceNo(string invoiceNo);
        Task<IList<BrandFamilyDataModel>> GetBrandFamilyDataByBrands(List<string> brands = null, bool isFamily = false);
        Task<IList<SalesDataModel>> GetSalesDataByCustomerAndDivision(SelectQueryOptionBuilder selectQueryBuilder,
            string customerNo, string startDate, string endDate, string division = "-1", List<string> materialCodes = null, List<string> brands = null);
        Task<IList<SalesDataModel>> GetSalesDataByTerritory(SelectQueryOptionBuilder selectQueryBuilder,
            string startDate, string endDate, string territory = "-1", List<string> brands = null);
        Task<IList<MTSDataModel>> GetMTSDataByCustomerAndDate(SelectQueryOptionBuilder selectQueryBuilder,
            string customerNo, string date, List<string> brands = null);
        Task<IList<MTSDataModel>> GetMTSDataByTerritory(SelectQueryOptionBuilder selectQueryBuilder,
            string date, string territory = "-1", List<string> brands = null);
        Task<IList<FinancialDataModel>> GetFinancialDataByCustomerAndCreditControlArea(SelectQueryOptionBuilder selectQueryBuilder,
            string customerNo, string startDate = "", string endDate = "", string creditControlArea = "");
        Task<IList<BalanceDataModel>> GetBalanceDataByCustomerAndCreditControlArea(SelectQueryOptionBuilder selectQueryBuilder,
            string customerNo, string startDate = "", string endDate = "", string creditControlArea = "");
        Task<IList<CollectionDataModel>> GetCollectionDataByCustomerAndCreditControlArea(SelectQueryOptionBuilder selectQueryBuilder,
            string customerNo, string startPostingDate = "", string endPostingDate = "", string startClearDate = "", string endClearDate = "", string creditControlArea = "", string bounceStatus = "");
        Task<IList<CustomerDataModel>> GetCustomerDataByCustomerNo(SelectQueryOptionBuilder selectQueryBuilder,
            string customerNo);

        Task<IList<MTSDataModel>> GetMtsDataByCustomerAndDivision(SelectQueryOptionBuilder selectQueryBuilder,
            string customerNo, string compareMonth, string division = "-1");

        Task<IList<SalesDataModel>> GetSalesDataByMultipleCustomerAndDivision(
            SelectQueryOptionBuilder selectQueryBuilder,
            IList<int> dealerList, string startDate, string endDate, string division = "-1",
            List<string> materialCodes = null, List<string> brands = null, string customerClassification = "-1", string territory = "-1");

        Task<IList<MTSDataModel>> GetMtsDataByMultipleCustomerAndDivision(SelectQueryOptionBuilder selectQueryBuilder,
            IList<int> dealerIds, string compareMonth, string division = "-1");
        #endregion

        #region calculate data
        public decimal GetGrowth(decimal first, decimal second);
        public decimal GetAchivement(decimal target, decimal actual);
        public decimal GetTillDateGrowth(decimal first, decimal second, int totalDays, int countDays);
        public decimal GetTillDateAchivement(decimal target, decimal actual, int totalDays, int countDays);
        public decimal GetPercentage(decimal total, decimal value);
        #endregion
    }
}
