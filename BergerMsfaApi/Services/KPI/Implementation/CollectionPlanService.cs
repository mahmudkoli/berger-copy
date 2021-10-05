using AutoMapper;
using Berger.Common.Constants;
using Berger.Common.Enumerations;
using Berger.Common.Extensions;
using Berger.Data.MsfaEntity.KPI;
using Berger.Data.MsfaEntity.Master;
using Berger.Data.MsfaEntity.SAPTables;
using Berger.Data.MsfaEntity.Scheme;
using Berger.Odata.Services;
using BergerMsfaApi.Extensions;
using BergerMsfaApi.Models.Common;
using BergerMsfaApi.Models.KPI;
using BergerMsfaApi.Models.Scheme;
using BergerMsfaApi.Repositories;
using BergerMsfaApi.Services.KPI.interfaces;
using BergerMsfaApi.Services.Scheme.interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using X.PagedList;

namespace BergerMsfaApi.Services.KPI.Implementation
{
    public class CollectionPlanService : ICollectionPlanService
    {
        public readonly IRepository<CollectionConfig> _collectionConfigSvc;
        public readonly IRepository<CollectionPlan> _collectionPlanSvc;
        private readonly IRepository<DealerInfo> _dealerInfoSvc;
        private readonly IRepository<Depot> _depotSvc;
        private readonly IFinancialDataService _financialDataService;
        private readonly ICollectionDataService _collectionDataService;
        public readonly IMapper _mapper;

        public CollectionPlanService(
            IRepository<CollectionConfig> collectionConfigSvc,
            IRepository<CollectionPlan> collectionPlanSvc,
            IRepository<DealerInfo> dealerInfoSvc,
            IRepository<Depot> depotSvc,
            IFinancialDataService financialDataService,
            ICollectionDataService collectionDataService,
            IMapper mapper
            )
        {
            _collectionConfigSvc = collectionConfigSvc;
            _collectionPlanSvc = collectionPlanSvc;
            this._dealerInfoSvc = dealerInfoSvc;
            this._depotSvc = depotSvc;
            this._financialDataService = financialDataService;
            this._collectionDataService = collectionDataService;
            _mapper = mapper;
        }

        #region Collection Config
        public async Task<IList<CollectionConfigModel>> GetAllCollectionConfigsAsync()
        {
            var result = await _collectionConfigSvc.GetAllIncludeAsync(x => x,
                            null,
                            null,
                            null,
                            true);

            var modelResult = _mapper.Map<IList<CollectionConfigModel>>(result);

            return modelResult;
        }

        public async Task<CollectionConfigModel> GetCollectionConfigByIdAsync(int id)
        {
            var result = await _collectionConfigSvc.GetFirstOrDefaultIncludeAsync(x => x,
                            x => x.Id == id,
                            null,
                            null,
                            //x => x.Include(i => i.CollectionPlans),
                            true);

            var modelResult = _mapper.Map<CollectionConfigModel>(result);

            return modelResult;
        }

        public async Task<int> UpdateCollectionConfigAsync(SaveCollectionConfigModel model)
        {
            var collectionConfig = _mapper.Map<CollectionConfig>(model);
            var result = await _collectionConfigSvc.UpdateAsync(collectionConfig);
            return result.Id;
        }
        #endregion

        #region Collection Plan
        public async Task<QueryResultModel<CollectionPlanModel>> GetAllCollectionPlansAsync(QueryObjectModel query)
        {
            var columnsMap = new Dictionary<string, Expression<Func<CollectionPlan, object>>>()
            {
                ["userFullName"] = v => v.User.FullName,
                ["collectionTargetAmount"] = v => v.CollectionTargetAmount,
                ["yearMonthText"] = v => v.Year
            };

            var result = await _collectionPlanSvc.GetAllIncludeAsync(
                                x => x,
                                x => (string.IsNullOrEmpty(query.GlobalSearchValue) || x.User.FullName.Contains(query.GlobalSearchValue)),
                                x => x.ApplyOrdering(columnsMap, query.SortBy, query.IsSortAscending),
                                x => x.Include(i => i.User),
                                query.Page,
                                query.PageSize,
                                true
                            );

            var modelResult = _mapper.Map<IList<CollectionPlanModel>>(result.Items);

            var queryResult = new QueryResultModel<CollectionPlanModel>();
            queryResult.Items = modelResult;
            queryResult.TotalFilter = result.TotalFilter;
            queryResult.Total = result.Total;

            #region collection config
            var dateNow = DateTime.Now;
            var daysInMonth = DateTime.DaysInMonth(dateNow.Year, dateNow.Month);
            var collectionConfig = await _collectionConfigSvc.GetAll().FirstOrDefaultAsync();
            var maxDateDay = collectionConfig?.ChangeableMaxDateDay ?? 1;
            var maxDate = maxDateDay <= daysInMonth ? new DateTime(dateNow.Year, dateNow.Month, maxDateDay) :
                                                        new DateTime(dateNow.Year, dateNow.Month, daysInMonth);
            var maxDateStr = maxDate.ToString("dd-MM-yyyy");

            foreach (var item in queryResult.Items)
            {
                item.ChangeableMaxDateDay = maxDateDay;
                item.ChangeableMaxDate = maxDate;
                item.ChangeableMaxDateText = maxDateStr;
            }
            #endregion

            return queryResult;
        }

        public async Task<QueryResultModel<CollectionPlanModel>> GetAllCollectionPlansByCurrentUserAsync(CollectionPlanQueryObjectModel query)
        {
            var userId = AppIdentity.AppUser.UserId;
            var orderby = new string[] { "yearMonthText", "businessArea", "territory" };

            Expression<Func<CollectionPlan, bool>> filterFunc = x => (string.IsNullOrEmpty(query.BusinessArea) || x.BusinessArea == query.BusinessArea)
                                                                            && (!query.Territories.Any() || query.Territories.Contains(x.Territory));

            #region collection config
            var dateNow = DateTime.Now;
            var daysInMonth = DateTime.DaysInMonth(dateNow.Year, dateNow.Month);
            var collectionConfig = await _collectionConfigSvc.GetAll().FirstOrDefaultAsync();
            var maxDateDay = collectionConfig?.ChangeableMaxDateDay ?? 1;
            var maxDate = maxDateDay <= daysInMonth ? new DateTime(dateNow.Year, dateNow.Month, maxDateDay) :
                                                        new DateTime(dateNow.Year, dateNow.Month, daysInMonth);
            var maxDateStr = maxDate.ToString("dd-MM-yyyy");
            #endregion

            var totalCount = await (from cpInfo in _collectionPlanSvc.FindAll(filterFunc) select cpInfo).CountAsync();

            var filterCount = await (from cpInfo in _collectionPlanSvc.FindAll(filterFunc)
                               join dp in _depotSvc.GetAll() on cpInfo.BusinessArea equals dp.Werks into cpdLeftJoin
                               from dpInfo in cpdLeftJoin.DefaultIfEmpty()
                               //where (
                               //    (string.IsNullOrEmpty(query.GlobalSearchValue) || dpInfo.Name1.Contains(query.GlobalSearchValue) ||
                               //    dpInfo.Werks.Contains(query.GlobalSearchValue) || cpInfo.Territory.Contains(query.GlobalSearchValue))
                               //)
                               select cpInfo
                               ).CountAsync();

            var result = await (from cpInfo in _collectionPlanSvc.FindAll(filterFunc)
                          join dp in _depotSvc.GetAll() on cpInfo.BusinessArea equals dp.Werks into cpdLeftJoin
                          from dpInfo in cpdLeftJoin.DefaultIfEmpty()
                          //where (
                          //    (string.IsNullOrEmpty(query.GlobalSearchValue) || dpInfo.Name1.Contains(query.GlobalSearchValue) ||
                          //    dpInfo.Werks.Contains(query.GlobalSearchValue) || cpInfo.Territory.Contains(query.GlobalSearchValue))
                          //)
                          orderby 
                                @orderby[0] == query.SortBy && query.IsSortAscending ? cpInfo.Year : 0,
                                @orderby[0] == query.SortBy && @orderby[1] != query.SortBy && @orderby[2] != query.SortBy && !query.IsSortAscending ? 0 : cpInfo.Year descending,
                                @orderby[0] == query.SortBy && query.IsSortAscending ? cpInfo.Month : 0,
                                @orderby[0] == query.SortBy && @orderby[1] != query.SortBy && @orderby[2] != query.SortBy && !query.IsSortAscending ? 0 : cpInfo.Month descending,
                                @orderby[1] == query.SortBy && query.IsSortAscending ? dpInfo.Name1 : null,
                                @orderby[1] == query.SortBy && @orderby[0] != query.SortBy && @orderby[2] != query.SortBy && !query.IsSortAscending ? null : dpInfo.Name1 descending,
                                @orderby[2] == query.SortBy && query.IsSortAscending ? cpInfo.Territory : null,
                                @orderby[2] == query.SortBy && @orderby[0] != query.SortBy && @orderby[1] != query.SortBy && !query.IsSortAscending ? null : cpInfo.Territory descending
                          select new CollectionPlanModel()
                          {
                              Id = cpInfo.Id,
                              UserId = cpInfo.UserId,
                              UserFullName = string.Empty,
                              BusinessArea = $"{dpInfo.Name1} ({dpInfo.Werks})",
                              Territory = cpInfo.Territory,
                              Year = cpInfo.Year,
                              Month = cpInfo.Month,
                              YearMonthText = new DateTime(cpInfo.Year > 0 ? cpInfo.Year : 1, cpInfo.Month > 0 ? cpInfo.Month : 1, 01).ToString("yyyy MMM"),
                              SlippageAmount = cpInfo.SlippageAmount,
                              CollectionTargetAmount = cpInfo.CollectionTargetAmount,
                              CollectionActualAmount = cpInfo.CollectionActualAmount,
                              SlippageCollectionActualAmount = cpInfo.SlippageCollectionActualAmount,
                              ChangeableMaxDateDay = maxDateDay,
                              ChangeableMaxDate = maxDate,
                              ChangeableMaxDateText = maxDateStr
                          }
                        ).Skip((query.Page - 1) * query.PageSize).Take(query.PageSize).ToListAsync();

            var queryResult = new QueryResultModel<CollectionPlanModel>();
            queryResult.Items = result;
            queryResult.TotalFilter = filterCount;
            queryResult.Total = totalCount;

            return queryResult;
        }

        public async Task<IList<CollectionPlanModel>> GetAllCollectionPlansAsync()
        {
            var result = await _collectionPlanSvc.GetAllIncludeAsync(x => x,
                            null,
                            null,
                            x => x.Include(i => i.User),
                            true);

            var modelResult = _mapper.Map<IList<CollectionPlanModel>>(result);

            return modelResult;
        }

        public async Task<CollectionPlanModel> GetCollectionPlansByIdAsync(int id)
        {
            var result = await _collectionPlanSvc.GetFirstOrDefaultIncludeAsync(x => x,
                            x => x.Id == id,
                            null,
                            x => x.Include(i => i.User),
                            true);

            var modelResult = _mapper.Map<CollectionPlanModel>(result);

            return modelResult;
        }

        public async Task<int> AddCollectionPlansAsync(SaveCollectionPlanModel model)
        {
            var collectionPlan = _mapper.Map<CollectionPlan>(model);
            collectionPlan.UserId = AppIdentity.AppUser.UserId;
            collectionPlan.Year = DateTime.Now.Year;
            collectionPlan.Month = DateTime.Now.Month;
            var result = await _collectionPlanSvc.CreateAsync(collectionPlan);
            return result.Id;
        }

        public async Task<int> UpdateCollectionPlansAsync(SaveCollectionPlanModel model)
        {
            var existingCollectionPlan = await _collectionPlanSvc.FindAsync(x => x.Id == model.Id);
            existingCollectionPlan.BusinessArea = model.BusinessArea;
            existingCollectionPlan.Territory = model.Territory;
            existingCollectionPlan.SlippageAmount = model.SlippageAmount;
            existingCollectionPlan.CollectionTargetAmount = model.CollectionTargetAmount;
            var result = await _collectionPlanSvc.UpdateAsync(existingCollectionPlan);
            return result.Id;
        }

        public async Task<int> DeleteCollectionPlansAsync(int id)
        {
            var result = await _collectionPlanSvc.DeleteAsync(f => f.Id == id);
            return result;
        }

        public async Task<bool> IsExitsCollectionPlansAsync(int id, string businessArea, string territory, int year = 0, int month = 0)
        {
            var result = await _collectionPlanSvc.IsExistAsync(f => f.Id != id && 
                                                    f.BusinessArea == businessArea && f.Territory == territory &&
                                                    (f.Year == 0 || f.Year == year) && (f.Month == 0 || f.Month == month));
            return result;
        }

        public async Task<decimal> GetCustomerSlippageAmountToLastMonth(CustomerSlippageQueryModel query)
        {
            var currentDate = DateTime.Now;
            var dealerIds = (await _dealerInfoSvc.FindAllAsync(x => x.BusinessArea == query.BusinessArea && x.Territory == query.Territory))
                                                    .Select(x => x.CustomerNo).Distinct().ToList();

            var lastMonthToDate = (new DateTime(currentDate.Year, currentDate.Month, 01)).AddDays(-1);

            var slippageData = await _financialDataService.GetCustomerSlippageAmount(dealerIds, lastMonthToDate);

            var result = slippageData.Sum(x => CustomConvertExtension.ObjectToDecimal(x.Amount));

            return result;
        }
        #endregion
    }
}
