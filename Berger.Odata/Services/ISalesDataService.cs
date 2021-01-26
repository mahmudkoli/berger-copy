using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Berger.Odata.Model;

namespace Berger.Odata.Services
{
    public interface ISalesDataService
    {
        Task<object> GetInvoiceHistory(InvoiceHistorySearchModel model);
        Task<object> GetInvoiceItemDetails(InvoiceItemDetailsSearchModel model);
        Task<object> GetBrandWiseMTDDetails(BrandWiseMTDSearchModel model);
    }
}
