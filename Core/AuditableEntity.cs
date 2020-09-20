using System;
using BergerMsfaApi.Attributes;
using BergerMsfaApi.Enumerations;

namespace BergerMsfaApi.Core
{
    [IgnoreEntity]
    public abstract class AuditableEntity<T> : Entity<T>, IAuditableEntity
    {
        protected AuditableEntity()
        {
            ModifiedBy = 0;
            ModifiedTime = DateTime.Now;
            // IsActive = true;
            CreatedBy = 0;
            CreatedTime = DateTime.Now;
            Status = Status.Active;
        }

        [IgnoreUpdate]
        public int CreatedBy { get; set; }

        [IgnoreUpdate]
        public DateTime CreatedTime { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime? ModifiedTime { get; set; }

        // public bool IsActive { get; set; }

        public Status Status { get; set; }
        //public int? WorkflowId { get; set; }
        //public WorkflowStatus WFStatus { get; set; }
    }

}
