using AutoMapper;
using Berger.Common.Constants;
using Berger.Common.Enumerations;
using Berger.Common.Extensions;
using Berger.Common.Model;
using Berger.Data.MsfaEntity.DemandGeneration;
using Berger.Data.MsfaEntity.Hirearchy;
using Berger.Data.MsfaEntity.Master;
using BergerMsfaApi.Extensions;
using BergerMsfaApi.Models.Common;
using BergerMsfaApi.Models.DealerSalesCall;
using BergerMsfaApi.Models.DemandGeneration;
using BergerMsfaApi.Models.FocusDealer;
using BergerMsfaApi.Models.Notification;
using BergerMsfaApi.Models.Report;
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

        public async Task<QueryResultModel<LeadGenerationModel>> GetAllAsync(LeadGenerationDetailsReportSearchModel query)
        {
            var columnsMap = new Dictionary<string, Expression<Func<LeadGeneration, object>>>()
            {
                ["userFullName"] = v => v.User.FullName,
                ["createdTime"] = v => v.CreatedTime,
            };

            var result = await _leadGenerationRepository.GetAllIncludeAsync(
                                x => x,
                                x =>
                                (
                                (!query.UserId.HasValue || x.UserId == query.UserId.Value)
                                && (string.IsNullOrWhiteSpace(query.Depot) || x.Depot == query.Depot)
                                && (!query.Territories.Any() || query.Territories.Contains(x.Territory))
                                && (!query.Zones.Any() || query.Zones.Contains(x.Zone))
                                && (!query.FromDate.HasValue || x.CreatedTime.Date >= query.FromDate.Value.Date)
                                && (!query.ToDate.HasValue || x.CreatedTime.Date <= query.ToDate.Value.Date)
                                && (string.IsNullOrWhiteSpace(query.ProjectName) || x.ProjectName.Contains(query.ProjectName))
                                && (!query.PaintingStageId.HasValue || x.PaintingStageId == query.PaintingStageId.Value)
                                && (query.LeadGenerateFrom == -1 || (x.LeadGenerateFrom == (EnumLeadGenerationFrom)query.LeadGenerateFrom))
                               ),
                                x => x.ApplyOrdering(columnsMap, query.SortBy, query.IsSortAscending),
                                x => x.Include(i => i.User),
                                query.Page,
                                query.PageSize,
                                true
                            );

            var modelResult = _mapper.Map<IList<LeadGenerationModel>>(result.Items);

            #region get area mapping data
            var depotIds = modelResult.Select(x => x.Depot).Distinct().ToList();

            var depots = (await _depotRepository.FindAllAsync(x => depotIds.Contains(x.Werks)));

            foreach (var item in modelResult)
            {
                var dep = depots.FirstOrDefault(x => x.Werks == item.Depot);
                if (dep != null)
                {
                    item.Depot = $"{dep.Name1} ({dep.Werks})";
                }
            }
            #endregion

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

            var appUser = AppIdentity.AppUser;

            var result = await _leadGenerationRepository.GetAllIncludeAsync(
                                 x => x,
                                 //x => x.UserId == userId && (!x.LeadFollowUps.Any() ||
                                 x => ((appUser.EmployeeRole == (int)EnumEmployeeRole.Admin || appUser.EmployeeRole == (int)EnumEmployeeRole.GM) || 
                                        ((!appUser.PlantIdList.Any() || appUser.PlantIdList.Contains(x.Depot)) &&
                                        (!appUser.TerritoryIdList.Any() || appUser.TerritoryIdList.Contains(x.Territory)) &&
                                        (!appUser.ZoneIdList.Any() || appUser.ZoneIdList.Contains(x.Zone)))) && 
                                        (!x.LeadFollowUps.Any() ||
                                            !x.LeadFollowUps.Any(l => l.ProjectStatus.DropdownCode == ConstantsLeadValue.ProjectStatusLeadCompletedDropdownCode
                                                                || l.ProjectStatus.DropdownCode == ConstantsLeadValue.ProjectStatusLeadHandOverDropdownCode)),
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
                modelRes.LeadGenerateFrom = res.LeadGenerateFrom;
                modelRes.LeadGenerateFromText = res.LeadGenerateFrom.ToString();

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

            return modelResult.OrderBy(x => CustomConvertExtension.ObjectToDateTime(x.NextVisitDatePlan)).ToList();
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
                                        .Include(i => i.LeadFollowUps).ThenInclude(i => i.ActualVolumeSolds).ThenInclude(i => i.BrandInfo)
                                        .Include(i => i.LeadFollowUps).ThenInclude(i => i.BusinessAchievement).ThenInclude(i => i.ProductSourcing),
                                true
                            );

            var modelResult = _mapper.Map<LeadGenerationModel>(result);

            var depot = _depotRepository.Find(f => f.Werks == modelResult.Depot);
            modelResult.Depot = depot != null ? $"{depot.Name1} ({depot.Werks})" : modelResult.Depot;

            return modelResult;
        }

        public async Task<LeadGenerationModel> GetLeadByIdAsync(int id)
        {
            var result = await _leadGenerationRepository.Where(p=>p.Id==id).FirstOrDefaultAsync();
                                
                            

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
                leadGeneration.PhotoCaptureUrl = await _fileUploadService.SaveImageAsync(model.PhotoCaptureUrl, fileName, FileUploadCode.LeadGeneration);
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
            modelResult.TypeOfClientDropdownCode = result.TypeOfClient != null ? $"{result.TypeOfClient.DropdownCode}" : string.Empty;
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
            modelResult.LeadGenerateFrom = result.LeadGenerateFrom;
            modelResult.LeadGenerateFromText = result.LeadGenerateFrom.ToString();
            modelResult.BusinessAchievement = new SaveLeadBusinessAchievementModel();

            if (result.LeadFollowUps.Any())
            {
                var leadFollowUp = result.LeadFollowUps.OrderByDescending(x => x.CreatedTime).FirstOrDefault();
                modelResult.TypeOfClientId = leadFollowUp.TypeOfClientId;
                modelResult.TypeOfClientText = leadFollowUp.TypeOfClient != null ? $"{leadFollowUp.TypeOfClient.DropdownName}" : string.Empty;
                modelResult.TypeOfClientDropdownCode = leadFollowUp.TypeOfClient != null ? $"{leadFollowUp.TypeOfClient.DropdownCode}" : string.Empty;
                modelResult.OtherClientName = leadFollowUp.OtherClientName ?? string.Empty;
                modelResult.ProjectStatusId = leadFollowUp.ProjectStatusId;
                modelResult.ProjectStatusText = leadFollowUp.ProjectStatus != null ? $"{leadFollowUp.ProjectStatus.DropdownName}" : string.Empty;
                modelResult.ProjectStatusDropdownCode = leadFollowUp.ProjectStatus != null ? $"{leadFollowUp.ProjectStatus.DropdownCode}" : string.Empty;
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
                modelResult.ActualPaintJobCompletedInteriorPercentage = leadFollowUp.ActualPaintJobCompletedInteriorPercentage;
                modelResult.ActualPaintJobCompletedExteriorPercentage = leadFollowUp.ActualPaintJobCompletedExteriorPercentage;

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
                leadFollowUp.BusinessAchievement.PhotoCaptureUrl = await _fileUploadService.SaveImageAsync(leadFollowUp.BusinessAchievement.PhotoCaptureUrl, fileName, FileUploadCode.LeadGeneration);
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

            if (leadFoll != null)
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

        public async Task<IList<AppLeadFollowUpNotificationModel>> GetAllTodayFollowUpByUserIdForNotificationAsync()
        {
            var result = new AreaSearchCommonModel();
            var appUser = AppIdentity.AppUser;

            result.Depots = appUser.PlantIdList;
            result.Territories = appUser.TerritoryIdList;
            result.Zones = appUser.ZoneIdList;

            var today = DateTime.Now;

            var results = await _leadGenerationRepository.GetAllIncludeAsync(
                                   x => x,
                                   x =>
                (!(result.Zones != null && result.Zones.Any()) || result.Zones.Contains(x.Zone)) &&
                (!(result.Territories != null && result.Territories.Any()) || result.Territories.Contains(x.Territory)) &&
                (!(result.Depots != null && result.Depots.Any()) || result.Depots.Contains(x.Depot)) &&
                (x.NextFollowUpDate.Date == today.Date || x.LeadFollowUps.Any(y => y.NextVisitDatePlan.Date == today.Date)),
                                   null,
                                   x => x.Include(i => i.LeadFollowUps),
                                   true
                               );

            var modelResult = new List<AppLeadFollowUpNotificationModel>();

            foreach (var lead in results)
            {
                //if (lead.NextFollowUpDate.Date == today.Date || lead.LeadFollowUps.Any(x => x.NextVisitDatePlan.Date == today.Date))
                //{
                var modelRes = new AppLeadFollowUpNotificationModel();
                modelRes.UserId = appUser.UserId;
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

        public async Task<bool> UpdateLeadGenerateAsync(UpdateLeadGenerationModel model)
        {
            var item = await _leadGenerationRepository.FirstOrDefaultAsync(x => x.Id == model.Id);

            if (item == null)
            {
                throw new Exception("Lead Not Found");
            }

            if (!string.IsNullOrWhiteSpace(model.PhotoCaptureUrlBase64))
            {
                var fileName = model.Id + "_" + Guid.NewGuid().ToString();
                model.PhotoCaptureUrl = model.PhotoCaptureUrlBase64.Substring(model.PhotoCaptureUrlBase64.LastIndexOf(',') + 1);
                item.PhotoCaptureUrl = await _fileUploadService.SaveImageAsync(model.PhotoCaptureUrl, fileName, FileUploadCode.LeadGeneration);
            }

            item.Code = model.Code;
            item.Depot = model.Depot;
            item.ExpectedDateOfPainting = CustomConvertExtension.ObjectToDateTime(model.ExpectedDateOfPainting);
            item.ExpectedMonthlyBusinessValue = model.ExpectedMonthlyBusinessValue;
            item.ExpectedMonthlyBusinessValueChangeCount = model.ExpectedMonthlyBusinessValueChangeCount;
            item.ExpectedValue = model.ExpectedValue;
            item.ExpectedValueChangeCount = model.ExpectedValueChangeCount;
            item.KeyContactPersonMobile = model.KeyContactPersonMobile;
            item.KeyContactPersonName = model.KeyContactPersonName;
            item.NextFollowUpDate = CustomConvertExtension.ObjectToDateTime(model.NextFollowUpDate);
            item.NumberOfStoriedBuilding = model.NumberOfStoriedBuilding;
            item.OtherClientName = model.OtherClientName;
            item.PaintContractorMobile = model.PaintContractorMobile;
            item.PaintContractorName = model.PaintContractorName;
            item.PaintingStageId = model.PaintingStageId;
            item.ProductSamplingRequired = model.ProductSamplingRequired;
            item.ProjectAddress = model.ProjectAddress;
            item.ProjectName = model.ProjectName;
            item.Remarks = model.Remarks;
            item.RequirementOfColorScheme = model.RequirementOfColorScheme;
            item.Territory = model.Territory;
            item.TotalPaintingAreaSqftExterior = model.TotalPaintingAreaSqftExterior;
            item.TotalPaintingAreaSqftExteriorChangeCount = model.TotalPaintingAreaSqftExteriorChangeCount;
            item.TotalPaintingAreaSqftInterior = model.TotalPaintingAreaSqftInterior;
            item.TotalPaintingAreaSqftInteriorChangeCount = model.TotalPaintingAreaSqftInteriorChangeCount;
            item.TypeOfClientId = model.TypeOfClientId;
            item.UserId = model.UserId;
            item.Zone = model.Zone;
            item.VisitDate = CustomConvertExtension.ObjectToDateTime(model.VisitDate);


           await _leadGenerationRepository.UpdateAsync(item);

            return true;
        }

        public async Task DeleteImage(DealerImageModel dealerImageModel)
        {
            var item = await _leadGenerationRepository.FirstOrDefaultAsync(x => x.Id == dealerImageModel.Id);

            //string fileDirectory = Path.Combine(
            //    Directory.GetCurrentDirectory(), @"wwwroot\");
            //var fullPath = fileDirectory + dealerImageModel.URL;

            var fullPath = dealerImageModel.URL;

            if (item != null)
            {

                item.PhotoCaptureUrl = null;

            }

            if (!string.IsNullOrWhiteSpace(fullPath))
            {
                await _fileUploadService.DeleteImageAsync(fullPath);
                await _leadGenerationRepository.UpdateAsync(item);

            }

            //File.Delete();


        }
    }
}

