using Berger.Common.Enumerations;
using Berger.Data.MsfaEntity.Hirearchy;
using Berger.Data.MsfaEntity.Master;
using Berger.Data.MsfaEntity.Users;
using BergerMsfaApi.Models.Dealer;
using BergerMsfaApi.Models.PainterRegistration;
using BergerMsfaApi.Models.Users;
using BergerMsfaApi.Services.Common.Implementation;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BergerMsfaApi.Services.Common.Interfaces
{
    public interface ICommonService
    {
        Task<IEnumerable<DealerInfoModel>> GetDealerInfoList();
        Task<IList<AppDealerInfoModel>> GetDealerListByArea(AreaDealerSearchModel model);
        Task<IEnumerable<UserInfoModel>> GetUserInfoList();
        Task<IEnumerable<UserInfoModel>> GetUserInfoListByCurrentUser();
        Task<IEnumerable<UserInfoModel>> GetUserInfoListByCurrentUserWithoutZoUser();
        Task<IEnumerable<SaleOffice>> GetSaleOfficeList();
        Task<IEnumerable<SaleGroup>> GetSaleGroupList();
        Task<IEnumerable<Territory>> GetTerritoryList();
        Task<IEnumerable<Territory>> GetTerritoryList(IList<string> depots);
        Task<IEnumerable<Zone>> GetZoneList();
        Task<IEnumerable<Zone>> GetZoneList(IList<string> depots, IList<string> territories);
        Task<IEnumerable<DepotModel>> GetDepotList();
        Task<IList<KeyValuePairAreaModel>> GetSaleGroupList(Expression<Func<SaleGroup, bool>> predicate);
        Task<IList<KeyValuePairAreaModel>> GetDepotList(Expression<Func<Depot, bool>> predicate);
        Task<IList<KeyValuePairAreaModel>> GetSaleOfficeList(Expression<Func<SaleOffice, bool>> predicate);
        Task<IList<KeyValuePairAreaModel>> GetTerritoryList(Expression<Func<Territory, bool>> predicate);
        Task<IList<KeyValuePairAreaModel>> GetZoneList(Expression<Func<Zone, bool>> predicate);
        Task<IEnumerable<RoleModel>> GetRoleList();
        Task<IEnumerable<Division>> GetDivisionList();
        Task<IEnumerable<PainterModel>> GetPainterList();
        IEnumerable<MonthModel> GetMonthList();
        IEnumerable<YearModel> GetYearList();
        //Task<IEnumerable<Division>> GetDivisionList(); 
        Task<IEnumerable<CreditControlArea>> GetCreditControlAreaList();
        Task<IEnumerable<AppDealerInfoModel>> AppGetDealerInfoList(string territory);
        Task<IEnumerable<AppDealerInfoModel>> AppGetFocusDealerInfoList(string EmployeeId);
        Task<IEnumerable<AppDealerInfoModel>> AppGetDealerInfoListByUserCategory(string userCategory, List<string> userCategoryIds);
        Task<IEnumerable<AppDealerInfoModel>> AppGetDealerInfoListByCurrentUser(int userId, IList<string> territoryIds = null, bool isDealerSubDealer = false);
        Task<IEnumerable<AppDealerInfoModel>> AppGetDealerInfoListByDealerCategory(AppDealerSearchModel model);
        Task<IEnumerable<AppDealerInfoModel>> AppGetDealerInfoListByCurrentUser(AppDealerSearchModel model);
        Task<IList<AppDealerInfoModel>> AppGetDealerListByArea(AppAreaDealerSearchModel model);
        Task<IList<KeyValuePairModel>> GetPSATZHierarchy(List<string> plantIds, List<string> salesOfficeIds, List<string> areaIds, List<string> territoryIds, List<string> zoneIds);
        Task<IList<KeyValuePairModel>> GetPSTZHierarchy(List<string> plantIds, List<string> salesOfficeIds, List<string> territoryIds, List<string> zoneIds);
        Task<IList<KeyValuePairModel>> GetPATZHierarchy(List<string> plantIds, List<string> areaIds, List<string> territoryIds, List<string> zoneIds);
        Task<IList<KeyValuePairModel>> GetPTZHierarchy(List<string> plantIds, List<string> territoryIds, List<string> zoneIds);
        void SetEmptyString<T>(List<T> items, params string[] propNames);
    }
}
