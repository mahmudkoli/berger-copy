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
        public bool IsCBInstalled { get; set; }
        public bool IsMTS { get; set; }
        public bool IsPremium { get; set; }
        public bool IsEnamel { get; set; }

        private string compositeKey;
        [NotMapped]
        public string CompositeKey
        {
            get => MaterialCode;
            set => compositeKey = value;
        }
    }
}
