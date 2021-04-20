using Berger.Data.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Berger.Data.MsfaEntity.SAPTables
{
    public class BrandFamilyInfo : AuditableEntity<int>
    {
        public string MatarialGroupOrBrandFamily { get; set; }
        public string MatarialGroupOrBrandFamilyName { get; set; }
        public string MatarialGroupOrBrand { get; set; }
        public string MatarialGroupOrBrandName { get; set; }
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
}
