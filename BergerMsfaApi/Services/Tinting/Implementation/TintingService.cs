using AutoMapper;
using Berger.Common.Enumerations;
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
        public readonly IDropdownService _dropdownService;
        public readonly IMapper _mapper;

        public TintingService(
              IRepository<TintingMachine> tintingMachineSvc,
              IDropdownService dropdownService,
              IMapper mapper
            )
        {
            _tintingMachineSvc = tintingMachineSvc;
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
                ["territory"] = v => v.Territory,
                ["companyName"] = v => v.Company.DropdownName,
                ["noOfActiveMachine"] = v => v.NoOfActiveMachine,
                ["noOfInactiveMachine"] = v => v.NoOfInactiveMachine,
                ["no"] = v => v.No,
                ["contribution"] = v => v.Contribution,
            };

            var result = await _tintingMachineSvc.GetAllIncludeAsync(
                                x => x,
                                x => (string.IsNullOrEmpty(query.GlobalSearchValue) || x.UserInfo.FullName.Contains(query.GlobalSearchValue) || x.Company.DropdownName.Contains(query.GlobalSearchValue) || x.Territory.Contains(query.GlobalSearchValue)),
                                x => x.ApplyOrdering(columnsMap, query.SortBy, query.IsSortAscending),
                                x => x.Include(i => i.UserInfo).Include(i => i.Company),
                                query.Page,
                                query.PageSize,
                                true
                            );

            var modelResult = _mapper.Map<IList<TintingMachineModel>>(result.Items);

            var queryResult = new QueryResultModel<TintingMachineModel>();
            queryResult.Items = modelResult;
            queryResult.TotalFilter = result.TotalFilter;
            queryResult.Total = result.Total;

            return queryResult;
        }

        public async Task<IList<SaveTintingMachineModel>> GetAllAsync(string territory, int userInfoId)
        {
            var allCompanies = await _dropdownService.GetDropdownByTypeCd(DynamicTypeCode.SwappingCompetition);

            var result = await _tintingMachineSvc.GetAllIncludeAsync(x => x, 
                                    x => x.Territory == territory && x.UserInfoId == userInfoId, 
                                    null, 
                                    x => x.Include(i => i.UserInfo).Include(i => i.Company), 
                                    true);

            var modelResult = _mapper.Map<IList<SaveTintingMachineModel>>(result);

            if(!modelResult.Any())
            {
                foreach (var comp in allCompanies)
                {
                    var res = new SaveTintingMachineModel();
                    res.Territory = territory;
                    res.UserInfoId = userInfoId;
                    res.CompanyId = comp.Id;
                    res.CompanyName = comp.DropdownName;

                    modelResult.Add(res);
                }
            }

            return modelResult;
        }

        public async Task<bool> UpdateAsync(List<SaveTintingMachineModel> model)
        {
            foreach (var tinMac in model)
            {
                var existTinMac = await _tintingMachineSvc.GetFirstOrDefaultIncludeAsync(x => x,
                                                        f => f.CompanyId == tinMac.CompanyId
                                                        && f.Territory == tinMac.Territory && 
                                                        f.UserInfoId == tinMac.UserInfoId);
                if (existTinMac == null)
                {
                    existTinMac = new TintingMachine();
                    existTinMac.Territory = tinMac.Territory;
                    existTinMac.CompanyId = tinMac.CompanyId;
                    existTinMac.UserInfoId = tinMac.UserInfoId;
                    existTinMac.NoOfActiveMachine = tinMac.NoOfActiveMachine;
                    existTinMac.NoOfInactiveMachine = tinMac.NoOfInactiveMachine;
                    existTinMac.No = tinMac.No;
                    existTinMac.Contribution = tinMac.Contribution;
                    existTinMac.NoOfCorrection = 0;
                    existTinMac.CreatedTime = DateTime.Now;

                    await _tintingMachineSvc.CreateAsync(existTinMac);
                }
                else
                {
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
    }
}
