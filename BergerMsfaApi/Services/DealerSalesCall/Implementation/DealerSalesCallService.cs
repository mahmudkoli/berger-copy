using AutoMapper;
using Berger.Common.Enumerations;
using Berger.Data.MsfaEntity.PainterRegistration;
using BergerMsfaApi.Extensions;
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

        public async Task<IList<DealerSalesCallModel>> GetAllAsync(int pageIndex, int pageSize)
        {
            var result = await _dealerSalesCallRepository.GetAllIncludeAsync(
                                x => x,
                                null,
                                null,
                                null,
                                pageIndex,
                                pageSize,
                                true
                            );

            var modelResult = _mapper.Map<IList<DealerSalesCallModel>>(result);

            return modelResult;
        }

        public async Task<IList<DealerSalesCallModel>> GetAllByUserIdAsync(int userId)
        {
            var result = await _dealerSalesCallRepository.GetAllIncludeAsync(
                                x => x,
                                x => x.UserId == userId,
                                null,
                                null,
                                true
                            );

            var modelResult = _mapper.Map<IList<DealerSalesCallModel>>(result);

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
            var results = await _dealerSalesCallRepository.GetAllIncludeAsync(
                                x => x,
                                x => ids.Contains(x.DealerId),
                                x => x.OrderByDescending(o => o.CreatedTime),
                                x => x.Include(i => i.DealerCompetitionSales),
                                true
                            );

            var modelResults = new List<SaveDealerSalesCallModel>();

            var companyList = await _dropdownService.GetDropdownByTypeCd("C01");

            if(!results.Any())
            {
                var modelResult = new SaveDealerSalesCallModel();
                modelResult.DealerCompetitionSales = new List<SaveDealerCompetitionSalesModel>();
                modelResult.DealerSalesIssues = new List<SaveDealerSalesIssueModel>();

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

                modelResults.Add(modelResult);
            }

            foreach (var result in results)
            {
                var modelResult = new SaveDealerSalesCallModel();
                modelResult.DealerCompetitionSales = new List<SaveDealerCompetitionSalesModel>();
                modelResult.DealerSalesIssues = new List<SaveDealerSalesIssueModel>();

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

                modelResult.HasSubDealerInfluence = result.HasSubDealerInfluence;
                modelResult.SubDealerInfluenceId = result.SubDealerInfluenceId;
                modelResult.HasPainterInfluence = result.HasPainterInfluence;
                modelResult.PainterInfluenceId = result.PainterInfluenceId;

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
                                null,
                                true
                            );

            var modelResult = _mapper.Map<DealerSalesCallModel>(result);

            return modelResult;
        }
    }
}
