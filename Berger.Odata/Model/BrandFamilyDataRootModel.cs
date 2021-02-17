using System;
using System.Collections.Generic;
using System.Text;

namespace Berger.Odata.Model
{
    public class BrandFamilyDataRootModel
    {
        public string MATKL_MTS { get; set; }
        public string WGBEZMTS { get; set; }
        public string MATKL { get; set; }
        public string WGBEZ { get; set; }

        public BrandFamilyDataModel ToModel()
        {
            var model = new BrandFamilyDataModel();
            model.MatarialGroupOrBrandFamily = this.MATKL_MTS;
            model.MatarialGroupOrBrandFamilyName = this.WGBEZMTS;
            model.MatarialGroupOrBrand = this.MATKL;
            model.MatarialGroupOrBrandName = this.WGBEZ;
            return model;
        }
    }
}
