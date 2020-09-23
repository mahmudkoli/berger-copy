using System;
using Berger.Common.Enumerations;
using Berger.Data.Attributes;

namespace Berger.Data.Common
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

    }
}
