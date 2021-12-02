using AutoMapper;
using Berger.Common.Enumerations;
using Berger.Common.Extensions;
using Berger.Data.Common;
using Berger.Data.MsfaEntity.DemandGeneration;
using Berger.Data.MsfaEntity.Setup;
using BergerMsfaApi.Mappings;
using BergerMsfaApi.Models.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BergerMsfaApi.Models.HappyWallet.Lead
{
    public class HappyWalletAppLeadFollowUpModel : IMapFrom<LeadFollowUp>
    {
        public int LeadFollowUpsId { get; set; }
        public string KeyContactPersonName { get; set; }
        public string KeyContactPersonMobile { get; set; }
        public string PaintContractorName { get; set; }
        public string PaintContractorMobile { get; set; }
        //public string MsfaLeadGeneration { get; set; }
        public int LeadGenerationId { get; set; }
        public IList<HappyWalletAppLeadActualVolumeSoldModel> LeadActualVolumeSold { get; set; }

        public HappyWalletAppLeadFollowUpModel()
        {
            this.LeadActualVolumeSold = new List<HappyWalletAppLeadActualVolumeSoldModel>();
        }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<LeadFollowUp, HappyWalletAppLeadFollowUpModel>()
                .ForMember(dest => dest.LeadFollowUpsId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.LeadActualVolumeSold, opt => opt.MapFrom(src => src.ActualVolumeSolds));
        }
    }

    public class HappyWalletAppLeadActualVolumeSoldModel : IMapFrom<LeadActualVolumeSold>
    {
        public int LeadActualVolumeSoldId { get; set; }
        public int BrandInfoId { get; set; }
        public string BrandName { get; set; }
        public string MaterialCode { get; set; }
        public int Quantity { get; set; }
        public decimal TotalAmount { get; set; }
        public EnumLeadActualVolumeSoldType ActualVolumeSoldType { get; set; }
        //public string MsfaLeadFollowUp { get; set; }
        public int LeadFollowUpId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<LeadActualVolumeSold, HappyWalletAppLeadActualVolumeSoldModel>()
                .ForMember(dest => dest.LeadActualVolumeSoldId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.BrandInfo != null ? src.BrandInfo.MaterialDescription : ""))
                .ForMember(dest => dest.MaterialCode, opt => opt.MapFrom(src => src.BrandInfo != null ? src.BrandInfo.MaterialCode : ""));
        }
    }
}
