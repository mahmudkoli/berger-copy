using Berger.Data.Common;
using Berger.Data.MsfaEntity.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Berger.Data.MsfaEntity.KPI
{
    public class NewDealerDevelopment : AuditableEntity<int>
    {
        public string BusinessArea { get; set; } // Plant, Depot
        public string Territory { get; set; }
        [NotMapped]
        public string MonthName { get; set; }
        public int Year { get; set; }
        public int FiscalYear { get; set; }
        public int Month { get; set; }
        public int Target { get; set; }
        public int ConversionTarget { get; set; }
        public int NumberofConvertedfromCompetition { get; set; }
        
    }
}
