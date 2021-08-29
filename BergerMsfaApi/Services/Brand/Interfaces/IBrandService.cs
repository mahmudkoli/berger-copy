using Berger.Data.MsfaEntity.SAPTables;
using BergerMsfaApi.Models.Dealer;
using BergerMsfaApi.Models.Brand;
using System.Threading.Tasks;
using X.PagedList;
using BergerMsfaApi.Models.Common;
using System.Collections.Generic;
using BergerMsfaApi.Services.Brand.Implementation;

namespace BergerMsfaApi.Services.Brand.Interfaces
{
    public interface IBrandService
    {
        Task<QueryResultModel<BrandInfoModel>> GetBrandsAsync(BrandQueryObjectModel query);
        Task<IList<AppMaterialBrandModel>> GetBrandsAsync(AppBrandSearchModel query);
        Task<object> GetBrandsFamilyAsync();
        Task<BrandInfoModel> GetBrandById(int id);
        Task<bool> BrandStatusUpdate(BrandStatusModel brandStatus);
        public Task<IEnumerable<BrandInfoStatusLogModel>> GetBrandInfoStatusLog(int brandInfoId);
        Task<IList<KeyValuePairObjectModel>> GetBrandDropDownAsync(BrandFilterModel model);
        Task<IList<KeyValuePairObjectModel>> GetBrandFamilyDropDownAsync();
    }
}
