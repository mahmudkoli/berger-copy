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
        #endregion

        #region get selectable data
        Task<IList<DriverDataModel>> GetDriverDataByInvoiceNos(List<string> invoiceNos);
        Task<IList<BrandFamilyDataModel>> GetBrandFamilyDataByBrands(List<string> brands, bool isFamily = false);
        Task<IList<SalesDataModel>> GetSalesDataByCustomerAndDivision(SelectQueryOptionBuilder selectQueryBuilder,
            string customerNo, string startDate, string endDate, string division = "-1");
        Task<IList<MTSDataModel>> GetMTSDataByCustomerAndDate(SelectQueryOptionBuilder selectQueryBuilder,
            string customerNo, string date);
        #endregion
    }
}
