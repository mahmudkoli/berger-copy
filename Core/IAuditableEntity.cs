using System;
using BergerMsfaApi.Attributes;
using BergerMsfaApi.Enumerations;

namespace BergerMsfaApi.Core
{
    [IgnoreEntity]
    public interface IAuditableEntity
    {
        //  bool IsActive { get; set; }
        int CreatedBy { get; set; }
        DateTime CreatedTime { get; set; }
        int? ModifiedBy { get; set; }
        DateTime? ModifiedTime { get; set; }
        Status Status { get; set; }
        //int? WorkflowId { get; set; }
        //WorkflowStatus WFStatus { get; set; }

    }
}
