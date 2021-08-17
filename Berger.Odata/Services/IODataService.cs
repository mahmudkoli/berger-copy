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
        Task<IList<StockDataModel>> GetStockData(string query);
        Task<IList<CustomerOccasionDataModel>> GetCustomerOccasionData(string query);
        Task<IList<CustomerCreditDataModel>> GetCustomerCreditData(string query);
        Task<IList<CustomerDeliveryDataModel>> GetCustomerDeliveryData(string query);
        #endregion

        #region get selectable data
        Task<DriverDataModel> GetDriverDataByInvoiceNo(string invoiceNo);
        Task<IList<BrandFamilyDataModel>> GetBrandFamilyDataByBrands(List<string> brands = null, bool isFamily = false);
        Task<IList<SalesDataModel>> GetSalesDataByCustomerAndDivision(SelectQueryOptionBuilder selectQueryBuilder,
            string customerNo, string startDate, string endDate, string division = "-1", List<string> materialCodes = null, List<string> brands = null);
        Task<IList<SalesDataModel>> GetSalesDataByArea(SelectQueryOptionBuilder selectQueryBuilder,
            string startDate, string endDate, string territory = "", List<string> brands = null, string depot = "", string salesGroup = "", string salesOffice = "", string zone = "");

        Task<IList<SalesDataModel>> GetSalesDataByArea(SelectQueryOptionBuilder selectQueryBuilder,
            string startDate, string endDate, List<string> territories = null, List<string> brands = null, string depot = "", List<string> salesGroups = null, List<string> zones = null);

        Task<IList<SalesDataModel>> GetSalesDataByMultipleArea(SelectQueryOptionBuilder selectQueryBuilder,
            string startDate, string endDate, string depot, List<string> salesOffices = null, List<string> salesGroups = null, List<string> territories = null, List<string> zones = null, List<string> brands = null);

        Task<IList<SalesDataModel>> GetSalesDataByMultipleTerritory(SelectQueryOptionBuilder selectQueryBuilder,
            string startDate, string endDate, string depot, List<string> territories = null, List<string> zones = null, string dealerId = "", List<string> brands = null, List<string> salesGroups = null, List<string> salesOffices = null);

        Task<IList<MTSDataModel>> GetMTSDataByCustomerAndDate(SelectQueryOptionBuilder selectQueryBuilder,
            string customerNo, string date, List<string> brands = null);

        Task<IList<MTSDataModel>> GetMTSDataByArea(SelectQueryOptionBuilder selectQueryBuilder,
            string date, string territory = "", List<string> brands = null, string depot = "", string salesGroup = "", string salesOffice = "", string zone = "");

        Task<IList<MTSDataModel>> GetMTSDataByArea(SelectQueryOptionBuilder selectQueryBuilder,
            string date, List<string> territories = null, List<string> brands = null, string depot = "", List<string> salesGroups = null, List<string> zones = null);

        Task<IList<FinancialDataModel>> GetFinancialDataByCustomerAndCreditControlArea(SelectQueryOptionBuilder selectQueryBuilder,
            string customerNo, string startDate = "", string endDate = "", string creditControlArea = "");
        //Task<IList<FinancialDataModel>> GetFinancialDataByMultipleCustomerAndCreditControlArea(SelectQueryOptionBuilder selectQueryBuilder,
        //    IList<int> dealerIds, string startDate = "", string endDate = "", string creditControlArea = "");
        Task<IList<BalanceDataModel>> GetBalanceDataByCustomerAndCreditControlArea(SelectQueryOptionBuilder selectQueryBuilder,
            string customerNo, string startDate = "", string endDate = "", string creditControlArea = "");
        Task<IList<CollectionDataModel>> GetCollectionDataByCustomerAndCreditControlArea(SelectQueryOptionBuilder selectQueryBuilder,
            string customerNo, string startPostingDate = "", string endPostingDate = "", string startClearDate = "", string endClearDate = "", string creditControlArea = "", string bounceStatus = "");
        Task<IList<CollectionDataModel>> GetCollectionDataByMultipleCustomerAndCreditControlArea(SelectQueryOptionBuilder selectQueryBuilder,
            List<string> dealerIds, string startPostingDate = "", string endPostingDate = "", string startClearDate = "", string endClearDate = "", string creditControlArea = "", string bounceStatus = "");
        Task<IList<CustomerDataModel>> GetCustomerDataByCustomerNo(SelectQueryOptionBuilder selectQueryBuilder,
            string customerNo);
        Task<IList<CustomerDataModel>> GetCustomerDataByMultipleCustomerNo(SelectQueryOptionBuilder selectQueryBuilder,
            IList<string> dealerIds);

        Task<IList<MTSDataModel>> GetMtsDataByCustomerAndDivision(SelectQueryOptionBuilder selectQueryBuilder,
            string customerNo, string compareMonth, string division = "-1");

        Task<IList<SalesDataModel>> GetSalesDataByMultipleCustomerAndDivision(
            SelectQueryOptionBuilder selectQueryBuilder,
            IList<string> dealerList, string startDate, string endDate, string division = "-1",
            List<string> materialCodes = null, List<string> brands = null, string customerClassification = "-1", string territory = "-1");

        Task<IList<MTSDataModel>> GetMtsDataByMultipleCustomerAndDivision(SelectQueryOptionBuilder selectQueryBuilder,
            IList<string> dealerIds, string compareMonth, string division = "-1", List<string> brands = null);

        Task<IList<StockDataModel>> GetStockData(SelectQueryOptionBuilder selectQueryBuilder,
            string plant = "", string materialGroupOrBrand = "", string materialCode = "");

        Task<IList<CollectionDataModel>> GetCollectionData(SelectQueryOptionBuilder selectQueryBuilder,
            IList<string> dealerIds, string fromDate, string endDate);

        Task<IList<MTSDataModel>> GetMTSDataByMultipleTerritory(SelectQueryOptionBuilder selectQueryBuilder,
            string startDate, string endDate, string depot = "", List<string> territories = null, List<string> zones = null, string dealerId = "", List<string> brands = null, List<string> salesGroups = null, List<string> salesOffices = null);

        Task<IList<CustomerOccasionDataModel>> GetCustomerOccasionData(SelectQueryOptionBuilder selectQueryBuilder, IList<string> dealerIds);
        Task<IList<CustomerCreditDataModel>> GetCustomerCreditData(SelectQueryOptionBuilder selectQueryBuilder,
            string customerNo, string creditControlArea);

        Task<IList<CustomerDeliveryDataModel>> GetCustomerDeliveryData(SelectQueryOptionBuilder selectQueryBuilder,
            string customerNo, string startDate, string endDate);

        Task<IList<SalesDataModel>> GetSalesDataByDate(SelectQueryOptionBuilder selectQueryBuilder, string date);

        Task<IList<MTSDataModel>> GetMtsTarget(SelectQueryOptionBuilder selectQueryBuilder, string date);

        #endregion

        #region get selectable data By Area
        Task<IList<SalesDataModel>> GetSalesData(SelectQueryOptionBuilder selectQueryBuilder,
            string startDate, string endDate, IList<string> depots = null,
            IList<string> salesOffices = null, IList<string> salesGroups = null,
            IList<string> territories = null, IList<string> zones = null,
            IList<string> brands = null,
            string division = "",
            string channel = "",
            string classification = "",
            string creditControlArea = "",
            string customerNo = "");

        Task<IList<MTSDataModel>> GetMTSData(SelectQueryOptionBuilder selectQueryBuilder,
            string startDate, string endDate, IList<string> depots = null,
            IList<string> salesOffices = null, IList<string> salesGroups = null,
            IList<string> territories = null, IList<string> zones = null,
            IList<string> brands = null,
            string division = "",
            string channel = "",
            string customerNo = "");

        Task<IList<CustomerDataModel>> GetCustomerData(SelectQueryOptionBuilder selectQueryBuilder,
            IList<string> depots = null,
            IList<string> salesOffices = null, IList<string> salesGroups = null,
            IList<string> territories = null, IList<string> zones = null,
            IList<string> customerNos = null,
            string division = "",
            string creditControlArea = "",
            string channel = "");

        Task<IList<FinancialDataModel>> GetFinancialData(SelectQueryOptionBuilder selectQueryBuilder,
            string customerNo, string endDate, string creditControlArea = "");

        Task<IList<CollectionDataModel>> GetCollectionData(SelectQueryOptionBuilder selectQueryBuilder,
            IList<string> depots = null, IList<string> territories = null,
            IList<string> customerNos = null,
            string startPostingDate = "", string endPostingDate = "",
            string startClearDate = "", string endClearDate = "",
            string creditControlArea = "", string bounceStatus = "");
        #endregion

        #region calculate data
        public decimal GetGrowth(decimal lyValue, decimal cyValue);
        //public decimal GetGrowthNew(decimal first, decimal second);
        public decimal GetAchivement(decimal target, decimal actual);
        public decimal GetTillDateGrowth(decimal lyValue, decimal cyValue, int totalDays, int countDays);
        public decimal GetTillDateAchivement(decimal target, decimal actual, int totalDays, int countDays);
        public decimal GetPercentage(decimal total, decimal value);
        decimal GetContribution(decimal first, decimal second);

        #endregion

        Task<IList<ColorBankMachineDataModel>> GetColorBankMachine(string query);
        Task<IList<ColorBankMachineDataModel>> GetColorBankInstallData(SelectQueryOptionBuilder selectQueryBuilder, string depot = "", string startDate = "", string endDate = "");
    }
}
