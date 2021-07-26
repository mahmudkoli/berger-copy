using AutoMapper;
using Berger.Data.MsfaEntity.PainterRegistration;
using BergerMsfaApi.Mappings;
using BergerMsfaApi.Models.Painter;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using PNTR = Berger.Data.MsfaEntity.PainterRegistration;

namespace BergerMsfaApi.Models.PainterRegistration
{
    public class PainterModel : IMapFrom<PNTR.Painter>
    {
        public void Mapping(Profile profile)
        {
          
            profile.CreateMap<PainterAttachmentModel, PNTR.PainterAttachment>().ReverseMap();

            profile.CreateMap<PainterModel, PNTR.Painter>()
                .ForMember(src => src.AttachedDealers, map => map.MapFrom(dest => dest.AttachedDealers.Select(dealer => new PNTR.AttachedDealerPainter { DealerId = dealer })));


            profile.CreateMap<PNTR.Painter, PainterModel>()
                   .ForMember(src => src.AttachedDealers, dest => dest.MapFrom(s => s.AttachedDealers.Select(s => s.DealerId)))
                   .ForMember(src => src.PainterCatName, dest => dest.MapFrom(s => s.PainterCat != null ? s.PainterCat.DropdownName : string.Empty));

            profile.CreateMap<PNTR.PainterCall, PainterCallModel>();
            profile.CreateMap<PainterCompanyMTDValue, PainterCompanyMTDValueModel>()
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src != null ? $"{src.Company.DropdownName}" : string.Empty));
                //.ForMember(dest => dest.CumelativeInPercent, opt => opt.MapFrom(src => src.CountInPercent));

        }

        public PainterModel()
        {
            AttachedDealers = new List<int>();
            Attachments = new List<PainterAttachmentModel>();
            PainterCalls = new List<PainterCallModel>();
            DealerDetails = new List<AttachedDealerDetails>();

        }
        public int Id { get; set; }
        public string DepotName { get; set; }
        public string Depot { get; set; }
        public string SaleGroupName { get; set; }
        public string SaleGroup { get; set; }
        public string TerritoryName { get; set; }
        public string Territory { get; set; }
        public string ZoneName { get; set; }
        public string Zone { get; set; }

        public string PainterName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public int NoOfPainterAttached { get; set; }
        public bool HasDbbl { get; set; }
        public string AccDbblNumber { get; set; }
        public string AccDbblHolderName { get; set; }
        public string PassportNo { get; set; }
        public string NationalIdNo { get; set; }
        public string BrithCertificateNo { get; set; }
        [RegularExpression(
            "^([A-Za-z0-9+/]{4})*([A-Za-z0-9+/]{3}=|[A-Za-z0-9+/]{2}==)?$", ErrorMessage = "painter image must be properly base64 formatted.")]
        public string PainterImageUrl { get; set; }
        public int PainterCatId { get; set; }
        public string PainterCatName { get; set; }
        public bool IsAppInstalled { get; set; }
        public string Remark { get; set; }
        public decimal AvgMonthlyVal { get; set; }
        public float Loyality { get; set; }
        [Required]
        public string EmployeeId { get; set; }
        public List<PainterAttachmentModel> Attachments { get; set; }
        public List<int> AttachedDealers { get; set; }
        public List<PainterCallModel> PainterCalls { get; set; }

        public List<AttachedDealerDetails> DealerDetails {get;set;}
        public int Status { get; set; }
    }
    public class AttachedDealerDetails
    {
        public string CustomerName { get; set; }
        public string CustomerNo { get; set; }
    }

    public class PainterStatusUpdateModel
    {
        public int Id { get; set; }
        public int Status { get; set; }
        public string Reason { get; set; }
    }

    
}
