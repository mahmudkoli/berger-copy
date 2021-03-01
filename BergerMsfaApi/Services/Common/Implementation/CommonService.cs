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
                             IsSubdealer = cu.IsSubdealer()
                         };

            return result;
        }

        public async Task<IEnumerable<AppDealerInfoModel>> AppGetDealerInfoListByDealerCategory(AppDealerSearchModel model)
        {
            model.PageNo = model.PageNo ?? 1;
            model.PageSize = model.PageSize ?? int.MaxValue;
            model.DealerCategory = model.DealerCategory ?? EnumDealerCategory.All;
            model.DealerName = model.DealerName ?? string.Empty;
            var columnsMap = new Dictionary<string, Expression<Func<DealerInfo, bool>>>()
            {
                [EnumUserCategory.Plant.ToString()] = f => model.UserCategoryIds.Contains(f.BusinessArea),
                [EnumUserCategory.SalesOffice.ToString()] = f => model.UserCategoryIds.Contains(f.SalesOffice),
                [EnumUserCategory.Area.ToString()] = f => model.UserCategoryIds.Contains(f.SalesGroup),
                [EnumUserCategory.Territory.ToString()] = f => model.UserCategoryIds.Contains(f.Territory),
                [EnumUserCategory.Zone.ToString()] = f => model.UserCategoryIds.Contains(f.CustZone)
            };

            //var result = (await _dealerInfoSvc.FindAllAsync(columnsMap[userCategory])).ToList();
            //var result = (from dealer in _dealerInfoSvc.GetAll()
            var result = (from dealer in _dealerInfoSvc.FindAll(columnsMap[model.UserCategory])
                         join custGrp in _customerGroupSvc.GetAll()
                         on dealer.AccountGroup equals custGrp.CustomerAccountGroup
                         into cust
                         from cu in cust.DefaultIfEmpty()

                         join focusDealer in _focusDealerSvc.GetAll()
                         on dealer.Id equals focusDealer.Code
                         into focus
                         from fd in focus.DefaultIfEmpty()

                         where ((EnumDealerCategory.All == model.DealerCategory) ||
                         //(EnumDealerCategory.Focus == model.DealerCategory && fd.IsFocused()))
                         (EnumDealerCategory.Focus == model.DealerCategory && fd != null && fd.Code > 0 && fd.ValidTo != null && fd.ValidTo.Date >= DateTime.Now.Date))
                         && (dealer.CustomerName.Contains(model.DealerName))

                         select new AppDealerInfoModel
                         { // result selector 
                             Id = dealer.Id,
                             CustomerNo = dealer.CustomerNo,
                             CustomerName = dealer.CustomerName,
                             Address = dealer.Address,
                             ContactNo = dealer.ContactNo,
                             Territory = dealer.Territory,
                             IsSubdealer = cu.IsSubdealer(),
                             //IsFocused = fd.IsFocused(),
                             IsFocused = fd != null && fd.Code > 0 && fd.ValidTo != null && fd.ValidTo.Date >= DateTime.Now.Date,
                         }).Skip((model.PageNo.Value-1)* model.PageSize.Value).Take(model.PageSize.Value).ToList();

            return result;
        }

        public async Task<IList<KeyValuePairModel>> GetPSATZHierarchy(List<string> plantIds, List<string> salesOfficeIds, List<string> areaIds, List<string> territoryIds, List<string> zoneIds)
        {
            var plants = (await GetPSATZMappingsAsync(EnumUserCategory.Plant.ToString(), plantIds, new List<string>(), EnumTypeOfEmployeeHierarchy.PSATZ))
                                    .Select(x => new KeyValuePairModel() { Id = x.PlantId, Name = x.Name }).ToList();
            var salesOffices = (await GetPSATZMappingsAsync(EnumUserCategory.SalesOffice.ToString(), salesOfficeIds, plantIds, EnumTypeOfEmployeeHierarchy.PSATZ))
                                .Select(x => new KeyValuePairModel() { Id = x.SalesOfficeId, Name = x.Name, ParentId = x.PlantId }).ToList();
            var areas = (await GetPSATZMappingsAsync(EnumUserCategory.Area.ToString(), areaIds, salesOfficeIds, EnumTypeOfEmployeeHierarchy.PSATZ))
                                .Select(x => new KeyValuePairModel() { Id = x.AreaId, Name = x.Name, ParentId = x.SalesOfficeId }).ToList();
            var territories = (await GetPSATZMappingsAsync(EnumUserCategory.Territory.ToString(), territoryIds, areaIds, EnumTypeOfEmployeeHierarchy.PSATZ))
                                .Select(x => new KeyValuePairModel() { Id = x.TerritoryId, Name = x.Name, ParentId = x.AreaId }).ToList();
            var zones = (await GetPSATZMappingsAsync(EnumUserCategory.Zone.ToString(), zoneIds, territoryIds, EnumTypeOfEmployeeHierarchy.PSATZ))
                                .Select(x => new KeyValuePairModel() { Id = x.ZoneId, Name = x.Name, ParentId = x.TerritoryId }).ToList();

            foreach (var plant in plants)
            {
                plant.Children = salesOffices.Where(x => x.ParentId == plant.Id).ToList();
                foreach (var salesOffice in plant.Children)
                {
                    salesOffice.Children = areas.Where(x => x.ParentId == salesOffice.Id).ToList();
                    foreach (var area in salesOffice.Children)
                    {
                        area.Children = territories.Where(x => x.ParentId == area.Id).ToList();
                        foreach (var territory in area.Children)
                        {
                            territory.Children = zones.Where(x => x.ParentId == territory.Id).ToList();
                        }
                    }
                }
            }

            return plants;
        }
        
        public async Task<IList<KeyValuePairModel>> GetPATZHierarchy(List<string> plantIds, List<string> areaIds, List<string> territoryIds, List<string> zoneIds)
        {
            var plants = (await GetPSATZMappingsAsync(EnumUserCategory.Plant.ToString(), plantIds, new List<string>(), EnumTypeOfEmployeeHierarchy.PATZ))
                                    .Select(x => new KeyValuePairModel() { Id = x.PlantId, Name = x.Name }).ToList();
            var areas = (await GetPSATZMappingsAsync(EnumUserCategory.Area.ToString(), areaIds, plantIds, EnumTypeOfEmployeeHierarchy.PATZ))
                                .Select(x => new KeyValuePairModel() { Id = x.AreaId, Name = x.Name, ParentId = x.PlantId }).ToList();
            var territories = (await GetPSATZMappingsAsync(EnumUserCategory.Territory.ToString(), territoryIds, areaIds, EnumTypeOfEmployeeHierarchy.PATZ))
                                .Select(x => new KeyValuePairModel() { Id = x.TerritoryId, Name = x.Name, ParentId = x.AreaId }).ToList();
            var zones = (await GetPSATZMappingsAsync(EnumUserCategory.Zone.ToString(), zoneIds, territoryIds, EnumTypeOfEmployeeHierarchy.PATZ))
                                .Select(x => new KeyValuePairModel() { Id = x.ZoneId, Name = x.Name, ParentId = x.TerritoryId }).ToList();

            foreach (var plant in plants)
            {
                plant.Children = areas.Where(x => x.ParentId == plant.Id).ToList();
                foreach (var area in plant.Children)
                {
                    area.Children = territories.Where(x => x.ParentId == area.Id).ToList();
                    foreach (var territory in area.Children)
                    {
                        territory.Children = zones.Where(x => x.ParentId == territory.Id).ToList();
                    }
                }
            }

            return plants;
        }

        public async Task<IList<KeyValuePairModel>> GetPTZHierarchy(List<string> plantIds, List<string> territoryIds, List<string> zoneIds)
        {
            var plants = (await GetPSATZMappingsAsync(EnumUserCategory.Plant.ToString(), plantIds, new List<string>(), EnumTypeOfEmployeeHierarchy.PTZ))
                                    .Select(x => new KeyValuePairModel() { Id = x.PlantId, Name = x.Name }).ToList();
            var territories = (await GetPSATZMappingsAsync(EnumUserCategory.Territory.ToString(), territoryIds, plantIds, EnumTypeOfEmployeeHierarchy.PTZ))
                                .Select(x => new KeyValuePairModel() { Id = x.TerritoryId, Name = x.Name, ParentId = x.PlantId }).ToList();
            var zones = (await GetPSATZMappingsAsync(EnumUserCategory.Zone.ToString(), zoneIds, territoryIds, EnumTypeOfEmployeeHierarchy.PTZ))
                                .Select(x => new KeyValuePairModel() { Id = x.ZoneId, Name = x.Name, ParentId = x.TerritoryId }).ToList();

            foreach (var plant in plants)
            {
                plant.Children = territories.Where(x => x.ParentId == plant.Id).ToList();
                foreach (var territory in plant.Children)
                {
                    territory.Children = zones.Where(x => x.ParentId == territory.Id).ToList();
                }
            }

            return plants;
        }

        private async Task<IList<PSATZMappingModel>> GetPSATZMappingsAsync(string userCategory, List<string> userCategoryIds, List<string> userParentCategoryIds, EnumTypeOfEmployeeHierarchy typeOfEmployeeHierarchy)
        {
            var columnsMap = new Dictionary<string, Expression<Func<DealerInfo, bool>>>();

            if (typeOfEmployeeHierarchy == EnumTypeOfEmployeeHierarchy.PSATZ)
            {
                columnsMap = new Dictionary<string, Expression<Func<DealerInfo, bool>>>()
                {
                    [EnumUserCategory.Plant.ToString()] = f => userCategoryIds.Contains(f.BusinessArea),
                    [EnumUserCategory.SalesOffice.ToString()] = f => userCategoryIds.Contains(f.SalesOffice) && (!userParentCategoryIds.Any() || userParentCategoryIds.Contains(f.BusinessArea)),
                    [EnumUserCategory.Area.ToString()] = f => userCategoryIds.Contains(f.SalesGroup) && (!userParentCategoryIds.Any() || userParentCategoryIds.Contains(f.SalesOffice)),
                    [EnumUserCategory.Territory.ToString()] = f => userCategoryIds.Contains(f.Territory) && (!userParentCategoryIds.Any() || userParentCategoryIds.Contains(f.SalesGroup)),
                    [EnumUserCategory.Zone.ToString()] = f => userCategoryIds.Contains(f.CustZone) && (!userParentCategoryIds.Any() || userParentCategoryIds.Contains(f.Territory))
                };
            }
            else if (typeOfEmployeeHierarchy == EnumTypeOfEmployeeHierarchy.PATZ)
            {
                columnsMap = new Dictionary<string, Expression<Func<DealerInfo, bool>>>()
                {
                    [EnumUserCategory.Plant.ToString()] = f => userCategoryIds.Contains(f.BusinessArea),
                    [EnumUserCategory.Area.ToString()] = f => userCategoryIds.Contains(f.SalesGroup) && (!userParentCategoryIds.Any() || userParentCategoryIds.Contains(f.BusinessArea)),
                    [EnumUserCategory.Territory.ToString()] = f => userCategoryIds.Contains(f.Territory) && (!userParentCategoryIds.Any() || userParentCategoryIds.Contains(f.SalesGroup)),
                    [EnumUserCategory.Zone.ToString()] = f => userCategoryIds.Contains(f.CustZone) && (!userParentCategoryIds.Any() || userParentCategoryIds.Contains(f.Territory))
                };
            }
            else if (typeOfEmployeeHierarchy == EnumTypeOfEmployeeHierarchy.PTZ)
            {
                columnsMap = new Dictionary<string, Expression<Func<DealerInfo, bool>>>()
                {
                    [EnumUserCategory.Plant.ToString()] = f => userCategoryIds.Contains(f.BusinessArea),
                    [EnumUserCategory.Territory.ToString()] = f => userCategoryIds.Contains(f.Territory) && (!userParentCategoryIds.Any() || userParentCategoryIds.Contains(f.BusinessArea)),
                    [EnumUserCategory.Zone.ToString()] = f => userCategoryIds.Contains(f.CustZone) && (!userParentCategoryIds.Any() || userParentCategoryIds.Contains(f.Territory))
                };
            }

            var result = (await _dealerInfoSvc.FindAllAsync(columnsMap[userCategory])).Select(x => new PSATZMappingModel()
            {
                PlantId = x.BusinessArea,
                SalesOfficeId = x.SalesOffice,
                AreaId = x.SalesGroup,
                TerritoryId = x.Territory,
                ZoneId = x.CustZone,
            }).ToList();

            if (EnumUserCategory.Plant.ToString() == userCategory)
            {
                result = result.GroupBy(x => x.PlantId).Select(x =>
                {
                    var g = x.FirstOrDefault();
                    var r = new PSATZMappingModel();
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
                    var r = new PSATZMappingModel();
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
                    var r = new PSATZMappingModel();
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
                    var r = new PSATZMappingModel();
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
                    var r = new PSATZMappingModel();
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

        private void SetName<T>(PSATZMappingModel item,
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

    public class PSATZMappingModel
    {
        public string PlantId { get; set; } // BusinessArea, Depot
        public string SalesOfficeId { get; set; }
        public string AreaId { get; set; } // SalesGroup
        public string TerritoryId { get; set; }
        public string ZoneId { get; set; } // CustZone
        public string Name { get; set; }
    }

    public enum EnumTypeOfEmployeeHierarchy
    {
        PSATZ = 1, //Plant > SalesOffice > Area > Territory > Zone
        PATZ = 2, //Plant > Area > Territory > Zone
        PTZ = 3 //Plant > Territory > Zone
    }
}
