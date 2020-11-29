using AutoMapper;
using Berger.Common.Enumerations;
using Berger.Data.MsfaEntity.PainterRegistration;
using BergerMsfaApi.Extensions;
using BergerMsfaApi.Models.DealerSalesCall;
using BergerMsfaApi.Repositories;
using BergerMsfaApi.Services.DealerSalesCall.Interfaces;
using BergerMsfaApi.Services.FileUploads.Interfaces;
using Microsoft.AspNetCore.Http;
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
        private readonly IFileUploadService _fileUploadService;
        private readonly IMapper _mapper;

        public DealerSalesCallService(
                IRepository<DSC.DealerSalesCall> dealerSalesCallRepository,
                IFileUploadService fileUploadService,
                IMapper mapper
            )
        {
            this._dealerSalesCallRepository = dealerSalesCallRepository;
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
            var result = (await _dealerSalesCallRepository.GetAllIncludeAsync(
                                x => x,
                                x => x.DealerId == id,
                                null,
                                null,
                                true
                            )).OrderByDescending(x => x.CreatedTime).FirstOrDefault();

            var modelResult = new SaveDealerSalesCallModel();
            modelResult.DealerCompetitionSales = new List<SaveDealerCompetitionSalesModel>();
            modelResult.DealerSalesIssues = new List<SaveDealerSalesIssueModel>();

            if(result != null)
            {
                modelResult.HasSubDealerInfluence = result.HasSubDealerInfluence;
                modelResult.SubDealerInfluenceId = result.SubDealerInfluenceId;
                modelResult.HasPainterInfluence = result.HasPainterInfluence;
                modelResult.PainterInfluenceId = result.PainterInfluenceId;
            }

            return modelResult;
        }

        public async Task<DealerSalesCallModel> GetByIdAsync(int id)
        {
            var result = await _dealerSalesCallRepository.GetFirstOrDefaultIncludeAsync(
                                x => x,
                                x => x.Id == id,
                                null,
                                true
                            );

            var modelResult = _mapper.Map<DealerSalesCallModel>(result);

            return modelResult;
        }
    }
}
