using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Berger.Data.Common;

namespace Berger.Data.MsfaEntity.Sync
{
    public class SyncDailyTargetLog:AuditableEntity<int>
    {
        public int Year { get; set; }
        public int Month { get; set; }

        [StringLength(150)]
        public string Zone { get; set; }
        [StringLength(20)]
        public string TerritoryCode { get; set; }

        [StringLength(20)]
        public string BusinessArea { get; set; }

        [StringLength(20)]
        public string SalesGroup { get; set; }

        [StringLength(20)]
        public string SalesOffice { get; set; }

        [StringLength(20)]
        public string AccountGroup { get; set; }

        [StringLength(20)]
        public string BrandCode { get; set; }

        [StringLength(20)]
        public string Division { get; set; }

        public int CustNo { get; set; }
        public double TargetVolume { get; set; }
        public double TargetValue { get; set; }

        [StringLength(50)]
        public string DistributionChannel { get; set; }
    }
}