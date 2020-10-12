using Berger.Data.MsfaEntity.Setup;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BergerMsfaApi.Models.PainterRegistration
{
    public class PainterModel
    {
        public int Id { get; set; }
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
        //public string DealerName { get; set; }
        public int PainterCatId { get; set; }
        //public string PainterCatName { get; set; }
        public int TerritoryId { get; set; }
        //public string TerritoryName { get; set; }


    }
}
