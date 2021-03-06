using System.ComponentModel.DataAnnotations.Schema;

namespace Berger.Worker.Model
{
    class BrandModel
    {
        public string MaterialCode { get; set; } // matnr
        public string MaterialDescription { get; set; } // maktx
        public string MaterialType { get; set; } // mtart 
        public string MaterialGroupOrBrand { get; set; } // matkl
        public string PackSize { get; set; } // groes
        public string Division { get; set; } // spart
        public string CreatedDate { get; set; } // ersda 
        public string UpdatedDate { get; set; } // laeda 
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }

        private string compositeKey;
        [NotMapped]
        public string CompositeKey
        {
            get => MaterialCode;
            set => compositeKey = value;
        }
    }

    class BrandRootModel
    {
        public string matnr { get; set; } // 
        public string maktx { get; set; } // 
        public string mtart { get; set; } // 
        public string matkl { get; set; } // 
        public string groes { get; set; } // 
        public string spart { get; set; } // 
        public string ersda { get; set; } // 
        public string laeda { get; set; } // 

        public BrandModel ToModel()
        {
            var model = new BrandModel();
            model.MaterialCode = this.matnr;
            model.MaterialDescription = this.maktx;
            model.MaterialType = this.mtart;
            model.MaterialGroupOrBrand = this.matkl;
            model.PackSize = this.groes;
            model.Division = this.spart;
            model.CreatedDate = this.ersda;
            model.UpdatedDate = this.laeda;
            return model;
        }
    }
}
