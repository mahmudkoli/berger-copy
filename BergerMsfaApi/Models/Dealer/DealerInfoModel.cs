﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Berger.Common.Enumerations;

namespace BergerMsfaApi.Models.Dealer
{
    public class DealerInfoModel
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public int CustomerNo { get; set; }
        public string AccountGroup { get; set; }
        public string Territory { get; set; }
        public string Zone { get; set; }
        public string ContactNo { get; set; }
        public string Address { get; set; }
        public bool IsFocused { get; set; }
        public bool IsSubdealer { get; set; }
        
        public DateTime VisitDate { get; set; }
        public string PlanDate { get; set; }

    }

    public class DealerModel
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public int CustomerNo { get; set; }
        public string AccountGroup { get; set; }
        public string Territory { get; set; }
        public string Area { get; set; }
        public string CustZone { get; set; }
        public string BusinessArea { get; set; }

        public string ContactNo { get; set; }
        public string Address { get; set; }
        public bool IsFocused { get; set; }
        public bool IsSubdealer { get; set; }
        public DateTime VisitDate { get; set; }
        public bool IsCBInstalled { get; set; }
        public string IsCBInstalledLabel { get; set; }
        public bool IsExclusive { get; set; }
        public string IsExclusiveLabel { get; set; }
        public bool IsLastYearAppointed { get; set; }
        public string IsLastYearAppointedLabel { get; set; }
        public EnumClubSupreme ClubSupremeType { get; set; }
        public string IsClubSupremeLabel { get; set; }
        public bool IsAp { get; set; }
        public string IsApLabel { get; set; }
        public string SalesGroup { get; set; }
        public string SalesOffice { get; set; }
        public EnumClubSupreme ClubSupremeType { get; set; }
    }
}
