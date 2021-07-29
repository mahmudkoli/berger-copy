using Berger.Common.Enumerations;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BergerMsfaApi.Models.Dealer
{
    public class DealerStatusClubSupremeExcelModel
    {
        public string DealerId { get; set; }
        public string ClubSupremeStatus { get; set; }
        public EnumClubSupreme? ClubSupremeType { get; set; }
    }

    public class DealerStatusBussinessCategoryExcelModel
    {
        public string DealerId { get; set; }
        public string BussinesCategoryStatus { get; set; }
        public EnumBussinesCategory? BussinesCategoryType { get; set; }
    }

    public class DealerStatusExclusiveExcelModel
    {
        public string DealerId { get; set; }
        public string ExclusiveStatus { get; set; }
    }

    public class DealerStatusLastYearAppointedExcelModel
    {
        public string DealerId { get; set; }
        public string LastYearAppointedStatus { get; set; }
    }

    public class DealerStatusAPExcelModel
    {
        public string DealerId { get; set; }
        public string APStatus { get; set; }
    }

    public class DealerStatusExcelExportModel
    {
        public object File { get; set; }
        public string FileName { get; set; }
    }

    public class DealerStatusExcelExportDataModel
    {
        public string DealerId { get; set; }
        public string Status { get; set; }
        public string Result { get; set; }
    }

    public class DealerStatusExcelImportModel
    {
        public IFormFile File { get; set; }
        public EnumDealerStatusExcelImportType Type { get; set; }
    }

    public enum EnumDealerStatusExcelImportType
    {
        Exclusive = 1,
        LastYearAppointed = 2,
        ClubSupreme = 3,
        AP = 4,
        BussinessCategory=5
    }
}
