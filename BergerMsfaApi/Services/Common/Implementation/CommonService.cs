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
        private readonly IRepository<CustomerGroup> _customerGroupSvc;
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
            IRepository<CustomerGroup> customerGroupSvc,
            IRepository<Zone> zoneSvc,
            IRepository<Territory> territorySvc,
            IRepository<SaleGroup> saleGroupSvc,
            IRepository<SaleOffice> saleOfficeSvc,
            IRepository<Role> roleSvc,
            IRepository<JourneyPlanDetail> journeyPlanDetailSvc,
            IRepository<Depot> depotSvc,
            IRepository<FocusDealer> focusDealerSvc,
            IRepository<UserInfo> userInfosvc,
            IRepository<Division> divisionSvc)
        {
            _focusDealerSvc = focusDealerSvc;
            _dealerInfoSvc = dealerInfoSvc;
            _customerGroupSvc = customerGroupSvc;
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
            var result = await _dealerInfoSvc.FindAllAsync(f => f.Territory == territory);
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

        private List<UserInfoModel> Getuser(List<UserInfoModel> result, string employeeId)
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
                [EnumUserCategory.Area.ToString()] = f => userCategoryIds.Contains(f.SalesGroup),
                [EnumUserCategory.Territory.ToString()] = f => userCategoryIds.Contains(f.Territory),
                [EnumUserCategory.Zone.ToString()] = f => userCategoryIds.Contains(f.CustZone)
            };

            //var result = (await _dealerInfoSvc.FindAllAsync(columnsMap[userCategory])).ToList();
            var result = from dealer in (await _dealerInfoSvc.FindAllAsync(columnsMap[userCategory]))
                         join custGrp in (await _customerGroupSvc.FindAllAsync(x => true))
                         on dealer.AccountGroup equals custGrp.CustomerAccountGroup 
                         into cust from cu in cust.DefaultIfEmpty()
                         select new AppDealerInfoModel
                         { // result selector 
                             Id = dealer.Id,
                             CustomerNo = dealer.CustomerNo,
                             CustomerName = dealer.CustomerName,
                             Address = dealer.Address,
                             ContactNo = dealer.ContactNo,
                             Territory = dealer.Territory,
                             IsSubdealer = cu != null ? cu.CustomerAccountGroup.StartsWith("Subdealer") : false
                         };

            return result;
        }

        public async Task<IList<UserZoneAreaMappingModel>> GetUserZoneAreaMappingsAsync(string userCategory, List<string> userCategoryIds)
        {
            var columnsMap = new Dictionary<string, Expression<Func<DealerInfo, bool>>>()
            {
                [EnumUserCategory.Plant.ToString()] = f => userCategoryIds.Contains(f.BusinessArea),
                [EnumUserCategory.SalesOffice.ToString()] = f => userCategoryIds.Contains(f.SalesOffice),
                [EnumUserCategory.Area.ToString()] = f => userCategoryIds.Contains(f.SalesGroup),
                [EnumUserCategory.Territory.ToString()] = f => userCategoryIds.Contains(f.Territory),
                [EnumUserCategory.Zone.ToString()] = f => userCategoryIds.Contains(f.CustZone)
            };

            var result = (await _dealerInfoSvc.FindAllAsync(columnsMap[userCategory])).Select(x => new UserZoneAreaMappingModel() 
                            { 
                                PlantId = x.BusinessArea,
                                SalesOfficeId = x.SalesOffice,
                                AreaId = x.SalesGroup,
                                TerritoryId = x.Territory,
                                ZoneId = x.CustZone,
                            }).ToList();

            //foreach (var item in result)
            //{
            //    var name = string.Empty;

            //    if (EnumUserCategory.Plant.ToString() == userCategory)
            //        name = _depotSvc.Find(x => x.Werks == item.PlantId)?.Name1 ?? string.Empty;
            //    else if (EnumUserCategory.SalesOffice.ToString() == userCategory)
            //        name = _saleOfficeSvc.Find(x => x.Code == item.SalesOfficeId)?.Name ?? string.Empty;
            //    else if (EnumUserCategory.Area.ToString() == userCategory)
            //        name = _saleGroupSvc.Find(x => x.Code == item.AreaId)?.Name ?? string.Empty;
            //    else if (EnumUserCategory.Territory.ToString() == userCategory)
            //        name = _territorySvc.Find(x => x.Code == item.TerritoryId)?.Name ?? string.Empty;
            //    else if (EnumUserCategory.Zone.ToString() == userCategory)
            //        name = _zoneSvc.Find(x => x.Code == item.ZoneId)?.Name ?? string.Empty;

            //    item.Name = name;
            //}
            
            if (EnumUserCategory.Plant.ToString() == userCategory)
            {
                result = result.GroupBy(x => x.PlantId).Select(x =>
                {
                    var g = x.FirstOrDefault();
                    var r = new UserZoneAreaMappingModel();
                    r.PlantId = x.Key;
                    if (g == null) return r;
                    r.SalesOfficeId = g.SalesOfficeId;
                    r.AreaId = g.AreaId;
                    r.TerritoryId = g.TerritoryId;
                    r.ZoneId = g.ZoneId;
                    return r;
                }).ToList();
                var filter = result.Select(s => s.PlantId);
                var data = _depotSvc.FindAll(x => filter.Contains(x.Werks)).ToList();
                foreach (var item in result)
                    SetName<Depot>(item, data, x => x.Werks == item.PlantId, x => x.Name1);
            }
            else if (EnumUserCategory.SalesOffice.ToString() == userCategory)
            {
                result = result.GroupBy(x => x.SalesOfficeId).Select(x =>
                {
                    var g = x.FirstOrDefault();
                    var r = new UserZoneAreaMappingModel();
                    r.SalesOfficeId = x.Key;
                    if (g == null) return r;
                    r.PlantId = g.PlantId;
                    r.AreaId = g.AreaId;
                    r.TerritoryId = g.TerritoryId;
                    r.ZoneId = g.ZoneId;
                    return r;
                }).ToList();
                var filter = result.Select(s => s.SalesOfficeId);
                var data = _saleOfficeSvc.FindAll(x => filter.Contains(x.Code)).ToList();
                foreach (var item in result)
                    SetName<SaleOffice>(item, data, x => x.Code == item.SalesOfficeId, x => x.Name);
            }
            else if (EnumUserCategory.Area.ToString() == userCategory)
            {
                result = result.GroupBy(x => x.AreaId).Select(x =>
                {
                    var g = x.FirstOrDefault();
                    var r = new UserZoneAreaMappingModel();
                    r.AreaId = x.Key;
                    if (g == null) return r;
                    r.PlantId = g.PlantId;
                    r.SalesOfficeId = g.SalesOfficeId;
                    r.TerritoryId = g.TerritoryId;
                    r.ZoneId = g.ZoneId;
                    return r;
                }).ToList();
                var filter = result.Select(s => s.AreaId);
                var data = _saleGroupSvc.FindAll(x => filter.Contains(x.Code)).ToList();
                foreach (var item in result)
                    SetName<SaleGroup>(item, data, x => x.Code == item.AreaId, x => x.Name);
            }
            else if (EnumUserCategory.Territory.ToString() == userCategory)
            {
                result = result.GroupBy(x => x.TerritoryId).Select(x =>
                {
                    var g = x.FirstOrDefault();
                    var r = new UserZoneAreaMappingModel();
                    r.TerritoryId = x.Key;
                    if (g == null) return r;
                    r.PlantId = g.PlantId;
                    r.SalesOfficeId = g.SalesOfficeId;
                    r.AreaId = g.AreaId;
                    r.ZoneId = g.ZoneId;
                    return r;
                }).ToList();
                var filter = result.Select(s => s.TerritoryId);
                var data = _territorySvc.FindAll(x => filter.Contains(x.Code)).ToList();
                foreach (var item in result)
                    SetName<Territory>(item, data, x => x.Code == item.TerritoryId, x => x.Name);
            }
            else if (EnumUserCategory.Zone.ToString() == userCategory)
            {
                result = result.GroupBy(x => x.ZoneId).Select(x =>
                {
                    var g = x.FirstOrDefault();
                    var r = new UserZoneAreaMappingModel();
                    r.ZoneId = x.Key;
                    if (g == null) return r;
                    r.PlantId = g.PlantId;
                    r.SalesOfficeId = g.SalesOfficeId;
                    r.AreaId = g.AreaId;
                    r.TerritoryId = g.TerritoryId;
                    return r;
                }).ToList();
                var filter = result.Select(s => s.ZoneId);
                var data = _zoneSvc.FindAll(x => filter.Contains(x.Code)).ToList();
                foreach (var item in result)
                    SetName<Zone>(item, data, x => x.Code == item.ZoneId, x => x.Name);
            }

            return result;
        }

        private void SetName<T>(UserZoneAreaMappingModel item,
            List<T> data, 
            Func<T, bool> predicate,
            Func<T, string> selector)
        {
            var name = data.Where(predicate).Select(selector).FirstOrDefault() ?? string.Empty;
            if (item == null) return;
            item.Name = name;
        }
    }

    public class DepotModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }

    public class UserZoneAreaMappingModel
    {
        public string PlantId { get; set; } // BusinessArea, Depot
        public string SalesOfficeId { get; set; }
        public string AreaId { get; set; } // SalesGroup
        public string TerritoryId { get; set; }
        public string ZoneId { get; set; } // CustZone
        public string Name { get; set; }
    }
}
