using Berger.Data.MsfaEntity.Hirearchy;
using BergerMsfaApi.Models.Dealer;
using BergerMsfaApi.Models.Users;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BergerMsfaApi.Services.Common.Interfaces
{
    public interface ICommonService
    {
        Task<IEnumerable<DealerInfoModel>> GetDealerInfoList();
        Task<IEnumerable<SaleOffice>> GetSaleOfficeList();
        Task<IEnumerable<SaleGroup>> GetSaleGroupList();
        Task<IEnumerable<Territory>> GetTerritoryList();
        Task<IEnumerable<Zone>> GetZoneList();
        Task<IEnumerable<RoleModel>> GetRoleList();
        Task<IEnumerable<AppDealerInfoModel>> AppGetDealerInfoList(string territory);
    }
}
