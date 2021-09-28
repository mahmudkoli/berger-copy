using System;
using System.Collections.Generic;
using System.Text;

namespace Berger.Odata.Model
{
    public class BrandFamilyDataRootModel
    {
        public string matkl_mts { get; set; }
        public string wgbezmts { get; set; }
        public string matkl { get; set; }
        public string wgbez { get; set; }

        public BrandFamilyDataModel ToModel()
        {
            var model = new BrandFamilyDataModel();
            model.MatarialGroupOrBrandFamily = this.matkl_mts;
            model.MatarialGroupOrBrandFamilyName = this.wgbezmts;
            model.MatarialGroupOrBrand = this.matkl;
            model.MatarialGroupOrBrandName = this.wgbez;
            return model;
        }
    }
}
