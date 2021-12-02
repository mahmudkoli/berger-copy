using AutoMapper;
using Berger.Common.Extensions;
using Berger.Data.Common;
using Berger.Data.MsfaEntity.DemandGeneration;
using Berger.Data.MsfaEntity.Setup;
using Berger.Data.MsfaEntity.Users;
using BergerMsfaApi.Mappings;
using BergerMsfaApi.Models.Setup;
using BergerMsfaApi.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BergerMsfaApi.Models.HappyWallet.Lead
{
    public class HappyWalletAppLeadDetailModel : IMapFrom<LeadGeneration>
    {
        public int MfsaLeadId { get; set; }
        public string ProjectCode { get; set; }
        public string Depot { get; set; }
        public string Territory { get; set; }
        public string Zone { get; set; }
        public string ProjectName { get; set; }
        public string ProjectAddress { get; set; }
        public string LeadStatus { get; set; }
        public string KeyContactPersonName { get; set; }
        public string KeyContactPersonNumber { get; set; }
        public IList<HappyWalletAppLeadFollowUpModel> LeadFollowUps { get; set; }

        public HappyWalletAppLeadDetailModel()
        {
            this.LeadFollowUps = new List<HappyWalletAppLeadFollowUpModel>();
        }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<LeadGeneration, HappyWalletAppLeadDetailModel>()
                .ForMember(dest => dest.MfsaLeadId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ProjectCode, opt => opt.MapFrom(src => src.Code))
                .ForMember(dest => dest.KeyContactPersonNumber, opt => opt.MapFrom(src => src.KeyContactPersonMobile));
        }
    }

    public class HappyWalletAppLeadStatusModel
    {
        public int MfsaLeadId { get; set; }
        public string LeadStatus { get; set; }
    }
}
