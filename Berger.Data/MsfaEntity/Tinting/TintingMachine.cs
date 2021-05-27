using Berger.Common.Enumerations;
using Berger.Data.Common;
using Berger.Data.MsfaEntity.Setup;
using Berger.Data.MsfaEntity.Users;
using System.ComponentModel.DataAnnotations.Schema;

namespace Berger.Data.MsfaEntity.Tinting
{
    public class TintingMachine : AuditableEntity<int>
    {
        public string Depot { get; set; }
        public string Territory { get; set; }
        public int UserInfoId { get; set; }
        public UserInfo UserInfo { get; set; }
        public int CompanyId { get; set; }
        public DropdownDetail Company { get; set; }
        public int NoOfActiveMachine { get; set; }
        public int NoOfInactiveMachine { get; set; }
        public int No { get; set; }
        public decimal Contribution { get; set; }
        public int NoOfCorrection { get; set; }
    }

}
