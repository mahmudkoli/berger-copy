using System;
using System.Collections.Generic;
using BergerMsfaApi.Enumerations;

namespace BergerMsfaApi.Models.WorkFlows
{
    public class WorkFlowLogModel
    {
        public int Id { get; set; }
        public int RowId { get; set; }
        public int WorkFlowFor { get; set; }
        public int MasterWorkFlowId { get; set; }
        public WorkflowStatus WorkflowStatus { get; set; }
        //public WorkFlowModel MasterWorkFlow { get; set; }
        public int PendingWorkflowCount { get; set; }
        public string WorkflowMessage { get; set; }
        public object Data { get; set; }
        public DateTime CreatedTime { get; set; }

        public List<WorkFlowLogHistoryModel> LogHistories { get; set; }

    }
}
