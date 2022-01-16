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
using BergerMsfaApi.Models.HappyWallet.Lead;
using BergerMsfaApi.Models.Notification;
using BergerMsfaApi.Models.Report;
using BergerMsfaApi.Repositories;
using BergerMsfaApi.Services.DemandGeneration.Interfaces;
using BergerMsfaApi.Services.FileUploads.Interfaces;
using BergerMsfaApi.Services.HappyWallet.Lead.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BergerMsfaApi.Services.HappyWallet.Lead.Implementation
{
    public class HappyWalletLeadService : IHappyWalletLeadService
    {
        private readonly IRepository<LeadGeneration> _leadGenerationRepository;
        private readonly IRepository<LeadFollowUp> _leadFollowUpRepository;
        private readonly IRepository<Depot> _depotRepository;
        private readonly IRepository<Territory> _territoryRepository;
        private readonly IRepository<Zone> _zoneRepository;
        private readonly IFileUploadService _fileUploadService;
        private readonly IMapper _mapper;

        public HappyWalletLeadService(
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

        public async Task<IList<HappyWalletAppLeadStatusModel>> GetAllHappyWalletLeadsStatusByLeadIdsAsync(IList<string> ids)
        {
            var idList = ids.Select(x => CustomConvertExtension.ObjectToInt(x)).ToList();

            var result = await _leadGenerationRepository.GetAllIncludeAsync(
                                 x => x,
                                 x => idList.Contains(x.Id),
                                 x => x.OrderBy(o => o.Id),
                                 x => x.Include(i => i.LeadFollowUps).ThenInclude(i => i.ProjectStatus),
                                 true
                             );

            var modelResult = new List<HappyWalletAppLeadStatusModel>();

            foreach (var res in result)
            {
                var modelRes = new HappyWalletAppLeadStatusModel();
                modelRes.MfsaLeadId = res.Id;
                modelRes.LeadStatus = "Unknown";

                if (res.LeadFollowUps.Any())
                {
                    var leadFollowUp = res.LeadFollowUps.OrderByDescending(x => x.CreatedTime).FirstOrDefault();
                    modelRes.LeadStatus = CustomConvertExtension.GetValueOrDefault(ConstantsLeadProjectStatus.HappyWalletProjectStatusDict,
                                            leadFollowUp.ProjectStatus.DropdownCode, "Unknown");
                }

                modelResult.Add(modelRes);
            }

            return modelResult;
        }

        public async Task<IList<HappyWalletAppLeadDetailModel>> GetAllHappyWalletLeadsDetailByLeadIdsAsync(IList<string> ids)
        {
            var idList = ids.Select(x => CustomConvertExtension.ObjectToInt(x)).ToList();

            var result = await _leadGenerationRepository.GetAllIncludeAsync(
                                 x => x,
                                 x => idList.Contains(x.Id),
                                 x => x.OrderBy(o => o.Id),
                                 x => x.Include(i => i.LeadFollowUps).ThenInclude(i => i.ProjectStatus)
                                        .Include(i => i.LeadFollowUps).ThenInclude(i => i.ActualVolumeSolds).ThenInclude(i => i.BrandInfo),
                                 true
                             );

            var modelResult = new List<HappyWalletAppLeadDetailModel>();

            foreach (var res in result)
            {
                var modelRes = _mapper.Map<HappyWalletAppLeadDetailModel>(res);
                modelRes.LeadStatus = "Unknown";

                if (res.LeadFollowUps.Any())
                {
                    var leadFollowUp = res.LeadFollowUps.OrderByDescending(x => x.CreatedTime).FirstOrDefault();
                    modelRes.LeadStatus = CustomConvertExtension.GetValueOrDefault(ConstantsLeadProjectStatus.HappyWalletProjectStatusDict, 
                                            leadFollowUp.ProjectStatus.DropdownCode, "Unknown");
                }

                modelResult.Add(modelRes);
            }

            return modelResult;
        }
    }
}

