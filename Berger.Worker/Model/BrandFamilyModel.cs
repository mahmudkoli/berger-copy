using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Berger.Worker.Model
{
    public class BrandFamilyModel
    {
        public string MatarialGroupOrBrandFamily { get; internal set; }
        public string MatarialGroupOrBrandFamilyName { get; internal set; }
        public string MatarialGroupOrBrand { get; internal set; }
        public string MatarialGroupOrBrandName { get; internal set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }

        private string compositeKey;
        [NotMapped]
        public string CompositeKey
        {
            get => MatarialGroupOrBrand;
            set => compositeKey = value;
        }
    }

    class BrandFamilyRootModel
    {
        public string MATKL_MTS { get; set; }
        public string WGBEZMTS { get; set; }
        public string MATKL { get; set; }
        public string WGBEZ { get; set; }

        public BrandFamilyModel ToModel()
        {
            var model = new BrandFamilyModel();
            model.MatarialGroupOrBrandFamily = this.MATKL_MTS;
            model.MatarialGroupOrBrandFamilyName = this.WGBEZMTS;
            model.MatarialGroupOrBrand = this.MATKL;
            model.MatarialGroupOrBrandName = this.WGBEZ;
            return model;
        }
    }
}
