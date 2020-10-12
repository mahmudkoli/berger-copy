using Berger.Data.Common;
using Berger.Data.MsfaEntity.Setup;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Berger.Data.MsfaEntity.PainterRegistration
{
    public class Painter : AuditableEntity<int>
    {
        public string DepotName { get; set; }
        public string SaleGroup { get; set; }

        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public bool HasDbbl { get; set; }
        //add attached file
        public string AccNumber { get; set; }
        public string AccHolderName { get; set; }
        public string PersonlIdentityNo { get; set; }
        public string PainterImage { get; set; }
        public bool IsAppInstalled { get; set; }
        public string Remark { get; set; }
        public string AvgMonthlyVal { get; set; }
        public float Loality { get; set; }
        public int DealerId { get; set; }

        [ForeignKey("DealerId")]
        public DropdownDetail Dealer { get; set; }
        public int PainterCatId { get; set; }

        [ForeignKey("PainterCatId")]
        public DropdownDetail PainterCategory { get; set; }
        public int TerritoryId { get; set; }

        [ForeignKey("TerritoryId")]
        public DropdownDetail Territory { get; set; }

    }
}
