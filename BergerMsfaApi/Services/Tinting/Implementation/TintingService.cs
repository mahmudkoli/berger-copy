using AutoMapper;
using Berger.Common.Enumerations;
using Berger.Data.MsfaEntity.Master;
using Berger.Data.MsfaEntity.Setup;
using Berger.Data.MsfaEntity.Tinting;
using BergerMsfaApi.Extensions;
using BergerMsfaApi.Models.Common;
using BergerMsfaApi.Models.Tinting;
using BergerMsfaApi.Repositories;
using BergerMsfaApi.Services.Setup.Interfaces;
using BergerMsfaApi.Services.Tinting.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using X.PagedList;

namespace BergerMsfaApi.Services.Tinting.Implementation
{
    public class TintingService : ITintiningService
    {
        public readonly IRepository<TintingMachine> _tintingMachineSvc;
        private readonly IRepository<Depot> _depotSvc;
        public readonly IDropdownService _dropdownService;
        public readonly IMapper _mapper;

        public TintingService(
              IRepository<TintingMachine> tintingMachineSvc,
              IRepository<Depot> depotSvc,
              IDropdownService dropdownService,
              IMapper mapper
            )
        {
            _tintingMachineSvc = tintingMachineSvc;
            this._depotSvc = depotSvc;
            _dropdownService = dropdownService;
            _mapper = mapper;
        }

        public async Task<IPagedList<TintingMachineModel>> GetAllAsync(int index, int pageSize, string search)
        {
            var result = await _tintingMachineSvc.GetAllIncludeAsync(x => x,
                            x => (string.IsNullOrEmpty(search) || x.Territory.Contains(search) ||
                            x.UserInfo.FullName.Contains(search) || x.Company.DropdownName.Contains(search)),
                            null,
                            x => x.Include(i => i.UserInfo).Include(i => i.Company),
                            index, pageSize, true);

            var modelResult = _mapper.Map<IList<TintingMachineModel>>(result.Items);

            //if (!string.IsNullOrEmpty(search))
            //    result = result.Search(search);
            //return result.ToPagedList(index, pageSize);

            return new StaticPagedList<TintingMachineModel>(modelResult, index, pageSize, result.TotalFilter);
        }

        public async Task<QueryResultModel<TintingMachineModel>> GetAllAsync(QueryObjectModel query)
        {
            var columnsMap = new Dictionary<string, Expression<Func<TintingMachine, object>>>()
            {
                ["userFullName"] = v => v.UserInfo.FullName,
                ["depot"] = v => v.Depot,
                ["territory"] = v => v.Territory,
                ["companyName"] = v => v.Company.DropdownName,
                ["noOfActiveMachine"] = v => v.NoOfActiveMachine,
                ["noOfInactiveMachine"] = v => v.NoOfInactiveMachine,
                ["no"] = v => v.No,
                ["contribution"] = v => v.Contribution,
            };
            var user = AppIdentity.AppUser;
            //var isPermitted = user.ActiveRoleName == RoleEnum.GM.ToString() || user.ActiveRoleName == RoleEnum.Admin.ToString();
            var isPermitted = (user.EmployeeRole == (int)EnumEmployeeRole.Admin || user.EmployeeRole == (int)EnumEmployeeRole.GM);
            var result = await _tintingMachineSvc.GetAllIncludeAsync(
                                x => x,
                                x => (string.IsNullOrEmpty(query.GlobalSearchValue) || x.UserInfo.FullName.Contains(query.GlobalSearchValue) || x.Company.DropdownName.Contains(query.GlobalSearchValue) || x.Territory.Contains(query.GlobalSearchValue))
                                && isPermitted ? isPermitted : user.PlantIdList.Contains(x.Depot)
                                && isPermitted ? isPermitted : user.TerritoryIdList.Contains(x.Territory)

                                ,
                                x => x.ApplyOrdering(columnsMap, query.SortBy, query.IsSortAscending),
                                x => x.Include(i => i.UserInfo).Include(i => i.Company),
                                query.Page,
                                query.PageSize,
                                true
                            );

            var modelResult = _mapper.Map<IList<TintingMachineModel>>(result.Items);

            #region get area mapping data
            var depotIds = modelResult.Select(x => x.Depot).Distinct().ToList();

            var depots = (await _depotSvc.FindAllAsync(x => depotIds.Contains(x.Werks)));

            foreach (var item in modelResult)
            {
                var dep = depots.FirstOrDefault(x => x.Werks == item.Depot);
                if (dep != null)
                {
                    item.Depot = $"{dep.Name1} ({dep.Werks})";
                }
            }
            #endregion

            var queryResult = new QueryResultModel<TintingMachineModel>
            {
                Items = modelResult,
                TotalFilter = result.TotalFilter,
                Total = result.Total
            };

            return queryResult;
        }

        public async Task<IList<SaveTintingMachineModel>> GetAllAsync(string territory, int userInfoId)
        {

            string plantId = AppIdentity.AppUser.PlantIdList?.FirstOrDefault() ?? string.Empty;

            var allCompanies = await _dropdownService.GetDropdownByTypeCd(DynamicTypeCode.TintingCompany);

            var result = await _tintingMachineSvc.GetAllIncludeAsync(x => x,
                                    x => x.Territory == territory && x.Depot == plantId,// x.UserInfoId == userInfoId,
                                    null,
                                    x => x.Include(i => i.UserInfo).Include(i => i.Company),
                                    true);

            var modelResult = _mapper.Map<IList<SaveTintingMachineModel>>(result);

            if (!modelResult.Any())
            {
                foreach (var comp in allCompanies)
                {
                    var res = new SaveTintingMachineModel
                    {
                        Territory = territory,
                        UserInfoId = userInfoId,
                        CompanyId = comp.Id,
                        CompanyName = comp.DropdownName
                    };

                    modelResult.Add(res);
                }
            }

            return modelResult;
        }

        public async Task<bool> UpdateAsync(List<SaveTintingMachineModel> model)
        {
            string plantId = AppIdentity.AppUser.PlantIdList?.FirstOrDefault() ?? string.Empty;
            int userId = AppIdentity.AppUser.UserId;

            foreach (var tinMac in model)
            {
                var existTinMac = await _tintingMachineSvc.GetFirstOrDefaultIncludeAsync(x => x,
                                                        f => f.CompanyId == tinMac.CompanyId
                                                        && f.Territory == tinMac.Territory &&
                                                        f.Depot == plantId);
                if (existTinMac == null)
                {
                    existTinMac = new TintingMachine
                    {
                        Depot = plantId,
                        Territory = tinMac.Territory,
                        CompanyId = tinMac.CompanyId,
                        UserInfoId = userId,
                        NoOfActiveMachine = tinMac.NoOfActiveMachine,
                        NoOfInactiveMachine = tinMac.NoOfInactiveMachine,
                        No = tinMac.No,
                        Contribution = tinMac.Contribution,
                        NoOfCorrection = 0,
                        CreatedTime = DateTime.Now
                    };

                    await _tintingMachineSvc.CreateAsync(existTinMac);
                }
                else
                {
                    existTinMac.UserInfoId = userId;
                    if (existTinMac.NoOfActiveMachine > tinMac.NoOfActiveMachine ||
                        existTinMac.NoOfInactiveMachine > tinMac.NoOfInactiveMachine)
                    {
                        existTinMac.NoOfCorrection = existTinMac.NoOfCorrection + 1;
                    }

                    existTinMac.NoOfActiveMachine = tinMac.NoOfActiveMachine;
                    existTinMac.NoOfInactiveMachine = tinMac.NoOfInactiveMachine;
                    existTinMac.No = tinMac.No;
                    existTinMac.Contribution = tinMac.Contribution;

                    existTinMac.ModifiedTime = DateTime.Now;

                    await _tintingMachineSvc.UpdateAsync(existTinMac);
                }
            }

            return true;
        }

        public async Task<IList<AppTintingMachineModel>> GetAllAsync(AppTintingMachineSearchModel model)
        {

            var result = await _tintingMachineSvc.GetAllIncludeAsync(x => x,
                                    x => x.Depot == model.Depot && x.Territory == model.Territory,
                                    null,
                                    x => x.Include(i => i.UserInfo).Include(i => i.Company),
                                    true);

            var modelResult = _mapper.Map<IList<AppTintingMachineModel>>(result);

            return modelResult;
        }
    }
}
