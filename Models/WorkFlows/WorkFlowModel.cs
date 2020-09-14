using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BergerMsfaApi.Enumerations;

namespace BergerMsfaApi.Models.WorkFlows
{
    public class WorkFlowModel
    {
        
        public WorkFlowModel()
        {
            this.ConfigList = new List<WorkFlowConfigurationModel>();
        }
        public int Id { get; set; }
        [Required]
        // [StringLength(256, MinimumLength = 1)]
        public string Name { get; set; }

        // [StringLength(128)]
        public string Action { get; set; }

        public int WorkflowType { get; set; }

        public int WorkflowStep { get; set; }//Workflow Step (numbrt)

        // [UniqueKey]
        // [Required]
        // [StringLength(128, MinimumLength = 3)]
        public string Code { get; set; }


        public Status Status { get; set; }
        public int? WorkflowId { get; set; }
        public WFStatus WFStatus { get; set; }

        public List<WorkFlowConfigurationModel> ConfigList;


    }
}