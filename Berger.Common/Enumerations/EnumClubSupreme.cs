
using System.ComponentModel;

namespace Berger.Common.Enumerations
{
    public enum EnumClubSupreme
    {
        [Description("None")]
        None = 0,

        [Description( "Gold")]
        Gold = 1,

        [Description("Platinum")]
        Platinum = 2,

        [Description("Platinum Plus")]
        PlatinumPlus = 3,
    }

    public enum EnumBussinesCategory
    {
        [Description("Exclusive")]
        None= 0,

        [Description("Exclusive")]
        Exclusive = 1,

        [Description("Non-Exclusive")]
        NonExclusive = 2,

        [Description("Non AP Non-Exclusive")]
        NonAPNonExclusive = 3,

        [Description("New Dealer")]
        NewDealer = 4,
    }
}
