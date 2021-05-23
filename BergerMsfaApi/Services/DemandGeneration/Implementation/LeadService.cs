using AutoMapper;
using Berger.Common.Constants;
using Berger.Common.Enumerations;
using Berger.Common.Extensions;
using Berger.Data.MsfaEntity.DemandGeneration;
using Berger.Data.MsfaEntity.Hirearchy;
using Berger.Data.MsfaEntity.Master;
using BergerMsfaApi.Extensions;
using BergerMsfaApi.Models.Common;
using BergerMsfaApi.Models.DemandGeneration;
using BergerMsfaApi.Models.Notification;
using BergerMsfaApi.Repositories;
using BergerMsfaApi.Services.DemandGeneration.Interfaces;
using BergerMsfaApi.Services.FileUploads.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BergerMsfaApi.Services.DemandGeneration.Implementation
{
    public class LeadService : ILeadService
    {
        private readonly IRepository<LeadGeneration> _leadGenerationRepository;
        private readonly IRepository<LeadFollowUp> _leadFollowUpRepository;
        private readonly IRepository<Depot> _depotRepository;
        private readonly IRepository<Territory> _territoryRepository;
        private readonly IRepository<Zone> _zoneRepository;
        private readonly IFileUploadService _fileUploadService;
        private readonly IMapper _mapper;

        public LeadService(
                IRepository<LeadGeneration> leadGenerationRepository,
                IRepository<LeadFollowUp> leadFollowUpRepository,
                IRepository<Depot> depotRepository,
                IRepository<Territory> territoryRepository,
                IRepository<Zone> zoneRepository,
                IFileUploadService fileUploadService,
                IMapper mapper
            )
        {
            this._leadGenerationRepository = leadGenerationRepository;
            this._leadFollowUpRepository = leadFollowUpRepository;
            this._depotRepository = depotRepository;
            this._territoryRepository = territoryRepository;
            this._zoneRepository = zoneRepository;
            this._fileUploadService = fileUploadService;
            this._mapper = mapper;
        }

        public async Task<QueryResultModel<LeadGenerationModel>> GetAllAsync(QueryObjectModel query)
        {
            var columnsMap = new Dictionary<string, Expression<Func<LeadGeneration, object>>>()
            {
                ["userFullName"] = v => v.User.FullName,
            };

            var result = await _leadGenerationRepository.GetAllIncludeAsync(
                                x => x,
                                x => (string.IsNullOrEmpty(query.GlobalSearchValue) || x.User.FullName.Contains(query.GlobalSearchValue)),
                                x => x.ApplyOrdering(columnsMap, query.SortBy, query.IsSortAscending),
                                x => x.Include(i => i.User),
                                query.Page,
                                query.PageSize,
                                true
                            );

            var modelResult = _mapper.Map<IList<LeadGenerationModel>>(result.Items);

            var queryResult = new QueryResultModel<LeadGenerationModel>();
            queryResult.Items = modelResult;
            queryResult.TotalFilter = result.TotalFilter;
            queryResult.Total = result.Total;

            return queryResult;
        }

        public async Task<IList<AppLeadGenerationModel>> GetAllPendingProjectByUserIdAsync(int userId)
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
                                 x => x.UserId == userId && (!x.LeadFollowUps.Any() || 
                                        !x.LeadFollowUps.Any(l => l.ProjectStatus.DropdownName == ConstantsLeadValue.ProjectStatusLeadCompleted)),
                                 null,
                                 x => x.Include(i => i.LeadFollowUps).ThenInclude(i => i.ProjectStatus),
                                 true
                             );

            var modelResult = new List<AppLeadGenerationModel>();

            foreach (var res in result)
            {
                var modelRes = new AppLeadGenerationModel();
                modelRes.Id = res.Id;
                modelRes.UserId = res.UserId;
                modelRes.Code = res.Code;
                modelRes.Depot = res.Depot;
                modelRes.Territory = res.Territory;
                modelRes.Zone = res.Zone;
                modelRes.ProjectName = res.ProjectName;
                modelRes.ProjectAddress = res.ProjectAddress;
                modelRes.LastVisitedDate = CustomConvertExtension.ObjectToDateString(res.VisitDate);
                modelRes.NextVisitDatePlan = CustomConvertExtension.ObjectToDateString(res.NextFollowUpDate);
                modelRes.KeyContactPersonName = res.KeyContactPersonName;
                modelRes.KeyContactPersonMobile = res.KeyContactPersonMobile;

                if (res.LeadFollowUps.Any())
                {
                    var leadFollowUp = res.LeadFollowUps.OrderByDescending(x => x.CreatedTime).FirstOrDefault();
                    modelRes.LastVisitedDate = CustomConvertExtension.ObjectToDateString(leadFollowUp.ActualVisitDate);
                    modelRes.NextVisitDatePlan = CustomConvertExtension.ObjectToDateString(leadFollowUp.NextVisitDatePlan);
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
                                x => x.Include(i => i.User).Include(i => i.TypeOfClient).Include(i => i.PaintingStage)
                                        .Include(i => i.LeadFollowUps).ThenInclude(i => i.TypeOfClient)
                                        .Include(i => i.LeadFollowUps).ThenInclude(i => i.ProjectStatus)
                                        .Include(i => i.LeadFollowUps).ThenInclude(i => i.ProjectStatusLeadCompleted)
                                        .Include(i => i.LeadFollowUps).ThenInclude(i => i.SwappingCompetition)
                                        .Include(i => i.LeadFollowUps).ThenInclude(i => i.BusinessAchievement),
                                true
                            );

            var modelResult = _mapper.Map<LeadGenerationModel>(result);

            return modelResult;
        }

        public async Task<int> AddLeadGenerateAsync(AppSaveLeadGenerationModel model)
        {
            var leadGeneration = _mapper.Map<LeadGeneration>(model);
            //TODO: need to generate code
            leadGeneration.Code = ((Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds).ToString();

            if (!string.IsNullOrWhiteSpace(model.PhotoCaptureUrl))
            {
                var fileName = leadGeneration.Code + "_" + Guid.NewGuid().ToString();
                leadGeneration.PhotoCaptureUrl = await _fileUploadService.SaveImageAsync(model.PhotoCaptureUrl, fileName, FileUploadCode.LeadGeneration, 1200, 800);
            }

            var result = await _leadGenerationRepository.CreateAsync(leadGeneration);

            return result.Id;
        }

        public async Task<AppSaveLeadFollowUpModel> GetLeadFollowUpByLeadGenerateIdAsync(int id)
        {
            var result = await _leadGenerationRepository.GetFirstOrDefaultIncludeAsync(
                                x => x,
                                x => x.Id == id,
                                null,
                                x => x.Include(i => i.TypeOfClient)
                                        .Include(i => i.LeadFollowUps).ThenInclude(i => i.TypeOfClient)
                                        .Include(i => i.LeadFollowUps).ThenInclude(i => i.ProjectStatus)
                                        .Include(i => i.LeadFollowUps).ThenInclude(i => i.ProjectStatusLeadCompleted)
                                        .Include(i => i.LeadFollowUps).ThenInclude(i => i.SwappingCompetition)
                                        .Include(i => i.LeadFollowUps).ThenInclude(i => i.BusinessAchievement),
                                true
                            );

            var modelResult = new AppSaveLeadFollowUpModel();

            modelResult.LeadGenerationId = result.Id;
            modelResult.Code = result.Code;
            modelResult.Depot = result.Depot;
            modelResult.Territory = result.Territory;
            modelResult.Zone = result.Zone;
            modelResult.TypeOfClientId = result.TypeOfClientId;
            modelResult.TypeOfClientText = result.TypeOfClient != null ? $"{result.TypeOfClient.DropdownName}" : string.Empty;
            modelResult.OtherClientName = result.OtherClientName ?? string.Empty;
            modelResult.ProjectName = result.ProjectName;
            modelResult.ProjectAddress = result.ProjectAddress;
            modelResult.LastVisitedDate = CustomConvertExtension.ObjectToDateString(result.VisitDate);
            modelResult.NextVisitDatePlan = CustomConvertExtension.ObjectToDateString(result.NextFollowUpDate);
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
                modelResult.TypeOfClientId = leadFollowUp.TypeOfClientId;
                modelResult.TypeOfClientText = leadFollowUp.TypeOfClient != null ? $"{leadFollowUp.TypeOfClient.DropdownName}" : string.Empty;
                modelResult.OtherClientName = leadFollowUp.OtherClientName ?? string.Empty;
                modelResult.ProjectStatusId = leadFollowUp.ProjectStatusId;
                modelResult.ProjectStatusText = leadFollowUp.ProjectStatus != null ? $"{leadFollowUp.ProjectStatus.DropdownName}" : string.Empty;
                modelResult.HasSwappingCompetition = leadFollowUp.HasSwappingCompetition;
                modelResult.SwappingCompetitionId = leadFollowUp.SwappingCompetitionId;
                modelResult.SwappingCompetitionText = leadFollowUp.SwappingCompetition != null ? $"{leadFollowUp.SwappingCompetition.DropdownName}" : string.Empty;
                modelResult.ProjectStatusLeadCompletedId = leadFollowUp.ProjectStatusLeadCompletedId;
                modelResult.ProjectStatusLeadCompletedText = leadFollowUp.ProjectStatusLeadCompleted != null ? $"{leadFollowUp.ProjectStatusLeadCompleted.DropdownName}" : string.Empty;
                modelResult.LastVisitedDate = CustomConvertExtension.ObjectToDateString(leadFollowUp.ActualVisitDate);
                modelResult.NextVisitDatePlan = CustomConvertExtension.ObjectToDateString(leadFollowUp.NextVisitDatePlan);
                modelResult.KeyContactPersonName = leadFollowUp.KeyContactPersonName;
                modelResult.KeyContactPersonMobile = leadFollowUp.KeyContactPersonMobile;
                modelResult.PaintContractorName = leadFollowUp.PaintContractorName;
                modelResult.PaintContractorMobile = leadFollowUp.PaintContractorMobile;
                modelResult.NumberOfStoriedBuilding = leadFollowUp.NumberOfStoriedBuilding;
                modelResult.ExpectedValue = leadFollowUp.ExpectedValue;
                modelResult.ExpectedMonthlyBusinessValue = leadFollowUp.ExpectedMonthlyBusinessValue;
                modelResult.TotalPaintingAreaSqftInterior = leadFollowUp.TotalPaintingAreaSqftInterior;
                modelResult.TotalPaintingAreaSqftExterior = leadFollowUp.TotalPaintingAreaSqftExterior;

                modelResult.ProjectStatusPartialBusinessPercentage = leadFollowUp.ProjectStatusPartialBusinessPercentage;
                modelResult.UpTradingFromBrandName = leadFollowUp.UpTradingFromBrandName;
                modelResult.UpTradingToBrandName = leadFollowUp.UpTradingToBrandName;
                modelResult.BrandUsedInteriorBrandName = leadFollowUp.BrandUsedInteriorBrandName;
                modelResult.BrandUsedExteriorBrandName = leadFollowUp.BrandUsedExteriorBrandName;
                modelResult.BrandUsedTopCoatBrandName = leadFollowUp.BrandUsedTopCoatBrandName;
                modelResult.BrandUsedUnderCoatBrandName = leadFollowUp.BrandUsedUnderCoatBrandName;
                modelResult.StringToList(leadFollowUp, modelResult);

                if (leadFollowUp.BusinessAchievement != null)
                {
                    modelResult.BusinessAchievement.ProductSamplingBrandName = leadFollowUp.BusinessAchievement.ProductSamplingBrandName;
                    modelResult.BusinessAchievement.StringToList(leadFollowUp.BusinessAchievement, modelResult.BusinessAchievement);
                }
            }

            #region Depot, Territory, Zone
            var depot = _depotRepository.Find(f => f.Werks == modelResult.Depot);
            modelResult.DepotName = depot != null ? $"{depot.Name1} ({depot.Werks})" : string.Empty;
            modelResult.TerritoryName = modelResult.Territory ?? string.Empty;
            modelResult.ZoneName = modelResult.Zone ?? string.Empty;
            #endregion

            return modelResult;
        }

        public async Task<int> AddLeadFollowUpAsync(AppSaveLeadFollowUpModel model)
        {
            var leadFollowUp = _mapper.Map<LeadFollowUp>(model);

            if (leadFollowUp.BusinessAchievement != null)
            {
                leadFollowUp.NextVisitDatePlan = leadFollowUp.BusinessAchievement.NextVisitDate;
            }

            if (leadFollowUp.BusinessAchievement != null && !string.IsNullOrWhiteSpace(leadFollowUp.BusinessAchievement.PhotoCaptureUrl))
            {
                var fileName = leadFollowUp.LeadGenerationId + "_" + Guid.NewGuid().ToString();
                leadFollowUp.BusinessAchievement.PhotoCaptureUrl = await _fileUploadService.SaveImageAsync(leadFollowUp.BusinessAchievement.PhotoCaptureUrl, fileName, FileUploadCode.LeadGeneration, 1200, 800);
            }

            var leadFoll = (await _leadFollowUpRepository.GetFirstOrDefaultIncludeAsync(
                                                            x => x,
                                                            x => x.Id == leadFollowUp.LeadGenerationId,
                                                            x => x.OrderByDescending(o => o.CreatedTime),
                                                            null,
                                                            true));

            var result = await _leadFollowUpRepository.CreateAsync(leadFollowUp);

            #region Lead Generation update
            var leadGen = await _leadGenerationRepository.GetFirstOrDefaultIncludeAsync(
                                                            x => x, 
                                                            x => x.Id == leadFollowUp.LeadGenerationId, 
                                                            null,
                                                            null,
                                                            true);
            
            if(leadFoll != null)
            {
                if (leadFoll.ExpectedValue != leadFollowUp.ExpectedValue)
                    leadGen.ExpectedValueChangeCount++;
                if (leadFoll.ExpectedMonthlyBusinessValue != leadFollowUp.ExpectedMonthlyBusinessValue)
                    leadGen.ExpectedMonthlyBusinessValueChangeCount++;
                if (leadFoll.TotalPaintingAreaSqftInterior != leadFollowUp.TotalPaintingAreaSqftInterior)
                    leadGen.TotalPaintingAreaSqftInteriorChangeCount++;
                if (leadFoll.TotalPaintingAreaSqftExterior != leadFollowUp.TotalPaintingAreaSqftExterior)
                    leadGen.TotalPaintingAreaSqftExteriorChangeCount++;
            }
            else
            {
                if (leadGen.ExpectedValue != leadFollowUp.ExpectedValue)
                    leadGen.ExpectedValueChangeCount++;
                if (leadGen.ExpectedMonthlyBusinessValue != leadFollowUp.ExpectedMonthlyBusinessValue)
                    leadGen.ExpectedMonthlyBusinessValueChangeCount++;
                if (leadGen.TotalPaintingAreaSqftInterior != leadFollowUp.TotalPaintingAreaSqftInterior)
                    leadGen.TotalPaintingAreaSqftInteriorChangeCount++;
                if (leadGen.TotalPaintingAreaSqftExterior != leadFollowUp.TotalPaintingAreaSqftExterior)
                    leadGen.TotalPaintingAreaSqftExteriorChangeCount++;
            }

            await _leadGenerationRepository.UpdateAsync(leadGen);
            #endregion

            return result.Id;
        }

        public async Task<IList<AppLeadFollowUpNotificationModel>> GetAllTodayFollowUpByUserIdForNotificationAsync(int userId)
        {
            var today = DateTime.Now;

            var result = await _leadGenerationRepository.GetAllIncludeAsync(
                                   x => x,
                                   x => x.UserId == userId &&
                                    (x.NextFollowUpDate.Date == today.Date || x.LeadFollowUps.Any(y => y.NextVisitDatePlan.Date == today.Date)),
                                   null,
                                   x => x.Include(i => i.LeadFollowUps),
                                   true
                               );

            var modelResult = new List<AppLeadFollowUpNotificationModel>();

            foreach (var lead in result)
            {
                //if (lead.NextFollowUpDate.Date == today.Date || lead.LeadFollowUps.Any(x => x.NextVisitDatePlan.Date == today.Date))
                //{
                    var modelRes = new AppLeadFollowUpNotificationModel();
                    modelRes.UserId = userId;
                    modelRes.Code = lead.Code;
                    modelRes.Depot = lead.Depot;
                    modelRes.Territory = lead.Territory;
                    modelRes.Zone = lead.Zone;
                    modelRes.ProjectName = lead.ProjectName;
                    modelRes.ProjectAddress = lead.ProjectAddress;

                    modelResult.Add(modelRes);
                //}
            }

            return modelResult;
        }

        public async Task<int> DeleteAsync(int id) => await _leadFollowUpRepository.DeleteAsync(s => s.Id == id);
        public async Task<bool> IsExistAsync(int id) => await _leadFollowUpRepository.IsExistAsync(f => f.Id == id);
    }
}
