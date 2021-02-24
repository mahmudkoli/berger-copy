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
using X.PagedList;

namespace BergerMsfaApi.Services.Scheme.Implementation
{
    public class SchemeService : ISchemeService
    {
        public readonly IRepository<SchemeMaster> _schemeMasterSvc;
        public readonly IRepository<SchemeDetail> _schemeDetailSvc;
        public readonly IMapper _mapper;

        public SchemeService(
            IRepository<SchemeMaster> schemeMasterSvc,
            IRepository<SchemeDetail> schemeDetailSvc,
            IMapper mapper
            )
        {
            _schemeMasterSvc = schemeMasterSvc;
            _schemeDetailSvc = schemeDetailSvc;
            _mapper = mapper;
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
            var columnsMap = new Dictionary<string, Expression<Func<SchemeMaster, object>>>()
            {
                ["schemeName"] = v => v.SchemeName
            };

            var result = await _schemeMasterSvc.GetAllIncludeAsync(
                                x => x,
                                x => (string.IsNullOrEmpty(query.GlobalSearchValue) || x.SchemeName.Contains(query.GlobalSearchValue)),
                                x => x.ApplyOrdering(columnsMap, query.SortBy, query.IsSortAscending),
                                null,
                                query.Page,
                                query.PageSize,
                                true
                            );

            var modelResult = _mapper.Map<IList<SchemeMasterModel>>(result.Items);

            var queryResult = new QueryResultModel<SchemeMasterModel>();
            queryResult.Items = modelResult;
            queryResult.TotalFilter = result.TotalFilter;
            queryResult.Total = result.Total;

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
            var result =  await _schemeMasterSvc.DeleteAsync(f => f.Id == id);
            return result;
        }

        public async Task<object> GetAllSchemeMastersForSelectAsync()
        {
            var result = await _schemeMasterSvc.GetAllIncludeAsync(
                                x => new { x.Id, Label = x.SchemeName },
                                x => x.Status == Status.Active,
                                null,
                                null,
                                true
                            );

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
            var columnsMap = new Dictionary<string, Expression<Func<SchemeDetail, object>>>()
            {
                ["schemeMasterName"] = v => v.SchemeMaster.SchemeName
            };

            var result = await _schemeDetailSvc.GetAllIncludeAsync(
                                x => x,
                                x => (string.IsNullOrEmpty(query.GlobalSearchValue) || x.SchemeMaster.SchemeName.Contains(query.GlobalSearchValue)),
                                x => x.ApplyOrdering(columnsMap, query.SortBy, query.IsSortAscending),
                                x => x.Include(i => i.SchemeMaster),
                                query.Page,
                                query.PageSize,
                                true
                            );

            var modelResult = _mapper.Map<IList<SchemeDetailModel>>(result.Items);

            var queryResult = new QueryResultModel<SchemeDetailModel>();
            queryResult.Items = modelResult;
            queryResult.TotalFilter = result.TotalFilter;
            queryResult.Total = result.Total;

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
