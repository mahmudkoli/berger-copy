using Berger.Common.Constants;
using Berger.Common.Enumerations;
using Berger.Data.MsfaEntity;
using Berger.Data.MsfaEntity.DealerFocus;
using Berger.Data.MsfaEntity.Hirearchy;
using Berger.Data.MsfaEntity.Master;
using Berger.Data.MsfaEntity.PainterRegistration;
using Berger.Data.MsfaEntity.SAPTables;
using Berger.Data.MsfaEntity.Users;
using BergerMsfaApi.Extensions;
using BergerMsfaApi.Models.Dealer;
using BergerMsfaApi.Models.PainterRegistration;
using BergerMsfaApi.Models.Users;
using BergerMsfaApi.Repositories;
using BergerMsfaApi.Services.Common.Interfaces;
using BergerMsfaApi.Services.Users.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
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
        private readonly IRepository<Painter> _painterSvc;
        private readonly IRepository<CreditControlArea> _creditControlAreaSvc;
        private readonly IUserInfoService _userService;

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
            IRepository<Division> divisionSvc,
            IRepository<Painter> painterSvc,
            IRepository<CreditControlArea> creditControlAreaSvc,
            IUserInfoService user)
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
            _painterSvc = painterSvc;
            _creditControlAreaSvc = creditControlAreaSvc;
            _userService = user;
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
         
            var result = await _userInfosvc.FindAllAsync(f => f.Status == Status.Active);
            return result.ToMap<UserInfo, UserInfoModel>();
        }

        public async Task<IEnumerable<UserInfoModel>> GetUserInfoListByCurrentUser()
        {
            // var result1 = new List<UserInfoModel>();
            // var u = _userInfosvc.GetAll().ToList();
            //var v= Getuser(result1, AppIdentity.AppUser.EmployeeId);

            //var userId = AppIdentity.AppUser.UserId;
            //var userInfo = await _userService.GetUserAsync(userId);
            var appUser = AppIdentity.AppUser;

            var result = await _userInfosvc.FindAllAsync(f => f.Status == Status.Active && ((appUser.EmployeeRole == (int)EnumEmployeeRole.Admin || appUser.EmployeeRole == (int)EnumEmployeeRole.GM) || 
                                (f.ManagerId == appUser.EmployeeId || f.EmployeeId == appUser.EmployeeId)));
            return result.ToMap<UserInfo, UserInfoModel>();
        }

        public async Task<IEnumerable<UserInfoModel>> GetUserInfoListByCurrentUserWithoutZoUser()
        {
            var appUser = AppIdentity.AppUser;

            var result = await _userInfosvc.FindAllAsync(f => f.Status == Status.Active && (((appUser.EmployeeRole == (int)EnumEmployeeRole.Admin || appUser.EmployeeRole == (int)EnumEmployeeRole.GM) || 
                                (f.ManagerId == appUser.EmployeeId || f.EmployeeId == appUser.EmployeeId)) 
                                && (f.EmployeeRole != EnumEmployeeRole.ZO)));
            return result.ToMap<UserInfo, UserInfoModel>();
        }

        public async Task<IEnumerable<Division>> GetDivisionList()
        {
            // var result1 = new List<UserInfoModel>();
            // var u = _userInfosvc.GetAll().ToList();
            //var v= Getuser(result1, AppIdentity.AppUser.EmployeeId);

            return await _divisionSvc.GetAllAsync();
        }

        public async Task<IEnumerable<CreditControlArea>> GetCreditControlAreaList()
        {
            return await _creditControlAreaSvc.GetAllAsync(); ;
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
            var appUser = AppIdentity.AppUser;
            var result =  await _saleGroupSvc.FindAllAsync(x => ((int)EnumEmployeeRole.Admin == appUser.EmployeeRole || appUser.EmployeeRole == (int)EnumEmployeeRole.GM) || (appUser.SalesAreaIdList.Contains(x.Code)));
            return result.Select(s => new SaleGroup { Code = s.Code, Name = $"{s.Name} ({s.Code})" }).OrderBy(x => x.Name).ToList();
        }

        public async Task<IEnumerable<DepotModel>> GetDepotList()
        {
            var appUser = AppIdentity.AppUser;
            var result = await _depotSvc.FindAllAsync(x => ((int)EnumEmployeeRole.Admin == appUser.EmployeeRole || appUser.EmployeeRole == (int)EnumEmployeeRole.GM) || (appUser.PlantIdList.Contains(x.Werks)));
            return result.Select(s => new DepotModel  { Code = s.Werks, Name = $"{s.Name1} ({s.Werks})" }).OrderBy(x => x.Name).ToList();
        }

        public  async Task<IEnumerable<SaleOffice>> GetSaleOfficeList()
        {
            var appUser = AppIdentity.AppUser;
            var result = await _saleOfficeSvc.FindAllAsync(x => ((int)EnumEmployeeRole.Admin == appUser.EmployeeRole || appUser.EmployeeRole == (int)EnumEmployeeRole.GM) || (appUser.SalesOfficeIdList.Contains(x.Code)));
            return result.Select(s => new SaleOffice { Code = s.Code, Name = $"{s.Name} ({s.Code})" }).OrderBy(x => x.Name).ToList();
        }

        public async Task<IEnumerable<Territory>> GetTerritoryList()
        {
            var appUser = AppIdentity.AppUser;
            var result = await _territorySvc.FindAllAsync(x => ((int)EnumEmployeeRole.Admin == appUser.EmployeeRole || appUser.EmployeeRole == (int)EnumEmployeeRole.GM) || (appUser.TerritoryIdList.Contains(x.Code)));
            return result.Select(s => new Territory { Code = s.Code, Name = s.Code }).OrderBy(x => x.Name).ToList();
        }

        public async Task<IEnumerable<Zone>> GetZoneList()
        {
            var appUser = AppIdentity.AppUser;
            var result = await _zoneSvc.FindAllAsync(x => ((int)EnumEmployeeRole.Admin == appUser.EmployeeRole || appUser.EmployeeRole == (int)EnumEmployeeRole.GM) || (appUser.ZoneIdList.Contains(x.Code)));
            return result.Select(s => new Zone { Code = s.Code, Name = s.Code }).OrderBy(x => x.Name).ToList();
        }

        public async Task<IList<KeyValuePairAreaModel>> GetSaleGroupList(Expression<Func<SaleGroup, bool>> predicate)
        {
            var result = await _saleGroupSvc.FindAllAsync(predicate);
            return result.Select(s => new KeyValuePairAreaModel { Code = s.Code, Name = $"{s.Name} ({s.Code})" }).OrderBy(x => x.Name).ToList();
        }

        public async Task<IList<KeyValuePairAreaModel>> GetDepotList(Expression<Func<Depot, bool>> predicate)
        {
            var appUser = AppIdentity.AppUser;
            var result = await _depotSvc.FindAllAsync(predicate);
            return result.Select(s => new KeyValuePairAreaModel { Code = s.Werks, Name = $"{s.Name1} ({s.Werks})" }).OrderBy(x => x.Name).ToList();
        }

        public async Task<IList<KeyValuePairAreaModel>> GetSaleOfficeList(Expression<Func<SaleOffice, bool>> predicate)
        {
            var appUser = AppIdentity.AppUser;
            var result = await _saleOfficeSvc.FindAllAsync(predicate);
            return result.Select(s => new KeyValuePairAreaModel { Code = s.Code, Name = $"{s.Name} ({s.Code})" }).OrderBy(x => x.Name).ToList();
        }

        public async Task<IList<KeyValuePairAreaModel>> GetTerritoryList(Expression<Func<Territory, bool>> predicate)
        {
            var appUser = AppIdentity.AppUser;
            var result = await _territorySvc.FindAllAsync(predicate);
            return result.Select(s => new KeyValuePairAreaModel { Code = s.Code, Name = s.Code }).OrderBy(x => x.Name).ToList();
        }

        public async Task<IList<KeyValuePairAreaModel>> GetZoneList(Expression<Func<Zone, bool>> predicate)
        {
            var appUser = AppIdentity.AppUser;
            var result = await _zoneSvc.FindAllAsync(predicate);
            return result.Select(s => new KeyValuePairAreaModel { Code = s.Code, Name = s.Code }).OrderBy(x => x.Name).ToList();
        }

        public async Task<IEnumerable<RoleModel>> GetRoleList()
        {
            var result= await _roleSvc.FindAllAsync(f => f.Status == Status.Active);
            return result.ToMap<Role, RoleModel>();
        }

        public async Task<IEnumerable<PainterModel>> GetPainterList()
        {
            var result = await _painterSvc.GetAllAsync();
            foreach (var item in result)
            {
                item.PainterName = item.PainterName + " (" + item.PainterNo + ")";
            }
            return result.ToMap<Painter, PainterModel>();
        }

        public IEnumerable<MonthModel> GetMonthList()
        {
            var months = CultureInfo.CurrentCulture.DateTimeFormat.MonthNames;

            List<MonthModel> monthModels = new List<MonthModel>();
            
            for (int i = 0; i < months.Length; i++)
            {
                monthModels.Add(new MonthModel { Id = i + 1, Name = months[i] });
            }
            return monthModels;
        }

        public IEnumerable<YearModel> GetYearList()
        {
            List<YearModel> yearModels = new List<YearModel>();
            var currentYear = DateTime.Now.Year;

            yearModels.Add(new YearModel { Id = currentYear + 1, Name = (currentYear + 1).ToString() });

            for (int i = 0; i < 5; i++)
            {
                yearModels.Add(new YearModel { Id = currentYear - i, Name = (currentYear - i).ToString() });
            }
            return yearModels;
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
                             IsSubdealer = cu != null && !string.IsNullOrEmpty(cu.Description) && cu.Description.StartsWith("Subdealer")
                         };

            return result;
        }
        
        public async Task<IEnumerable<AppDealerInfoModel>> AppGetDealerInfoListByCurrentUser(int userId)
        {
            //var userId = AppIdentity.AppUser.UserId;
            var userInfo = await _userService.GetUserAsync(userId);

            Expression<Func<DealerInfo, bool>> dealerPredicate = (x) => !x.IsDeleted && 
                x.Channel == ConstantsODataValue.DistrbutionChannelDealer && 
                x.Division == ConstantsODataValue.DivisionDecorative && 
                ((userInfo.EmployeeRole == EnumEmployeeRole.Admin || userInfo.EmployeeRole == EnumEmployeeRole.GM) ||
                ((!(userInfo.PlantIds != null && userInfo.PlantIds.Any()) || userInfo.PlantIds.Contains(x.BusinessArea)) &&
                (!(userInfo.SaleOfficeIds != null && userInfo.SaleOfficeIds.Any()) || userInfo.SaleOfficeIds.Contains(x.SalesOffice)) &&
                (!(userInfo.AreaIds != null && userInfo.AreaIds.Any()) || userInfo.AreaIds.Contains(x.SalesGroup)) &&
                (!(userInfo.TerritoryIds != null && userInfo.TerritoryIds.Any()) || userInfo.TerritoryIds.Contains(x.Territory)) &&
                (!(userInfo.ZoneIds != null && userInfo.ZoneIds.Any()) || userInfo.ZoneIds.Contains(x.CustZone))));

            var result = from dealer in (await _dealerInfoSvc.FindAllAsync(dealerPredicate))
                         join custGrp in (await _customerGroupSvc.FindAllAsync(x => true))
                         on dealer.AccountGroup equals custGrp.CustomerAccountGroup 
                         into cust from cu in cust.DefaultIfEmpty()
                         select new AppDealerInfoModel
                         { 
                            Id = dealer.Id,
                            CustomerNo = dealer.CustomerNo,
                            CustomerName = $"{dealer.CustomerName} ({dealer.CustomerNo})",
                            Address = dealer.Address,
                            ContactNo = dealer.ContactNo,
                            Territory = dealer.Territory,
                            IsSubdealer = cu != null && !string.IsNullOrEmpty(cu.Description) && cu.Description.StartsWith("Subdealer")
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
                //[EnumUserCategory.Plant.ToString()] = f => model.UserCategoryIds.Contains(f.BusinessArea),
                //[EnumUserCategory.SalesOffice.ToString()] = f => model.UserCategoryIds.Contains(f.SalesOffice),
                //[EnumUserCategory.Area.ToString()] = f => model.UserCategoryIds.Contains(f.SalesGroup),
                //[EnumUserCategory.Territory.ToString()] = f => model.UserCategoryIds.Contains(f.Territory),
                //[EnumUserCategory.Zone.ToString()] = f => model.UserCategoryIds.Contains(f.CustZone)
            };

            //var result = (await _dealerInfoSvc.FindAllAsync(columnsMap[userCategory])).ToList();
            //var result = (from dealer in _dealerInfoSvc.GetAll()
            //var result = (from dealer in _dealerInfoSvc.FindAll(columnsMap[model.UserCategory])
            var result = (from dealer in _dealerInfoSvc.GetAll()
                         join custGrp in _customerGroupSvc.GetAll()
                         on dealer.AccountGroup equals custGrp.CustomerAccountGroup
                         into cust
                         from cu in cust.DefaultIfEmpty()

                         join focusDealer in _focusDealerSvc.GetAll()
                         on dealer.Id equals focusDealer.DealerId
                         into focus
                         from fd in focus.DefaultIfEmpty()

                         where ((EnumDealerCategory.All == model.DealerCategory) ||
                         //(EnumDealerCategory.Focus == model.DealerCategory && fd.IsFocused()))
                         (EnumDealerCategory.Focus == model.DealerCategory && fd != null && fd.DealerId > 0 && fd.ValidTo.Date >= DateTime.Now.Date &&
                             fd.ValidFrom.Date <= DateTime.Now.Date))
                         && (dealer.CustomerName.Contains(model.DealerName))

                         select new AppDealerInfoModel
                         { // result selector 
                             Id = dealer.Id,
                             CustomerNo = dealer.CustomerNo,
                             CustomerName = dealer.CustomerName,
                             Address = dealer.Address,
                             ContactNo = dealer.ContactNo,
                             Territory = dealer.Territory,
                             IsSubdealer = cu != null && !string.IsNullOrEmpty(cu.Description) && cu.Description.StartsWith("Subdealer"),
                             //IsFocused = fd.IsFocused(),
                             IsFocused = fd != null && fd.DealerId > 0 && fd.ValidTo.Date >= DateTime.Now.Date &&
                             fd.ValidFrom.Date <= DateTime.Now.Date,
                         }).Skip((model.PageNo.Value-1)* model.PageSize.Value).Take(model.PageSize.Value).ToList();

            return result;
        }

        public async Task<IEnumerable<AppDealerInfoModel>> AppGetDealerInfoListByCurrentUser(AppDealerSearchModel model)
        {
            model.PageNo = model.PageNo ?? 1;
            model.PageSize = model.PageSize ?? int.MaxValue;
            model.DealerCategory = model.DealerCategory ?? EnumDealerCategory.All;
            model.DealerName = model.DealerName ?? string.Empty;

            var userId = AppIdentity.AppUser.UserId;
            var userInfo = await _userService.GetUserAsync(userId);

            Expression<Func<DealerInfo, bool>> dealerPredicate = (x) => !x.IsDeleted && 
                x.Channel == ConstantsODataValue.DistrbutionChannelDealer &&
                x.Division == ConstantsODataValue.DivisionDecorative &&
                ((userInfo.EmployeeRole == EnumEmployeeRole.Admin || userInfo.EmployeeRole == EnumEmployeeRole.GM) ||
                ((!(userInfo.PlantIds != null && userInfo.PlantIds.Any()) || userInfo.PlantIds.Contains(x.BusinessArea)) &&
                (!(userInfo.SaleOfficeIds != null && userInfo.SaleOfficeIds.Any()) || userInfo.SaleOfficeIds.Contains(x.SalesOffice)) &&
                (!(userInfo.AreaIds != null && userInfo.AreaIds.Any()) || userInfo.AreaIds.Contains(x.SalesGroup)) &&
                (!(userInfo.TerritoryIds != null && userInfo.TerritoryIds.Any()) || userInfo.TerritoryIds.Contains(x.Territory)) &&
                (!(userInfo.ZoneIds != null && userInfo.ZoneIds.Any()) || userInfo.ZoneIds.Contains(x.CustZone))));

            var result = (from dealer in _dealerInfoSvc.FindAll(dealerPredicate)
                          join custGrp in _customerGroupSvc.GetAll()
                          on dealer.AccountGroup equals custGrp.CustomerAccountGroup
                          into cust
                          from cu in cust.DefaultIfEmpty()

                          join focusDealer in _focusDealerSvc.GetAll()
                          on dealer.Id equals focusDealer.DealerId
                          into focus
                          from fd in focus.DefaultIfEmpty()

                          where ((EnumDealerCategory.All == model.DealerCategory) ||
                          //(EnumDealerCategory.Focus == model.DealerCategory && fd.IsFocused()))
                          (EnumDealerCategory.Focus == model.DealerCategory && fd != null && fd.DealerId > 0 && fd.ValidTo.Date >= DateTime.Now.Date &&
                             fd.ValidFrom.Date <= DateTime.Now.Date))
                          && (dealer.CustomerName.Contains(model.DealerName))

                          select new AppDealerInfoModel
                          {
                              Id = dealer.Id,
                              CustomerNo = dealer.CustomerNo,
                              CustomerName = $"{dealer.CustomerName} ({dealer.CustomerNo})",
                              Address = dealer.Address,
                              ContactNo = dealer.ContactNo,
                              Territory = dealer.Territory,
                              IsSubdealer = cu != null && !string.IsNullOrEmpty(cu.Description) && cu.Description.StartsWith("Subdealer"),
                              //IsFocused = fd.IsFocused(),
                              IsFocused = fd != null && fd.DealerId > 0 && fd.ValidTo.Date >= DateTime.Now.Date &&
                             fd.ValidFrom.Date <= DateTime.Now.Date,
                          }).Skip((model.PageNo.Value - 1) * model.PageSize.Value).Take(model.PageSize.Value).ToList();

            return result;
        }

        public async Task<IList<AppDealerInfoModel>> AppGetDealerListByArea(AppAreaDealerSearchModel model)
        {
            model.PageNo = model.PageNo ?? 1;
            model.PageSize = model.PageSize ?? int.MaxValue;
            model.DealerCategory = model.DealerCategory ?? EnumDealerCategory.All;
            model.DealerName = model.DealerName ?? string.Empty;

            Expression<Func<DealerInfo, bool>> dealerPredicate = (x) => !x.IsDeleted &&
                x.Channel == ConstantsODataValue.DistrbutionChannelDealer &&
                x.Division == ConstantsODataValue.DivisionDecorative &&
                (((!(model.Depots != null && model.Depots.Any()) || model.Depots.Contains(x.BusinessArea)) &&
                (!(model.SalesOffices != null && model.SalesOffices.Any()) || model.SalesOffices.Contains(x.SalesOffice)) &&
                (!(model.SalesGroups != null && model.SalesGroups.Any()) || model.SalesGroups.Contains(x.SalesGroup)) &&
                (!(model.Territories != null && model.Territories.Any()) || model.Territories.Contains(x.Territory)) &&
                (!(model.Zones != null && model.Zones.Any()) || model.Zones.Contains(x.CustZone))));

            var result = (from dealer in _dealerInfoSvc.FindAll(dealerPredicate)
                          join custGrp in _customerGroupSvc.GetAll()
                          on dealer.AccountGroup equals custGrp.CustomerAccountGroup
                          into cust
                          from cu in cust.DefaultIfEmpty()

                          join focusDealer in _focusDealerSvc.GetAll()
                          on dealer.Id equals focusDealer.DealerId
                          into focus
                          from fd in focus.DefaultIfEmpty()

                          where ((EnumDealerCategory.All == model.DealerCategory) ||
                          //(EnumDealerCategory.Focus == model.DealerCategory && fd.IsFocused()))
                          (EnumDealerCategory.Focus == model.DealerCategory && fd != null && fd.DealerId > 0 && fd.ValidTo.Date >= DateTime.Now.Date &&
                             fd.ValidFrom.Date <= DateTime.Now.Date))
                          && (dealer.CustomerName.Contains(model.DealerName))

                          select new AppDealerInfoModel
                          {
                              Id = dealer.Id,
                              CustomerNo = dealer.CustomerNo,
                              CustomerName = $"{dealer.CustomerName} ({dealer.CustomerNo})",
                              Address = dealer.Address,
                              ContactNo = dealer.ContactNo,
                              Territory = dealer.Territory,
                              IsSubdealer = cu != null && !string.IsNullOrEmpty(cu.Description) && cu.Description.StartsWith("Subdealer"),
                              //IsFocused = fd.IsFocused(),
                              IsFocused = fd != null && fd.DealerId > 0 && fd.ValidTo.Date >= DateTime.Now.Date &&
                             fd.ValidFrom.Date <= DateTime.Now.Date,
                          }).Skip((model.PageNo.Value - 1) * model.PageSize.Value).Take(model.PageSize.Value).ToList();

            return result;
        }

        public async Task<IList<AppDealerInfoModel>> GetDealerListByArea(AreaDealerSearchModel model)
        {
            model.DealerCategory = model.DealerCategory ?? EnumDealerCategory.All;

            Expression<Func<DealerInfo, bool>> dealerPredicate = (x) => !x.IsDeleted &&
                x.Channel == ConstantsODataValue.DistrbutionChannelDealer &&
                x.Division == ConstantsODataValue.DivisionDecorative &&
                (((string.IsNullOrEmpty(model.Depots) || model.Depots==x.BusinessArea) &&
                (!(model.SalesOffices != null && model.SalesOffices.Any()) || model.SalesOffices.Contains(x.SalesOffice)) &&
                (!(model.SalesGroups != null && model.SalesGroups.Any()) || model.SalesGroups.Contains(x.SalesGroup)) &&
                (!(model.Territories != null && model.Territories.Any()) || model.Territories.Contains(x.Territory)) &&
                (!(model.Zones != null && model.Zones.Any()) || model.Zones.Contains(x.CustZone))));

            var result = (from dealer in _dealerInfoSvc.FindAll(dealerPredicate)
                          join custGrp in _customerGroupSvc.GetAll()
                          on dealer.AccountGroup equals custGrp.CustomerAccountGroup
                          into cust
                          from cu in cust.DefaultIfEmpty()

                          join focusDealer in _focusDealerSvc.GetAll()
                          on dealer.Id equals focusDealer.DealerId
                          into focus
                          from fd in focus.DefaultIfEmpty()

                          where ((EnumDealerCategory.All == model.DealerCategory) ||
                          //(EnumDealerCategory.Focus == model.DealerCategory && fd.IsFocused()))
                          (EnumDealerCategory.Focus == model.DealerCategory && fd != null && fd.DealerId > 0 && fd.ValidTo.Date >= DateTime.Now.Date &&
                             fd.ValidFrom.Date <= DateTime.Now.Date))
                         

                          select new AppDealerInfoModel
                          {
                              Id = dealer.Id,
                              CustomerNo = dealer.CustomerNo,
                              CustomerName = $"{dealer.CustomerName} ({dealer.CustomerNo})",
                              Address = dealer.Address,
                              ContactNo = dealer.ContactNo,
                              Territory = dealer.Territory,
                              IsSubdealer = cu != null && !string.IsNullOrEmpty(cu.Description) && cu.Description.StartsWith("Subdealer"),
                              //IsFocused = fd.IsFocused(),
                              IsFocused = fd != null && fd.DealerId > 0 && fd.ValidTo.Date >= DateTime.Now.Date &&
                             fd.ValidFrom.Date <= DateTime.Now.Date,
                          }).ToList();

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
                    SetName<Depot>(item, data, x => x.Werks == item.PlantId, x => $"{x.Name1 } ({x.Werks})");
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
                    SetName<SaleOffice>(item, data, x => x.Code == item.SalesOfficeId, x => $"{x.Name} ({x.Code})");
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
                    SetName<SaleGroup>(item, data, x => x.Code == item.AreaId, x => $"{x.Name} ({x.Code})");
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
                    SetName<Territory>(item, data, x => x.Code == item.TerritoryId, x => x.Code);
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
                    SetName<Zone>(item, data, x => x.Code == item.ZoneId, x => x.Code);
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

        public void SetEmptyString<T>(List<T> items, params string[] propNames)
        {
            var entity = typeof(T);
            foreach (var item in items)
            {
                foreach (var propName in propNames)
                {
                    var prop = entity.GetProperty(propName);
                    if (prop != null)
                    {
                        prop.SetValue(item, string.Empty);
                    }
                }
            }

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

    public class MonthModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class YearModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    
}
