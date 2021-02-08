using AutoMapper;
using Berger.Data.MsfaEntity.SAPTables;
using BergerMsfaApi.Mappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BergerMsfaApi.Models.Brand
{
    public class BrandInfoModel : IMapFrom<BrandInfo>
    {
        public int Id { get; set; }
        public string MatrialCode { get; set; } // matnr
        public string MatarialDescription { get; set; } // maktx - not specific
        public string mtart { get; set; } // mtart
        public string MatarialGroupOrBrand { get; set; } // matkl
        public string PackSize { get; set; } // groes
        public string Division { get; set; } // spart
        public string ersda { get; set; } // ersda
        public string laeda { get; set; } // laeda
        public bool IsCBInstalled { get; set; }
        public bool IsMTS { get; set; }
        public bool IsPremium { get; set; }
        public string IsCBInstalledText { get; set; }
        public string IsMTSText { get; set; }
        public string IsPremiumText { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<BrandInfo, BrandInfoModel>()
                .ForMember(dest => dest.IsCBInstalledText, opt => opt.MapFrom(src => src.IsCBInstalled ? "Installed" : "Not Installed"))
                .ForMember(dest => dest.IsMTSText, opt => opt.MapFrom(src => src.IsMTS ? "MTS" : "Not MTS"))
                .ForMember(dest => dest.IsPremiumText, opt => opt.MapFrom(src => src.IsPremium ? "Premium" : "Not Premium"));
        }
    }

    public class BrandStatusModel
    {
        public string MaterialCode { get; set; }
        public string PropertyName { get; set; }
    }
}
