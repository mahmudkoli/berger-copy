using Berger.Data.MsfaEntity.SAPTables;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using X.PagedList;

namespace Berger.Odata.Services
{
    public interface IODataBrandService
    {
        Task<IList<string>> GetCBMaterialCodesAsync();
        Task<IList<string>> GetMTSBrandCodesAsync();
        Task<IList<string>> GetPremiumBrandCodesAsync();
        Task<IList<string>> GetEnamelBrandCodesAsync();
        Task<IList<string>> GetPowderBrandCodesAsync();
        Task<IList<string>> GetLiquidBrandCodesAsync();
        Task<IList<BrandFamilyInfo>> GetBrandFamilyInfosAsync(Expression<Func<BrandFamilyInfo, bool>> predicate = null);
    }
}
