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
        private readonly IRepository<UserInfo> _userInfoSvc;
        public CommonService(
            IRepository<DealerInfo> dealerInfoSvc,
            IRepository<UserInfo> userInfoSvc,
        IRepository<Zone> zoneSvc,
            IRepository<Territory> territorySvc,
            IRepository<SaleGroup> saleGroupSvc,
            IRepository<SaleOffice> saleOfficeSvc,
            IRepository<Role> roleSvc,
            IRepository<JourneyPlanDetail> journeyPlanDetailSvc,
            IRepository<Depot> depotSvc,
            IRepository<FocusDealer> focusDealerSvc

            )
        {
            _focusDealerSvc = focusDealerSvc;
            _dealerInfoSvc = dealerInfoSvc;
            _userInfoSvc = userInfoSvc;
            _zoneSvc = zoneSvc;
            _territorySvc = territorySvc;
            _saleGroupSvc = saleGroupSvc;
            _saleOfficeSvc = saleOfficeSvc;
            _roleSvc = roleSvc; 
            _journeyPlanDetailSvc = journeyPlanDetailSvc;
            _depotSvc = depotSvc;
        }
        //this method expose dealer list by territory for App
        public async Task<IEnumerable<AppDealerInfoModel>> AppGetDealerInfoList(string territory)
        {
            var result = await _dealerInfoSvc.FindAllAsync(f=>f.Territory== territory && f.IsActive== false);
           return result.ToMap<DealerInfo, AppDealerInfoModel>();
        }
        public async Task<IEnumerable<AppDealerInfoModel>> AppGetFocusDealerInfoList(string EmployeeId)
        {
            var result = await _focusDealerSvc.FindAllAsync(f => f.EmployeeRegId == EmployeeId && f.ValidFrom < DateTime.Now.Date);
            throw new NotImplementedException();
        }
        public async Task<IEnumerable<DealerInfoModel>> GetDealerInfoList(string territory)
        {
            var result = await _dealerInfoSvc.FindAllAsync(f=>f.IsActive==false && f.Territory== territory);
            return result.ToMap<DealerInfo, DealerInfoModel>();
        }

        public async Task<IEnumerable<SaleGroup>> GetSaleGroupList()
        {
            return await _saleGroupSvc.GetAllAsync();
        }
        public async Task<IEnumerable<Depot>> GetDepotList()
        {
            return await _depotSvc.GetAllAsync();
        }
        public async Task<IEnumerable<UserInfoModel>> GetEmployeeList()
        {
            var result= await _userInfoSvc.FindAllAsync(f => f.LinemanagerId == 0/*AppIdentity.AppUser.EmployeeId*/);
            return result.ToMap<UserInfo, UserInfoModel>();

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
    }
}
