using Berger.Data.MsfaEntity.SAPTables;
using System.Collections.Generic;
using System.Threading.Tasks;
using X.PagedList;

namespace Berger.Odata.Services
{
    public interface IODataBrandService
    {
        Task<IList<string>> GetCBMaterialCodesAsync();
        Task<IList<string>> GetMTSBrandCodesAsync();
        Task<IList<string>> GetPremiumBrandCodesAsync();
    }
}
