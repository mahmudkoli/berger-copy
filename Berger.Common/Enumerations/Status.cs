using System.ComponentModel.DataAnnotations;

namespace Berger.Common.Enumerations
{
    public enum Status
    {
        [Display(Name = "In Active")]
        InActive,
        Active,
        //Pending,
        //Revert,
        //Rejected,
        //Completed,
        //NotCompleted,
        //InCompleted,
        //InPlace,
        //NotInPlace
    }

    public enum PlanStatus
    {
        Pending = 0,
        Approved = 1,
        Edited = 2,
        ChangeRequested=3
    }

    public enum DealerOpeningStatus
    {
        Pending = 0,
        Approved = 1,
        Edited = 2,
        Rejected = 3
    }


    public enum EmailStatus
    {
        Success = 1,
        Fail = 2,
        
    }

    public enum KpiResultType
    {
        value = 1,
        volume = 2,
    }
}