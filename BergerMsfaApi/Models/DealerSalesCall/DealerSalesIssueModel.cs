using AutoMapper;
using Berger.Common.Enumerations;
using Berger.Data.MsfaEntity.DealerSalesCall;
using Berger.Data.MsfaEntity.Setup;
using BergerMsfaApi.Mappings;
using BergerMsfaApi.Models.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BergerMsfaApi.Models.DealerSalesCall
{
    public class DealerSalesIssueModel : IMapFrom<DealerSalesIssue>
    {
        public int DealerSalesCallId { get; set; }
        public DealerSalesCallModel DealerSalesCall { get; set; }

        public int DealerSalesIssueCategoryId { get; set; }
        public DropdownModel DealerSalesIssueCategory { get; set; }
        public string MaterialName { get; set; }
        public string MaterialGroup { get; set; }
        public int Quantity { get; set; }
        public string BatchNumber { get; set; }
        public string Comments { get; set; }
        //public EnumPriority Priority { get; set; }
        public int PriorityId { get; set; }
        public DropdownModel Priority { get; set; }

        public bool HasCBMachineMantainance { get; set; }
        //public bool IsCBMachineMantainanceRegular { get; set; }
        public int? CBMachineMantainanceId { get; set; }
        public DropdownModel CBMachineMantainance { get; set; }
        public string CBMachineMantainanceRegularReason { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<DealerSalesIssue, DealerSalesIssueModel>();
            profile.CreateMap<DealerSalesIssueModel, DealerSalesIssue>();
            profile.CreateMap<DropdownDetail, DropdownModel>();
            profile.CreateMap<DropdownModel, DropdownDetail>();
        }
    }
}
