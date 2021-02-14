using System.ComponentModel.DataAnnotations.Schema;

namespace Berger.Worker.Model
{
    class BrandModel
    {
        public string MatrialCode { get; set; } // matnr
        public string MatarialDescription { get; set; } // maktx
        public string mtart { get; set; } // mtart // Material Type
        public string MatarialGroupOrBrand { get; set; } // matkl
        public string PackSize { get; set; } // groes
        public string Division { get; set; } // spart
        public string ersda { get; set; } // ersda // Create Date
        public string laeda { get; set; } // laeda // Update Date
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }

        private string compositeKey;
        [NotMapped]
        public string CompositeKey
        {
            get => MatrialCode;
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
            model.MatrialCode = this.matnr;
            model.MatarialDescription = this.maktx;
            model.mtart = this.mtart;
            model.MatarialGroupOrBrand = this.matkl;
            model.PackSize = this.groes;
            model.Division = this.spart;
            model.ersda = this.ersda;
            model.laeda = this.laeda;
            return model;
        }
    }
}
