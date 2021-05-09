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
        Task<IEnumerable<UserInfoModel>> GetUserInfoList();
        Task<IEnumerable<UserInfoModel>> GetUserInfoListByLoggedInManager();
        Task<IEnumerable<SaleOffice>> GetSaleOfficeList();
        Task<IEnumerable<SaleGroup>> GetSaleGroupList();
        Task<IEnumerable<Territory>> GetTerritoryList();
        Task<IEnumerable<Zone>> GetZoneList();
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
        Task<IEnumerable<AppDealerInfoModel>> AppGetDealerInfoListByCurrentUser(int userId);
        Task<IEnumerable<AppDealerInfoModel>> AppGetDealerInfoListByDealerCategory(AppDealerSearchModel model);
        Task<IEnumerable<AppDealerInfoModel>> AppGetDealerInfoListByCurrentUser(AppDealerSearchModel model);
        Task<IList<KeyValuePairModel>> GetPSATZHierarchy(List<string> plantIds, List<string> salesOfficeIds, List<string> areaIds, List<string> territoryIds, List<string> zoneIds);
        Task<IList<KeyValuePairModel>> GetPATZHierarchy(List<string> plantIds, List<string> areaIds, List<string> territoryIds, List<string> zoneIds);
        Task<IList<KeyValuePairModel>> GetPTZHierarchy(List<string> plantIds, List<string> territoryIds, List<string> zoneIds);
        void SetEmptyString<T>(List<T> items, params string[] propNames);
    }
}
