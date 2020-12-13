using Berger.Common.Enumerations;
using Berger.Data.Common;
using Berger.Data.MsfaEntity.Setup;
using System.ComponentModel.DataAnnotations.Schema;

namespace Berger.Data.MsfaEntity.Tinting
{
    public class TintiningMachine : AuditableEntity<int>
    {

        public string TerritoryCd { get; set; }
        public string EmployeeId { get; set; }
        public int CompanyId { get; set; }
        [ForeignKey("CompanyId")]
        public DropdownDetail Company { get; set; }
        public decimal No { get; set; }
        public float Cont { get; set; }
        public int NoOfCorrection { get; set; }
    }

}
