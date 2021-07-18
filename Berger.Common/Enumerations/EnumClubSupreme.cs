
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
        Exclusive= 0,

        [Description("Non-Exclusive")]
        NonExclusive = 1,

        [Description("Non AP Non-Exclusive")]
        NonAPNonExclusive = 2,

        [Description("New Dealer")]
        NewDealer = 3,
    }
}
