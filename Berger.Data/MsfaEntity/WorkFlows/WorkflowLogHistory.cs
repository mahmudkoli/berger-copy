using System.ComponentModel.DataAnnotations.Schema;
using Berger.Common.Enumerations;
using Berger.Data.Common;
using Berger.Data.MsfaEntity.Users;

namespace Berger.Data.MsfaEntity.WorkFlows
{
    public class WorkflowLogHistory : AuditableEntity<int>
    {
        public int UserId { get; set; }
        public UserInfo User { get; set; }
        public int WorkflowLogId { get; set; }
        [ForeignKey("WorkflowLogId")]
        public WorkflowLog WorkflowLog { get; set; }
        public WorkflowStatus WorkflowStatus { get; set; }
        public string WorkflowTitle { get; set; }
        public string Comments { get; set; }
        public bool IsSeen { get; set; }
    }
}
