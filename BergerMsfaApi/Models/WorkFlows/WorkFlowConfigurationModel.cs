using Berger.Common.Enumerations;

namespace BergerMsfaApi.Models.WorkFlows 
{
    public class WorkFlowConfigurationModel
    {
        public int Id { get; set; }
        public int OrgRoleId { get; set; }
        public string OrgRoleName { get; set; }
        public int MasterWorkFlowId { get; set; }
        public string MasterWorkFlowName { get; set; }
        //public WorkFlow WorkFlow { get; set; }
        public ModeOfApproval ModeOfApproval { get; set; }
        public ApprovalStatus ApprovalStatus { get; set; }
        public ReceivedStatus ReceivedStatus { get; set; }
        public RejectedStatus RejectedStatus { get; set; }
        public NotificationStatus NotificationStatus { get; set; }
        public Status Status { get; set; }
        public int? WorkflowId { get; set; }
        public WFStatus WFStatus { get; set; }
        public int sequence { get; set; }
    }
}
