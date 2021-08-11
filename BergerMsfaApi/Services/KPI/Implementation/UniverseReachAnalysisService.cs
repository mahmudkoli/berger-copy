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
    public class UniverseReachAnalysisService : IUniverseReachAnalysisService
    {
        public readonly IRepository<UniverseReachAnalysis> _UniverseReachAnalysisSvc;
        private readonly IRepository<Depot> _depotSvc;
        private readonly IRepository<DealerInfo> _dealerInfoSvc;
        private readonly IRepository<CustomerGroup> _customerGroupSvc;
        public readonly IMapper _mapper;

        public UniverseReachAnalysisService(
            IRepository<UniverseReachAnalysis> UniverseReachAnalysisSvc,
            IRepository<Depot> depotSvc,
            IRepository<DealerInfo> dealerInfoSvc,
            IRepository<CustomerGroup> customerGroupSvc,
            IMapper mapper
            )
        {
            _UniverseReachAnalysisSvc = UniverseReachAnalysisSvc;
            this._depotSvc = depotSvc;
            this._dealerInfoSvc = dealerInfoSvc;
            _customerGroupSvc = customerGroupSvc;
            _mapper = mapper;
        }

        #region Collection Plan
        public async Task<QueryResultModel<UniverseReachAnalysisModel>> GetAllUniverseReachAnalysissAsync(UniverseReachAnalysisQueryObjectModel query)
        {
            var columnsMap = new Dictionary<string, Expression<Func<UniverseReachAnalysis, object>>>()
            {
                ["businessArea"] = v => v.BusinessArea,
                ["territory"] = v => v.Territory,
                ["fiscalYear"] = v => v.FiscalYear
            };

            var result = await _UniverseReachAnalysisSvc.GetAllIncludeAsync(
                                x => x,
                                x => (string.IsNullOrEmpty(query.BusinessArea) || x.BusinessArea == query.BusinessArea) 
                                        && (!query.Territories.Any() || query.Territories.Contains(x.Territory)),
                                x => x.ApplyOrdering(columnsMap, query.SortBy, query.IsSortAscending),
                                null,
                                query.Page,
                                query.PageSize,
                                true
                            );

            var modelResult = _mapper.Map<IList<UniverseReachAnalysisModel>>(result.Items);

            var queryResult = new QueryResultModel<UniverseReachAnalysisModel>();
            queryResult.Items = modelResult;
            queryResult.TotalFilter = result.TotalFilter;
            queryResult.Total = result.Total;

            return queryResult;
        }

        public async Task<QueryResultModel<UniverseReachAnalysisModel>> GetAllUniverseReachAnalysissByCurrentUserAsync(UniverseReachAnalysisQueryObjectModel query)
        {
            var userId = AppIdentity.AppUser.UserId;
            var orderby = new string[] { "fiscalYear", "businessArea", "territory" };

            Expression<Func<UniverseReachAnalysis, bool>> filterFunc = x => (string.IsNullOrEmpty(query.BusinessArea) || x.BusinessArea == query.BusinessArea)
                                                                            && (!query.Territories.Any() || query.Territories.Contains(x.Territory));

            var totalCount = await (from cpInfo in _UniverseReachAnalysisSvc.FindAll(filterFunc) select cpInfo).CountAsync();

            var filterCount = await (from cpInfo in _UniverseReachAnalysisSvc.FindAll(filterFunc)
                               join dp in _depotSvc.GetAll() on cpInfo.BusinessArea equals dp.Werks into cpdLeftJoin
                               from dpInfo in cpdLeftJoin.DefaultIfEmpty()
                               //where (
                               //    (string.IsNullOrEmpty(query.GlobalSearchValue) || dpInfo.Name1.Contains(query.GlobalSearchValue) ||
                               //    dpInfo.Werks.Contains(query.GlobalSearchValue) || cpInfo.Territory.Contains(query.GlobalSearchValue))
                               //)
                               select cpInfo
                               ).CountAsync();

            var result = await (from cpInfo in _UniverseReachAnalysisSvc.FindAll(filterFunc)
                          join dp in _depotSvc.GetAll() on cpInfo.BusinessArea equals dp.Werks into cpdLeftJoin
                          from dpInfo in cpdLeftJoin.DefaultIfEmpty()
                          //where (
                          //    (string.IsNullOrEmpty(query.GlobalSearchValue) || dpInfo.Name1.Contains(query.GlobalSearchValue) ||
                          //    dpInfo.Werks.Contains(query.GlobalSearchValue) || cpInfo.Territory.Contains(query.GlobalSearchValue))
                          //)
                          orderby
                                @orderby[0] == query.SortBy && query.IsSortAscending ? cpInfo.FiscalYear : null,
                                @orderby[0] == query.SortBy && @orderby[1] != query.SortBy && @orderby[2] != query.SortBy && !query.IsSortAscending ? null : cpInfo.FiscalYear descending,
                                @orderby[1] == query.SortBy && query.IsSortAscending ? dpInfo.Name1 : null,
                                @orderby[1] == query.SortBy && @orderby[0] != query.SortBy && @orderby[2] != query.SortBy && !query.IsSortAscending ? null : dpInfo.Name1 descending,
                                @orderby[2] == query.SortBy && query.IsSortAscending ? cpInfo.Territory : null,
                                @orderby[2] == query.SortBy && @orderby[0] != query.SortBy && @orderby[1] != query.SortBy && !query.IsSortAscending ? null : cpInfo.Territory descending
                          select new UniverseReachAnalysisModel()
                          {
                              Id = cpInfo.Id,
                              BusinessArea = $"{dpInfo.Name1} ({dpInfo.Werks})",
                              Territory = cpInfo.Territory,
                              FiscalYear = cpInfo.FiscalYear,
                              OutletNumber = cpInfo.OutletNumber,
                              DirectCovered = cpInfo.DirectCovered,
                              IndirectCovered = cpInfo.IndirectCovered,
                              DirectTarget = cpInfo.DirectTarget,
                              IndirectTarget = cpInfo.IndirectTarget,
                              IndirectManual = cpInfo.IndirectManual,
                          }
                        ).Skip((query.Page - 1) * query.PageSize).Take(query.PageSize).ToListAsync();

            var queryResult = new QueryResultModel<UniverseReachAnalysisModel>();
            queryResult.Items = result;
            queryResult.TotalFilter = filterCount;
            queryResult.Total = totalCount;

            return queryResult;
        }

        public async Task<IList<UniverseReachAnalysisModel>> GetAllUniverseReachAnalysissAsync()
        {
            var result = await _UniverseReachAnalysisSvc.GetAllIncludeAsync(x => x,
                            null,
                            null,
                            null,
                            true);

            var modelResult = _mapper.Map<IList<UniverseReachAnalysisModel>>(result);

            return modelResult;
        }

        public async Task<UniverseReachAnalysisModel> GetUniverseReachAnalysissByIdAsync(int id)
        {
            var result = await _UniverseReachAnalysisSvc.GetFirstOrDefaultIncludeAsync(x => x,
                            x => x.Id == id,
                            null,
                            null,
                            true);

            var modelResult = _mapper.Map<UniverseReachAnalysisModel>(result);

            return modelResult;
        }

        public async Task<int> AddUniverseReachAnalysissAsync(SaveUniverseReachAnalysisModel model)
        {
            var UniverseReachAnalysis = _mapper.Map<UniverseReachAnalysis>(model);
            var fiscalYear = this.GetCurrentFiscalYear();
            UniverseReachAnalysis.FiscalYear = fiscalYear;
            var result = await _UniverseReachAnalysisSvc.CreateAsync(UniverseReachAnalysis);
            return result.Id;
        }

        public async Task<int> UpdateUniverseReachAnalysissAsync(SaveUniverseReachAnalysisModel model)
        {
            var existingUniverseReachAnalysis = await _UniverseReachAnalysisSvc.FindAsync(x => x.Id == model.Id);
            existingUniverseReachAnalysis.BusinessArea = model.BusinessArea;
            existingUniverseReachAnalysis.Territory = model.Territory;
            existingUniverseReachAnalysis.OutletNumber = model.OutletNumber;
            existingUniverseReachAnalysis.DirectCovered = model.DirectCovered;
            existingUniverseReachAnalysis.IndirectCovered = model.IndirectCovered;
            existingUniverseReachAnalysis.DirectTarget = model.DirectTarget;
            existingUniverseReachAnalysis.IndirectTarget = model.IndirectTarget;
            existingUniverseReachAnalysis.IndirectManual = model.IndirectManual;
            var result = await _UniverseReachAnalysisSvc.UpdateAsync(existingUniverseReachAnalysis);
            return result.Id;
        }

        public async Task<int> DeleteUniverseReachAnalysissAsync(int id)
        {
            var result = await _UniverseReachAnalysisSvc.DeleteAsync(f => f.Id == id);
            return result;
        }

        public async Task<bool> IsExitsUniverseReachAnalysissAsync(int id, string businessArea, string territory, string fiscalYear = null)
        {
            var result = await _UniverseReachAnalysisSvc.IsExistAsync(f => f.Id != id && 
                                                    f.BusinessArea == businessArea && f.Territory == territory &&
                                                    (string.IsNullOrEmpty(fiscalYear) || f.FiscalYear == fiscalYear));
            return result;
        }

        public async Task<int> UpdateAppUniverseReachAnalysissAsync(SaveAppUniverseReachAnalysisModel model)
        {
            var existingUniverseReachAnalysis = await _UniverseReachAnalysisSvc.FindAsync(x => x.Id == model.Id);
            existingUniverseReachAnalysis.IndirectManual = model.IndirectManual;
            var result = await _UniverseReachAnalysisSvc.UpdateAsync(existingUniverseReachAnalysis);
            return result.Id;
        }

        public async Task<SaveAppUniverseReachAnalysisModel> GetAppUniverseReachAnalysissAsync(AppUniverseReachAnalysisQueryObjectModel query)
        {
            var fiscalYear = this.GetCurrentFiscalYear();
            var existingUniverseReachAnalysis = await _UniverseReachAnalysisSvc.FindAsync(x => x.BusinessArea == query.BusinessArea 
                                                        && x.Territory == query.Territory && x.FiscalYear == fiscalYear);
            if(existingUniverseReachAnalysis == null) throw new Exception("Universe/Reach Plan of this area and this fiscal year is not found.");
            var result = _mapper.Map<SaveAppUniverseReachAnalysisModel>(existingUniverseReachAnalysis);

            var CFYFD = Berger.Odata.Extensions.DateTimeExtension.GetCFYFD(DateTime.Now);
            var CFYLD = Berger.Odata.Extensions.DateTimeExtension.GetCFYLD(DateTime.Now);

            var resultDealer = (from dealer in (await _dealerInfoSvc.FindAllAsync(x => true))
                                join custGrp in (await _customerGroupSvc.FindAllAsync(x => true))
                                on dealer.AccountGroup equals custGrp.CustomerAccountGroup
                                into cust
                                from cu in cust.DefaultIfEmpty()
                                where (
                                    (dealer.Channel == ConstantsODataValue.DistrbutionChannelDealer
                                        && dealer.Division == ConstantsODataValue.DivisionDecorative) &&
                                    (dealer.BusinessArea == query.BusinessArea
                                        && dealer.Territory == query.Territory) &&
                                    (
                                        CustomConvertExtension.ObjectToDateTime(dealer.CreatedOn).Date >= CFYFD.Date
                                        && CustomConvertExtension.ObjectToDateTime(dealer.CreatedOn).Date <= CFYLD.Date
                                    )
                                )
                                select new 
                                {
                                    Id = dealer.Id,
                                    CustomerNo = dealer.CustomerNo,
                                    IsSubdealer = cu != null && !string.IsNullOrEmpty(cu.Description) && cu.Description.StartsWith("Subdealer")
                                }).ToList();

            result.DirectActual = resultDealer.Where(x => !x.IsSubdealer).Select(x => x.CustomerNo).Distinct().Count();
            result.IndirectActual = resultDealer.Where(x => x.IsSubdealer).Select(x => x.CustomerNo).Distinct().Count();

            return result;
        }

        public async Task<IList<UniverseReachAnalysisReportResultModel>> GetUniverseReachAnalysisReportAsync(UniverseReachAnalysisReportSearchModel query)
        {
            var CFYFD = new DateTime(query.Year, 4, 1);
            var CFYLD = new DateTime(query.Year+1, 3, 31);

            var fiscalYear = $"{CFYFD.Year}-{(CFYLD.Year % 100)}";
            //var fiscalYear = $"{CFYFD}-{CFYLD.ToString().Substring(2)}";

            var result = (await _UniverseReachAnalysisSvc.FindAllAsync(x => x.FiscalYear == fiscalYear && x.BusinessArea == query.Depot
                                                                            && (!query.Territories.Any() || query.Territories.Contains(x.Territory)))).ToList();

            var resultDealer = (from dealer in (await _dealerInfoSvc.FindAllAsync(x => true))
                                join custGrp in (await _customerGroupSvc.FindAllAsync(x => true))
                                on dealer.AccountGroup equals custGrp.CustomerAccountGroup
                                into cust
                                from cu in cust.DefaultIfEmpty()
                                where (
                                    (dealer.Channel == ConstantsODataValue.DistrbutionChannelDealer
                                        && dealer.Division == ConstantsODataValue.DivisionDecorative) &&
                                    (dealer.BusinessArea == query.Depot
                                        && (!query.SalesGroups.Any() || query.SalesGroups.Contains(dealer.SalesGroup))
                                        && (!query.Territories.Any() || query.Territories.Contains(dealer.Territory))) &&
                                    (
                                        CustomConvertExtension.ObjectToDateTime(dealer.CreatedOn).Date >= CFYFD.Date
                                        && CustomConvertExtension.ObjectToDateTime(dealer.CreatedOn).Date <= CFYLD.Date
                                    )
                                )
                                select new
                                {
                                    Id = dealer.Id,
                                    CustomerNo = dealer.CustomerNo,
                                    Territory = dealer.Territory,
                                    IsSubdealer = cu != null && !string.IsNullOrEmpty(cu.Description) && cu.Description.StartsWith("Subdealer")
                                }).ToList();

            var returnResult = _mapper.Map<IList<UniverseReachAnalysisReportResultModel>>(result);

            foreach (var res in returnResult)
            {
                res.DirectActual = resultDealer.Where(x => x.Territory == res.Territory && !x.IsSubdealer).Select(x => x.CustomerNo).Distinct().Count();
                res.IndirectActual = resultDealer.Where(x => x.Territory == res.Territory && x.IsSubdealer).Select(x => x.CustomerNo).Distinct().Count();

                res.UnCovered = res.OutletNumber - (res.DirectCovered + res.IndirectCovered);
                res.Covered = res.UnCovered - (res.DirectActual + res.IndirectActual + res.IndirectManual);
            }

            if (query.ForApp)
            {
                returnResult = new List<UniverseReachAnalysisReportResultModel>() 
                {
                    new UniverseReachAnalysisReportResultModel ()
                    {
                        Territory = null,
                        OutletNumber = returnResult.Sum(x => x.OutletNumber),
                        DirectCovered = returnResult.Sum(x => x.DirectCovered),
                        IndirectCovered = returnResult.Sum(x => x.IndirectCovered),
                        UnCovered = returnResult.Sum(x => x.UnCovered),
                        DirectTarget = returnResult.Sum(x => x.DirectTarget),
                        IndirectTarget = returnResult.Sum(x => x.IndirectTarget),
                        DirectActual = returnResult.Sum(x => x.DirectActual),
                        IndirectActual = returnResult.Sum(x => x.IndirectActual),
                        IndirectManual = returnResult.Sum(x => x.IndirectManual),
                        Covered = returnResult.Sum(x => x.Covered),
                    }
                };
            }

            return returnResult;
        }

        public string GetCurrentFiscalYear()
        {
            var CFYFD = Berger.Odata.Extensions.DateTimeExtension.GetCFYFD(DateTime.Now).Year;
            var CFYLD = Berger.Odata.Extensions.DateTimeExtension.GetCFYLD(DateTime.Now).Year;
            //var fiscalYear = $"{CFYFD}-{CFYLD.ToString().Substring(2)}";
            var fiscalYear = $"{CFYFD}-{(CFYFD % 100) + 1}";
            return fiscalYear;
        }
        #endregion
    }
}
