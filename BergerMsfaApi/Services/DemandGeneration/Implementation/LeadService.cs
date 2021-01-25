using AutoMapper;
using Berger.Common.Enumerations;
using Berger.Data.MsfaEntity.DemandGeneration;
using BergerMsfaApi.Models.DemandGeneration;
using BergerMsfaApi.Repositories;
using BergerMsfaApi.Services.DemandGeneration.Interfaces;
using BergerMsfaApi.Services.FileUploads.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BergerMsfaApi.Services.DemandGeneration.Implementation
{
    public class LeadService : ILeadService
    {
        private readonly IRepository<LeadGeneration> _leadGenerationRepository;
        private readonly IRepository<LeadFollowUp> _leadFollowUpRepository;
        private readonly IFileUploadService _fileUploadService;
        private readonly IMapper _mapper;

        public LeadService(
                IRepository<LeadGeneration> leadGenerationRepository,
                IRepository<LeadFollowUp> leadFollowUpRepository,
                IFileUploadService fileUploadService,
                IMapper mapper
            )
        {
            this._leadGenerationRepository = leadGenerationRepository;
            this._leadFollowUpRepository = leadFollowUpRepository;
            this._fileUploadService = fileUploadService;
            this._mapper = mapper;
        }

        public async Task<IList<LeadGenerationModel>> GetAllAsync(int pageIndex, int pageSize)
        {
            var result = await _leadGenerationRepository.GetAllIncludeAsync(
                                x => x,
                                null,
                                null,
                                null,
                                pageIndex,
                                pageSize,
                                true
                            );

            var modelResult = _mapper.Map<IList<LeadGenerationModel>>(result);

            return modelResult;
        }

        public async Task<IList<AppLeadGenerationModel>> GetAllByUserIdAsync(int userId)
        {
            //var result = await _leadGenerationRepository.GetAllIncludeAsync(
            //                    x => x,
            //                    x => x.UserId == userId,
            //                    null,
            //                    null,
            //                    true
            //                );

            //var modelResult = _mapper.Map<IList<LeadGenerationModel>>(result);

            //return modelResult; 
            
            var result = await _leadGenerationRepository.GetAllIncludeAsync(
                                 x => x,
                                 x => x.UserId == userId,
                                 null,
                                 x => x.Include(i => i.LeadFollowUps),
                                 true
                             );

            var modelResult = new List<AppLeadGenerationModel>();

            foreach (var res in result)
            {
                var modelRes = new AppLeadGenerationModel();
                modelRes.Id = res.Id;
                modelRes.UserId = res.UserId;
                modelRes.Depot = res.Depot;
                modelRes.Territory = res.Territory;
                modelRes.Zone = res.Zone;
                modelRes.LastVisitedDate = res.VisitDate;
                modelRes.NextVisitDatePlan = res.NextFollowUpDate;
                modelRes.KeyContactPersonName = res.KeyContactPersonName;
                modelRes.KeyContactPersonMobile = res.KeyContactPersonMobile;

                if (res.LeadFollowUps.Any())
                {
                    var leadFollowUp = res.LeadFollowUps.OrderByDescending(x => x.CreatedTime).FirstOrDefault();
                    modelRes.LastVisitedDate = leadFollowUp.ActualVisitDate;
                    modelRes.NextVisitDatePlan = leadFollowUp.NextVisitDatePlan;
                    modelRes.KeyContactPersonName = leadFollowUp.KeyContactPersonName;
                    modelRes.KeyContactPersonMobile = leadFollowUp.KeyContactPersonMobile;
                }

                modelResult.Add(modelRes);
            }

            return modelResult;
        }

        public async Task<LeadGenerationModel> GetByIdAsync(int id)
        {
            var result = await _leadGenerationRepository.GetFirstOrDefaultIncludeAsync(
                                x => x,
                                x => x.Id == id,
                                null,
                                null,
                                true
                            );

            var modelResult = _mapper.Map<LeadGenerationModel>(result);

            return modelResult;
        }

        public async Task<int> AddLeadGenerateAsync(SaveLeadGenerationModel model)
        {
            var leadGeneration = _mapper.Map<LeadGeneration>(model);

            if (!string.IsNullOrWhiteSpace(model.PhotoCaptureUrl))
            {
                var fileName = leadGeneration.Code + "_" + Guid.NewGuid().ToString();
                leadGeneration.PhotoCaptureUrl = await _fileUploadService.SaveImageAsync(model.PhotoCaptureUrl, fileName, FileUploadCode.LeadGeneration, 1200, 800);
            }

            leadGeneration.Code = DateTime.Now.ToString("yyyyMMddHHmmss");
            leadGeneration.CreatedTime = DateTime.Now;

            var result = await _leadGenerationRepository.CreateAsync(leadGeneration);

            return result.Id;
        }

        public async Task<SaveLeadFollowUpModel> GetLeadFollowUpByLeadGenerateIdAsync(int id)
        {
            var result = await _leadGenerationRepository.GetFirstOrDefaultIncludeAsync(
                                x => x,
                                x => x.Id == id,
                                null,
                                x => x.Include(i => i.LeadFollowUps),
                                true
                            );

            var modelResult = new SaveLeadFollowUpModel();

            modelResult.LeadGenerationId = result.Id;
            modelResult.Depot = result.Depot;
            modelResult.Territory = result.Territory;
            modelResult.Zone = result.Zone;
            modelResult.LastVisitedDate = result.VisitDate;
            modelResult.NextVisitDatePlan = result.NextFollowUpDate;
            modelResult.KeyContactPersonName = result.KeyContactPersonName;
            modelResult.KeyContactPersonMobile = result.KeyContactPersonMobile;
            modelResult.PaintContractorName = result.PaintContractorName;
            modelResult.PaintContractorMobile = result.PaintContractorMobile;
            modelResult.NumberOfStoriedBuilding = result.NumberOfStoriedBuilding;
            modelResult.ExpectedValue = result.ExpectedValue;
            modelResult.ExpectedValueChangeCount = result.ExpectedValueChangeCount;
            modelResult.ExpectedMonthlyBusinessValue = result.ExpectedMonthlyBusinessValue;
            modelResult.ExpectedMonthlyBusinessValueChangeCount = result.ExpectedMonthlyBusinessValueChangeCount;
            modelResult.TotalPaintingAreaSqftInterior = result.TotalPaintingAreaSqftInterior;
            modelResult.TotalPaintingAreaSqftInteriorChangeCount = result.TotalPaintingAreaSqftInteriorChangeCount;
            modelResult.TotalPaintingAreaSqftExterior = result.TotalPaintingAreaSqftExterior;
            modelResult.TotalPaintingAreaSqftExteriorChangeCount = result.TotalPaintingAreaSqftExteriorChangeCount;
            modelResult.BusinessAchievement = new SaveLeadBusinessAchievementModel();

            if(result.LeadFollowUps.Any())
            {
                var leadFollowUp = result.LeadFollowUps.OrderByDescending(x => x.CreatedTime).FirstOrDefault();
                modelResult.LastVisitedDate = leadFollowUp.ActualVisitDate;
                modelResult.NextVisitDatePlan = leadFollowUp.NextVisitDatePlan;
                modelResult.KeyContactPersonName = leadFollowUp.KeyContactPersonName;
                modelResult.KeyContactPersonMobile = leadFollowUp.KeyContactPersonMobile;
                modelResult.PaintContractorName = leadFollowUp.PaintContractorName;
                modelResult.PaintContractorMobile = leadFollowUp.PaintContractorMobile;
                modelResult.NumberOfStoriedBuilding = leadFollowUp.NumberOfStoriedBuilding;
                modelResult.ExpectedValue = leadFollowUp.ExpectedValue;
                modelResult.ExpectedMonthlyBusinessValue = leadFollowUp.ExpectedMonthlyBusinessValue;
                modelResult.TotalPaintingAreaSqftInterior = leadFollowUp.TotalPaintingAreaSqftInterior;
                modelResult.TotalPaintingAreaSqftExterior = leadFollowUp.TotalPaintingAreaSqftExterior;
            }

            return modelResult;
        }

        public async Task<int> AddLeadFollowUpAsync(SaveLeadFollowUpModel model)
        {
            var leadFollowUp = _mapper.Map<LeadFollowUp>(model);

            if (model.BusinessAchievement != null && !string.IsNullOrWhiteSpace(model.BusinessAchievement.PhotoCaptureUrl))
            {
                var fileName = leadFollowUp.LeadGenerationId + "_" + Guid.NewGuid().ToString();
                model.BusinessAchievement.PhotoCaptureUrl = await _fileUploadService.SaveImageAsync(model.BusinessAchievement.PhotoCaptureUrl, fileName, FileUploadCode.LeadGeneration, 1200, 800);
            }

            leadFollowUp.CreatedTime = DateTime.Now;

            var result = await _leadFollowUpRepository.CreateAsync(leadFollowUp);

            #region Lead Generation update
            var leadGen = await _leadGenerationRepository.GetFirstOrDefaultIncludeAsync(
                                                            x => x, 
                                                            x => x.Id == leadFollowUp.LeadGenerationId, 
                                                            null,
                                                            null,
                                                            true);

            var leadFoll = (await _leadFollowUpRepository.GetAllIncludeAsync(
                                                            x => x, 
                                                            x => x.Id == leadFollowUp.LeadGenerationId, 
                                                            x => x.OrderByDescending(o => o.CreatedTime),
                                                            null,
                                                            true)).FirstOrDefault();
            
            if(leadFoll != null)
            {
                if (leadFoll.ExpectedValue != model.ExpectedValue)
                    leadGen.ExpectedValueChangeCount++;
                if (leadFoll.ExpectedMonthlyBusinessValue != model.ExpectedMonthlyBusinessValue)
                    leadGen.ExpectedMonthlyBusinessValueChangeCount++;
                if (leadFoll.TotalPaintingAreaSqftInterior != model.TotalPaintingAreaSqftInterior)
                    leadGen.TotalPaintingAreaSqftInteriorChangeCount++;
                if (leadFoll.TotalPaintingAreaSqftExterior != model.TotalPaintingAreaSqftExterior)
                    leadGen.TotalPaintingAreaSqftExteriorChangeCount++;
            }
            else
            {
                if (leadGen.ExpectedValue != model.ExpectedValue)
                    leadGen.ExpectedValueChangeCount++;
                if (leadGen.ExpectedMonthlyBusinessValue != model.ExpectedMonthlyBusinessValue)
                    leadGen.ExpectedMonthlyBusinessValueChangeCount++;
                if (leadGen.TotalPaintingAreaSqftInterior != model.TotalPaintingAreaSqftInterior)
                    leadGen.TotalPaintingAreaSqftInteriorChangeCount++;
                if (leadGen.TotalPaintingAreaSqftExterior != model.TotalPaintingAreaSqftExterior)
                    leadGen.TotalPaintingAreaSqftExteriorChangeCount++;
            }

            await _leadGenerationRepository.UpdateAsync(leadGen);
            #endregion

            return result.Id;
        }
    }
}
