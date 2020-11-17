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
        #region App
        Task<IEnumerable<AppDealerInfoModel>> AppGetDealerInfoList(string territory);
        #endregion
        #region Portal

        #endregion
        #region Common
        Task<IEnumerable<DealerInfoModel>> GetDealerInfoList(string territory);
        #endregion

        Task<IEnumerable<UserInfoModel>> GetEmployeeList();
        Task<IEnumerable<SaleOffice>> GetSaleOfficeList();
        Task<IEnumerable<SaleGroup>> GetSaleGroupList();
        Task<IEnumerable<Territory>> GetTerritoryList();
        Task<IEnumerable<Zone>> GetZoneList();
        Task<IEnumerable<Depot>> GetDepotList();
        Task<IEnumerable<RoleModel>> GetRoleList();
        Task<IEnumerable<DealerInfoModel>> GetDealerList();

        Task<IEnumerable<AppDealerInfoModel>> AppGetFocusDealerInfoList(string EmployeeId);
    }
}
