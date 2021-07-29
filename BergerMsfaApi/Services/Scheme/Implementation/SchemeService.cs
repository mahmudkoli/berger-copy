using AutoMapper;
using Berger.Common.Enumerations;
using Berger.Data.MsfaEntity.Scheme;
using BergerMsfaApi.Extensions;
using BergerMsfaApi.Models.Common;
using BergerMsfaApi.Models.Scheme;
using BergerMsfaApi.Repositories;
using BergerMsfaApi.Services.Scheme.interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Berger.Data.MsfaEntity;
using X.PagedList;

namespace BergerMsfaApi.Services.Scheme.Implementation
{
    public class SchemeService : ISchemeService
    {
        public readonly IRepository<SchemeMaster> _schemeMasterSvc;
        public readonly IRepository<SchemeDetail> _schemeDetailSvc;
        public readonly IMapper _mapper;
        private readonly ApplicationDbContext _applicationDbContext;

        public SchemeService(
            IRepository<SchemeMaster> schemeMasterSvc,
            IRepository<SchemeDetail> schemeDetailSvc,
            IMapper mapper, ApplicationDbContext applicationDbContext
            )
        {
            _schemeMasterSvc = schemeMasterSvc;
            _schemeDetailSvc = schemeDetailSvc;
            _mapper = mapper;
            _applicationDbContext = applicationDbContext;
        }

        #region Scheme Master
        public async Task<IPagedList<SchemeMasterModel>> GetAllSchemeMastersAsync(int index, int pageSize, string search)
        {
            var result = await _schemeMasterSvc.GetAllIncludeAsync(x => x,
                            x => (string.IsNullOrEmpty(search) || x.SchemeName.Contains(search)),
                            null,
                            null,
                            index, pageSize, true);

            var modelResult = _mapper.Map<IList<SchemeMasterModel>>(result.Items);

            //if (!string.IsNullOrEmpty(search))
            //    result = result.Search(search);
            //return result.ToPagedList(index, pageSize);

            return new StaticPagedList<SchemeMasterModel>(modelResult, index, pageSize, result.TotalFilter);
        }

        public async Task<QueryResultModel<SchemeMasterModel>> GetAllSchemeMastersAsync(QueryObjectModel query)
        {
            var columnsMap = new Dictionary<string, Expression<Func<SchemeMasterModel, object>>>()
            {
                ["schemeName"] = v => v.SchemeName
            };

            var user = AppIdentity.AppUser;
            var isPermitted = (user.RoleIds == RoleEnum.Admin.ToString() || user.RoleIds == RoleEnum.GM.ToString()) ;



            var result = (from sm in _applicationDbContext.SchemeMasters
                          join dep in _applicationDbContext.Depots on sm.BusinessArea equals dep.Werks into details
                          from m in details.DefaultIfEmpty()
                          where (
                          isPermitted? isPermitted : user.PlantIdList.Contains(sm.BusinessArea)
                          )
                          select new SchemeMasterModel
                          {
                              SchemeName = sm.SchemeName,
                              BusinessArea = sm.BusinessArea,
                              BusinessAreaName = !string.IsNullOrWhiteSpace(sm.BusinessArea) ? m.Name1 + " (" + sm.BusinessArea + ")" : "",
                              Condition = sm.Condition,
                              Id = sm.Id
                          });

            Expression<Func<SchemeMasterModel, object>> keySelector = columnsMap[query.SortBy];

            var total = await result.CountAsync();

            result = result.Where(x =>
                string.IsNullOrEmpty(query.GlobalSearchValue) || x.SchemeName.Contains(query.GlobalSearchValue));

            var filterCount = await result.CountAsync();

            result = query.IsSortAscending ? result.OrderBy(keySelector).Skip((query.Page - 1) * query.PageSize).Take(query.PageSize) :
                result.OrderByDescending(keySelector).Skip((query.Page - 1) * query.PageSize).Take(query.PageSize);

            result = result.AsNoTracking();

            var items = await result.ToListAsync();

            //var result = await _schemeMasterSvc.GetAllIncludeAsync(
            //                    x => x,
            //                    x => (string.IsNullOrEmpty(query.GlobalSearchValue) || x.SchemeName.Contains(query.GlobalSearchValue)),
            //                    x => x.ApplyOrdering(columnsMap, query.SortBy, query.IsSortAscending),
            //                    null,
            //                    query.Page,
            //                    query.PageSize,
            //                    true
            //                );

            //var modelResult = _mapper.Map<IList<SchemeMasterModel>>(result.Items);

            var queryResult = new QueryResultModel<SchemeMasterModel>
            {
                Items = items,
                TotalFilter = filterCount,
                Total = total
            };

            return queryResult;
        }

        public async Task<IList<SchemeMasterModel>> GetAllSchemeMastersAsync()
        {
            var result = await _schemeMasterSvc.GetAllIncludeAsync(x => x,
                            null,
                            null,
                            null,
                            true);

            var modelResult = _mapper.Map<IList<SchemeMasterModel>>(result);

            return modelResult;
        }

        public async Task<SchemeMasterModel> GetSchemeMasterByIdAsync(int id)
        {
            var result = await _schemeMasterSvc.GetFirstOrDefaultIncludeAsync(x => x,
                            x => x.Id == id,
                            null,
                            null,
                            //x => x.Include(i => i.SchemeDetails),
                            true);

            var modelResult = _mapper.Map<SchemeMasterModel>(result);

            return modelResult;
        }

        public async Task<int> AddSchemeMasterAsync(SaveSchemeMasterModel model)
        {
            var schemeMaster = _mapper.Map<SchemeMaster>(model);
            var result = await _schemeMasterSvc.CreateAsync(schemeMaster);
            return result.Id;
        }

        public async Task<int> UpdateSchemeMasterAsync(SaveSchemeMasterModel model)
        {
            var schemeMaster = _mapper.Map<SchemeMaster>(model);
            var result = await _schemeMasterSvc.UpdateAsync(schemeMaster);
            return result.Id;
        }

        public async Task<int> DeleteSchemeMasterAsync(int id)
        {
            //var isExists = await _schemeDetailSvc.AnyAsync(f => f.SchemeMasterId == id);
            //if (isExists) await _schemeDetailSvc.DeleteAsync(f => f.SchemeMasterId == id);
            var result = await _schemeMasterSvc.DeleteAsync(f => f.Id == id);
            return result;
        }

        public async Task<object> GetAllSchemeMastersForSelectAsync()
        {
            var user = AppIdentity.AppUser;
            var isPermitted = (user.RoleIds == RoleEnum.Admin.ToString() || user.RoleIds == RoleEnum.GM.ToString());
            var result = await (from sm in _applicationDbContext.SchemeMasters
                join d in _applicationDbContext.Depots on sm.BusinessArea equals d.Werks into depots
                from dep in depots.DefaultIfEmpty()
                where(
                  isPermitted? isPermitted : user.PlantIdList.Contains(sm.BusinessArea)

                )
                                select new
                {
                    sm.Id,
                    Label = string.IsNullOrWhiteSpace(sm.BusinessArea)
                        ? sm.SchemeName
                        : sm.SchemeName + "-" + dep.Name1 + "(" + sm.BusinessArea + ")"
                }).OrderBy(x => x.Label).AsNoTracking().ToListAsync();
                    


            //var result = await _schemeMasterSvc.GetAllIncludeAsync(
            //                    x => new { x.Id, Label = x.SchemeName },
            //                    x => x.Status == Status.Active,
            //                    null,
            //                    null,
            //                    true
            //                );

            return result;
        }
        #endregion

        #region Scheme Details
        public async Task<IPagedList<SchemeDetailModel>> GetAllSchemeDetailsAsync(int index, int pageSize, string search)
        {
            var result = await _schemeDetailSvc.GetAllIncludeAsync(x => x,
                            x => (string.IsNullOrEmpty(search) || x.SchemeMaster.SchemeName.Contains(search)),
                            null,
                            x => x.Include(i => i.SchemeMaster),
                            index, pageSize, true);

            var modelResult = _mapper.Map<IList<SchemeDetailModel>>(result.Items);

            //if (!string.IsNullOrEmpty(search))
            //    result = result.Search(search);
            //return result.ToPagedList(index, pageSize);

            return new StaticPagedList<SchemeDetailModel>(modelResult, index, pageSize, result.TotalFilter);
        }

        public async Task<QueryResultModel<SchemeDetailModel>> GetAllSchemeDetailsAsync(QueryObjectModel query)
        {
            var columnsMap = new Dictionary<string, Expression<Func<SchemeDetailModel, object>>>()
            {
                ["schemeMasterName"] = v => v.SchemeMasterName,
                ["benefitStartDateText"] = v => v.BenefitStartDate,
                ["benefitEndDateText"] = v => v.BenefitEndDate
            };
            var user = AppIdentity.AppUser;
            var isPermitted = (user.RoleIds == RoleEnum.Admin.ToString() || user.RoleIds == RoleEnum.GM.ToString());
            var result = (
                            from sm in _applicationDbContext.SchemeMasters
                            join det in _applicationDbContext.SchemeDetails on sm.Id equals det.SchemeMasterId
                            join dep in _applicationDbContext.Depots on sm.BusinessArea equals dep.Werks into details
                            from m in details.DefaultIfEmpty()
                            where(
                                isPermitted ? isPermitted : user.PlantIdList.Contains(sm.BusinessArea)

                            )
                            select new SchemeDetailModel
                            {
                                SchemeMasterName = !string.IsNullOrWhiteSpace(sm.BusinessArea) ? sm.SchemeName + " - " + m.Name1 + " (" + sm.BusinessArea + ")" : sm.SchemeName,
                                SchemeMasterCondition = sm.Condition,
                                Condition = det.Condition,
                                Brand = det.Brand,
                                Slab = det.Slab,
                                Status = sm.Status,
                                Id = det.Id,
                                BenefitStartDate = det.BenefitStartDate,
                                BenefitEndDate = det.BenefitEndDate,
                                BenefitStartDateText = det.BenefitStartDate.ToString("yyyy-MM-dd"),
                                BenefitEndDateText = det.BenefitEndDate.HasValue?det.BenefitEndDate.Value.ToString("yyyy-MM-dd"):""
                            });

            Expression<Func<SchemeDetailModel, object>> keySelector = columnsMap[query.SortBy];

            var total = await result.CountAsync();

            result = result.Where(x =>
                (string.IsNullOrEmpty(query.GlobalSearchValue) || x.SchemeMasterName.Contains(query.GlobalSearchValue)));

            var filterCount = await result.CountAsync();


            result = query.IsSortAscending ? result.OrderBy(keySelector).Skip((query.Page - 1) * query.PageSize).Take(query.PageSize) :
                result.OrderByDescending(keySelector).Skip((query.Page - 1) * query.PageSize).Take(query.PageSize);

            result = result.AsNoTracking();

            var items = await result.ToListAsync();


            //var result = await _schemeDetailSvc.GetAllIncludeAsync(
            //                    x => x,
            //                    x => (string.IsNullOrEmpty(query.GlobalSearchValue) || x.SchemeMaster.SchemeName.Contains(query.GlobalSearchValue)),
            //                    x => x.ApplyOrdering(columnsMap, query.SortBy, query.IsSortAscending),
            //                    x => x.Include(i => i.SchemeMaster),
            //                    query.Page,
            //                    query.PageSize,
            //                    true
            //                );

            //var modelResult = _mapper.Map<IList<SchemeDetailModel>>(result.Items);

            var queryResult = new QueryResultModel<SchemeDetailModel>
            {
                Items = items,
                TotalFilter = filterCount,
                Total = total
            };

            return queryResult;
        }

        public async Task<IList<SchemeDetailModel>> GetAllSchemeDetailsAsync()
        {
            var result = await _schemeDetailSvc.GetAllIncludeAsync(x => x,
                            null,
                            null,
                            x => x.Include(i => i.SchemeMaster),
                            true);

            var modelResult = _mapper.Map<IList<SchemeDetailModel>>(result);

            return modelResult;
        }

        public async Task<IList<AppSchemeDetailModel>> GetAppAllSchemeDetailsByCurrentUserAsync()
        {
            var depots = AppIdentity.AppUser.PlantIdList;
            var currentDate = DateTime.Now;

            var result = await _schemeDetailSvc.GetAllIncludeAsync(x => x,
                            x => (string.IsNullOrEmpty(x.SchemeMaster.BusinessArea) || depots.Contains(x.SchemeMaster.BusinessArea))
                                && (x.BenefitStartDate.Date <= currentDate.Date && (!x.BenefitEndDate.HasValue || x.BenefitEndDate >= currentDate.Date))
                                && x.Status == Status.Active,
                            null,
                            x => x.Include(i => i.SchemeMaster),
                            true);

            var modelResult = _mapper.Map<IList<AppSchemeDetailModel>>(result);

            return modelResult;
        }

        public async Task<SchemeDetailModel> GetSchemeDetailsByIdAsync(int id)
        {
            var result = await _schemeDetailSvc.GetFirstOrDefaultIncludeAsync(x => x,
                            x => x.Id == id,
                            null,
                            x => x.Include(i => i.SchemeMaster),
                            true);

            var modelResult = _mapper.Map<SchemeDetailModel>(result);

            return modelResult;
        }

        public async Task<int> AddSchemeDeatilsAsync(SaveSchemeDetailModel model)
        {
            var schemeDetail = _mapper.Map<SchemeDetail>(model);
            var result = await _schemeDetailSvc.CreateAsync(schemeDetail);
            return result.Id;
        }

        public async Task<int> UpdateSchemeDetailsAsync(SaveSchemeDetailModel model)
        {
            var schemeDetail = _mapper.Map<SchemeDetail>(model);
            var result = await _schemeDetailSvc.UpdateAsync(schemeDetail);
            return result.Id;
        }

        public async Task<int> DeleteSchemeDetailsAsync(int id)
        {
            var result = await _schemeDetailSvc.DeleteAsync(f => f.Id == id);
            return result;
        }
        #endregion
    }
}
