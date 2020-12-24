using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Berger.Odata.Model;

namespace Berger.Odata.Services
{
    public interface ISalesData
    {
        Task<IEnumerable<SalesDataModel>> GetInvoiceHistory(SalesDataSearchModel model);
        Task<dynamic> GetInvoiceDetails(SalesDataSearchModel model);
        Task<dynamic> GetInvoiceItemDetails(SalesDataSearchModel model);
        Task<dynamic> GetBrandWiseMTDDetails(SalesDataSearchModel model);


    }
}
