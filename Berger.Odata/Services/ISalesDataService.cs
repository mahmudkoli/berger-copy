using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Berger.Odata.Model;

namespace Berger.Odata.Services
{
    public interface ISalesDataService
    {
        Task<IList<InvoiceHistoryResultModel>> GetInvoiceHistory(InvoiceHistorySearchModel model);
        Task<IList<InvoiceItemDetailsResultModel>> GetInvoiceItemDetails(InvoiceItemDetailsSearchModel model);
        Task<IList<BrandWiseMTDResultModel>> GetBrandWiseMTDDetails(BrandWiseMTDSearchModel model);
    }
}
