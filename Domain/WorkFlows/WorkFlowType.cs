using System.ComponentModel.DataAnnotations;
using BergerMsfaApi.Core;

namespace BergerMsfaApi.Domain.WorkFlows
{
    public class WorkFlowType : AuditableEntity<int>
    {
        public int WorkflowTypeId { get; set; }
        [StringLength(256, MinimumLength = 1)]
        public string WorkflowTypeName { get; set; }
        public string WorkflowMessage { get; set; }

        [StringLength(256, MinimumLength = 1)]
        public string DbTableName { get; set; }
        public bool IsActive { get; set; }
        public bool IsWorkflowDefAvailable { get; set; }
        public bool IsWorkflowConfigAvailable { get; set; }

        [StringLength(256, MinimumLength = 1)]
        public string ViewName { get; set; }
    }
}
