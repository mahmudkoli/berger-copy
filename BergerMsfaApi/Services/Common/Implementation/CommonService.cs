using Berger.Common.Enumerations;
using Berger.Data.MsfaEntity;
using Berger.Data.MsfaEntity.DealerFocus;
using Berger.Data.MsfaEntity.Hirearchy;
using Berger.Data.MsfaEntity.Master;
using Berger.Data.MsfaEntity.SAPTables;
using Berger.Data.MsfaEntity.Users;
using BergerMsfaApi.Extensions;
using BergerMsfaApi.Models.Dealer;
using BergerMsfaApi.Models.Users;
using BergerMsfaApi.Repositories;
using BergerMsfaApi.Services.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BergerMsfaApi.Services.Common.Implementation
{
    public class CommonService : ICommonService
    {
        private readonly IRepository<DealerInfo> _dealerInfoSvc;
        private readonly IRepository<Depot> _depotSvc;
        private readonly IRepository<JourneyPlanDetail> _journeyPlanDetailSvc;
        private readonly IRepository<Zone> _zoneSvc;
        private readonly IRepository<Role> _roleSvc;
        private readonly IRepository<Territory> _territorySvc;
        private readonly IRepository<SaleGroup> _saleGroupSvc;
        private readonly IRepository<SaleOffice> _saleOfficeSvc;
        private readonly IRepository<FocusDealer> _focusDealerSvc;
        private readonly IRepository<UserInfo> _userInfosvc;
        private readonly IRepository<Division> _divisionSvc;
        public CommonService(
            IRepository<DealerInfo> dealerInfoSvc,

            IRepository<Zone> zoneSvc,
            IRepository<Territory> territorySvc,
            IRepository<SaleGroup> saleGroupSvc,
            IRepository<SaleOffice> saleOfficeSvc,
            IRepository<Role> roleSvc,
            IRepository<JourneyPlanDetail> journeyPlanDetailSvc,
            IRepository<Depot> depotSvc,
            IRepository<FocusDealer> focusDealerSvc,
            IRepository<UserInfo> userInfosvc,
            IRepository<Division> divisionSvc

            )
        {
            _focusDealerSvc = focusDealerSvc;
            _dealerInfoSvc = dealerInfoSvc;
            _zoneSvc = zoneSvc;
            _territorySvc = territorySvc;
            _saleGroupSvc = saleGroupSvc;
            _saleOfficeSvc = saleOfficeSvc;
            _roleSvc = roleSvc; 
            _journeyPlanDetailSvc = journeyPlanDetailSvc;
            _depotSvc = depotSvc;
            _userInfosvc = userInfosvc;
            _divisionSvc = divisionSvc;
        }

        public async Task<IEnumerable<AppDealerInfoModel>> AppGetDealerInfoList(string territory)
        {
            var result = await _dealerInfoSvc.FindAllAsync(f=>f.Territory== territory);
           return result.ToMap<DealerInfo, AppDealerInfoModel>();
        }

        public async Task<IEnumerable<AppDealerInfoModel>> AppGetFocusDealerInfoList(string EmployeeId)
        {
            var result = await _focusDealerSvc.FindAllAsync(f => f.EmployeeId == EmployeeId && f.ValidFrom < DateTime.Now.Date);
            throw new NotImplementedException();
        }
        public async Task<IEnumerable<DealerInfoModel>> GetDealerInfoList()
        {
            var result = await _dealerInfoSvc.GetAllAsync();
            return result.ToMap<DealerInfo, DealerInfoModel>();
        }
        public async Task<IEnumerable<UserInfoModel>> GetUserInfoList()
        {
           // var result1 = new List<UserInfoModel>();
           // var u = _userInfosvc.GetAll().ToList();
           //var v= Getuser(result1, AppIdentity.AppUser.EmployeeId);
         
            var result = await _userInfosvc.FindAllAsync(f => f.ManagerId == AppIdentity.AppUser.EmployeeId);
            return result.ToMap<UserInfo, UserInfoModel>();
        }
        public async Task<IEnumerable<Division>> GetDivisionList()
        {
            // var result1 = new List<UserInfoModel>();
            // var u = _userInfosvc.GetAll().ToList();
            //var v= Getuser(result1, AppIdentity.AppUser.EmployeeId);

            return await _divisionSvc.GetAllAsync();


        }
        private List<UserInfoModel> Getuser( List<UserInfoModel> result ,string employeeId)
        {
       
            foreach (var u in _userInfosvc.GetAll())
            {
                if (u.ManagerId == employeeId)
                {
                    result.Add(u.ToMap<UserInfo, UserInfoModel>());
                    Getuser(result, u.ManagerId);
                }
               
            }
            return result;
        }
        public async Task<IEnumerable<SaleGroup>> GetSaleGroupList()
        {
            return await _saleGroupSvc.GetAllAsync();
        }
        public async Task<IEnumerable<DepotModel>> GetDepotList()
        {
            var result = await _depotSvc.GetAllAsync();
            return result.Select(s => new DepotModel  { Code = s.Werks, Name = s.Name1 }).ToList();
        }
        public  async Task<IEnumerable<SaleOffice>> GetSaleOfficeList()
        {
            return await _saleOfficeSvc.GetAllAsync();
        }

        public async Task<IEnumerable<Territory>> GetTerritoryList()
        {
            return await _territorySvc.GetAllAsync();
        }

        public async Task<IEnumerable<Zone>> GetZoneList()
        {
            return await _zoneSvc.GetAllAsync();
        }
        public async Task<IEnumerable<RoleModel>> GetRoleList()
        {
            var result= await _roleSvc.GetAllAsync();
            return result.ToMap<Role, RoleModel>();
        }

        public async Task<IEnumerable<AppDealerInfoModel>> AppGetDealerInfoListByUserCategory(string userCategory, List<string> userCategoryIds)
        {
            var columnsMap = new Dictionary<string, Expression<Func<DealerInfo, bool>>>()
            {
                [EnumUserCategory.Plant.ToString()] = f => userCategoryIds.Contains(f.BusinessArea),
                [EnumUserCategory.SalesOffice.ToString()] = f => userCategoryIds.Contains(f.SalesOffice),
                [EnumUserCategory.Area.ToString()] = f => userCategoryIds.Contains(f.Area),
                [EnumUserCategory.Territory.ToString()] = f => userCategoryIds.Contains(f.Territory),
                [EnumUserCategory.Zone.ToString()] = f => userCategoryIds.Contains(f.CustZone)
            };

            var result = (await _dealerInfoSvc.FindAllAsync(columnsMap[userCategory])).ToList();
            return result.ToMap<DealerInfo, AppDealerInfoModel>();
        }
    }
    public class DepotModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
