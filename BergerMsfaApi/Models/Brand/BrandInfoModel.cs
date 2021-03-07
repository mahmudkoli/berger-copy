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
        public string MaterialCode { get; set; } // matnr
        public string MaterialDescription { get; set; } // maktx
        public string MaterialType { get; set; } // mtart
        public string MaterialGroupOrBrand { get; set; } // matkl
        public string PackSize { get; set; } // groes
        public string Division { get; set; } // spart
        public string CreatedDate { get; set; } // ersda
        public string UpdatedDate { get; set; } // laeda
        public bool IsCBInstalled { get; set; }
        public bool IsMTS { get; set; }
        public bool IsPremium { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<BrandInfo, BrandInfoModel>();
        }
    }

    public class BrandStatusModel
    {
        public string MaterialOrBrandCode { get; set; }
        public string PropertyName { get; set; }
    }
}
