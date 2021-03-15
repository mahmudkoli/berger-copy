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
        Task<IList<CustomerDataModel>> GetCustomerData(string query);
        #endregion

        #region get selectable data
        Task<IList<DriverDataModel>> GetDriverDataByInvoiceNos(List<string> invoiceNos);
        Task<IList<BrandFamilyDataModel>> GetBrandFamilyDataByBrands(List<string> brands = null, bool isFamily = false);
        Task<IList<SalesDataModel>> GetSalesDataByCustomerAndDivision(SelectQueryOptionBuilder selectQueryBuilder,
            string customerNo, string startDate, string endDate, string division = "-1", List<string> materialCodes = null, List<string> brands = null);
        Task<IList<MTSDataModel>> GetMTSDataByCustomerAndDate(SelectQueryOptionBuilder selectQueryBuilder,
            string customerNo, string date, List<string> brands = null);
        Task<IList<FinancialDataModel>> GetFinancialDataByCustomerAndCreditControlArea(SelectQueryOptionBuilder selectQueryBuilder,
            string customerNo, string startDate = "", string endDate = "", string creditControlArea = "");
        Task<IList<BalanceDataModel>> GetBalanceDataByCustomerAndCreditControlArea(SelectQueryOptionBuilder selectQueryBuilder,
            string customerNo, string startDate = "", string endDate = "", string creditControlArea = "");
        Task<IList<CustomerDataModel>> GetCustomerDataByCustomerNo(SelectQueryOptionBuilder selectQueryBuilder,
            string customerNo);
        #endregion
    }
}
