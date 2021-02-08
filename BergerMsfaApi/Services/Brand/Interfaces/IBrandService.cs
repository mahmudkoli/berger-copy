using Berger.Data.MsfaEntity.SAPTables;
using BergerMsfaApi.Models.Dealer;
using BergerMsfaApi.Models.Brand;
using System.Threading.Tasks;
using X.PagedList;
using BergerMsfaApi.Models.Common;

namespace BergerMsfaApi.Services.Brand.Interfaces
{
    public interface IBrandService
    {
        Task<QueryResultModel<BrandInfoModel>> GetBrandsAsync(QueryObjectModel query);
        Task<BrandInfoModel> GetBrandById(int id);
        Task<bool> BrandStatusUpdate(BrandStatusModel brandStatus);
    }
}
