using Berger.Data.MsfaEntity.Hirearchy;
using Berger.Data.MsfaEntity.Master;
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
        Task<IEnumerable<Depot>> GetDepotList();
        Task<IEnumerable<RoleModel>> GetRoleList();
        Task<IEnumerable<DealerModel>> GetDealerListByCode(string code);
        Task<IEnumerable<AppDealerInfoModel>> AppGetDealerInfoList(string territory);
        Task<IEnumerable<AppDealerInfoModel>> AppGetFocusDealerInfoList(string EmployeeId);
        Task<IEnumerable<AppDealerInfoModel>> AppGetDealerInfoListByUserCategory(string userCategory, List<string> userCategoryIds);
    }
}
