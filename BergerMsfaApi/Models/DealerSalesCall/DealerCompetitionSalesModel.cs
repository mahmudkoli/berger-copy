using AutoMapper;
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
    public class DealerCompetitionSalesModel : IMapFrom<DealerCompetitionSales>
    {
        public int DealerSalesCallId { get; set; }
        //public DealerSalesCallModel DealerSalesCall { get; set; }

        public int CompanyId { get; set; }
        public DropdownModel Company { get; set; }
        public decimal AverageMonthlySales { get; set; }
        public decimal ActualAMDSales { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<DealerCompetitionSales, DealerCompetitionSalesModel>();
            profile.CreateMap<DealerCompetitionSalesModel, DealerCompetitionSales>();
            profile.CreateMap<DropdownDetail, DropdownModel>();
            profile.CreateMap<DropdownModel, DropdownDetail>();
        }
    }

    public class SaveDealerCompetitionSalesModel : IMapFrom<DealerCompetitionSales>
    {
        //public int DealerSalesCallId { get; set; }
        //public DealerSalesCallModel DealerSalesCall { get; set; }

        public int CompanyId { get; set; }
        //public DropdownModel Company { get; set; }
        public decimal AverageMonthlySales { get; set; }
        public decimal ActualAMDSales { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<DealerCompetitionSales, SaveDealerCompetitionSalesModel>();
            profile.CreateMap<SaveDealerCompetitionSalesModel, DealerCompetitionSales>();
        }
    }
}
