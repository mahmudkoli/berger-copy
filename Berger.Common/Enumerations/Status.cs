using System.ComponentModel.DataAnnotations;

namespace Berger.Common.Enumerations
{
    public enum Status
    {
        [Display(Name = "In Active")]
        InActive,
        Active,
        Pending,
        Revert,
        Rejected,
        Completed,
        NotCompleted,
        InCompleted,
        InPlace,
        NotInPlace
    }
}