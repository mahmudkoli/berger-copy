using System.ComponentModel.DataAnnotations.Schema;
using BergerMsfaApi.Core;
using BergerMsfaApi.Domain.Organizations;
using BergerMsfaApi.Enumerations;

namespace BergerMsfaApi.Domain.WorkFlows
{

    public class WorkFlowConfiguration : AuditableEntity<int>
    {
        public int OrgRoleId { get; set; }

        public int MasterWorkFlowId { get; set; }

        [ForeignKey("MasterWorkFlowId")]
        public WorkFlow WorkFlow { get; set; }
        [ForeignKey("OrgRoleId")]
        public OrganizationRole OrganizationRole { get; set; }

        public ModeOfApproval ModeOfApproval { get; set; }
        public ApprovalStatus ApprovalStatus { get; set; }
        public ReceivedStatus ReceivedStatus { get; set; }
        public RejectedStatus RejectedStatus { get; set; }
        public NotificationStatus NotificationStatus { get; set; }

        public int sequence { get; set; }
    }
}
