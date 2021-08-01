using AutoMapper;
using Berger.Data.MsfaEntity.PainterRegistration;

namespace BergerMsfaApi.Models.PainterRegistration
{
    public class PainterCompanyMTDValueModel : MapFrom<PainterCompanyMTDValue>

    {
        public void Mapping(Profile profile)
        {
            profile.CreateMap<PainterCompanyMTDValue, PainterCompanyMTDValueModel>();
                //.ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src!=null? $"{src.Company.DropdownName}":string.Empty))
                //.ForMember(dest => dest.CumelativeInPercent, opt => opt.MapFrom(src => src.CountInPercent));
            
            
        }

        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public decimal Value { get; set; }
        public float CountInPercent { get; set; }
        public float CumelativeInPercent { get; set; }

    }

    public class AttachedDealerPainterCallModel : MapFrom<AttachedDealerPainterCall>
    {
        public void Mapping(Profile profile)
        {
            profile.CreateMap<AttachedDealerPainterCall, AttachedDealerPainterCallModel>();
        }

        public int Id { get; set; }
        public int DealerId { get; set; } // DealerInfo Id
        public string CustomerName { get; set; }
        public string CustomerNo { get; set; }
    }
}
