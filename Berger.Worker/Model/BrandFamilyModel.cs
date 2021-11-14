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
        public string matkl_mts { get; set; }
        public string wgbezmts { get; set; }
        public string matkl { get; set; }
        public string wgbez { get; set; }

        public BrandFamilyModel ToModel()
        {
            var model = new BrandFamilyModel();
            model.MatarialGroupOrBrandFamily = this.matkl_mts;
            model.MatarialGroupOrBrandFamilyName = this.wgbezmts;
            model.MatarialGroupOrBrand = this.matkl;
            model.MatarialGroupOrBrandName = this.wgbez;
            return model;
        }
    }
}
