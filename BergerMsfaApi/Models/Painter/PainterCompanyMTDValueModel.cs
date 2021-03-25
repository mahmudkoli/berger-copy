using AutoMapper;
using Berger.Data.MsfaEntity.PainterRegistration;

namespace BergerMsfaApi.Models.PainterRegistration
{
    public class PainterCompanyMTDValueModel : MapFrom<PainterCompanyMTDValue>

    {
        public void Mapping(Profile profile)
        {
            profile.CreateMap<PainterCompanyMTDValue, PainterCompanyMTDValueModel>()
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Company.DropdownName))
                .ForMember(dest => dest.CumelativeInPercent, opt => opt.MapFrom(src => src.CountInPercent));
            

        }

        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public decimal Value { get; set; }
        public float CountInPercent { get; set; }
        public float CumelativeInPercent { get; set; }

    }
}
