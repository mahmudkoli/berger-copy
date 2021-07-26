using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Berger.Odata.Model
{
    public enum EnumBrandCategory
    {
        Liquid = 1,
        Powder = 2
    }

    public enum EnumDealerPerformanceCategory
    {
        TopPerformer = 1,
        BottomPerformer = 2,
        NotPurchasedLastMonth = 3
    }

    public enum EnumCustomerClassification
    {
        All = 1,
        Exclusive = 2,
        NonExclusive = 3
    }

    public enum EnumPaymentFollowUpType
    {
        RPRS = 1,
        FastPayCarry = 2
    }

    public enum EnumBrandType
    {
        AllBrands = 1,
        MTSBrands = 2
    }

    public enum DealerPerformanceReportType
    {
        LastYearAppointed = 1,
        ClubSupremeTerritoryWise = 2,
        ClubSupremeTerritoryAndDealerWise = 3
    }

    public enum LastYearAppointedDealerPerformanceType
    {
        Summary = 1,
        Details = 2,
    }

    public enum EnumBrandOrDivision
    {
        AllBrands = 1,
        MTSBrands = 2,
        Division = 3
    }

    public enum EnumVolumeOrValue
    {
        Volume = 1,
        Value = 2,
    }

    public enum ClubSupremeReportType
    {
        Summary = 1,
        Detail = 2
    }

    //public enum EnumPeriod
    //{
    //    Fiscal_Year_Apr_Mar = 1,
    //}
}
