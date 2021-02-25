using AutoMapper;
using Berger.Common.Enumerations;
using Berger.Data.MsfaEntity.PainterRegistration;
using BergerMsfaApi.Extensions;
using BergerMsfaApi.Models.Common;
using BergerMsfaApi.Models.DealerSalesCall;
using BergerMsfaApi.Repositories;
using BergerMsfaApi.Services.DealerSalesCall.Interfaces;
using BergerMsfaApi.Services.FileUploads.Interfaces;
using BergerMsfaApi.Services.Setup.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DSC = Berger.Data.MsfaEntity.DealerSalesCall;

namespace BergerMsfaApi.Services.DealerSalesCall.Implementation
{
    public class DealerSalesCallService : IDealerSalesCallService
    {
        private readonly IRepository<DSC.DealerSalesCall> _dealerSalesCallRepository;
        private readonly IDropdownService _dropdownService;
        private readonly IFileUploadService _fileUploadService;
        private readonly IMapper _mapper;

        public DealerSalesCallService(
                IRepository<DSC.DealerSalesCall> dealerSalesCallRepository,
                IDropdownService dropdownService,
                IFileUploadService fileUploadService,
                IMapper mapper
            )
        {
            this._dealerSalesCallRepository = dealerSalesCallRepository;
            this._dropdownService = dropdownService;
            this._fileUploadService = fileUploadService;
            this._mapper = mapper;
        }

        public async Task<int> AddAsync(SaveDealerSalesCallModel model)
        {
            var dealerSalesCall = _mapper.Map<DSC.DealerSalesCall>(model);

            if (!string.IsNullOrWhiteSpace(model.CompetitionProductDisplayImageUrl))
            {
                var fileName = dealerSalesCall.DealerId + "_" + Guid.NewGuid().ToString();
                dealerSalesCall.CompetitionProductDisplayImageUrl = await _fileUploadService.SaveImageAsync(model.CompetitionProductDisplayImageUrl, fileName, FileUploadCode.DealerSalesCall, 1200, 800);
            }

            if (!string.IsNullOrWhiteSpace(model.CompetitionSchemeModalityImageUrl))
            {
                var fileName = dealerSalesCall.DealerId + "_" + Guid.NewGuid().ToString();
                dealerSalesCall.CompetitionSchemeModalityImageUrl = await _fileUploadService.SaveImageAsync(model.CompetitionSchemeModalityImageUrl, fileName, FileUploadCode.DealerSalesCall, 1200, 800);
            }

            dealerSalesCall.CreatedTime = DateTime.Now;

            var result = await _dealerSalesCallRepository.CreateAsync(dealerSalesCall);
            return result.Id;
        }

        public async Task<QueryResultModel<DealerSalesCallModel>> GetAllAsync(QueryObjectModel query)
        {
            var columnsMap = new Dictionary<string, Expression<Func<DSC.DealerSalesCall, object>>>()
            {
                ["dealerName"] = v => v.Dealer.CustomerName,
                ["userFullName"] = v => v.User.FullName,
            };

            var result = await _dealerSalesCallRepository.GetAllIncludeAsync(
                                x => x,
                                x => (string.IsNullOrEmpty(query.GlobalSearchValue) || x.User.FullName.Contains(query.GlobalSearchValue) || x.Dealer.CustomerName.Contains(query.GlobalSearchValue)),
                                x => x.ApplyOrdering(columnsMap, query.SortBy, query.IsSortAscending),
                                x => x.Include(i => i.User).Include(i => i.Dealer),
                                query.Page,
                                query.PageSize,
                                true
                            );

            var modelResult = _mapper.Map<IList<DealerSalesCallModel>>(result.Items);

            var queryResult = new QueryResultModel<DealerSalesCallModel>();
            queryResult.Items = modelResult;
            queryResult.TotalFilter = result.TotalFilter;
            queryResult.Total = result.Total;

            return queryResult;
        }

        public async Task<IList<AppDealerSalesCallModel>> GetAllByUserIdAsync(int userId)
        {
            var result = await _dealerSalesCallRepository.GetAllIncludeAsync(
                                x => x,
                                x => x.UserId == userId,
                                null,
                                null,
                                true
                            );

            var modelResult = _mapper.Map<IList<AppDealerSalesCallModel>>(result);

            return modelResult;
        }

        public async Task<SaveDealerSalesCallModel> GetDealerSalesCallByDealerIdAsync(int id)
        {
            var result = await _dealerSalesCallRepository.GetFirstOrDefaultIncludeAsync(
                                x => x,
                                x => x.DealerId == id,
                                x => x.OrderByDescending(o => o.CreatedTime),
                                x => x.Include(i => i.DealerCompetitionSales),
                                true
                            );

            var modelResult = new SaveDealerSalesCallModel();
            modelResult.DealerCompetitionSales = new List<SaveDealerCompetitionSalesModel>();
            modelResult.DealerSalesIssues = new List<SaveDealerSalesIssueModel>();
            modelResult.DealerId = id;

            var companyList = await _dropdownService.GetDropdownByTypeCd("C01");

            foreach (var item in companyList)
            {
                modelResult.DealerCompetitionSales.Add(
                        new SaveDealerCompetitionSalesModel 
                        { 
                            CompanyId = item.Id, 
                            CompanyName = item.DropdownName 
                        }
                    );
            }

            if(result != null)
            {
                modelResult.HasSubDealerInfluence = result.HasSubDealerInfluence;
                modelResult.SubDealerInfluenceId = result.SubDealerInfluenceId;
                modelResult.HasPainterInfluence = result.HasPainterInfluence;
                modelResult.PainterInfluenceId = result.PainterInfluenceId;

                modelResult.HasBPBLSales = result.HasBPBLSales;
                modelResult.BPBLAverageMonthlySales = result.BPBLAverageMonthlySales;
                modelResult.BPBLActualMTDSales = result.BPBLActualMTDSales;

                if(result.DealerCompetitionSales != null)
                {
                    foreach (var item in result.DealerCompetitionSales)
                    {
                        var dcs = modelResult.DealerCompetitionSales.FirstOrDefault(X => X.CompanyId == item.CompanyId);
                        if(dcs != null)
                        {
                            dcs.AverageMonthlySales = item.AverageMonthlySales;
                            dcs.ActualMTDSales = item.ActualMTDSales;
                        }
                    }
                }
            }

            return modelResult;
        }

        public async Task<IList<SaveDealerSalesCallModel>> GetDealerSalesCallListByDealerIdsAsync(IList<int> ids)
        {
            var modelResults = new List<SaveDealerSalesCallModel>();

            var companyList = await _dropdownService.GetDropdownByTypeCd(DynamicTypeCode.Company);

            foreach (var id in ids)
            {

                var result = await _dealerSalesCallRepository.GetFirstOrDefaultIncludeAsync(
                                    x => x,
                                    x => x.DealerId == id,
                                    x => x.OrderByDescending(o => o.CreatedTime),
                                    x => x.Include(i => i.DealerCompetitionSales),
                                    true
                                );

                var modelResult = new SaveDealerSalesCallModel();
                modelResult.DealerCompetitionSales = new List<SaveDealerCompetitionSalesModel>();
                modelResult.DealerSalesIssues = new List<SaveDealerSalesIssueModel>();
                modelResult.DealerId = id;

                foreach (var item in companyList)
                {
                    modelResult.DealerCompetitionSales.Add(
                            new SaveDealerCompetitionSalesModel
                            {
                                CompanyId = item.Id,
                                CompanyName = item.DropdownName
                            }
                        );
                }

                if (result != null)
                {
                    modelResult.HasSubDealerInfluence = result.HasSubDealerInfluence;
                    modelResult.SubDealerInfluenceId = result.SubDealerInfluenceId;
                    modelResult.HasPainterInfluence = result.HasPainterInfluence;
                    modelResult.PainterInfluenceId = result.PainterInfluenceId;

                    modelResult.HasBPBLSales = result.HasBPBLSales;
                    modelResult.BPBLAverageMonthlySales = result.BPBLAverageMonthlySales;
                    modelResult.BPBLActualMTDSales = result.BPBLActualMTDSales;

                    if (result.DealerCompetitionSales != null)
                    {
                        foreach (var item in result.DealerCompetitionSales)
                        {
                            var dcs = modelResult.DealerCompetitionSales.FirstOrDefault(X => X.CompanyId == item.CompanyId);
                            if (dcs != null)
                            {
                                dcs.AverageMonthlySales = item.AverageMonthlySales;
                                dcs.ActualMTDSales = item.ActualMTDSales;
                            }
                        }
                    }
                }

                modelResults.Add(modelResult);
            }

            return modelResults;
        }

        public async Task<DealerSalesCallModel> GetByIdAsync(int id)
        {
            var result = await _dealerSalesCallRepository.GetFirstOrDefaultIncludeAsync(
                                x => x,
                                x => x.Id == id,
                                null,
                                x => x.Include(i => i.User).Include(i => i.Dealer)
                                        .Include(i => i.SecondarySalesRatings).Include(i => i.PremiumProductLifting)
                                        .Include(i => i.Merchendising).Include(i => i.SubDealerInfluence)
                                        .Include(i => i.PainterInfluence).Include(i => i.DealerSatisfaction)
                                        .Include(i => i.DealerCompetitionSales).ThenInclude(i => i.Company)
                                        .Include(i => i.DealerSalesIssues).ThenInclude(i => i.DealerSalesIssueCategory)
                                        .Include(i => i.DealerSalesIssues).ThenInclude(i => i.Priority)
                                        .Include(i => i.DealerSalesIssues).ThenInclude(i => i.CBMachineMantainance),
                                true
                            );

            var modelResult = _mapper.Map<DealerSalesCallModel>(result);

            return modelResult;
        }
    }
}
