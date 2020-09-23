using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Berger.Data.Attributes;
using Berger.Data.Common;

namespace Berger.Data.MsfaEntity.WorkFlows
{
    public class WorkFlow : AuditableEntity<int>
    {
        //[Required]
        [StringLength(256, MinimumLength = 1)]
        public string Name { get; set; }

        [StringLength(128)]
        public string Action { get; set; }

        public int WorkflowType { get; set; }

        public int WorkflowStep { get; set; }//Workflow Step (numbrt)

        [UniqueKey]
        //[Required]
        [StringLength(128, MinimumLength = 3)]
        public string Code { get; set; }

        public List<WorkflowLog> Logs { get; set; }

        public List<WorkFlowConfiguration> Configurations { get; set; }

    }
}