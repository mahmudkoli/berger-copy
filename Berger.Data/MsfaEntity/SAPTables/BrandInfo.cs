using Berger.Data.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Berger.Data.MsfaEntity.SAPTables
{
    public class BrandInfo : AuditableEntity<int>
    {
        public string MatrialCode { get; set; } // matnr
        public string MatarialDescription { get; set; } // maktx - not specific
        public string mtart { get; set; } // mtart
        public string MatarialGroupOrBrand { get; set; } // matkl
        public string PackSize { get; set; } // groes
        public string Division { get; set; } // spart
        public string ersda { get; set; } // ersda
        public string laeda { get; set; } // laeda
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public bool IsCBInstalled { get; set; }
        public bool IsMTS { get; set; }
        public bool IsPremium { get; set; }

        private string compositeKey;
        [NotMapped]
        public string CompositeKey
        {
            get => MatrialCode;
            set => compositeKey = value;
        }
    }
}
