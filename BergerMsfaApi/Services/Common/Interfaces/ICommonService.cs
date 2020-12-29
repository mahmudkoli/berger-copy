using Berger.Data.MsfaEntity.Hirearchy;
using Berger.Data.MsfaEntity.Master;
using BergerMsfaApi.Models.Dealer;
using BergerMsfaApi.Models.Users;
using BergerMsfaApi.Services.Common.Implementation;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BergerMsfaApi.Services.Common.Interfaces
{
    public interface ICommonService
    {
        Task<IEnumerable<DealerInfoModel>> GetDealerInfoList();
        Task<IEnumerable<UserInfoModel>> GetUserInfoList();
        Task<IEnumerable<SaleOffice>> GetSaleOfficeList();
        Task<IEnumerable<SaleGroup>> GetSaleGroupList();
        Task<IEnumerable<Territory>> GetTerritoryList();
        Task<IEnumerable<Zone>> GetZoneList();
        Task<IEnumerable<DepotModel>> GetDepotList();
        Task<IEnumerable<RoleModel>> GetRoleList();
        Task<IEnumerable<Division>> GetDivisionList();
        Task<IEnumerable<AppDealerInfoModel>> AppGetDealerInfoList(string territory);
        Task<IEnumerable<AppDealerInfoModel>> AppGetFocusDealerInfoList(string EmployeeId);
        Task<IEnumerable<AppDealerInfoModel>> AppGetDealerInfoListByUserCategory(string userCategory, List<string> userCategoryIds);
    }
}
